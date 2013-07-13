using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineText tag defines a block of static text. 
    /// </summary>
    /// <remarks>
    /// The DefineText tag defines a block of static text. It describes the font, size, color, and exact position of every character in the text object.
    /// </remarks>
    public class DefineText : AbstractTagHandler, Helper.ISwfCharacter
    {
        private UInt16 _characterID;
        private Rect _textBounds;
        private Matrix _textMatrix;
        private byte _glyphBits;
        private byte _advancedBits;
        /// <summary>
        /// 
        /// </summary>
        public List<TextRecord> TextRecords { get; internal set; }

        /// <summary>
        /// The DefineText tag defines a block of static text
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineText(byte InitialVersion) : base(InitialVersion)
        {
            this._textBounds = new Rect(this._SwfVersion);
            this._textMatrix = new Matrix(this._SwfVersion);
            this.TextRecords = new List<TextRecord>();
        }

        /// <summary>
        /// Character ID of the defined character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
            }
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
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(ms);

                    bw.Write(this._characterID);

                    this._textBounds.Write(ms);
                    this._textMatrix.Write(ms);

                    bw.Write(this._glyphBits);
                    bw.Write(this._advancedBits);

                    for (int i = 0; i < this.TextRecords.Count; i++)
                    {
                        this.TextRecords[i].Write(ms);
                    }
                    ms.WriteByte(0);// EndOfRecordsFlag
                    return (ulong)ms.Position;
                }
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

            this._characterID = br.ReadUInt16();
            this._textBounds.Parse(this._dataStream);
            this._textMatrix.Parse(this._dataStream);
            this._glyphBits = br.ReadByte();
            this._advancedBits = br.ReadByte();

            byte temp = 0;
            TextRecord tempRecord = null;

            while (0 != (temp = br.ReadByte()))
            {
                tempRecord = new TextRecord(this._SwfVersion);
                tempRecord.Parse(this._dataStream, this._tag.TagType, this._glyphBits, this._advancedBits, temp);
                this.TextRecords.Add(tempRecord);
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

            bw.Write(this._characterID);

            this._textBounds.Write(output);
            this._textMatrix.Write(output);

            bw.Write(this._glyphBits);
            bw.Write(this._advancedBits);

            for (int i = 0; i < this.TextRecords.Count; i++)
            {
                this.TextRecords[i].Write(output);
            }
            output.WriteByte(0);// EndOfRecordsFlag
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
