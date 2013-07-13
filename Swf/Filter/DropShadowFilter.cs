using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class DropShadowFilter : AbstractFilter
    {
        internal const FilterTypes _FilterType = FilterTypes.DropShadowFilter;
        internal Rgba _DropShadowColor;
        internal UInt32 _BlurX;
        internal UInt32 _BlurY;
        internal UInt32 _Angle;
        internal UInt32 _Distance;
        internal UInt16 _Strength;
        internal bool _InnerShadow;
        internal bool _KnockOut;
        /// <summary>
        /// 
        /// </summary>
        internal bool _CompositeSource;
        internal byte _Passes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DropShadowFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );

            _DropShadowColor = new Rgba( this.Version );
            _DropShadowColor.Parse( input );
            _BlurX = br.ReadUInt32();
            _BlurY = br.ReadUInt32();
            _Angle = br.ReadUInt32();
            _Distance = br.ReadUInt32();
            _Strength = br.ReadUInt16();

            BitStream bits = new BitStream( input );
            _InnerShadow = ( 0 != bits.GetBits( 1 ) );
            _KnockOut = ( 0 != bits.GetBits( 1 ) );
            _CompositeSource = ( 0 != bits.GetBits( 1 ) );
            _Passes = (byte)bits.GetBits( 5 );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return (
                    _DropShadowColor.Length +
                    ( 4 * sizeof( UInt32 ) ) +
                    sizeof( UInt16 ) +
                    sizeof( byte )
                    + sizeof(byte) // FilterType !
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

            bw.Write( (byte)_FilterType );

            _DropShadowColor.Write( output );            
            bw.Write( _BlurX );
            bw.Write( _BlurY );
            bw.Write( _Angle );
            bw.Write( _Distance );
            bw.Write( _Strength );
            BitStream bits = new BitStream( output );
            bits.WriteBits( 1, ( _InnerShadow ? 1 : 0 ) );
            bits.WriteBits( 1, ( _KnockOut ? 1 : 0 ) );
            bits.WriteBits( 1, ( _CompositeSource ? 1 : 0 ) );
            bits.WriteBits( 5, _Passes );
            bits.WriteFlush();
        }
    }
}
