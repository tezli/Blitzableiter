using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The RGBA record represents a color as 32-bit red, green, blue and alpha value. An RGBA</para>
    /// <para>color with an alpha value of 255 is completely opaque. An RGBA color with an alpha value of</para>
    /// <para>zero is completely transparent. Alpha values between zero and 255 are partially transparent.</para>
    /// </summary>
    public class Rgba : Rgb
    {
        /// <summary>
        /// 
        /// </summary>
        protected byte _Alpha;

        /// <summary>
        /// <para>The RGBA record represents a color as 32-bit red, green, blue and alpha value.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the swf file using this object</param>
        public Rgba( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
        {
            get
            {
                return ( sizeof( byte ) * 4 );
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
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The input stream.</param>
        public override void Parse( Stream input )
        {
            base.Parse( input );
            BinaryReader br = new BinaryReader( input );
            _Alpha = br.ReadByte();
            //String s = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", _Red, _Green, _Blue, _Alpha);
            //Log.Debug(this, "[Begin Structure] RGBA : color=" + s);
            //Log.Debug(this, "[End Structure] RGBA");

        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write( Stream output )
        {
            base.Write( output );
            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( _Alpha );
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendFormat( " A:{0:d}", _Alpha );
            return sb.ToString();
        }
    }
}
