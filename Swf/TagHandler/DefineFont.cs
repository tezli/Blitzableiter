using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>     
    /// The DefineFont tag defines the shape outlines of each glyph used in a particular font
    /// </summary>
    /// <remarks>
    /// The DefineFont tag defines the shape outlines of each glyph used in a particular font. Only
    /// the glyphs that are used by subsequent DefineText tags are actually defined.
    /// DefineFont tags cannot be used for dynamic text. Dynamic text requires the DefineFont2 tag.
    /// </remarks>
    public class DefineFont : AbstractTagHandler, ISwfCharacter
    {

        /// <summary>
        /// 
        /// </summary>
        private UInt16[] _offsetTable;

        /// <summary>
        /// 
        /// </summary>
        protected Shape[] _glyphShapeTable;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numberOfGlyphs;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _fontID;


        /// <summary>
        /// 
        /// </summary>
        public UInt16 NumberOfGlyphs
        {
            get
            {
                return this._numberOfGlyphs;
            }
        }

        /// <summary>
        /// The DefineFont tag defines the shape outlines of each glyph used in a particular font.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFont(byte InitialVersion) : base(InitialVersion)
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

            this._fontID = br.ReadUInt16();

            UInt16 glyphTableOffset = br.ReadUInt16();

            this._numberOfGlyphs = (UInt16)(glyphTableOffset / 2);

            this._offsetTable = new UInt16[_numberOfGlyphs];
            this._offsetTable[0] = glyphTableOffset;

            long glyphTableStreamOffset = glyphTableOffset + 2;

            for (int i = 1; i < this._numberOfGlyphs; i++)
            {
                this._offsetTable[i] = br.ReadUInt16();
            }

            this._glyphShapeTable = new Shape[this._numberOfGlyphs];

            for (int i = 0; i < this._numberOfGlyphs; i++)
            {
                Shape s = new Shape(this._SwfVersion);

                if (i < this._numberOfGlyphs - 1)
                {
                    s.Parse(this._dataStream, this._offsetTable[i + 1] - this._offsetTable[i], this._tag.TagType);
                    this._glyphShapeTable[i] = s;
                }

                else
                {
                    s.Parse(this._dataStream, this._dataStream.Length - this._dataStream.Position, this._tag.TagType);
                    this._glyphShapeTable[i] = s;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            this.WriteTagHeader(output);

            bw.Write(this._fontID);

            for (int i = 0; i < this._offsetTable.Length; i++ )
            {
                bw.Write(this._offsetTable[i]);
            }

            for (int i = 0; i < this._glyphShapeTable.Length; i++)
            {
                this._glyphShapeTable[i].Write(output);
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

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public virtual UInt16 CharacterID
        {
            get
            {
                return _fontID;
            }
        }
    }
}
