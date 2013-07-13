using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonRecord2 : ButtonRecord
    {
        internal CxFormWithAlpha _ColorTransform;
        internal FilterList _FilterList;
        internal byte _BlendMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public ButtonRecord2( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse( Stream input )
        {
            base.Parse( input );

            _ColorTransform = new CxFormWithAlpha( this.Version );
            _ColorTransform.Parse( input );
           //Log.Debug(this,  _ColorTransform.ToString() );

            if ( _ButtonHasFilterList )
            {
                _FilterList = new FilterList( this.Version );
                _FilterList.Parse( input );
            }

            if ( _ButtonHasBlendMode )
            {
                BinaryReader br = new BinaryReader( input );
                _BlendMode = br.ReadByte();

                if ( _BlendMode > 14 )
                {
                    throw new SwfFormatException( "ButtonRecord2 BlendMode > 14" );
                }
            }

           //Log.Debug(this,  this.ToString() );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {                
                uint calculated = ( uint )(
                    base.Length +
                    _ColorTransform.Length +
                    ( _ButtonHasFilterList ? _FilterList.Length : 0 ) +
                    ( _ButtonHasBlendMode ? sizeof( byte ) : 0 )
                );

                return calculated;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="version"></param>
        public override void Write( Stream output, byte version )
        {
            long pos = output.Position;

            if ( ( null != _FilterList ) && ( _FilterList._Filters.Count > 0 ) )
            {
                _ButtonHasFilterList = true;
            }
            else
            {
                _ButtonHasFilterList = false;
            }

            base.Write( output, version );

            _ColorTransform.Write( output );
            if ( _ButtonHasFilterList )
            {
                _FilterList.Write( output );
            }
            if ( _ButtonHasBlendMode )
            {
                BinaryWriter bw = new BinaryWriter( output );
                bw.Write( _BlendMode );
            }

           //Log.Debug(this,  "wrote " + ( ( ulong )( output.Position - pos ) ).ToString( "d" ) + " bytes (" + this.Length + " calulcated)" );            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append( this.GetType().ToString() );
            //sb.Append( "(" );
            sb.Append( base.ToString() );
            //sb.Append( ") " );

            sb.Append( " CxTransFrom: " );
            sb.Append( _ColorTransform.ToString() );
            if ( _ButtonHasFilterList )
            {
                sb.Append( "FilterList: " );
                sb.Append( _FilterList.ToString() );
            }
            else
            {
                sb.Append( "(no FilterList)" );
            }
            sb.Append(" ");
            if ( _ButtonHasBlendMode )
            {
                sb.AppendFormat( "BlendMode: {0:d}", _BlendMode );
            }
            else
            {
                sb.Append( "(no BlendMode)" );
            }
            return sb.ToString();
        }
    }
}
