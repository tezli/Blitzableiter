using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The ALPHACOLORMAPDATA extends COLORMAPDATA by using RGBA </para>
    /// <para>insteadf of RGB for the  image data.</para>
    /// </summary>
    public class AlphaColorMapData : ColorMapData
    {
        private List<Rgba> _colorTableRGB;
        private byte[] _colormapPixelData;

        /// <summary>
        /// <para>The ALPHACOLORMAPDATA extends COLORMAPDATA by using RGBA </para>
        /// <para>insteadf of RGB for the  image data.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public AlphaColorMapData( byte InitialVersion ) : base( InitialVersion )
        {
            this._colorTableRGB = new List<Rgba>();
            this._colormapPixelData = new byte[ 0 ];
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
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
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="colorTableSize"></param>
        /// <param name="imageDataSize"></param>
        public override void Parse( Stream input, byte colorTableSize, UInt32 imageDataSize )
        {
            Rgba temp = null;

            for (int i = 0; i <= colorTableSize; i++) // BitmapColorTableSize + 1.
            {
                temp = new Rgba( this._SwfVersion );
                temp.Parse( input );
                this._colorTableRGB.Add(temp);
            }

            this._colormapPixelData = new byte[(Int32)imageDataSize];
            int read = input.Read( this._colormapPixelData, 0, (Int32)imageDataSize );
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write( Stream output )
        {
            for (int i = 0; i < this._colorTableRGB.Count; i++)
            {
                this._colorTableRGB[i].Write(output);
            }

            output.Write(this._colormapPixelData, 0, this._colormapPixelData.Length);
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
