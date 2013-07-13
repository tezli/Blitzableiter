using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The GLYPHENTRY structure describes a single character in a line of text. It is composed of</para>
    /// <para>an index into the current font’s glyph table, and an advance value. The advance value is the</para>
    /// <para>horizontal distance between the reference point of this character and the reference point of the</para>
    /// <para>following character.</para>
    /// </summary>
    public class GlyphEntry : AbstractSwfElement
    {
        private Int32 _glyphBits;
        private Int32 _advancedBits;

        private UInt32 _glyphIndex;
        private Int32 _glyphAdvance;

        /// <summary>
        /// The GLYPHENTRY structure describes a single character in a line of text.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public GlyphEntry(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return 0;
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
        public void Parse(BitStream bits, byte glyphBits, byte advancedBits)
        {
            this._glyphBits = glyphBits;
            this._advancedBits = advancedBits;

            this._glyphIndex =   bits.GetBits((UInt32)glyphBits);
            this._glyphAdvance = bits.GetBitsSigned((UInt32)advancedBits);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits"></param>
        public void Write(BitStream bits)
        {
            bits.WriteBits(this._glyphBits, this._glyphIndex);
            bits.WriteBits(this._advancedBits, this._glyphAdvance);
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
