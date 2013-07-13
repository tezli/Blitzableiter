using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// This Tag is used in Swf version 5 only. It is depricated in later versions
    /// </summary>
    class EnableDebugger2 : AbstractTagHandler
    {

        private string _md5Password;

        public EnableDebugger2(byte init) : base(init) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// Password of the tag
        /// </summary>
        public string Password
        {
            get
            {
                return _md5Password;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return sizeof(UInt16) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _md5Password);
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            return true;
        }

        protected override void Parse()
        {
            String s = String.Format("0x{0:X08}: reading EnableDebugger2-Tag", this.Tag.OffsetData);
            //Log.Debug(this, s);
            
            BinaryReader br = new BinaryReader(_dataStream);

            UInt16 _reserved = br.ReadUInt16(); // reserved: must be 0x00

            if (_reserved != 0)
            {
                String s1 = String.Format("Reserved bits has been set.");
                Log.Warn(this, s1);
            }
                

            _md5Password = SwfStrings.SwfString(this._SwfVersion, br);

            String s2 = String.Format("0x{0:X08}:\t{1}", this.Tag.OffsetData, _md5Password);
            //Log.Debug(this, s2);
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write((UInt16)0x00);
            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._md5Password);
        }


    }
}
