using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{ 
    /// <summary>
    /// 
    /// </summary>
    public class ColorMatrixFilter : AbstractFilter
    {
        /// <summary>
        /// 
        /// </summary>
        internal const FilterTypes _FilterType = FilterTypes.ColorMatrixFilter;

        /// <summary>
        /// 
        /// </summary>
        internal List<UInt32> _MatrixValues;        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public ColorMatrixFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            BinaryReader br = new BinaryReader( input );
            _MatrixValues = new List<uint>();
            for ( int i = 0; i < 20; i++ )
            {
                UInt32 a = br.ReadUInt32();
                _MatrixValues.Add( a );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get 
            {
                return ( 20 * sizeof( UInt32 ) ) + sizeof( byte ); // FilterType !
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

            if ( _MatrixValues.Count != 20 )
            {
                IndexOutOfRangeException e = new IndexOutOfRangeException( "_MatrixValues does not have 20 entries, does have " + _MatrixValues.Count.ToString( "d" ) );
                throw e;
            }

            for ( int i = 0; i < 20; i++ )
            {
                bw.Write( _MatrixValues[ i ] );
            }
        }
    }
}
