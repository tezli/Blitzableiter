using System;
using System.IO;
using System.Collections.Generic;
using Recurity.Swf.Interfaces;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>Swf 8 and later supports up to 15 gradient control points, spread modes and a new</para>
    /// <para>interpolation type.</para>
    /// <para>Note that for the DefineShape, DefineShape2 or DefineShape3 tags, the SpreadMode and</para>
    /// <para>InterpolationMode fields must be 0, and the NumGradients field can not exceed 8.</para>
    /// <para>Source: Adobe[swf_file_format_spec_v10.pdf]</para>
    /// </summary>
    public class Gradient : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected SpreadMode _spreadMode;

        /// <summary>
        /// 
        /// </summary>
        protected InterPolation _interpolationMode;

        /// <summary>
        /// 
        /// </summary>
        protected byte _numGradients;

        /// <summary>
        /// 
        /// </summary>
        protected List<GradRecord> _gradientRecords;

        /// <summary>
        /// <para>Swf 8 and later supports up to 15 gradient control points, spread modes and a new</para>
        /// <para>interpolation type.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Gradient( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._spreadMode = SpreadMode.PadMode;
            this._interpolationMode = InterPolation.Linear;
            this._numGradients = 0;
            this._gradientRecords = new List<GradRecord>();
        }
        
        /// <summary>
        /// The length of this object
        /// </summary>
        public virtual ulong Length 
        {
            get
            {
                UInt64 length = sizeof( byte );
                length += ( UInt64 )this._gradientRecords.Count;
                return length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
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
        public virtual void Parse( Stream input, TagTypes caller )
        {
            BitStream bits = new BitStream( input );
            GradRecord temp = new GradRecord(this._SwfVersion);

            this._spreadMode = ( SpreadMode )bits.GetBits( 2 );
            this._interpolationMode = ( InterPolation )bits.GetBits( 2 );
            this._numGradients = ( byte )bits.GetBits( 4 );

            bits.Reset();
            
            for ( int i = 0; i < this._numGradients; i++ )
            {
                temp = new GradRecord( this._SwfVersion );
                temp.Parse( input, caller );
                this._gradientRecords.Add( temp );
            }
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to</param>
        public virtual void Write( Stream output )
        {
            BitStream bits = new BitStream( output );

            bits.WriteBits( 2, ( Int32 )this._spreadMode );
            bits.WriteBits( 2, ( Int32 )this._interpolationMode );
            bits.WriteBits( 4, ( Int32 )this._numGradients );

            bits.WriteFlush();

            for(int i = 0; i < this._numGradients; i++)
            {
                this._gradientRecords[i].Write( output );
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SpreadMode: {0}, Interpolation Mode: {1}, Number Of Gradients{2}", this._spreadMode, this._interpolationMode, (int)this._numGradients);
            return sb.ToString();
        }
    }
}
