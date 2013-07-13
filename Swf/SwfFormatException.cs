using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class SwfFormatException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SwfFormatException( string message ) : base( message ) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SwfFormatException( string message, Exception inner ) : base( message, inner ) { }    
    }
}
