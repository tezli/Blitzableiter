using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The DefineMorphShape tag defines the start and end states of a morph sequence. A morph</para>
    /// <para>object should be displayed with the PlaceObject2 tag, where the ratio field specifies how far</para>
    /// <para>the morph has progressed.</para>
    /// </summary>
    public class DefineMorphShape : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _characterID;
        private Rect _startBounds;
        private Rect _endBounds;
        private UInt32 _offset;
        private MorphFillStyleArray _morphFillStyles;
        private MorphLineStyleArray _morphLineStyles;
        private Shape _startEdges;
        private Shape _endEdges;

        /// <summary>
        /// The DefineMorphShape tag defines the start and end states of a morph sequence
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineMorphShape(byte InitialVersion) : base(InitialVersion)
        {
            this._startBounds = new Rect(this._SwfVersion);
            this._endBounds = new Rect(this._SwfVersion);
            this._morphFillStyles = new MorphFillStyleArray(this._SwfVersion);
            this._morphLineStyles = new MorphLineStyleArray(this._SwfVersion);
            this._startEdges = new Shape(this._SwfVersion);
            this._endEdges = new Shape(this._SwfVersion);
    
        }

        /// <summary>
        /// Character ID of the definition
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
                UInt64 ret = 0;

                using (MemoryStream temp = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(temp);

                    bw.Write(this._characterID);

                    this._startBounds.Write(temp);
                    this._endBounds.Write(temp);

                    bw.Write(this._offset);

                    this._morphFillStyles.Write(temp);
                    this._morphLineStyles.Write(temp);

                    this._startEdges.Write(temp);
                    this._endEdges.Write(temp);

                    ret = (UInt64)temp.Position;
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
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();

            this._startBounds.Parse(this._dataStream);
            this._endBounds.Parse(this._dataStream);

            this._offset = br.ReadUInt32();

            Int64 calculatedEndEdgeOffset = this._dataStream.Position + (Int64)this._offset;

            this._morphFillStyles.Parse(this._dataStream, this._tag.TagType);
            this._morphLineStyles.Parse(this._dataStream, this._tag.TagType);

            this._startEdges.Parse(this._dataStream, (Int64)calculatedEndEdgeOffset - this._dataStream.Position, this._tag.TagType);
            this._endEdges.Parse(this._dataStream, (this._dataStream.Length - (Int64)calculatedEndEdgeOffset), this._tag.TagType);

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

            this._startBounds.Write(output);
            this._endBounds.Write(output);

            bw.Write(this._offset);

            this._morphFillStyles.Write(output);
            this._morphLineStyles.Write(output);

            this._startEdges.Write(output);
            this._endEdges.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" Character ID: {0:d}, offset : {1:d}, number of morph fill styles: {2:d}, number of morph line styles: {3:d}",
                            this._characterID, this._offset, this._morphFillStyles.Count, this._morphLineStyles.Count);
            return sb.ToString();
        }
    }
}
