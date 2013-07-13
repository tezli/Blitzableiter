using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionToString represents the Adobe AVM1 ActionToString
    /// </summary>
    public class ActionToString : AbstractAction
    {
        /// <summary>
        /// Converts the object on the top of the stack into a String, 
        /// and pushes the string back to the stack
        /// </summary>
        public ActionToString()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String );
        }

        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 5; 
            }
        }
    }
}
