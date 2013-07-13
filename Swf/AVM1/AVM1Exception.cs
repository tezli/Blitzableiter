using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1Exception : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AVM1Exception( string message )
            : base( message )
        {
        }
    }
}
