using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Interfaces;

namespace Recurity.Swf
{
    /// <summary>
    /// A line style represents a width and color of a line. A line style is byte aligned.
    /// </summary>
    public class LineStyle : AbstractSwfElement
    {   
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _width;

        /// <summary>
        /// 
        /// </summary>
        protected Rgb _color;

        /// <summary>
        /// A line style represents a width and color of a line. 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this objct</param>
        public LineStyle(byte InitialVersion) : base(InitialVersion)
        {
            this._width = 0;
            this._color = new Rgb(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public virtual ulong Length
        {
            get
            {
                ulong length = sizeof(UInt16) + this._color.Length;
                return length * 8;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true; // nothing to verify here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public virtual void Parse(Stream input, TagTypes caller)
        {
            BinaryReader br = new BinaryReader(input);

            this._width = br.ReadUInt16();

            if (caller.Equals(TagTypes.DefineShape) || caller.Equals(TagTypes.DefineShape2))
            {
                this._color = new Rgb(this._SwfVersion);
                
                try
                {
                    this._color.Parse(input);
                }
                catch(SwfFormatException e)
                {
                    throw e;
                }
                
            }
            else if (caller.Equals(TagTypes.DefineShape3))
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
                SwfFormatException e = new SwfFormatException("LineStyle was called by illegal TagType (" + caller.ToString() +").");
               Log.Error(this, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public virtual void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this._width);
            this._color.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ToString());
            sb.AppendFormat(" width: {0:d} ", this._width);
            sb.Append(this._color.ToString());
            return sb.ToString();
        }

    }

}
