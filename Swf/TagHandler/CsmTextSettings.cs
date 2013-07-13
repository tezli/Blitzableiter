using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The CSMTextSettings tag modifies a previously streamed DefineText,</para> 
    /// <para>DefineText2, or DefineEditText tag. The CSMTextSettings tag turns advanced</para> 
    /// <para>anti-aliasing on or off for a text field, and can also be used to define</para>
    ///<para> quality and options.</para>
    /// </summary>
    public class CsmTextSettings : AbstractTagHandler
    {
        private UInt16 _textID;
        private byte _useFlashType;
        private byte _gridFit;
        private double _thickness;
        private double _sharpness;

        /// <summary>
        /// The CSMTextSettings tag turns advanced anti-aliasing on or off for a text field, 
        /// and can also be used to define quality and options.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public CsmTextSettings(byte InitialVersion): base(InitialVersion)
        {

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 8;
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
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._textID = br.ReadUInt16();

            BitStream bits = new BitStream(this._dataStream);

            this._useFlashType = (byte)bits.GetBits(2);
            this._gridFit = (byte)bits.GetBits(3);
            bits.GetBits(3); //reserved

            bits.GetBitsFB(32, out this._thickness);
            bits.GetBitsFB(32, out this._sharpness);

            br.ReadByte();// reserved byte

        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            this.WriteTagHeader(output);

            bw.Write(this._textID);

            BitStream bits = new BitStream(output);

            bits.WriteBits(2, (Int32)this._useFlashType);
            bits.WriteBits(3, (Int32)this._gridFit);
            bits.WriteBits(3, 0); // reserved
            bits.WriteFlush();

            bits.WriteBitsFB(32, this._thickness);
            bits.WriteBitsFB(32, this._sharpness);

            output.WriteByte(0); // reserved byte
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            return sb.ToString();
        }
    }
}
