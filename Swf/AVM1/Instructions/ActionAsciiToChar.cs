using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionAsciiToChar represents the the Adobe AVM1 ActionAsciiToChar
    /// </summary>
    public class ActionAsciiToChar : AbstractAction
    {
        /// <summary>
        /// Converts a value to an ASCII character code
        /// </summary>
        public ActionAsciiToChar()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // ASCII value of the first character
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String ); 
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
