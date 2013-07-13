using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class BevelFilter : AbstractFilter
    {
        /// <summary>
        /// 
        /// </summary>
        internal const FilterTypes _FilterType = FilterTypes.BevelFilter;

        /// <summary>
        /// 
        /// </summary>
        internal Rgba _ShadowColor;

        /// <summary>
        /// 
        /// </summary>
        internal Rgba _HighlightColor;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _BlurX;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _BlurY;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _Angle;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _Distance;

        /// <summary>
        /// 
        /// </summary>
        internal UInt16 _Strength;

        /// <summary>
        /// 
        /// </summary>
        internal bool _InnerShadow;

        /// <summary>
        /// 
        /// </summary>
        internal bool _KnockOut;

        /// <summary>
        /// 
        /// </summary>
        internal bool _CompositeSource;

        /// <summary>
        /// 
        /// </summary>
        internal bool _OnTop;

        /// <summary>
        /// 
        /// </summary>
        internal byte _Passes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public BevelFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );

            _ShadowColor = new Rgba( this.Version );
            _ShadowColor.Parse( input );
            _HighlightColor = new Rgba( this.Version );
            _HighlightColor.Parse( input );
            
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
                throw new SwfFormatException( "BevelFilter with CompositeSource false" );
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
                return (
                    _ShadowColor.Length +
                    _HighlightColor.Length +
                    ( sizeof( UInt32 ) * 4 ) +
                    sizeof( UInt16 ) +
                    sizeof( byte ) 
                    + sizeof( byte ) // FilterType
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

            _ShadowColor.Write( output );
            _HighlightColor.Write( output );
            
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
