using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// DefineFontInfo2 is identical to DefineFontInfo, except that it adds a field for a language code.
    /// </summary>
    /// <remarks>
    /// When generating Swf 6 or later, it is recommended that you use the new DefineFontInfo2
    /// tag rather than DefineFontInfo. DefineFontInfo2 is identical to DefineFontInfo, except that
    /// it adds a field for a language code. If you use the older DefineFontInfo, the language code will
    /// be assumed to be zero, which results in behavior that is dependent on the locale in which
    /// Flash Player is running.
    /// </remarks>
    public class DefineFontInfo2 : AbstractTagHandler
    {
        private UInt16 _fontID;
        private byte _fontNameLen;
        private byte[] _fontName;
        private Int32 _numberOfGlyphs;
        private bool _fontFlagsSmallText;
        private bool _fontFlagsShiftJIS;
        private bool _fontFlagsANSI;
        private bool _fontFlagsItalic;
        private bool _fontFlagsBold;
        private bool _fontFlagsWideCodes;
        private LangCode _languageCode;
        private List<byte> _codeTable;
        private List<UInt16> _wideCodeTable;

        /// <summary>
        /// DefineFontInfo2 is identical to DefineFontInfo, except that it adds a field for a language code.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFontInfo2(byte InitialVersion): base(InitialVersion)
        {
            this._fontName = new byte[0];
            this._codeTable = new List<byte>();
            this._wideCodeTable = new List<UInt16>();
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

                    byte[] fontID = BitConverter.GetBytes(this._fontID);
                    temp.Write(fontID, 0, 2);
                    temp.WriteByte(this._fontNameLen);
                    temp.Write(this._fontName, 0, this._fontNameLen);

                    BitStream bits = new BitStream(temp);

                    bits.WriteBits(2, 0); // reserved bits

                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsSmallText));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsShiftJIS));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsANSI));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsItalic));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsWideCodes));

                    temp.WriteByte((byte)this._languageCode);

                    if (this._fontFlagsWideCodes)
                    {
                        byte[] tempArray = null;

                        for (int i = 0; i < this._wideCodeTable.Count; i++)
                        {
                            tempArray = BitConverter.GetBytes(this._codeTable[i]);
                            temp.Write(tempArray, 0, 2);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this._codeTable.Count; i++)
                        {
                            temp.WriteByte(this._codeTable[i]);
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

            for (int i = 0; i < this._fontNameLen; i++)
            {
                this._fontName[i] = br.ReadByte();
            }

            BitStream bits = new BitStream(this._dataStream);

            bits.GetBits(2); // reserved
            this._fontFlagsSmallText = (0 != bits.GetBits(1) ? true : false);
            this._fontFlagsShiftJIS = (0 != bits.GetBits(1) ? true : false);
            this._fontFlagsANSI = (0 != bits.GetBits(1) ? true : false);
            this._fontFlagsItalic = (0 != bits.GetBits(1) ? true : false);
            this._fontFlagsBold = (0 != bits.GetBits(1) ? true : false);
            this._fontFlagsWideCodes = (0 != bits.GetBits(1) ? true : false);

            this._languageCode = (LangCode)br.ReadByte();

            if (this._fontFlagsWideCodes)
            {
                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._wideCodeTable.Add(br.ReadUInt16());
                }
            }
            else
            {
                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._wideCodeTable.Add(br.ReadByte());
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

            if (!this._fontNameLen.Equals(this._fontName.Length))
            {
                SwfFormatException e = new SwfFormatException("The font name length field and the length of the font name array must be equal!");
               Log.Error(this, e.Message);
                throw e;
            }

            byte[] fontID = BitConverter.GetBytes(this._fontID);
            output.Write(fontID, 0, 2);
            output.WriteByte(this._fontNameLen);
            output.Write(this._fontName, 0, this._fontNameLen);

            BitStream bits = new BitStream(output);

            bits.WriteBits(2, 0); // reserved bits

            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsSmallText));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsShiftJIS));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsANSI));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsItalic));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsWideCodes));

            output.WriteByte((byte)this._languageCode);

            if (this._fontFlagsWideCodes)
            {
                byte[] tempArray = null;

                for (int i = 0; i < this._wideCodeTable.Count; i++)
                {
                    tempArray = BitConverter.GetBytes(this._codeTable[i]);
                    output.Write(tempArray, 0, 2);
                }
            }
            else
            {
                for (int i = 0; i < this._codeTable.Count; i++)
                {
                    output.WriteByte(this._codeTable[i]);
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

