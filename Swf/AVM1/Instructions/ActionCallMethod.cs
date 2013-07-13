using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCallMethod represents the Adobe AVM1 ActionCallMethod
    /// </summary>
    public class ActionCallMethod : AbstractAction
    {
        /// <summary>
        /// Pushes a method (function) call onto the stack, similar to ActionNewMethod.
        /// </summary>
        public ActionCallMethod()
        {
            // FIXME: Variable stack operation
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
