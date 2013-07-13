using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineFontAlignZones tag modifies a DefineFont3 tag.
    /// </summary>
    /// <remarks>
    /// The advanced text rendering engine uses alignment zones to establish
    /// the borders of a glyph for pixel snapping. Alignment zones are critical
    /// for high-quality display of fonts.The alignment zone defines a bounding
    /// box for strong vertical and horizontal components of a glyph. The box
    /// is described by a left coordinate, thickness, baseline coordinate, and
    /// height. Small thicknesses or heights are often set to 0.
    /// </remarks>
    public class DefineFontAlignZones : AbstractTagHandler, Helper.ISwfCharacter
    {
        private UInt16 _fontID;
        private byte _CSMTableHint;
        private UInt16 _numberOfGlyphs;
        private ZoneRecord[] _ZoneTable;

        /// <summary>
        /// The DefineFontAlignZones tag modifies a DefineFont3 tag.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFontAlignZones(byte InitialVersion)
            : base(InitialVersion)
        {

        }

        /// <summary>
        /// Character ID of the defined character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _fontID;
            }
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
                return this._tag.Length;
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
                }
                else if (handler.Tag.TagType.Equals(TagTypes.DefineFont3))
                {
                    DefineFont3 font = (DefineFont3)handler;
                    this._numberOfGlyphs = font.NumberOfGlyphs;
                }
                else
                {
                    throw new SwfFormatException("The character that matches the font ID(" + this._fontID + ") is no font!");
                }


            }
            catch (Exception e)
            {
               Log.Error(this, e.Message);
               throw e;
            }

            BitStream bits = new BitStream(this._dataStream);

            this._CSMTableHint = (byte)bits.GetBits(2);
            byte reserved = (byte)bits.GetBits(6);

            this._ZoneTable = new ZoneRecord[this._numberOfGlyphs];
           
            for (int i = 0; i < this._numberOfGlyphs; i++)
            {
                ZoneRecord zr = new ZoneRecord(this._SwfVersion);
                zr.Parse(this._dataStream);
                this._ZoneTable[i] = zr;
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
            bw.Write(_fontID);

            BitStream bits = new BitStream(output);

            bits.WriteBits(2, (Int32)this._CSMTableHint);
            bits.WriteBits(6, 0);
            bits.WriteFlush();

            try
            {
                for (int i = 0; i < this._ZoneTable.Length; i++)
                {
                    this._ZoneTable[i].Write(output);
                }
            }
            catch(Exception ex)
            {
                Log.Warn(this,ex.Message);
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
