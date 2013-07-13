using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>A Kerning Record defines the distance between two glyphs in EM square coordinates. Certain</para>
    /// <para>pairs of glyphs appear more aesthetically pleasing if they are moved closer together, or farther</para>
    /// <para>apart. The FontKerningCode1 and FontKerningCode2 fields are the character codes for the</para>
    /// <para>left and right characters. The FontKerningAdjustment field is a signed integer that defines a</para>
    /// <para>value to be added to the advance value of the left character.</para>
    /// </summary>
    public class KerningRecord : AbstractSwfElement
    {
        private UInt16 _wideFontKerningCode1;
        private byte _fontKerningCode1;

        private UInt16 _wideFontKerningCode2;
        private byte _fontKerningCode2;

        private Int16 _fontKerningAdjustment;

        /// <summary>
        /// A Kerning Record defines the distance between two glyphs in EM square coordinates.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public KerningRecord(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return sizeof(UInt16) * 3 + sizeof(byte) * 2;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        public void Parse(Stream input, bool fontFlagsWideCodes)
        {
            BinaryReader br = new BinaryReader(input);

            if (fontFlagsWideCodes)
            {
                this._wideFontKerningCode1 = br.ReadUInt16();
                this._wideFontKerningCode2 = br.ReadUInt16();
            }
            else
            {
                this._fontKerningCode1 = br.ReadByte();
                this._fontKerningCode2 = br.ReadByte();
            }

            this._fontKerningAdjustment = br.ReadInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="fontFlagsWideCodes"></param>
        public void Write(Stream output, bool fontFlagsWideCodes)
        {
            if (fontFlagsWideCodes)
            {
                byte[] code1 = BitConverter.GetBytes(this._wideFontKerningCode1);
                output.Write(code1, 0, 2);
                byte[] code2 = BitConverter.GetBytes(this._wideFontKerningCode2);
                output.Write(code2, 0, 2);
            }
            else
            {
                output.WriteByte(this._fontKerningCode1);
                output.WriteByte(this._fontKerningCode2);
            }

            byte[] kerningAdj = BitConverter.GetBytes(this._fontKerningAdjustment);
            output.Write(kerningAdj, 0, 2);
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
