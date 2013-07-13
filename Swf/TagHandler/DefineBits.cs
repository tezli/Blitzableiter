using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>This tag defines a bitmap character with JPEG compression. </para>
    /// <para>It contains only the JPEG compressed image data (from the Frame Header onward). </para>
    /// <para>A separate JPEGTables tag contains the JPEG encoding data used to encode </para>
    /// <para>this image (the Tables/Misc segment).</para>
    /// <para>The data in this tag begins with the JPEG SOI marker 0xFF, 0xD8 and ends with the EOI</para>
    /// <para>marker 0xFF, 0xD9. Before version 8 of the Swf file format, Swf files could contain an</para>
    /// <para>erroneous header of 0xFF, 0xD9, 0xFF, 0xD8 before the JPEG SOI marker.</para>
    /// </summary>
    public class DefineBits : AbstractTagHandler, ISwfCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt16 _characterID;

        /// <summary>
        /// 
        /// </summary>
        private byte[] _jpegData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DefineBits(byte InitialVersion) : base(InitialVersion)
        {
            this._characterID = 0;
            this._jpegData = new byte[0];
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                return this.Tag.Length;
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

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            Stream input = this._dataStream;
            BinaryReader br = new BinaryReader(input);

            this._characterID = br.ReadUInt16();
            this._jpegData = br.ReadBytes((Int32)(input.Length - input.Position));

            String s = String.Format("Reading DefineBits. Character ID: {0:d}, data length {1:d}", this._characterID, this._jpegData.Length);
            //Log.Debug(this, s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this._characterID);
            output.Write(_jpegData, 0, _jpegData.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat(" Character ID : {0:d} , image data size: {1:d}", this._characterID, this._jpegData.Length);
            return sb.ToString();
        }

    }
}
