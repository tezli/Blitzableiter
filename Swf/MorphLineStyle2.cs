using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>MORPHLINESTYLE2 builds upon the capabilities of the MORPHLINESTYLE record by</para>
    /// <para>allowing the use of new types of joins and caps as well as scaling options and the </para>
    /// <para>ability to fill morph strokes.</para>
    /// </summary>
    public class MorphLineStyle2 : MorphLineStyle
    {
        private CapStyle _startCapStyle;
        private JoinStyle _joinstyle;
        private bool _hasFillFlag;
        private bool _noHScale;
        private bool _noVScale;
        private bool _pixelHinting;
        private bool _noClose;
        private CapStyle _endCapStyle;
        private Double _miterLimtiFactor;
        private MorphFillStyle _fillStyle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public MorphLineStyle2(byte InitialVersion) : base(InitialVersion)
        {
            this._endColor = new Rgba(this._SwfVersion);
            this._startColor = new Rgba(this._SwfVersion);
            this._fillStyle = new MorphFillStyle(this._SwfVersion);
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                return 0;
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
        /// <param name="input">The input stream.</param>
        public override void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._startWidth = br.ReadUInt16();
            this._endWidth = br.ReadUInt16();

            BitStream bits = new BitStream(input);

            this._startCapStyle = (CapStyle)(Convert.ToByte(bits.GetBits(2)));
            this._joinstyle = (JoinStyle)(Convert.ToByte(bits.GetBits(2)));

            this._hasFillFlag = (0 != bits.GetBits(1) ? true : false);
            this._noHScale = (0 != bits.GetBits(1) ? true : false);
            this._noVScale = (0 != bits.GetBits(1) ? true : false);
            this._pixelHinting = (0 != bits.GetBits(1) ? true : false);

            bits.GetBits(5); // reserved must be null

            this._noClose = (0 != bits.GetBits(1) ? true : false);

            this._endCapStyle = (CapStyle)bits.GetBits(2);

            if (this._joinstyle.Equals(JoinStyle.Miter))
            {
                this._miterLimtiFactor = br.ReadUInt16();
            }
            if (!this._hasFillFlag)
            {
                this._startColor.Parse(input);
                this._endColor.Parse(input);
            }
            if (this._hasFillFlag)
            {
                //bits.Reset();
                this._fillStyle.Parse(input);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BitStream bits = new BitStream(output);
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._startWidth);
            bw.Write(this._endWidth);

            bits.WriteBits(2, (Int32)this._startCapStyle);
            bits.WriteBits(2, (Int32)this._joinstyle);
            bits.WriteBits(1, Convert.ToInt32(this._hasFillFlag));
            bits.WriteBits(1, Convert.ToInt32(this._noHScale));
            bits.WriteBits(1, Convert.ToInt32(this._noVScale));
            bits.WriteBits(1, Convert.ToInt32(this._pixelHinting));
            bits.WriteBits(5, 0); // reserved
            bits.WriteBits(1, Convert.ToInt32(this._noClose));
            bits.WriteBits(2, (Int32)this._endCapStyle);
            bits.WriteFlush();

            if (this._joinstyle.Equals(JoinStyle.Miter))
            {
                bits.WriteBitsFB(16, this._miterLimtiFactor);
            }

            if (!this._hasFillFlag)
            {
                this._startColor.Write(output);
                this._endColor.Write(output);
            }
            else
            {
                this._fillStyle.Write(output);
            }

        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }
    }
}
