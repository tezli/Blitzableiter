using System;
using System.IO;
using System.Diagnostics;
using Recurity.Swf;
using Recurity.Swf.Interfaces;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// The way how object like lines and shapes get filled.
    /// </summary>
    public class FillStyle : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected FillStyleType _fillStyleType;
        
        /// <summary>
        /// 
        /// </summary>
        protected Rgb _color;

        /// <summary>
        /// 
        /// </summary>
        protected Matrix _gradientMatrix;

        /// <summary>
        /// 
        /// </summary>
        protected Gradient _gradient;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _bitmapID;

        /// <summary>
        /// 
        /// </summary>
        protected Matrix _bitmapMatrix;

        /// <summary>
        /// 
        /// </summary>
        protected TagTypes _caller;

        /// <summary>
        /// The way how object like lines and shapes get filled.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public FillStyle(byte InitialVersion) : base(InitialVersion)
        {
            this._color = new Rgb(this._SwfVersion);
            this._gradientMatrix = new Matrix(this._SwfVersion);
            this._gradient = new Gradient(this._SwfVersion);
            this._bitmapMatrix = new Matrix(this._SwfVersion);
            this._caller = TagTypes.DefineShape;
        }

        /// <summary>
        /// The length of this object.
        /// </summary>
        public ulong Length
        {
            get
            {
                uint ret = 0;
                using (MemoryStream temp = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(temp);
                    this.Write(temp);
                    ret = (uint)temp.Position;
                }
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public void Parse(Stream input, TagTypes caller)
        {
            BinaryReader br = new BinaryReader(input);

            this._caller = caller;
            this._fillStyleType = (FillStyleType)br.ReadByte();

            if (this._fillStyleType.Equals(FillStyleType.SolidFill))
            {
                if (caller.Equals(TagTypes.DefineShape3) )
                {
                    this._color = new Rgba(this._SwfVersion);

                    try
                    {
                        this._color.Parse(input);
                        //Log.InfoFormat("Valid fill style type. FillstyleType: 0x{0:X2} at address: 0x{1:X2} in {2}", (Int32)this._fillStyleType, ((Int32)input.Position) - 1, caller);
                    }
                    catch (SwfFormatException e)
                    {
                        throw e;
                    }
                
                    
                }
                else if (caller.Equals(TagTypes.DefineShape4))
                {
                    this._color = new Rgba(this._SwfVersion);

                    try
                    {
                        this._color.Parse(input);
                    }
                    catch (SwfFormatException e)
                    {
                        throw e;
                    }
                }
                else
                {
                    this._color = new Rgb(this._SwfVersion);

                    try
                    {
                        this._color.Parse(input);
                    }
                    catch (SwfFormatException e)
                    {
                        throw e;
                    }
                }
            }
            else if (this._fillStyleType.Equals(FillStyleType.LinearGradientFill) || this._fillStyleType.Equals(FillStyleType.RadialGradientFill))
            {
                this._gradientMatrix = new Matrix(this._SwfVersion);

                try
                {
                    this._gradientMatrix.Parse(input);
                }
                catch (SwfFormatException e)
                {
                    throw e;
                }
                

                this._gradient = new Gradient(this._SwfVersion);

                try
                {
                    this._gradient.Parse(input, caller);
                    //Log.InfoFormat("Valid fill style type. FillstyleType: 0x{0:X2} at address: 0x{1:X4} in {2}", (Int32)this._fillStyleType, ((Int32)input.Position) - 1, caller);
                }
                catch (SwfFormatException e)
                {
                    throw e;
                }
                
            }
            else if (this._fillStyleType.Equals(FillStyleType.FocalRadialGradientFill))
            {
                if (this._SwfVersion >= 8)
                {
                    this._gradientMatrix = new Matrix(this._SwfVersion);

                    try
                    {
                        this._gradientMatrix.Parse(input);
                        
                    }
                    catch(SwfFormatException e)
                    {
                        throw e;
                    }

                    this._gradient = new FocalGradient(this._SwfVersion);

                    try
                    {
                        this._gradient.Parse(input, caller);
                        //Log.InfoFormat("Valid fill style type. FillstyleType: 0x{0:X2} at address: 0x{1:X4} in {2}", (Int32)this._fillStyleType, ((Int32)input.Position) - 1, caller);
                    }
                    catch (SwfFormatException e)
                    {
                        throw e;
                    }
                    
                }
                else
                {
                    SwfFormatException e = new SwfFormatException("Focal gradients are supported by Swf 8 and later only. This version is: " + this._SwfVersion.ToString());
                   Log.Error(this, e);
                }
            }
            else if (this._fillStyleType.Equals(FillStyleType.RepeatingBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.ClippedBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedRepeatingBitmap) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedClippedBitmap))
            {
                this._bitmapID = br.ReadUInt16();
                this._bitmapMatrix = new Matrix(this._SwfVersion);

                try
                {
                    this._bitmapMatrix.Parse(input);
                    //Log.InfoFormat("Valid fill style type. FillstyleType: 0x{0:X2} at address: 0x{1:X4} in {2}", (Int32)this._fillStyleType, ((Int32)input.Position) - 1, caller);
                }
                catch (SwfFormatException e)
                {
                    throw e;
                }
                
            }
            else
            {
                SwfFormatException e = new SwfFormatException("Invalid fill style type! (" + this._fillStyleType +")" +" caller: " + caller.ToString());
                Log.Error(this, e);
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            output.WriteByte((byte)this._fillStyleType);

            if (this._fillStyleType.Equals(FillStyleType.SolidFill))
            {
                this._color.Write(output);
            }
            else if (this._fillStyleType.Equals(FillStyleType.LinearGradientFill) ||
                      this._fillStyleType.Equals(FillStyleType.RadialGradientFill) ||
                      this._fillStyleType.Equals(FillStyleType.FocalRadialGradientFill))
            {
                this._gradientMatrix.Write(output);
                this._gradient.Write(output);
            }
            else if (this._fillStyleType.Equals(FillStyleType.RepeatingBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.ClippedBitmapFill) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedRepeatingBitmap) ||
                      this._fillStyleType.Equals(FillStyleType.NonSmoothedClippedBitmap))
            {
                bw.Write(this._bitmapID);
                this._bitmapMatrix.Write(output);
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (this._fillStyleType == FillStyleType.SolidFill)
            {
                sb.AppendFormat(" Solid Fill. RGB : {0}", this._color);
            }
            if (this._fillStyleType == FillStyleType.LinearGradientFill )
            {
                sb.AppendFormat("Linear Gradient Fill : {0}", this._gradient);
            }
            if (this._fillStyleType == FillStyleType.RadialGradientFill)
            {
                sb.AppendFormat("Radial Gradient Fill : {0}", this._gradient);
            }
            if(this._fillStyleType == FillStyleType.FocalRadialGradientFill)
            {
                sb.AppendFormat("Focal Radial Gradient Fill: {0}", this._gradient);
                sb.AppendFormat("Gradient Matrix :  {0}", this._gradientMatrix);
            }
            if (this._fillStyleType.Equals(FillStyleType.RepeatingBitmapFill) ||
                 this._fillStyleType.Equals(FillStyleType.ClippedBitmapFill) ||
                 this._fillStyleType.Equals(FillStyleType.NonSmoothedRepeatingBitmap) ||
                 this._fillStyleType.Equals(FillStyleType.NonSmoothedClippedBitmap))
            {
                sb.AppendFormat(" Bitmap ID : {0:d} ", this._bitmapID);
            }

            return sb.ToString();
        }

    }
}