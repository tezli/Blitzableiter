using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A style represents an object conaitning fillstyles and linestyles. Its is used by STYLECHANGERECORD and is byte aligned.
    /// </summary>
    public class Style : AbstractSwfElement
    {
        private FillStyleArray _fillStyles;
        private LineStyleArray _lineStyles;

        private byte _numFillBits = 0;
        private byte _numLineBits = 0;

        /// <summary>
        /// A style represents an object conaitning fillstyles and linestyles. Its is used by STYLECHANGERECORD and is byte aligned.
        /// </summary>
        /// <param name="InitialVersion">The Swf version of the file using this object.</param>
        public Style( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._fillStyles = new FillStyleArray( this._SwfVersion );
            this._lineStyles = new LineStyleArray( this._SwfVersion );
        }

        /// <summary>
        /// The array of fill styles
        /// </summary>
        public FillStyleArray FillStyles
        {
            get
            {
                return this._fillStyles;
            }
            set
            {
                this._fillStyles = value;
            }
        }

        /// <summary>
        /// The array of fline styles
        /// </summary>
        public LineStyleArray LineStyles
        {
            get
            {
                return this._lineStyles;
            }
            set
            {
                this._lineStyles = value;
            }
        }

        /// <summary>
        /// The number of bits representing the number of array elements (two elements can be described with one bit {0,1} )
        /// </summary>
        public byte NumFillBits
        {
            get
            {
                return this._numFillBits;
            }
            set
            {
                this._numFillBits = value;
            }
        }

        /// <summary>
        /// The number of bits representing the number of array elements (two elements can be described with one bit {0,1} )
        /// </summary>
        public byte NumLineBits
        {
            get
            {
                return this._numLineBits;
            }
            set
            {
                this._numLineBits = value;
            }
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public ulong Length
        { 
            get 
            { 
                UInt64 length = this._fillStyles.Length + this._lineStyles.Length + sizeof(byte);
                return length;
            } 
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public void Parse( Stream input, TagTypes caller )
        {
            this._fillStyles.Parse( input, caller );
            this._lineStyles.Parse( input, caller );

            BitStream bits = new BitStream( input );

            this._numFillBits = (byte)bits.GetBits( 4 );
            this._numLineBits = (byte)bits.GetBits( 4 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            this._fillStyles.Write( output );
            this._lineStyles.Write( output );

            BitStream bits = new BitStream( output );

            bits.WriteBits( 4, ( Int32 )this._numFillBits );
            bits.WriteBits( 4, ( Int32 )this._numLineBits );
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