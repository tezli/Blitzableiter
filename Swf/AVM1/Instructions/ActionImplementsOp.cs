using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionImplementsOp represents the Adobe AVM1 ActionImplementsOp
    /// </summary>
    public class ActionImplementsOp : AbstractAction
    {
        /// <summary>
        /// Implements the ActionScript implements keyword.
        /// </summary>
        public ActionImplementsOp()
        {
            _StackOps = new StackChange[ 0 ];
            // FIXME: variable stack operation
        }
        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 7; 
            }
        }
    }
}
