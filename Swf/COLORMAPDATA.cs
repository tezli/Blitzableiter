using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Interfaces;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The COLORMAPDATA structure contains image data. This</para>
    /// <para>structure is compressed as a single block of data.</para>
    /// <para>Row widths in the pixel data fields of these structures</para>
    /// <para>must be rounded up to the next 32-bit word boundary. </para>
    /// <para>For example, an 8-bit image that is 253 pixels wide must be</para>
    /// <para>padded out to 256 bytes per line. To determine the appropriate </para>
    /// <para>padding, make sure to take into account the actual size of</para>
    /// <para>the individual pixel structures; 15-bit pixels occupy 2</para>
    /// <para>bytes and 24-bit pixels occupy 4 bytes (see PIX15 and PIX24).</para>
    /// </summary>
    public class ColorMapData : AbstractSwfElement
    {
        
        private List<Rgb> _colorTableRGB;
        private byte[] _colormapPixelData;

        /// <summary>
        /// <para>The COLORMAPDATA structure contains image data. This</para>
        /// <para>structure is compressed as a single block of data.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public ColorMapData( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._colorTableRGB = new List<Rgb>();
            this._colormapPixelData = new byte[ 0 ];
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public virtual ulong  Length 
        { 
            get
            {
                return ( UInt64 )( this._colorTableRGB.Count + this._colormapPixelData.Length );
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true; // nothing to verify here
        }

        // TODO: sometimes index out of bounds exception occurs here. Reheck
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="colorTableSize"></param>
        /// <param name="imageDataSize"></param>
        public virtual void Parse( Stream input, byte colorTableSize, UInt32 imageDataSize )
        {
           //Log.Debug(this, "Image data size is:" + imageDataSize + ". Color table size is: " + colorTableSize  );
            Rgb temp = null;

            for ( byte i = 0; i <= colorTableSize; i++ ) // because we need BitmapColorTableSize + 1 elements
            {
                temp = new Rgb( this._SwfVersion );
                temp.Parse( input );
                _colorTableRGB.Add( temp );
            }

            try
            {
                this._colormapPixelData = new byte[(Int32)imageDataSize];
                input.Read(this._colormapPixelData, 0, (Int32)imageDataSize);
            }
            catch (IndexOutOfRangeException e)
            {
               Log.Error(this, e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Writes this object to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public virtual void Write( Stream output )
        {
            for ( int i = 0; i < this._colorTableRGB.Count; i++ )
            {
                _colorTableRGB[ i ].Write( output );
            }
            output.Write( this._colormapPixelData, 0, this._colormapPixelData.Length );
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            return sb.ToString();
        }


    }
}
