using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.AVM2.ABC;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM2Argument
    {

        /// <summary>
        /// 
        /// </summary>
        public readonly string _ArgumentType;

        /// <summary>
        /// 
        /// </summary>
        public readonly string _ArgumentName;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool _IsOptional;

        /// <summary>
        /// 
        /// </summary>
        public readonly OptionType _OptionalType;

        /// <summary>
        /// 
        /// </summary>
        public readonly string _OptionalTypeName;

        /// <summary>
        /// 
        /// </summary>
        public readonly object _OptionalValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        /// <param name="method"></param>
        /// <param name="argument"></param>
        public AVM2Argument( AbcFile abc, UInt32 method, UInt32 argument )
        {
            _ArgumentType = abc.ConstantPool.Multinames[ ( int )abc.Methods[ ( int )method ].ParamType[ ( int )argument ] ].ToString( abc );

            if ( abc.Methods[ ( int )method ].FlagHasParamNames )
            {
                _ArgumentName = abc.ConstantPool.Strings[ ( int )abc.Methods[ ( int )method ].ParamNames[ ( int )argument ] ];
            }
            else
            {
                _ArgumentName = "(no param name)";
            }

            if ( ( abc.Methods[ ( int )method ].FlagHasOptional ) && ( argument < abc.Methods[ ( int )method ].Option.Count ) )
            {
                _IsOptional = true;
                _OptionalType = abc.Methods[ ( int )method ].Option[ ( int )argument ].OptionType;
                _OptionalTypeName = abc.Methods[ ( int )method ].Option[ ( int )argument ].OptionTypeName;
                _OptionalValue = abc.Methods[ ( int )method ].Option[ ( int )argument ].GetValue( abc );
            }
            else
            {
                _IsOptional = false;
                _OptionalType = OptionType.TotallyInvalid;
                _OptionalValue = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( _ArgumentType );
            sb.Append( " " );
            sb.Append( _ArgumentName );
            if ( _IsOptional )
            {
                sb.Append( " = (" );
                sb.Append( _OptionalTypeName );
                sb.Append( ")" );
                sb.Append( _OptionalValue.ToString() );
            }
            return sb.ToString();
        }
    }
}
