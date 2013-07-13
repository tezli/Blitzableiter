using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A TEXTRECORD sets text styles for subsequent characters. It can 
    /// be used to select a font, change the text color, change the 
    /// point size, insert a line break, or set the x and y position of 
    /// the next character in the text. The new text styles apply until 
    /// another TEXTRECORD changes the styles. TextRecords TEXTRECORD[zero or more] 
    /// Text records. EndOfRecordsFlag UI8 Must be 0. DefineText Field Type Comment
    /// Static text tags 191 The TEXTRECORD also defines the actual characters
    /// in a text object. Characters are referred to by an index into the 
    /// current font’s glyph table, not by a character code. Each TEXTRECORD 
    /// contains a group of characters that all share the same text style, and are on
    /// the same line of text.
    /// </summary>
    public class TextRecord : AbstractSwfElement
    {
        private bool _textRecordType;
        private bool _StyleFlagsHasFont;
        private bool _StyleFlagsHasColor;
        private bool _StyleFlagsHasYOffset;
        private bool _StyleFlagsHasXOffset;

        private UInt16 _fontID;
        private Rgb _textColor;
        private Int16 _xOffset;
        private Int16 _yOffset;
        private UInt16 _textHeight;
        private byte _glyphCount;
        private List<GlyphEntry> _glyphEntries;

        /// <summary>
        /// A TEXTRECORD sets text styles for subsequent characters.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public TextRecord(byte InitialVersion) : base(InitialVersion)
        {
            this._textColor = new Rgb(this._SwfVersion);
            this._glyphEntries = new List<GlyphEntry>();
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
        public void Parse(Stream input, TagTypes caller, byte glyphBits, byte advancedBits, byte firstByte)
        {
            this._textRecordType = GetBit(firstByte, 0);
            bool reserved1 = GetBit(firstByte, 1);
            bool reserved2 = GetBit(firstByte, 2);
            bool reserved3 = GetBit(firstByte, 3);
            this._StyleFlagsHasFont = GetBit(firstByte, 4);
            this._StyleFlagsHasColor = GetBit(firstByte, 5);
            this._StyleFlagsHasYOffset = GetBit(firstByte, 6);
            this._StyleFlagsHasXOffset = GetBit(firstByte, 7);

            if (!this._textRecordType || (reserved1 || reserved2 || reserved3))
            {
                SwfFormatException e = new SwfFormatException("The first four bits have to have the static values {1,0,0,0} but set different.");
               Log.Error(this, e.Message);
                throw e; 
            }

            BinaryReader br = new BinaryReader(input);

            if (this._StyleFlagsHasFont)
            {
                this._fontID = br.ReadUInt16();
            }

            if (this._StyleFlagsHasColor)
            {
                if (caller.Equals(TagTypes.DefineFont2) || caller.Equals(TagTypes.DefineText2))
                {
                    this._textColor = new Rgba(this._SwfVersion);
                    this._textColor.Parse(input);
                }
                else
                {
                    this._textColor.Parse(input);
                }
            }
            if (this._StyleFlagsHasXOffset)
            {
                this._xOffset = br.ReadInt16();
            }
            if (this._StyleFlagsHasYOffset)
            {
                this._yOffset = br.ReadInt16();
            }
            if (this._StyleFlagsHasFont)
            {
                this._textHeight = br.ReadUInt16();
            }

            this._glyphCount = br.ReadByte();

            BitStream bits = new BitStream(input);

            GlyphEntry tempGlyph = null;

            for (int i = 0; i < this._glyphCount; i++)
            {
                tempGlyph = new GlyphEntry(this._SwfVersion);
                tempGlyph.Parse(bits, glyphBits, advancedBits);
                this._glyphEntries.Add(tempGlyph);
            }


        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            if (!this._glyphEntries.Count.Equals(this._glyphCount))
            {
                SwfFormatException e = new SwfFormatException("The value of glyph count and the list size of glyphs muste be equal.");
               Log.Error(this, e.Message);
                throw e;
            }
            BitStream bits = new BitStream(output);

            bits.WriteBits(1, 1); // TextRecordType (always 1)
            bits.WriteBits(3, 0); // StyleFlagsReserved (always 0)
            bits.WriteBits(1, Convert.ToInt32(this._StyleFlagsHasFont));
            bits.WriteBits(1, Convert.ToInt32(this._StyleFlagsHasColor));
            bits.WriteBits(1, Convert.ToInt32(this._StyleFlagsHasYOffset));
            bits.WriteBits(1, Convert.ToInt32(this._StyleFlagsHasXOffset));
            bits.WriteFlush();

            if (this._StyleFlagsHasFont)
            {
                byte[] fontID = BitConverter.GetBytes(this._fontID);
                output.Write(fontID, 0, 2);
            }
            if (this._StyleFlagsHasColor)
            {
                this._textColor.Write(output);
            }
            if (this._StyleFlagsHasXOffset)
            {
                byte[] xOffset = BitConverter.GetBytes(this._xOffset);
                output.Write(xOffset, 0, 2);
            }
            if (this._StyleFlagsHasYOffset)
            {
                byte[] yOffset = BitConverter.GetBytes(this._yOffset);
                output.Write(yOffset, 0, 2);
            }
            if (this._StyleFlagsHasFont)
            {
                byte[] textHeight = BitConverter.GetBytes(this._textHeight);
                output.Write(textHeight, 0, 2);
            }

            output.WriteByte(this._glyphCount);



            for (int i = 0; i < this._glyphCount; i++)
            {
                this._glyphEntries[i].Write(bits);

            }
            bits.WriteFlush();
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

        /// <summary>
        /// Gets single bit from a byte
        /// </summary>
        /// <param name="input">The input byte</param>
        /// <param name="position">The position where the byte stands.</param>
        /// <returns></returns>
        private bool GetBit(byte input, byte position)
        {
            bool ret = false;
            int mask = (0x80 >> (int)position);
            int masked = (int)input & mask;
            int shifted = masked >> 7 - (int)position;
            ret = (1 == shifted) ? true : false;
            return ret;
        }
    }
}
