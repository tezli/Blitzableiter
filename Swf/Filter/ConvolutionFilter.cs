using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvolutionFilter : AbstractFilter
    {
        internal const FilterTypes _FilterType = FilterTypes.ConvolutionFilter;

        internal byte _MatrixX;
        internal byte _MatrixY;
        internal UInt32 _DivisorFLOAT;
        internal UInt32 _BiasFLOAT;
        internal List<UInt32> _MatrixValues;
        internal Rgba _DefaultColor;

        /// <summary>
        /// 
        /// </summary>
        internal bool _Clamp;
        internal bool _PreserveAlpha;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public ConvolutionFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );

            _MatrixX = br.ReadByte();
            _MatrixY = br.ReadByte();
            _DivisorFLOAT = br.ReadUInt32();
            _BiasFLOAT = br.ReadUInt32();
            _MatrixValues = new List<uint>();
            for ( int i = 0; i < ( _MatrixX * _MatrixY ); i++ )
            {
                UInt32 a = br.ReadUInt32();
                _MatrixValues.Add( a );
            }
            _DefaultColor = new Rgba( this.Version );
            _DefaultColor.Parse( input );
            BitStream bits = new BitStream( input );

            uint reserved = bits.GetBits( 6 );
            if ( 0 != reserved )
            {
                throw new SwfFormatException( "ConvolutionFilter uses reserved bits" );
            }

            _Clamp = ( 0 != bits.GetBits( 1 ) );
            _PreserveAlpha = ( 0 != bits.GetBits( 1 ) );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return (uint) (
                    ( 2 * sizeof( byte ) ) +
                    ( 2 * sizeof( UInt32 ) ) +
                    ( _MatrixValues.Count * sizeof( UInt32 ) ) +
                    _DefaultColor.Length +
                    sizeof( byte )
                    + sizeof( byte ) // FilterType !
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( ( byte )_FilterType );

            bw.Write( _MatrixX );
            bw.Write( _MatrixY );
            bw.Write( _DivisorFLOAT );
            bw.Write( _BiasFLOAT );

            if ( _MatrixValues.Count != ( _MatrixX * _MatrixY ) )
            {
                IndexOutOfRangeException e = new IndexOutOfRangeException( "_MatrixValues does not have " + (_MatrixX*_MatrixY).ToString("d") + " entries, does have " + _MatrixValues.Count.ToString( "d" ) );
                throw e;
            }

            for ( int i = 0; i < _MatrixValues.Count; i++ )
            {
                bw.Write( _MatrixValues[ i ] );
            }
            
            _DefaultColor.Write( output );
            BitStream bits = new BitStream( output );
            bits.WriteBits( 6, 0 );
            bits.WriteBits( 1, ( _Clamp ? 1 : 0 ) );
            bits.WriteBits( 1, ( _PreserveAlpha ? 1 : 0 ) );
            bits.WriteFlush();
        }
    }
}
