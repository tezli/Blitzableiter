using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Interfaces;

namespace Recurity.Swf
{
    /// <summary>
    /// PIX15 is a RGB representation with 5 bits per channel
    /// </summary>
    public class Pix15 : Rgb
    {
        /// <summary>
        /// PIX15 is a RGB representation with 5 bits per channel
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Pix15( byte InitialVersion ) : base( InitialVersion )
        {

        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public override uint Length
        {
            get 
            {
                return 2;
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
            BitStream bits = new BitStream(input);

            bits.GetBits( 1 ); // reserved
            this._Red   = (Byte)bits.GetBits( 5 );
            this._Green = (Byte)bits.GetBits( 5 );
            this._Blue  = (Byte)bits.GetBits( 5 );
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            bits.WriteBits(1, 0); //reserved
            bits.WriteBits(5, this._Red);
            bits.WriteBits(5, this._Green);
            bits.WriteBits(5, this._Blue);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( this.ToString() );
            sb.AppendFormat(" Red:{0:d} Green:{1:d} Blue{2:d} ", this._Red, this,_Green, this._Blue);
            return sb.ToString();
           
        }


    }
}
