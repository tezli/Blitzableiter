using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionPop represents the Adobe AVM1 ActionPop
    /// </summary>
    public class ActionPop : AbstractAction
    {
        /// <summary>
        /// Pops a value from the stack and discards it
        /// </summary>
        public ActionPop()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop();
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
