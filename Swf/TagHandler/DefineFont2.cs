using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineFont2 tag extends the functionality of DefineFont.
    /// </summary>
    /// The DefineFont2 tag extends the functionality of DefineFont. Enhancements include the following:
    /// <remarks>
    /// <list type="bullet">
    /// <item>32-bit entries in the OffsetTable, for fonts with more than 64K glyphs.</item>
    /// <item>Mapping to device fonts, by incorporating all the functionality of DefineFontInfo.</item>
    /// <item>Font metrics for improved layout of dynamic glyph text.</item>
    /// <item>DefineFont2 tags are the only font definitions that can be used for dynamic text.</item>
    /// </list>
    /// </remarks>
    public class DefineFont2 : DefineFont, ISwfCharacter
    {

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsHasLayout;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsShiftJIS;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsSmallText;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsANSI;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsWideOffsets;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsWideCodes;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsItalic;

        /// <summary>
        /// 
        /// </summary>
        protected bool _fontFlagsBold;

        /// <summary>
        /// 
        /// </summary>
        protected bool _hasGlyphs;

        /// <summary>
        /// 
        /// </summary>
        protected LangCode _languageCode;


        /// <summary>
        /// 
        /// </summary>
        protected byte _fontNameLen;

        /// <summary>
        /// 
        /// </summary>
        protected byte[] _fontName;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16[] _offsetTable;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32[] _wideOffsetTable;

        /// <summary>
        /// 
        /// </summary>
        protected byte[] _codeTable;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16[] _wideCodeTable;

        /// <summary>
        /// 
        /// </summary>

        protected UInt16 _codeTableOffset;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _wideCodeTableOffset;


        /// <summary>
        /// 
        /// </summary>
        protected Int16 _fontAscent;

        /// <summary>
        /// 
        /// </summary>
        protected Int16 _fontDescent;

        /// <summary>
        /// 
        /// </summary>
        protected Int16 _fontLeading;

        /// <summary>
        /// 
        /// </summary>
        protected Int16[] _fontAdvanceTable;

        /// <summary>
        /// 
        /// </summary>
        protected Rect[] _fontBoundsTable;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _kerningCount;

        /// <summary>
        /// 
        /// </summary>
        protected KerningRecord[] _fontKerningTable;

        #endregion

        /// <summary>
        /// The DefineFont2 tag extends the functionality of DefineFont
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFont2(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 3;
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
                    this.WriteFlags(temp);

                    this.WriteCodeTable(temp);

                    this.WriteLayout(temp);

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

            this.ParseFlags(this._dataStream);

            this.ParseCodeTable(this._dataStream);

            this.ParseLayout(this._dataStream);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            this.WriteFlags(output);

            this.WriteCodeTable(output);

            this.WriteLayout(output);
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
        /// Parses the flags of this tag.
        /// </summary>
        /// <param name="input">The input stream where to read from.</param>
        protected virtual void ParseFlags(Stream input)
        {
            BinaryReader br = new BinaryReader(this._dataStream);
            BitStream bits = new BitStream(this._dataStream);

            base._fontID = br.ReadUInt16();

            this._fontFlagsHasLayout = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsShiftJIS = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsSmallText = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsANSI = 0 == bits.GetBits(1) ? false : true;

            this._fontFlagsWideOffsets = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsWideCodes = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsItalic = 0 == bits.GetBits(1) ? false : true;
            this._fontFlagsBold = 0 == bits.GetBits(1) ? false : true;

            if (this._SwfVersion <= 5)
            {
                br.ReadByte();
                this._languageCode = 0;
            }
            else
            {
                this._languageCode = (LangCode)br.ReadByte();
            }

            this._fontNameLen = br.ReadByte();
            this._fontName = new byte[this._fontNameLen];
            this._dataStream.Read(this._fontName, 0, this._fontNameLen);
           //Log.Debug(this, "Font name: " + Encoding.UTF8.GetString(this._fontName) + " Character ID:" + this._fontID);

            this._numberOfGlyphs = br.ReadUInt16(); // Zero for device fonts

            Int64 offsetTableOffset = this._dataStream.Position;

            if (this._dataStream.Position < this._dataStream.Length)
            {
                this._hasGlyphs = true;

                if (this._numberOfGlyphs.Equals(0))
                {
                    Log.Warn(this, "Number of glyphs is zero which means that this tag is used for a device font. For device fonts no glyph-rendering fallback is desired, but trailing data has been detected.");
                }

                if (this._fontFlagsWideOffsets)
                {
                    this._wideOffsetTable = new UInt32[this._numberOfGlyphs];

                    for (int i = 0; i < this._numberOfGlyphs; i++)
                    {
                        this._wideOffsetTable[i] = br.ReadUInt32();
                    }

                    this._wideCodeTableOffset = br.ReadUInt32();
                    Int64 realCodeTableOffset = offsetTableOffset + this._wideCodeTableOffset;

                    this._glyphShapeTable = new Shape[this._numberOfGlyphs];

                    for (int i = 0; i < this._numberOfGlyphs; i++)
                    {
                        Shape s = new Shape(this._SwfVersion);

                        if (i < this._numberOfGlyphs - 1)
                        {
                            s.Parse(this._dataStream, this._wideOffsetTable[i + 1] - this._wideOffsetTable[i], this._tag.TagType);
                            this._glyphShapeTable[i] = s;
                        }
                        else // last
                        {
                            s.Parse(this._dataStream, realCodeTableOffset - this._dataStream.Position, this._tag.TagType);
                            this._glyphShapeTable[i] = s;
                        }
                    }
                }

                else
                {
                    this._offsetTable = new UInt16[this._numberOfGlyphs];

                    for (int i = 0; i < this._numberOfGlyphs; i++)
                    {
                        this._offsetTable[i] = br.ReadUInt16();
                    }

                    this._codeTableOffset = br.ReadUInt16();
                    Int64 realCodeTableOffset = offsetTableOffset + this._codeTableOffset;

                    this._glyphShapeTable = new Shape[this._numberOfGlyphs];

                    for (int i = 0; i < this._numberOfGlyphs; i++)
                    {
                        Shape s = new Shape(this._SwfVersion);

                        if (i < this._numberOfGlyphs - 1)
                        {
                            s.Parse(this._dataStream, this._offsetTable[i + 1] - this._offsetTable[i], this._tag.TagType);
                            this._glyphShapeTable[i] = s;
                        }
                        else // last
                        {
                            s.Parse(this._dataStream, realCodeTableOffset - this._dataStream.Position, this._tag.TagType);
                            this._glyphShapeTable[i] = s;
                        }
                    }

                }
            }
            else
            {
                this._hasGlyphs = false;
            }
        }

        /// <summary>
        /// Parses the code table of this tag.
        /// </summary>
        /// <param name="input">The input stream where to read from.</param>
        protected virtual void ParseCodeTable(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            if (this._fontFlagsWideCodes)
            {
                this._wideCodeTable = new UInt16[this._numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._wideCodeTable[i] = br.ReadUInt16();
                }

            }
            else
            {
                this._codeTable = new byte[this._numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._codeTable[i] = br.ReadByte();
                }
            }

        }

        /// <summary>
        /// Parses the layout of this tag.
        /// </summary>
        /// <param name="input">The input stream where to read from.</param>
        protected virtual void ParseLayout(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            if (this._fontFlagsHasLayout)
            {

                this._fontAscent = br.ReadInt16();
                this._fontDescent = br.ReadInt16();
                this._fontLeading = br.ReadInt16();

                this._fontAdvanceTable = new Int16[this._numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    this._fontAdvanceTable[i] = br.ReadInt16();
                }

                this._fontBoundsTable = new Rect[this._numberOfGlyphs];

                for (int i = 0; i < this._numberOfGlyphs; i++)
                {
                    Rect r = new Rect(this._SwfVersion);
                    r.Parse(this._dataStream);
                    this._fontBoundsTable[i] = r;
                }

                this._kerningCount = br.ReadUInt16();

                this._fontKerningTable = new KerningRecord[this._kerningCount];

                for (int i = 0; i < this._kerningCount; i++)
                {
                    KerningRecord r = new KerningRecord(this._SwfVersion);
                    r.Parse(this._dataStream, this._fontFlagsWideCodes);
                    this._fontKerningTable[i] = r;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected virtual void WriteFlags(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            BitStream bits = new BitStream(output);

            bw.Write(this._fontID);

            bits.WriteBits(1, this._fontFlagsHasLayout ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsShiftJIS ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsSmallText ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsANSI ? 1 : 0);

            bits.WriteBits(1, this._fontFlagsWideOffsets ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsWideCodes ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsItalic ? 1 : 0);
            bits.WriteBits(1, this._fontFlagsBold ? 1 : 0);
            bits.WriteFlush();

            if (this._SwfVersion <= 5)
            {
                bw.Write((byte)0);
            }
            else
            {
                bw.Write((byte)this._languageCode);
            }

            bw.Write(this._fontNameLen);
            output.Write(this._fontName, 0, this._fontName.Length);

            bw.Write(this._numberOfGlyphs);


            if (this._hasGlyphs)
            {
                if (this._fontFlagsWideOffsets)
                {
                    for (int i = 0; i < this._wideOffsetTable.Length; i++)
                    {
                        bw.Write(this._wideOffsetTable[i]);
                    }

                    bw.Write(this._wideCodeTableOffset);
                }
                else
                {
                    for (int i = 0; i < this._offsetTable.Length; i++)
                    {
                        bw.Write(this._offsetTable[i]);
                    }

                    bw.Write(this._codeTableOffset);
                }

                for (int i = 0; i < this._glyphShapeTable.Length; i++)
                {
                    this._glyphShapeTable[i].Write(output);
                } 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected virtual void WriteCodeTable(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

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
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected virtual void WriteLayout(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            if (this._fontFlagsHasLayout)
            {
                bw.Write(this._fontAscent);
                bw.Write(this._fontDescent);
                bw.Write(this._fontLeading);

                for (int i = 0; i < this._fontAdvanceTable.Length; i++)
                {
                    bw.Write(this._fontAdvanceTable[i]);
                }


                for (int i = 0; i < this._fontBoundsTable.Length; i++)
                {
                    this._fontBoundsTable[i].Write(output);
                }

                bw.Write(this._kerningCount);

                for (int i = 0; i < this._fontKerningTable.Length; i++)
                {
                    this._fontKerningTable[i].Write(output, this._fontFlagsWideCodes);
                }

            }
        }
    }
}

