using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Recurity.Swf.AVM2;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class DoABC : AbstractTagHandler
    {
        private UInt32 _flags;
        private string _name;
        private AVM2.ABC.AbcFile _AbcFile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DoABC(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                // AS3 requires Swf 9
                return 9;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                int StringEncodedLength = Helper.SwfStrings.SwfStringLength(this.Version, _name);

                return (ulong)(
                    sizeof(UInt32) // Flags
                    + StringEncodedLength
                    + _AbcFile.Length
                );
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            if (!SwfFile.Configuration.AllowAVM2)
            {
                Log.Error(this, "AVM2 Code not allowed by configuration setting");
                return false;
            }

            //
            // do all the verifications within AbcFile
            //
            return _AbcFile.Verify();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            long before = _dataStream.Position;

            BinaryReader br = new BinaryReader(_dataStream);
            byte[] content;

            _flags = br.ReadUInt32();
            _name = Helper.SwfStrings.SwfString(this.Version, br);

            content = br.ReadBytes((int)(_tag.Length - (_dataStream.Position - before)));
            Log.Debug(this, "AVM2 Code named '" + _name + "' with Flags 0x" + _flags.ToString("X08") + ", " + content.Length.ToString("d") + " bytes");

            using (MemoryStream ms = new MemoryStream(content))
            {
                _AbcFile = new Recurity.Swf.AVM2.ABC.AbcFile();
                _AbcFile.Parse(ms);

                /*
                 * TODO: Move into AVM2Code 
                 * 
                for ( uint i = 0; i < abcFile.Methods.Count; i++ )
                {
                    AVM2Method m = new AVM2Method( i, abcFile );
                   //Log.Debug(this,  m.ToString() );
                }
                 */
                String s = String.Format("Calculated length of AbcFile: {0:d} (real: {1:d})", _AbcFile.Length, content.Length);
                Log.Debug(this, s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            Log.Debug(this, "0x" + output.Position.ToString("X08") + ": Write called");

            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(_flags);
            Helper.SwfStrings.SwfWriteString(this.Version, bw, _name);

            long pos1 = output.Position;

            _AbcFile.Write(output);

            String s = String.Format("Wrote {0:d} bytes AbcFile", (long)(output.Position - pos1));
            Log.Debug(this, s);
        }
    }
}
