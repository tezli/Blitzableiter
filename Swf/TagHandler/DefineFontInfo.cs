using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineFontInfo tag defines a mapping from a glyph font (defined with DefineFont) to a
    /// device font.
    /// </summary>
    /// <remarks>
    /// The DefineFontInfo tag defines a mapping from a glyph font (defined with DefineFont) to a
    /// device font. It provides a font name and style to pass to the playback platform’s text engine,
    /// and a table of character codes that identifies the character represented by each glyph in the
    /// corresponding DefineFont tag, allowing the glyph indices of a DefineText tag to be converted
    /// to character strings.
    /// </remarks>
    public class DefineFontInfo : AbstractTagHandler
    {
        private UInt16 _fontID;
        private byte _fontNameLen;
        private byte[] _fontName;
        private bool _fontFlagsSmallText;
        private bool _fontFlagsShiftJIS;
        private bool _fontFlagsANSI;
        private bool _fontFlagsItalic;
        private bool _fontFlagsBold;
        private bool _fontFlagsWideCodes;
        private byte[] _codeTable;
        private UInt16[] _wideCodeTable;
        private UInt16 _numberOfGlyphs;

        /// <summary>
        /// The DefineFontInfo tag defines a mapping from a glyph font (defined with DefineFont) to a
        /// device font.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFontInfo(byte InitialVersion) : base(InitialVersion)
        {

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
                uint ret = 0;
                using (MemoryStream temp = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(temp);

                    bw.Write(this._fontID);
                    bw.Write(this._fontNameLen);
                    bw.Write(this._fontName);

                    BitStream bits = new BitStream(temp);

                    bits.WriteBits(2, 0); // reserved bits

                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsSmallText));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsShiftJIS));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsANSI));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsItalic));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsWideCodes));
                    bits.WriteFlush();

                    if (this._fontFlagsWideCodes)
                    {
                        for (int i = 0; i < this._wideCodeTable.Length; i++)
                        {
                            bw.Write(this._wideCodeTable[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this._codeTable.Length; i++)
                        {
                            bw.Write(this._codeTable[i]);
                        }
                    }

                    ret = (uint)temp.Position;
                }
                return ret;
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

            this._fontID = br.ReadUInt16();

            try
            {
                AbstractTagHandler handler = this._SourceFileReference.GetCharacterByID(this._fontID);

                if (handler.Tag.TagType.Equals(TagTypes.DefineFont))
                {
                    DefineFont font = (DefineFont)handler;
                    this._numberOfGlyphs = font.NumberOfGlyphs;

                }
                else if (handler.Tag.TagType.Equals(TagTypes.DefineFont2))
                {
                    DefineFont2 font = (DefineFont2)handler;
                    this._numberOfGlyphs = font.NumberOfGlyphs;
                    Log.Warn(this, "This DefineFontInfo2 Tag is redundant and can be droppped");

                }
                else
                {
                    throw new SwfFormatException("The character that matches the font ID is no font!");
                }


            }
            catch (Exception e)
            {
               Log.Error(this, e.Message);
                throw e;
            }

            this._fontNameLen = br.ReadByte();
            this._fontName = new byte[this._fontNameLen];
            this._dataStream.Read(this._fontName, 0, this._fontNameLen);

            BitStream bits = new BitStream(this._dataStream);

            bits.GetBits(2); // reserved
            this._fontFlagsSmallText = 0 != bits.GetBits(1) ? true : false;
            this._fontFlagsShiftJIS = 0 != bits.GetBits(1) ? true : false;
            this._fontFlagsANSI = 0 != bits.GetBits(1) ? true : false;
            this._fontFlagsItalic = 0 != bits.GetBits(1) ? true : false;
            this._fontFlagsBold = 0 != bits.GetBits(1) ? true : false;
            this._fontFlagsWideCodes = 0 != bits.GetBits(1) ? true : false;

            if (this._fontFlagsWideCodes)
            {
                this._wideCodeTable = new UInt16[_numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._wideCodeTable[i] = br.ReadUInt16();
                }
            }
            else
            {
                this._codeTable = new Byte[_numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._codeTable[i] = br.ReadByte();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._fontID);
            bw.Write(this._fontNameLen);
            bw.Write(this._fontName);

            BitStream bits = new BitStream(output);

            bits.WriteBits(2, 0); // reserved bits

            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsSmallText));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsShiftJIS));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsANSI));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsItalic));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsWideCodes));
            bits.WriteFlush();

            if (this._fontFlagsWideCodes)
            {
                for (int i = 0; i < this._wideCodeTable.Length; i++)
                {
                    bw.Write(this._wideCodeTable[i]);
                }
            }
            else
            {
                for (int i = 0; i < this._codeTable.Length; i++)
                {
                    bw.Write(this._codeTable[i]);
                }
            }
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
