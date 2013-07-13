using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.AVM1;

namespace Recurity.Swf.AVM1Modifier
{
    /// <summary>
    /// 
    /// </summary>
    public class ModVariable
    {
        /// <summary>
        /// 
        /// </summary>
        string _Variable;

        /// <summary>
        /// 
        /// </summary>
        string _ReplaceWith;

        /// <summary>
        /// 
        /// </summary>
        public ModVariable()
        {
            _Variable = "$null";
            _ReplaceWith = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ModVariable( string name, string value )
        {
            Set( name, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set( string name, string value )
        {            
            if ( name.Length < 1 )
            {
                throw new ArgumentException( "'" + name + "' is an invalid variable name" );
            }            
            if ( !name.StartsWith( "$" ) )
            {
                name = name.Insert( 0, "$" );
            }

            _Variable = name.ToUpperInvariant();
            _ReplaceWith = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return _Variable;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get
            {
                return _ReplaceWith;
            }
            set
            {
                _ReplaceWith = value;
            }
        }
    }
}
