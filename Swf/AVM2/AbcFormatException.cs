using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public class AbcFormatException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AbcFormatException( string message ) : base( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AbcFormatException( string message, Exception inner ) : base( message, inner ) { }    
    }
}
