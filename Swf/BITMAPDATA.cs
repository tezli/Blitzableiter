using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Interfaces;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The BITMAPDATA structure contains image data. This</para>
    /// <para>structure is compressed as a single block of data.</para>
    /// </summary>
    public class BitmapData : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<Rgb> _bitmapPixelData;

        /// <summary>
        /// The BITMAPDATA structure contains image data. This
        /// structure is compressed as a single block of data.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public BitmapData(byte InitialVersion) : base(InitialVersion)
        {
            this._bitmapPixelData = new List<Rgb>();
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public virtual ulong Length
        {
            get
            {
                if (this._bitmapPixelData.GetType().Equals(typeof(Pix15)))
                {
                    return (UInt64)(this._bitmapPixelData.Count * 2);
                }
                else if (this._bitmapPixelData.GetType().Equals(typeof(Pix24)))
                {
                    return (UInt64)(this._bitmapPixelData.Count * 4);
                }
                else
                {
                    return 0;
                }

            }

        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true; // nothing to do here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="imageDataSize"></param>
        /// <param name="bitmapFormat"></param>
        public virtual void Parse(Stream input, UInt64 imageDataSize, byte bitmapFormat)
        {
            if (bitmapFormat.Equals(0x04))
            {
                Pix15 temp = null;

                for (UInt64 i = 0; i < imageDataSize; i++)
                {
                    temp = new Pix15(this._SwfVersion);
                    temp.Parse(input);
                    this._bitmapPixelData.Add(temp);
                }

            }
            else if (bitmapFormat.Equals(0x05))
            {
                for (UInt64 i = 0; i < imageDataSize; i++)
                {
                    Pix24 temp = new Pix24(this._SwfVersion);
                    temp.Parse(input);
                    this._bitmapPixelData.Add(temp);
                }
            }
            else
            {
                SwfFormatException e = new SwfFormatException("BITMAPDATA can not contain any other bitmap formats than 15-bit RGB images or 24-bit RGB images ");
               Log.Error(this, e.Message);
                throw e;
            }

        }

        /// <summary>
        /// Writes this object to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public virtual void Write(Stream output)
        {
            for (int i = 0; i < this._bitmapPixelData.Count; i++)
            {
                _bitmapPixelData[i].Write(output);
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ToString());
            return sb.ToString();

        }


    }
}
