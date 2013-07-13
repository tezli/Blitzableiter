using System;
using System.IO;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// The StraightEdgeRecord stores the edge as an X-Y delta. The delta is added to the current
    /// drawing position, and this becomes the new drawing position. The edge is rendered between
    /// the old and new drawing positions.
    /// Straight edge records support three types of lines:
    /// 1. General lines.
    /// 2. Horizontal lines.
    /// 3. Vertical lines.
    /// </summary>
    class StraightEdgeRecord : ShapeRecord
    {
        protected UInt32 _numbits;
        protected bool _generalLineFlag;
        protected bool _vertLineFlag;
        protected Int32 _deltaX;
        protected Int32 _deltaY;

        /// <summary>
        /// The STRAIGHTEDGERECORD stores the edge as an X-Y delta.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object</param>
        public StraightEdgeRecord(byte InitialVersion) : base(InitialVersion)
        {
            this._numbits = 0;
            this._generalLineFlag = false;
            this._vertLineFlag = false;
            this._deltaX = 0;
            this._deltaY = 0;
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
        {
            get
            {
                UInt32 length = 7; // the first 7 flags

                if (!this._generalLineFlag)
                {
                    length += 1;
                    length += this._numbits + 2; // the vert line flag and a deltax or a delta y
                }
                else
                {
                    length += (this._numbits + 2) * 2; // delta x and delta y
                }

                return length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public override bool Verify()
        {
            if (this._generalLineFlag && this._vertLineFlag)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inout"></param>
        /// <param name="bits"></param>
        public override void Parse(Stream inout, BitStream bits)
        {
            this._numbits = (UInt32)bits.GetBits(4);

            this._generalLineFlag = (0 != bits.GetBits(1)) ? true : false;

            if (this._generalLineFlag)
            {
                this._deltaX = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
                this._deltaY = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
            }
            else
            {
                this._vertLineFlag = (0 != bits.GetBits(1)) ? true : false;

                if (this._vertLineFlag)
                {
                    this._deltaY = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
                }
                else
                {
                    this._deltaX = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="bits"></param>
        public override void Write(Stream output, BitStream bits )
        {
            bits.WriteBits(1, 1); // type flag = 1
            bits.WriteBits(1, 1); // straight flag = 1
            bits.WriteBits(4, (UInt32)this._numbits);
            bits.WriteBits(1, (true == this._generalLineFlag ? 1 : 0));


            if (!this._generalLineFlag)
            {
                bits.WriteBits(1, (true == this._vertLineFlag ? 1 : 0)); 

                if (!this._vertLineFlag)
                {
                    bits.WriteBits((Int32)this._numbits + 2, this._deltaX);
                }
                else
                {
                    bits.WriteBits((Int32)this._numbits + 2, this._deltaY);
                }
            }
            else
            {
                bits.WriteBits((Int32)this._numbits + 2, this._deltaX);
                bits.WriteBits((Int32)this._numbits + 2, this._deltaY);
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
            sb.AppendFormat(" deltaX :{0:d} , deltaY : {1:d}", this._deltaX, this._deltaY);
            return sb.ToString();
        }

    }
}
