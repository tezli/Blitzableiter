using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Recurity.Swf.Filter;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterList : AbstractSwfElement
    {
        internal byte _numFilters;
        internal List<AbstractFilter> _Filters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public FilterList( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verfify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse( Stream input )
        {
            BinaryReader2 br = new BinaryReader2( input );

            _numFilters = br.ReadByte();
            _Filters = new List<AbstractFilter>( _numFilters );

            for ( int i = 0; i < _numFilters; i++ )
            {
                AbstractFilter.FilterTypes nextFilterType = ( AbstractFilter.FilterTypes )br.ReadByte();
                AbstractFilter aFilter;

                switch ( nextFilterType )
                {
                    case AbstractFilter.FilterTypes.DropShadowFilter:
                        aFilter = new Filter.DropShadowFilter( this.Version );
                        break;

                    case AbstractFilter.FilterTypes.BevelFilter:
                        aFilter = new Filter.BevelFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.BlurFilter:
                        aFilter = new Filter.BlurFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.ColorMatrixFilter:
                        aFilter = new Filter.ColorMatrixFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.ConvolutionFilter:
                        aFilter = new Filter.ConvolutionFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.GlowFilter:
                        aFilter = new Filter.GlowFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.GradientBevelFilter:
                        aFilter = new Filter.GradientBevelFilter( this.Version );                        
                        break;

                    case AbstractFilter.FilterTypes.GradientGlowFilter:
                        aFilter = new Filter.GradientGlowFilter( this.Version );                        
                        break;

                    default:
                        SwfFormatException e = new SwfFormatException( "Illegal Filter Type ID " + nextFilterType.ToString( "d" ) );
                       Log.Error(this,  e );
                        throw e;
                }

                aFilter.Parse( input );
                _Filters.Add( aFilter );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint filterLengthSum = 0;
                for ( int i = 0; i < _Filters.Count; i++ )
                {
                    filterLengthSum += _Filters[ i ].Length;
                }

                return ( sizeof( byte ) + filterLengthSum );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            BinaryWriter bw = new BinaryWriter( output );

            _numFilters = (byte)_Filters.Count;
            bw.Write( _numFilters );                        

            for ( int i = 0; i < _Filters.Count; i++ )
            {
                _Filters[ i ].Write( output );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            if ( null == _Filters )
            {
                sb.Append( ": (UNINITIALIZED)" );
            }
            else
            {
                sb.Append( ": " );
                sb.Append( _Filters.Count.ToString( "d" ) + " filters[" );
                for ( int i = 0; i < _Filters.Count; i++ )
                {                    
                    sb.Append( _Filters[ i ].GetType().ToString() );
                    sb.Append( ( ( i + 1 ) == _Filters.Count ? "" : "," ) );
                }
                sb.Append( "]" );
            }
            return sb.ToString();
        }
    }
}
