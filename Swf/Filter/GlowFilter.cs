using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class GlowFilter : AbstractFilter
    {
        internal const FilterTypes _FilterType = FilterTypes.GlowFilter;

        internal Rgba _GlowColor;
        internal UInt32 _BlurX;
        internal UInt32 _BlurY;
        internal UInt16 _Strength;
        internal bool _InnerGlow;
        internal bool _KnockOut;
        internal bool _CompositeSource;
        internal byte _Passes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public GlowFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            _GlowColor = new Rgba( this.Version );
            _GlowColor.Parse( input );

            BinaryReader br = new BinaryReader( input );

            _BlurX = br.ReadUInt32();
            _BlurY = br.ReadUInt32();
            _Strength = br.ReadUInt16();

            BitStream bits = new BitStream( input );
            _InnerGlow = ( 0 != bits.GetBits( 1 ) );
            _KnockOut = ( 0 != bits.GetBits( 1 ) );
            _CompositeSource = ( 0 != bits.GetBits( 1 ) );
            _Passes = ( byte )bits.GetBits( 5 );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return (
                    _GlowColor.Length +
                    ( 2 * sizeof( UInt32 ) ) +
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

            _GlowColor.Write( output );
            
            bw.Write( _BlurX );
            bw.Write( _BlurY );
            bw.Write( _Strength );
            BitStream bits = new BitStream( output );
            bits.WriteBits( 1, ( _InnerGlow ? 1 : 0 ) );
            bits.WriteBits( 1, ( _KnockOut ? 1 : 0 ) );
            bits.WriteBits( 1, ( _CompositeSource ? 1 : 0 ) );
            bits.WriteBits( 5, _Passes );
            bits.WriteFlush();
        }
    }
}
