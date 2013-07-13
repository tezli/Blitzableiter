using System;
using System.IO;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// The Swf file format differs from most vector file formats by using Quadratic Bezier curves
    /// rather than Cubic Bezier curves. PostScript™ uses Cubic Bezier curves, as do most drawing
    /// applications.The Swf file format uses Quadratic Bezier curves because they can be stored
    /// more compactly, and can be rendered more efficiently.
    /// </summary>
    public class CurvedEdgeRecord : ShapeRecord
    {
        /// <summary>
        /// 
        /// </summary>
        protected const bool _typeFlag = false;

        /// <summary>
        /// 
        /// </summary>
        protected const bool _straightFlag = false;

        /// <summary>
        /// 
        /// </summary>
        protected byte _numbits;
        
        /// <summary>
        /// 
        /// </summary>
        protected Int32 _controlDeltaX;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _controlDeltaY;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _anchorDeltaX;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _anchorDeltaY;

        /// <summary>
        /// A quadratic bezier curve.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public CurvedEdgeRecord(byte InitialVersion) : base(InitialVersion)
        {
            this._numbits = 0;
            this._controlDeltaX = 0;
            this._controlDeltaY = 0;
            this._anchorDeltaX = 0;
            this._anchorDeltaY = 0;
        }

        /// <summary>
        /// The length of this object in bits.
        /// </summary>
        public override uint Length
        {
            get
            {
                UInt32 length = (UInt32)(4 * ((this._numbits + 2) + 6));
                return length;
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
        /// <param name="output"></param>
        /// <param name="bits"></param>
        public override void Parse(Stream output, BitStream bits)
        {
            this._numbits = (byte)bits.GetBits(4);

            this._controlDeltaX = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
            this._controlDeltaY = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
            this._anchorDeltaX = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
            this._anchorDeltaY = (Int32)bits.GetBitsSigned((UInt32)this._numbits + 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        public override void Write(Stream input, BitStream bits)
        {
            bits.WriteBits(1, 1); // type flag
            bits.WriteBits(1, 0); // straight flag
            bits.WriteBits(4, (Int32)this._numbits);
            bits.WriteBits((Int32)this._numbits + 2, (Int32)this._controlDeltaX);
            bits.WriteBits((Int32)this._numbits + 2, (Int32)this._controlDeltaY);
            bits.WriteBits((Int32)this._numbits + 2, (Int32)this._anchorDeltaX);
            bits.WriteBits((Int32)this._numbits + 2, (Int32)this._anchorDeltaY);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: ControlDelta {1:d}/{2:d}, AnchorDelta {3:d}/{4:d}", this.GetType().ToString(),
                this._controlDeltaX, this._controlDeltaY, this._anchorDeltaX, this._anchorDeltaY);
            return sb.ToString();
        }

    }
}
