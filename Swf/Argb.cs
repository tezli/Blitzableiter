using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// The ARGB record behaves exactly like the RGBA record, but the alpha value for the ARGB
    /// record is in the first byte.
    /// </summary>
    public class Argb : Rgba
    {
        /// <summary>
        /// The ARGB record behaves exactly like the RGBA record, but the alpha value for the ARGB
        /// record is in the first byte.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public Argb( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
        {
            get 
            {
                return base.Length;
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
            BinaryReader br = new BinaryReader( input );
            this._Alpha = br.ReadByte();
            this._Red = br.ReadByte();
            this._Green = br.ReadByte();
            this._Blue = br.ReadByte();
            //String s = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", _Alpha, _Red, _Green, _Blue);
            //Log.Debug(this, "[Begin Structure] ARGB : color=" + s);
            //Log.Debug(this, "[End Structure] ARGB");
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write( Stream output )
        {
            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( _Alpha );
            bw.Write( _Red );
            bw.Write( _Green );
            bw.Write( _Blue );
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( " A:{0:d}", _Alpha );
            sb.Append( base.ToString() );
            return sb.ToString();
        }
       
    }
}
