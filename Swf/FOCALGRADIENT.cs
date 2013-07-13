using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>A FOCALGRADIENT must be declared in DefineShape4—not DefineShape, DefineShape2</para>
    /// <para>or DefineShape3.</para>
    /// <para>The value range is from -1.0 to 1.0, where -1.0 means the focal point is close to the left border</para>
    /// <para>of the radial gradient circle, 0.0 means that the focal point is in the center of the radial</para>
    /// <para>gradient circle, and 1.0 means that the focal point is close to the right border of the radial</para>
    /// <para>gradient circle.</para>
    /// </summary>
    public class FocalGradient : Gradient
    {
        private double _focalPoint;

        /// <summary>
        /// <para>A FOCALGRADIENT must be declared in DefineShape4—not DefineShape, DefineShape2</para>
        /// <para>or DefineShape3.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public FocalGradient( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._spreadMode = SpreadMode.PadMode;
            this._interpolationMode = InterPolation.Linear;
            this._gradientRecords = new List<GradRecord>();
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                uint ret = 0;
                using ( MemoryStream temp = new MemoryStream() )
                {
                    this.Write( temp );
                    ret = ( uint )temp.Position;
                }
                return ret;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            if ( ( byte )this._interpolationMode > 0x01 || ( ( byte )this._numGradients > 0x0F || ( byte )this._numGradients < 0x01 ) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public override void Parse( Stream input, TagTypes caller )
        {
            BitStream bits = new BitStream( input );

            this._spreadMode = ( SpreadMode )bits.GetBits( 2 );
            this._interpolationMode = ( InterPolation )bits.GetBits( 2 );
            this._numGradients = ( byte )bits.GetBits( 4 );

            bits.Reset();
            
            GradRecord temp = new GradRecord(this._SwfVersion);

            for ( int i = 0; i < _numGradients; i++ )
            {
                temp = new GradRecord( this._SwfVersion );
                temp.Parse( input, caller );
                this._gradientRecords.Add(temp);
            }

            bits.GetBitsFB( 16, out this._focalPoint );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            bits.WriteBits(2, (Int32)this._spreadMode);
            bits.WriteBits(2, (Int32)this._interpolationMode);
            bits.WriteBits(4, (Int32)this._numGradients);

            bits.WriteFlush();

            for (int i = 0; i < this._numGradients; i++)
            {
                this._gradientRecords[i].Write(output);
            }
            bits.WriteBitsFB(16, this._focalPoint);
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

