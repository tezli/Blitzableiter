using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class BlurFilter : AbstractFilter
    {

        /// <summary>
        /// 
        /// </summary>
        internal const FilterTypes _FilterType = FilterTypes.BlurFilter;


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
        internal byte _Passes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public BlurFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );

            _BlurX = br.ReadUInt32();
            _BlurY = br.ReadUInt32();         

            BitStream bits = new BitStream( input );
            _Passes = ( byte )bits.GetBits( 5 );

            uint reserved = bits.GetBits( 3 );
            if ( 0 != reserved )
            {
                throw new SwfFormatException( "BlurFilter uses reserved bits" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return (
                    ( 2 * sizeof( UInt32 ) ) +
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

            bw.Write( _BlurX );
            bw.Write( _BlurY );
            BitStream bits = new BitStream( output );
            bits.WriteBits( 5, _Passes );
            bits.WriteBits( 3, 0 );
            bits.WriteFlush();
        }
    }
}
