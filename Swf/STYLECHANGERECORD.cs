using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The style change record is also a non-edge record. It can be used to do the following:</para>
    /// <para>1. Select a fill or line style for drawing.</para>
    /// <para>2. Move the current drawing position (without drawing).</para>
    /// <para>3. Replace the current fill and line style arrays with a new set of styles.</para>
    /// </summary>
    public class StyleChangeRecord : ShapeRecord
    {
        // Flags
        /// <summary>
        /// 
        /// </summary>
        protected const bool _typeFlag = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool _stateNewStyles;
        /// <summary>
        /// 
        /// </summary>
        protected bool _stateLineStyle;
        /// <summary>
        /// 
        /// </summary>
        protected bool _stateFillStyle0;
        /// <summary>
        /// 
        /// </summary>
        protected bool _stateFillStyle1;
        /// <summary>
        /// 
        /// </summary>
        protected bool _stateMoveTo;
        // state move to
        /// <summary>
        /// 
        /// </summary>
        protected byte _moveBits;
        /// <summary>
        /// 
        /// </summary>
        protected Int32 _moveDeltaX;
        /// <summary>
        /// 
        /// </summary>
        protected Int32 _moveDeltaY;
        // state styles
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _fillStyle0;
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _fillStyle1;
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _lineStyle;
        // state new style (DefineShape)
        /// <summary>
        /// 
        /// </summary>
        protected FillStyleArray _fillStyles;
        /// <summary>
        /// 
        /// </summary>
        protected LineStyleArray _lineStyles;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numFillBits;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numLineBits;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _newNumFillBits;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _newNumLineBits;
        /// <summary>
        /// 
        /// </summary>
        protected bool _isFirst;
        /// <summary>
        /// 
        /// </summary>
        protected TagTypes _caller;

        /// <summary>
        /// A SHAPERECORD which changes the style.
        /// </summary>
        /// <param name="InitialVersion">The version of the swf file using this object.</param>
        public StyleChangeRecord(byte InitialVersion) : base(InitialVersion)
        {
            this._fillStyles = new FillStyleArray(this._SwfVersion);
            this._lineStyles = new LineStyleArray(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
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
            return true; // nothing to verify here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        /// <param name="firstFive"></param>
        /// <param name="caller"></param>
        /// <param name="fillBits"></param>
        /// <param name="lineBits"></param>
        /// <param name="first"></param>
        public void Parse(Stream input, BitStream bits, byte firstFive, TagTypes caller, ref UInt16 fillBits, ref UInt16 lineBits, bool first)
        {
            this._isFirst = first;
            this._caller = caller;
            this._numFillBits = fillBits;
            this._numLineBits = lineBits;

            this.GetFlags(firstFive);

            switch (caller)
            {
                case TagTypes.DefineShape2:
                    this.ParseDefineShape23(input, bits, ref fillBits, ref lineBits, first);
                    break;
                case TagTypes.DefineShape3:
                    this.ParseDefineShape23(input, bits, ref fillBits, ref lineBits, first);
                    break;
                default:
                    this.ParseGeneric(input, bits, ref fillBits, ref lineBits, first);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="bits"></param>
        public override void Write(Stream output, BitStream bits)
        {
            bits.WriteBits(1, 0); //type flag
            bits.WriteBits(1, Convert.ToUInt32(this._stateNewStyles));
            bits.WriteBits(1, Convert.ToUInt32(this._stateLineStyle));
            bits.WriteBits(1, Convert.ToUInt32(this._stateFillStyle1));
            bits.WriteBits(1, Convert.ToUInt32(this._stateFillStyle0));
            bits.WriteBits(1, Convert.ToUInt32(this._stateMoveTo));

            if (this._stateMoveTo)
            {
                bits.WriteBits(5, (Int32)this._moveBits);
                bits.WriteBits((Int32)this._moveBits, (Int32)this._moveDeltaX);
                bits.WriteBits((Int32)this._moveBits, (Int32)this._moveDeltaY);
            }
            if (this._stateFillStyle0)
            {
                bits.WriteBits((Int32)this._numFillBits, (Int32)this._fillStyle0);
            }
            if (this._stateFillStyle1)
            {
                bits.WriteBits((Int32)this._numFillBits, (Int32)this._fillStyle1);
            }
            if (this._stateLineStyle)
            {
                bits.WriteBits((Int32)this._numLineBits, (Int32)this._lineStyle);
            }
            if (this._stateNewStyles && (this._caller.Equals(TagTypes.DefineShape2) || this._caller.Equals(TagTypes.DefineShape3)))
            {
                bits.WriteFlush();
                this._fillStyles.Write(output);
                bits.WriteFlush();
                this._lineStyles.Write(output);
                bits.WriteFlush();
                bits.WriteBits(4, (Int32)this._newNumFillBits);
                bits.WriteBits(4, (Int32)this._newNumLineBits);
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

            if (this._stateNewStyles)
            {
                sb.Append(" Stating new styles. ");
            }
            if (this._stateMoveTo)
            {
                sb.AppendFormat(" Moving to: {0:d}/{0:d}", this._moveDeltaX, this._moveDeltaY);
            }
            if (this._stateFillStyle0)
            {
                if (this._fillStyle0 == 0)
                {
                    sb.Append("  fill 0 style will set to none  ");
                }
                else
                {
                    sb.AppendFormat(" fill style 0 is : {0:d},", this._fillStyle0);
                }
            }
            if (this._stateFillStyle1)
            {
                if (this._fillStyle1 == 0)
                {
                    sb.Append(" fill 1 style will set to none ");
                }
                else
                {
                    sb.AppendFormat(" fill style 1 is : {0:d},", this._fillStyle1);
                }
            }
            if (this._stateLineStyle)
            {
                if (this._lineStyle == 0)
                {
                    sb.Append(" line style will set to none ");
                }
                else
                {
                    sb.AppendFormat(" line style is : {0:d},", this._fillStyle0);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstFive"></param>
        private void GetFlags(byte firstFive)
        {
            if (this._caller.Equals(TagTypes.DefineShape2) || this._caller.Equals(TagTypes.DefineShape3))
            {
                this._stateNewStyles = GetBit(firstFive, 3);
                this._stateLineStyle = GetBit(firstFive, 4);
                this._stateFillStyle1 = GetBit(firstFive, 5);
                this._stateFillStyle0 = GetBit(firstFive, 6);
                this._stateMoveTo = GetBit(firstFive, 7);
            }
            else
            {
                this._stateNewStyles = false;
                this._stateLineStyle = GetBit(firstFive, 4);
                this._stateFillStyle1 = GetBit(firstFive, 5);
                this._stateFillStyle0 = GetBit(firstFive, 6);
                this._stateMoveTo = GetBit(firstFive, 7);
            }
        }

        /// <summary>
        /// Gets single bit from a byte
        /// </summary>
        /// <param name="input">The input byte</param>
        /// <param name="position">The position where the byte stands.</param>
        /// <returns></returns>
        private bool GetBit(byte input, byte position)
        {
            bool ret = false;
            int mask = (0x80 >> (int)position);
            int masked = (int)input & mask;
            int shifted = masked >> 7 - (int)position;
            ret = (1 == shifted) ? true : false;
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        /// <param name="fillBits"></param>
        /// <param name="lineBits"></param>
        /// <param name="first"></param>
        private void ParseDefineShape23(Stream input, BitStream bits, ref UInt16 fillBits, ref UInt16 lineBits, bool first)
        {
            if (this._stateMoveTo)
            {
                this._moveBits = (byte)bits.GetBits(5);
                this._moveDeltaX = (Int32)bits.GetBitsSigned((UInt32)this._moveBits);
                this._moveDeltaY = (Int32)bits.GetBitsSigned((UInt32)this._moveBits);
            }

            if (this._stateFillStyle0)
            {
                this._fillStyle0 = bits.GetBits((UInt32)fillBits);
            }

            if (this._stateFillStyle1)
            {
                this._fillStyle1 = bits.GetBits((UInt32)fillBits);
            }
            if (this._stateLineStyle)
            {
                this._lineStyle = bits.GetBits((UInt32)lineBits);
            }
            if (this._stateNewStyles)
            {

                bits.Reset();
                this._fillStyles.Parse(input, this._caller);
                this._lineStyles.Parse(input, this._caller);
                this._newNumFillBits = (UInt16)bits.GetBits(4);
                this._newNumLineBits = (UInt16)bits.GetBits(4);
                fillBits = this._newNumFillBits;
                lineBits = this._newNumLineBits;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        /// <param name="fillBits"></param>
        /// <param name="lineBits"></param>
        /// <param name="first"></param>
        private void ParseGeneric(Stream input, BitStream bits, ref UInt16 fillBits, ref UInt16 lineBits, bool first)
        {
            if (this._stateMoveTo)
            {
                this._moveBits = (byte)bits.GetBits(5);
                this._moveDeltaX = (Int32)bits.GetBitsSigned((UInt32)this._moveBits);
                this._moveDeltaY = (Int32)bits.GetBitsSigned((UInt32)this._moveBits);
            }
            if (this._stateFillStyle0)
            {
                this._fillStyle0 = bits.GetBits((UInt32)fillBits);
            }
            if (this._stateFillStyle1)
            {
                this._fillStyle1 = bits.GetBits((UInt32)fillBits);
            }
            if (this._stateLineStyle)
            {
                this._lineStyle = bits.GetBits((UInt32)lineBits);
            }
        }

    }
}
