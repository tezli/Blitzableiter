using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCharToAscii represents the Adobe AVM1 ActionCharToAscii
    /// </summary>
    public class ActionCharToAscii : AbstractAction
    {
        /// <summary>
        /// Converts character code to ASCII.
        /// </summary>
        public ActionCharToAscii()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String ); // ASCII value of the first character
        }
        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 4; 
            }
        }        
    }
}
