using System;
using System.IO;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class CxFormWithAlpha : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool _HasAddTerms;

        /// <summary>
        /// 
        /// </summary>
        protected bool _HasMultTerms;

        /// <summary>
        /// 
        /// </summary>
        protected byte _numBits;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _RedMultTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _GreenMultTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _BlueMultTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _AlphaMultTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _RedAddTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _GreenAddTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _BlueAddTerm;

        /// <summary>
        /// 
        /// </summary>
        protected Int32 _AlphaAddTerm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public CxFormWithAlpha( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse( Stream input )
        {
            BitStream bits = new BitStream( input );

            _HasAddTerms = ( 1 == bits.GetBits( 1 ) );
            _HasMultTerms = ( 1 == bits.GetBits( 1 ) );
            _numBits = (byte)bits.GetBits( 4 );

            if ( _HasMultTerms )
            {
                _RedMultTerm = bits.GetBitsSigned( _numBits );
                _GreenMultTerm = bits.GetBitsSigned( _numBits );
                _BlueMultTerm = bits.GetBitsSigned( _numBits );
                _AlphaMultTerm = bits.GetBitsSigned( _numBits );
            }

            if ( _HasAddTerms )
            {
                _RedAddTerm = bits.GetBitsSigned( _numBits );
                _GreenAddTerm = bits.GetBitsSigned( _numBits );
                _BlueAddTerm = bits.GetBitsSigned( _numBits );
                _AlphaAddTerm = bits.GetBitsSigned( _numBits );
            }

            if ( ( !_HasAddTerms ) && ( !_HasMultTerms ) )
            {
                //
                // When none of the two flags are set, get the remaining bits
                // so that the entire method here works byte aligned
                //
                bits.GetBitsSigned( 2 );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint res = 0;

                using ( MemoryStream temp = new MemoryStream() )
                {
                    this.Write( temp );
                    res = ( uint )temp.Length;
                }

                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int BitCount
        {
            get
            {
                BitStream bits = new BitStream( null );
                
                int mBits = 0;
                int aBits = 0;

                if ( _HasMultTerms )
                {
                    mBits = bits.CountMaximumBits( _RedMultTerm, _GreenMultTerm, _BlueMultTerm, _AlphaMultTerm );
                }
                if ( _HasAddTerms )
                {
                    aBits = bits.CountMaximumBits( _RedAddTerm, _GreenAddTerm, _BlueAddTerm, _AlphaAddTerm );
                }

                return ( aBits > mBits ? aBits : mBits );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            BitStream bits = new BitStream( output );
            
            int maxBits = this.BitCount;

            if ( 0 == maxBits )
            {
                // 
                // special case: must be byte-aligned by hand
                //
                bits.WriteBits( 2, 0 ); // has no Terms whatsoever
                bits.WriteBits( 4, 1 ); // 1 Bit per nothing
                bits.WriteBits( 2, 0 ); // nothing
            }
            else
            {
                bits.WriteBits( 1, ( _HasAddTerms ? 1 : 0 ) );
                bits.WriteBits( 1, ( _HasMultTerms ? 1 : 0 ) );
                bits.WriteBits( 4, maxBits );

                if ( _HasMultTerms )
                {
                    bits.WriteBits( maxBits, _RedMultTerm );
                    bits.WriteBits( maxBits, _GreenMultTerm );
                    bits.WriteBits( maxBits, _BlueMultTerm );
                    bits.WriteBits( maxBits, _AlphaMultTerm );
                }
                if ( _HasAddTerms )
                {
                    bits.WriteBits( maxBits, _RedAddTerm );
                    bits.WriteBits( maxBits, _GreenAddTerm );
                    bits.WriteBits( maxBits, _BlueAddTerm );
                    bits.WriteBits( maxBits, _AlphaAddTerm );
                }
            }
            bits.WriteFlush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "{0:s} ({1:d} Bits/entry) ", base.ToString(), _numBits );
            if ( _HasAddTerms )
            {
                sb.AppendFormat( "ADD( R {0:d}/ G {1:d} / B {2:d} / A {3:d} ) ",
                    _RedAddTerm, _GreenAddTerm, _BlueAddTerm, _AlphaAddTerm );
            }
            if ( _HasMultTerms )
            {
                sb.AppendFormat( "MULT( R {0:d}/ G {1:d} / B {2:d} / A {3:d} ) ",
                    _RedMultTerm, _GreenMultTerm, _BlueMultTerm, _AlphaMultTerm );
            }

            return sb.ToString();
        }
    }
}
