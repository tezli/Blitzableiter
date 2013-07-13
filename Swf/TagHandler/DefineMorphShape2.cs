using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The DefineMorphShape2 tag extends the capabilities of DefineMorphShape by using a new</para>
    /// <para>morph line style record in the morph shape. MORPHLINESTYLE2</para>
    /// </summary>
    public class DefineMorphShape2 : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _characterID;
        private Rect _startBounds;
        private Rect _endBounds;
        private Rect _startEdgeBounds;
        private Rect _endEdgeBounds;
        private bool _usesNonScalingStrokes;
        private bool _usesScalingStrokes;
        private UInt32 _offset;
        private MorphFillStyleArray _morphFillStyles;
        private MorphLineStyleArray _morphLineStyles;
        private Shape _startEdges;
        private Shape _endEdges;
        private Boolean _readingFailed;
        private Byte[] failBuffer;
        /// <summary>
        /// The DefineMorphShape2 tag defines the start and end states of a morph sequence
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineMorphShape2(byte InitialVersion): base(InitialVersion)
        {
            this._startBounds = new Rect(this._SwfVersion);
            this._endBounds = new Rect(this._SwfVersion);
            this._startEdgeBounds = new Rect(this._SwfVersion);
            this._endEdgeBounds = new Rect(this._SwfVersion);
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
                UInt64 ret = 0;

                using (MemoryStream temp = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(temp);

                    bw.Write(this._characterID);

                    this._startBounds.Write(temp);
                    this._endBounds.Write(temp);

                    this._startEdgeBounds.Write(temp);
                    this._endEdgeBounds.Write(temp);

                    BitStream bits = new BitStream(temp);

                    bits.WriteBits(6, 0); //reserved
                    bits.WriteBits(1, Convert.ToInt32(this._usesNonScalingStrokes));
                    bits.WriteBits(1, Convert.ToInt32(this._usesScalingStrokes));
                    bits.WriteFlush();

                    bw.Write(this._offset);

                    if (!_readingFailed)
                    {
                        this._morphFillStyles.Write(temp);
                        this._morphLineStyles.Write(temp);
                        this._startEdges.Write(temp);
                    }
                    else
                    {
                        temp.Write(failBuffer, 0, failBuffer.Length);
                    }

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
        /// /
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();

            this._startBounds.Parse(this._dataStream);
            this._endBounds.Parse(this._dataStream);
            
            this._startEdgeBounds.Parse(this._dataStream);
            this._endEdgeBounds.Parse(this._dataStream);

            BitStream bits = new BitStream(this._dataStream);

            if (0 != bits.GetBits(6))
            {
                Log.Warn(this, "Reserved bits has been set.");
            }

            this._usesNonScalingStrokes = 0 == bits.GetBits(1) ? false : true;
            this._usesScalingStrokes = 0 == bits.GetBits(1) ? false : true;

            this._offset = br.ReadUInt32();
            Int64 calculatedEndEdgeOffset = this._dataStream.Position + (Int64)this._offset;

            long positionBefore = this._dataStream.Position;

            try
            {
                this._morphFillStyles.Parse(this._dataStream, this._tag.TagType);
                this._morphLineStyles.Parse(this._dataStream, this._tag.TagType);
                this._startEdges.Parse(this._dataStream, (Int64)calculatedEndEdgeOffset - this._dataStream.Position, this._tag.TagType);
            }
            catch (SwfFormatException)
            {
                this._dataStream.Seek(positionBefore, SeekOrigin.Begin);
                this.failBuffer = new Byte[calculatedEndEdgeOffset - positionBefore];
                this._dataStream.Read(failBuffer, 0, failBuffer.Length);
                this._readingFailed = true;
            }

            this._endEdges.Parse(this._dataStream, (this._dataStream.Length - (Int64)calculatedEndEdgeOffset), this._tag.TagType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._characterID);

            this._startBounds.Write(output);
            this._endBounds.Write(output);

            this._startEdgeBounds.Write(output);
            this._endEdgeBounds.Write(output);

            BitStream bits = new BitStream(output);

            bits.WriteBits(6, 0); //reserved
            bits.WriteBits(1, Convert.ToInt32( this._usesNonScalingStrokes));
            bits.WriteBits(1, Convert.ToInt32(this._usesScalingStrokes));
            bits.WriteFlush();

            bw.Write(this._offset);

            if (!_readingFailed)
            {
                this._morphFillStyles.Write(output);
                this._morphLineStyles.Write(output);
                this._startEdges.Write(output);
            }
            else
            {
                output.Write(failBuffer, 0, failBuffer.Length);
            }

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
