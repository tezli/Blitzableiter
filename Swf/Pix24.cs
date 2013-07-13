using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// PIX24 is a RGB with a leading 0 byte
    /// </summary>
    public class Pix24 : Rgb
    {
        /// <summary>
        /// PIX24 is a RGB with a leading 0 byte
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Pix24( byte InitialVersion ) : base( InitialVersion ){}

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
        {
            get 
            {
                return 4;
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
            BinaryReader br = new BinaryReader(input);
       
            byte reserved = br.ReadByte();
            this._Red = br.ReadByte();
            this._Green = br.ReadByte();
            this._Blue = br.ReadByte();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            BinaryWriter bw = new BinaryWriter(output);
            
            bw.Write(0); // reserved
            bw.Write(this._Red);
            bw.Write(this._Green);
            bw.Write(this._Blue);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( this.ToString() );
            sb.AppendFormat( " Red:{0:d} Green:{1:d} Blue{2:d} ", this._Red, this, _Green, this._Blue );
            return sb.ToString();
        }


    }
}
