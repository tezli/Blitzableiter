using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class StackException : AVM1Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public StackException( string message )
            : base( message )
        {
        }
    }
}
