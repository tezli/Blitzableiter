using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionToNumber represents the Adobe AVM1 ActionToNumber
    /// </summary>
    public class ActionToNumber : AbstractAction
    {
        /// <summary>
        /// Converts the object on the top of the stack 
        /// into a number, and pushes the number back to the stack
        /// </summary>
        public ActionToNumber()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_integer );
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
