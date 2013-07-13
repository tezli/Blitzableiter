using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class Matrix : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _HasScale;
        private byte _numScaleBits;
        private double _xScaleF;
        private double _yScaleF;

        private bool _HasRotate;
        private byte _numRotateBits;       
        private double _rotateSkew0F;
        private double _rotateSkew1F;

        private byte _numTranslateBits;
        private Int32 _translateX;
        private Int32 _translateY;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Matrix( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public uint Length
        {
            get
            {
                uint ret = 0;
                using ( MemoryStream temp = new MemoryStream() )
                {
                    this.Write( temp );
                    ret = (uint)temp.Position;
                }
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse( Stream input )
        {
            BitStream bits = new BitStream( input );

            if ( _HasScale = ( 1 == bits.GetBits( 1 ) ) )
            {
                _numScaleBits = (byte)bits.GetBits( 5 );
                bits.GetBitsFB( _numScaleBits, out _xScaleF );
                bits.GetBitsFB( _numScaleBits, out _yScaleF );
            }
            if ( _HasRotate = ( 1 == bits.GetBits( 1 ) ) )
            {
                _numRotateBits = (byte)bits.GetBits( 5 );
                bits.GetBitsFB( _numRotateBits, out _rotateSkew0F );
                bits.GetBitsFB( _numRotateBits, out _rotateSkew1F );
            }
            _numTranslateBits = ( byte )bits.GetBits( 5 );
            _translateX = bits.GetBitsSigned( _numTranslateBits );
            _translateY = bits.GetBitsSigned( _numTranslateBits );


     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            BitStream bits = new BitStream( output );                                   

            if ( _HasScale )
            {
                int scaleBitsX = bits.CountNeededBitsFB( _xScaleF );
                int scaleBitsY = bits.CountNeededBitsFB( _yScaleF );
                int scaleBits = scaleBitsX > scaleBitsY ? scaleBitsX : scaleBitsY;

                bits.WriteBits( 1, 1 ); // HasScale
                bits.WriteBits( 5, scaleBits );
                bits.WriteBitsFB( scaleBits, _xScaleF );
                bits.WriteBitsFB( scaleBits, _yScaleF );

            }
            else
            {
                bits.WriteBits( 1, 0 ); // Has no Scale
            }

            if ( _HasRotate )
            {
                int rotateBits0 = bits.CountNeededBitsFB( _rotateSkew0F );
                int rotateBits1 = bits.CountNeededBitsFB( _rotateSkew1F );
                int rotateBits = rotateBits0 > rotateBits1 ? rotateBits0 : rotateBits1;

                bits.WriteBits( 1, 1 ); // HasRotate
                bits.WriteBits( 5, rotateBits );
                bits.WriteBitsFB( rotateBits, _rotateSkew0F );
                bits.WriteBitsFB( rotateBits, _rotateSkew1F );
            }
            else
            {
                bits.WriteBits( 1, 0 ); // has no Rotate
            }

            int translateBits = bits.CountMaximumBits( _translateX, _translateY );

            if (this._SwfVersion > 8)
            {
                bits.WriteBits(5, this._numTranslateBits);
                bits.WriteBits(this._numTranslateBits, _translateX);
                bits.WriteBits(this._numTranslateBits, _translateY);
            }
            else
            {
                bits.WriteBits(5, translateBits);
                bits.WriteBits(translateBits, _translateX);
                bits.WriteBits(translateBits, _translateY);
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

            sb.Append( base.ToString() );
            if ( _HasScale )
            {
                sb.AppendFormat( " Scale ({0:d} Bits/entry) X:{1:G},Y:{2:G}",
                    _numScaleBits, _xScaleF, _yScaleF );
            }
            if ( _HasRotate )
            {
                sb.AppendFormat( " Rotate ({0:d} Bits/entry) Skew0:{1:G},Skew1:{2:G}",
                    _numScaleBits, _rotateSkew0F, _rotateSkew1F );
            }
            sb.AppendFormat( " Translate ({0:d} Bits/entry) X:{1:d},Y:{2:d}",
                _numTranslateBits, _translateX, _translateY );
            
            return sb.ToString();
        }

        internal void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
