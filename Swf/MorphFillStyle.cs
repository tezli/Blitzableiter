using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A fill style represents how a closed shape is filled in.
    /// </summary>
    public class MorphFillStyle : AbstractSwfElement
    {
        private FillStyleType _fillStyleType;
        private Rgba _startColor;
        private Rgba _endColor;
        private MorphGradient _gradient;
        private Matrix _startGradientMatrix;
        private Matrix _endGradientMatrix;
        private UInt16 _bitmapID;
        private Matrix _startBitmapMatrix;
        private Matrix _endBitmapMatrix;

        /// <summary>
        /// A fill style represents how a closed shape is filled in.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphFillStyle(byte InitialVersion) : base(InitialVersion)
        {
            this._startColor = new Rgba(this._SwfVersion);
            this._endColor = new Rgba(this._SwfVersion);
            this._gradient = new MorphGradient(this._SwfVersion);
            this._startGradientMatrix = new Matrix(this._SwfVersion);
            this._endGradientMatrix = new Matrix(this._SwfVersion);
            this._startBitmapMatrix = new Matrix(this._SwfVersion);
            this._endBitmapMatrix = new Matrix(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object.
        /// </summary>
        public ulong Length
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
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._fillStyleType = (FillStyleType)br.ReadByte();

            if (this._fillStyleType.Equals(FillStyleType.SolidFill))
            {
                this._startColor.Parse(input);
                this._endColor.Parse(input);
            }
            else if (this._fillStyleType.Equals(FillStyleType.LinearGradientFill) || this._fillStyleType.Equals(FillStyleType.RadialGradientFill))
            {
                this._startGradientMatrix.Parse(input);
                this._endGradientMatrix.Parse(input);
                this._gradient.Parse(input);
            }
            else if (this._fillStyleType.Equals(FillStyleType.RepeatingBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.ClippedBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedRepeatingBitmap) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedClippedBitmap))
            {
                this._bitmapID = br.ReadUInt16();
                this._startBitmapMatrix.Parse(input);
                this._endBitmapMatrix.Parse(input);
            }
            else
            {
                SwfFormatException e = new SwfFormatException("Invalid fill style type!");
               Log.Error(this, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write(Stream output)
        {
            output.WriteByte((byte)this._fillStyleType);

            if (this._fillStyleType.Equals(FillStyleType.SolidFill))
            {
                this._startColor.Write(output);
                this._endColor.Write(output);
            }
            else if (this._fillStyleType.Equals(FillStyleType.LinearGradientFill) ||
                      this._fillStyleType.Equals(FillStyleType.RadialGradientFill) ||
                      this._fillStyleType.Equals(FillStyleType.FocalRadialGradientFill))
            {
                this._startGradientMatrix.Write(output);
                this._endGradientMatrix.Write(output);
                this._gradient.Write(output);
            }
            else if (this._fillStyleType.Equals(FillStyleType.RepeatingBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.ClippedBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedRepeatingBitmap) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedClippedBitmap))
            {
                byte[] id = BitConverter.GetBytes(this._bitmapID);
                output.Write(id, 0, 2);
                this._startBitmapMatrix.Write(output);
                this._endBitmapMatrix.Write(output);
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
