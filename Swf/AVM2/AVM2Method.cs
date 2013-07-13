using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.AVM2.ABC;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM2Method
    {
        /// <summary>
        /// 
        /// </summary>
        private AbcFile _Abc;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _MethodID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="abc"></param>
        public AVM2Method( UInt32 index, AbcFile abc )
        {
            if ( abc.Methods.Count <= index )
            {
                ArgumentException ae = new ArgumentException( "Method " + index.ToString( "d" ) + " not in ABC file" );
                Log.Error(this,  ae );
                throw ae;
            }

            _Abc = abc;
            _MethodID = index;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                UInt32 nameIndex = _Abc.Methods[(int)_MethodID].Name;
                return _Abc.ConstantPool.Strings[(int)nameIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ReturnType
        {
            get
            {
                return _Abc.ConstantPool.Multinames[ ( int )_Abc.Methods[ ( int )_MethodID ].ReturnType ].ToString( _Abc );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<AVM2Argument> Parameters
        {
            get
            {
                List<AVM2Argument> args = new List<AVM2Argument>();

                for ( uint i = 0; i < _Abc.Methods[ ( int )_MethodID ].ParamType.Count; i++ )
                {
                    AVM2Argument a = new AVM2Argument( _Abc, _MethodID, i );
                    args.Add( a );
                }

                return args;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.ReturnType );
            sb.Append( " " );
            sb.Append( this.Name );
            sb.Append( "(" );
            for ( int i = 0; i < this.Parameters.Count; i++ )
            {
                sb.AppendFormat( "{0:d}: {1}", i, this.Parameters[ i ].ToString() );
                if ( ( i + 1 ) != this.Parameters.Count )
                {
                    sb.Append( "," );
                }
            }
            sb.Append( ")" );
            return sb.ToString();
        }
    }
}
