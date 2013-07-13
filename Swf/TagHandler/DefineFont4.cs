using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// DefineFont4 supports only the new Flash Text Engine. The storage of font data for embedded fonts is in CFF format.
    /// </summary>
    public class DefineFont4 : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _fontID;
        private bool _fontFlagsHasFontData;
        private bool _fontFlagsItalic;
        private bool _fontFlagsBold;
        private string _fontName;
        private FontData _fontData;
        private Byte[] _restOfTheRecord;

        /// <summary>
        /// DefineFont4 supports only the new Flash Text Engine. The storage of font data for embedded fonts is in CFF format.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFont4(byte InitialVersion): base(InitialVersion)
        {
            this._fontData = new FontData(this._SwfVersion);
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 10;
            }
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

                    BitStream bits = new BitStream(temp);
                    bits.WriteBits(5, 0); // reserved
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsHasFontData));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsItalic));
                    bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
                    bits.WriteFlush();

                    SwfStrings.SwfWriteString(this._SwfVersion, bw, this._fontName);

                    temp.Write(this._restOfTheRecord, 0, this._restOfTheRecord.Length);

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

            BitStream bits = new BitStream(this._dataStream);
            bits.GetBits(5); //reserved
            this._fontFlagsHasFontData = ((0 != bits.GetBits(1)) ? true : false);
            this._fontFlagsItalic = ((0 != bits.GetBits(1)) ? true : false);
            this._fontFlagsBold = ((0 != bits.GetBits(1)) ? true : false);
            this._fontName = SwfStrings.SwfString(this._SwfVersion, br);
            bits.Reset();

            this._restOfTheRecord = new Byte[this._dataStream.Length - this._dataStream.Position];
            this._dataStream.Read(this._restOfTheRecord, 0, this._restOfTheRecord.Length);

            // another funny statement "FontData : When present, this is an OpenType"
            // Kräht der Gockel auf dem Mist ändert sich das Wetter oder es bleibt wie es ist. XD

            //if (!this._dataStream.Position.Equals(this._dataStream.Length))// if something is present ^^
            //{
            //    this._fontData.Parse(this._dataStream);
            //}

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
            
            BitStream bits = new BitStream(output);
            bits.WriteBits(5,0); // reserved
            bits.WriteBits(1, Convert.ToInt32( this._fontFlagsHasFontData));
            bits.WriteBits(1, Convert.ToInt32( this._fontFlagsItalic));
            bits.WriteBits(1, Convert.ToInt32(this._fontFlagsBold));
            bits.WriteFlush();

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._fontName);

            output.Write(this._restOfTheRecord, 0, this._restOfTheRecord.Length);
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
