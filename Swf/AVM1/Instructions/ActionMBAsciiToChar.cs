using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionMBAsciiToChar represents the Adobe AVM1 ActionMBAsciiToChar
    /// </summary>
    public class ActionMBAsciiToChar : AbstractAction
    {
        /// <summary>
        /// Converts ASCII to character code and is multi-byte aware
        /// </summary>
        public ActionMBAsciiToChar()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
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
