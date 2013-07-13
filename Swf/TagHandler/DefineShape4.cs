using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>DefineShape4 extends the capabilities of DefineShape3 by using a new line style record in the</para>
    /// <para>shape. LINESTYLE2 allows new types of joins and caps as well as scaling options and the</para>
    /// <para>ability to fill a stroke.</para>
    /// <para>DefineShape4 specifies not only the shape bounds but also the edge bounds of the shape.</para>
    /// <para>While the shape bounds are calculated along the outside of the strokes, the edge bounds are</para>
    /// <para>taken from the outside of the edges, as shown in the following diagram. The EdgeBounds</para>
    /// <para>field assists Flash Player in accurately determining certain layouts.</para>
    /// <para>In addition, DefineShape4 includes new hinting flags UsesNonScalingStrokes and</para>
    /// <para>UsesScalingStrokes. These flags assist Flash Player in creating the best possible area for</para>
    /// <para>invalidation.</para>
    /// </summary>
    public class DefineShape4 : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _shapeID;
        private Rect _shapeBounds;
        private Rect _edgeBounds;
        private Boolean _usesFillWindingRule;
        private Boolean _usesNonScalingStrokes;
        private Boolean _usesScalingStrokes;
        private ShapeWithStyle _shapes;
        private Byte _reserved;

        /// <summary>
        /// <para>DefineShape4 extends the capabilities of DefineShape3 by using a new line style record in the</para>
        /// <para>shape. LINESTYLE2 allows new types of joins and caps as well as scaling options and the</para>
        /// <para>ability to fill a stroke.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public DefineShape4(byte InitialVersion)
            : base(InitialVersion)
        {
            this._shapeBounds = new Rect(this._SwfVersion);
            this._edgeBounds = new Rect(this._SwfVersion);
            this._shapes = new ShapeWithStyle(this._SwfVersion);
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
                using (MemoryStream temp = new MemoryStream())
                {
                    BitStream bits = new BitStream(temp);
                    BinaryWriter bw = new BinaryWriter(temp);
                    bw.Write(this._shapeID);

                    this._shapeBounds.Write(temp);
                    this._edgeBounds.Write(temp);

                    bits.WriteBits(6, 0); // reserved
                    bits.WriteBits(1, Convert.ToInt32(this._usesFillWindingRule));
                    bits.WriteBits(1, Convert.ToInt32(this._usesNonScalingStrokes));
                    bits.WriteBits(1, Convert.ToInt32(this._usesScalingStrokes));

                    this._shapes.Write(temp);

                    return (ulong)temp.Position;
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
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _shapeID;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
           // Log.Debug(this, "Offset : " + _dataStream.Position);
            BinaryReader2 br = new BinaryReader2(this._dataStream);

            this._shapeID = br.ReadUInt16();

            this._shapeBounds.Parse(this._dataStream);
            this._edgeBounds.Parse(this._dataStream);

            BitStream bits = new BitStream(this._dataStream);

            this._reserved = (byte)bits.GetBits(5); //Reserved. Must be 0

            if (0 != this._reserved)
            {
                SwfFormatException e = new SwfFormatException(" Reserved bits has been set. ");
                Log.Warn(this, e.Message);
                //throw e;
            }

            this._usesFillWindingRule = 0 != bits.GetBits(1) ? true : false;
            this._usesNonScalingStrokes = 0 != bits.GetBits(1) ? true : false;
            this._usesScalingStrokes = 0 != bits.GetBits(1) ? true : false;

            Int64 shapesLength = this._dataStream.Length - this._dataStream.Position;
            this._shapes.Parse(this._dataStream, shapesLength, this._tag.TagType);
           

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BitStream bits = new BitStream(output);
            BinaryWriter bw = new BinaryWriter(output);

            this.WriteTagHeader(output);
            bw.Write(this._shapeID);

            this._shapeBounds.Write(output);
            this._edgeBounds.Write(output);

            bits.WriteBits(5, 0); // reserved

            bits.WriteBits(1, this._usesFillWindingRule ? 1 : 0);
            bits.WriteBits(1, this._usesNonScalingStrokes ? 1 : 0);
            bits.WriteBits(1, this._usesScalingStrokes ? 1 : 0);

            this._shapes.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._shapeID);
            return sb.ToString();
        }

    }
}
