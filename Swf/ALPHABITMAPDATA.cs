using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>A list of ARGB records. ARGB is the same as RGBA but the first byte is the alpha channel</para>
    /// </summary>
    public class AlphaBitmapData : BitmapData
    {
        /// <summary>
        /// A list of ARGB records. ARGB is the same as RGBA but the first byte is the alpha channel
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public AlphaBitmapData( byte InitialVersion ) : base( InitialVersion )
        {
            this._bitmapPixelData = new List<Rgb>();
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (UInt64)(this._bitmapPixelData.Count * 4);
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
        /// <param name="imageDataSize">The size of the image Data</param>
        public void Parse( Stream input, Int32 imageDataSize )
        {
            Argb temp = null;

            this._bitmapPixelData = new List<Rgb>();

            for ( int i = 0; i < imageDataSize; i++ )
            {
                temp = new Argb(this._SwfVersion);
                temp.Parse( input );
                this._bitmapPixelData.Add( temp );
            }
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write( Stream output )
        {
            for ( int i = 0; i < this._bitmapPixelData.Count; i++ )
            {
                this._bitmapPixelData[ i ].Write( output );
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.ToString() );
            return sb.ToString();
        }


    }
}
