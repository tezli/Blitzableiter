using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionPushDuplicate represents the Adobe AVM1 ActionPushDuplicate
    /// </summary>
    public class ActionPushDuplicate : AbstractAction
    {
        /// <summary>
        /// Pushes a duplicate of top of stack (the current return value) to the stack.
        /// </summary>
        public ActionPushDuplicate()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPush( AVM1DataTypes.AVM_ANY );
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
