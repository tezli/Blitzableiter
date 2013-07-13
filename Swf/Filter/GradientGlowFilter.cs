using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class GradientGlowFilter : AbstractFilter
    {
        internal FilterTypes _FilterType = FilterTypes.GradientGlowFilter;

        internal byte _numColors;
        internal List<Rgba> _GradientColors;
        internal List<byte> _GradientRatio;
        internal UInt32 _BlurX;
        internal UInt32 _BlurY;
        internal UInt32 _Angle;
        internal UInt32 _Distance;
        internal UInt16 _Strength;
        internal bool _InnerShadow;
        internal bool _KnockOut;
        internal bool _CompositeSource;
        internal bool _OnTop;
        internal byte _Passes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public GradientGlowFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );

            _numColors = br.ReadByte();

            _GradientColors = new List<Rgba>();
            for ( int i = 0; i < _numColors; i++ )
            {
                Rgba entry = new Rgba( this.Version );
                entry.Parse( input );
                _GradientColors.Add( entry );
            }
            _GradientRatio = new List<byte>();
            for ( int i = 0; i < _numColors; i++ )
            {
                byte entry2 = br.ReadByte();
                _GradientRatio.Add( entry2 );
            }

            _BlurX = br.ReadUInt32();
            _BlurY = br.ReadUInt32();
            _Angle = br.ReadUInt32();
            _Distance = br.ReadUInt32();
            _Strength = br.ReadUInt16();

            BitStream bits = new BitStream( input );
            _InnerShadow = ( 0 != bits.GetBits( 1 ) );
            _KnockOut = ( 0 != bits.GetBits( 1 ) );
            _CompositeSource = ( 0 != bits.GetBits( 1 ) );
            if ( !_CompositeSource )
            {
                throw new SwfFormatException( "GradientGlowFilter with CompositeSource false" );
            }
            _OnTop = ( 0 != bits.GetBits( 1 ) );
            _Passes = ( byte )bits.GetBits( 4 );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return (uint) (
                    sizeof( byte ) + // numColors
                    ( _GradientColors.Count * (new Rgba( this.Version )).Length ) +
                    ( _GradientRatio.Count * sizeof( byte ) ) +
                    ( 4 * sizeof( UInt32 ) ) +
                    sizeof( UInt16 ) +
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

            bw.Write( ( byte )_GradientColors.Count );
            for ( int i = 0; i < _GradientColors.Count; i++ )
            {
                _GradientColors[ i ].Write( output );
            }
            for ( int i = 0; i < _GradientRatio.Count; i++ )
            {
                bw.Write( _GradientRatio[ i ] );
            }
            bw.Write( _BlurX );
            bw.Write( _BlurY );
            bw.Write( _Angle );
            bw.Write( _Distance );
            bw.Write( _Strength );
            BitStream bits = new BitStream( output );
            bits.WriteBits( 1, ( _InnerShadow ? 1 : 0 ) );
            bits.WriteBits( 1, ( _KnockOut ? 1 : 0 ) );
            bits.WriteBits( 1, ( _CompositeSource ? 1 : 0 ) );
            bits.WriteBits( 1, ( _OnTop ? 1 : 0 ) );
            bits.WriteBits( 4, _Passes );
            bits.WriteFlush();
        }
    }
}
