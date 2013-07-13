using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// LINESTYLE2 builds upon the capabilities of the LINESTYLE record by allowing the use of new types of joins 
    /// and caps as well as scaling options and the ability to fill a stroke.
    /// </summary>
    public class LineStyle2 : LineStyle
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
        private FillStyle _fillStyle;

        /// <summary>
        /// LINESTYLE2 builds upon the capabilities of the LINESTYLE record by allowing the use of new types of joins 
        /// and caps as well as scaling options and the ability to fill a stroke.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public LineStyle2(byte InitialVersion) : base(InitialVersion)
        {
            this._startCapStyle = CapStyle.None;
            this._joinstyle = JoinStyle.Round;
            this._endCapStyle = CapStyle.None;
            this._color = new Rgba(this._SwfVersion);
            this._fillStyle = new FillStyle(this._SwfVersion);
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                UInt64 length = 32; // the first 32 bits are the length and flags

                if (this._joinstyle.Equals(JoinStyle.Miter))
                {
                    length += 16; // a UInt16

                }
                if (!this._hasFillFlag)
                {
                    length += 32; // a RGBA
                }
                else
                {
                    length += this._fillStyle.Length;
                }
                return length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify(Stream output)
        {
            return true;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="input"></param>
       /// <param name="caller"></param>
        public override void Parse(Stream input, TagTypes caller)
        {
            BinaryReader br = new BinaryReader(input);

            this._width = br.ReadUInt16();

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
                bits.Reset();
                this._miterLimtiFactor = br.ReadUInt16();
            }
            if (!this._hasFillFlag)
            {
                bits.Reset();
                this._color.Parse(input);
            }
            if (this._hasFillFlag)
            {
                bits.Reset();
                this._fillStyle.Parse(input, caller);
            }

        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            byte[] width = BitConverter.GetBytes(this._width);
            output.Write(width, 0, 2);

            bits.WriteBits(2, (Int32)this._startCapStyle);
            bits.WriteBits(2, (Int32)this._joinstyle);
            bits.WriteBits(1, Convert.ToInt32(this._hasFillFlag));
            bits.WriteBits(1, Convert.ToInt32(this._noHScale));
            bits.WriteBits(1, Convert.ToInt32(this._noVScale));
            bits.WriteBits(1, Convert.ToInt32(this._pixelHinting));
            bits.WriteBits(5, 0); // reserved
            bits.WriteBits(1, Convert.ToInt32(this._noClose));
            bits.WriteBits(2, (Int32)this._endCapStyle);

            if (this._joinstyle.Equals(JoinStyle.Miter))
            {
                bits.WriteFlush();
                bits.WriteBitsFB(16, this._miterLimtiFactor);
            }
            if (!this._hasFillFlag)
            {
                bits.WriteFlush();
                this._color.Write(output);

            }
            else
            {
                bits.WriteFlush();
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
            sb.Append(base.ToString());
            sb.AppendFormat(" Width: {0:d} ", this._width);
            sb.Append(this._color.ToString());
            return sb.ToString();
        }
    }
}
