using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCallFunction represents the Adobe AVM1 ActionCallFunction
    /// </summary>
    public class ActionCallFunction : AbstractAction
    {
        /// <summary>
        /// Executes a function. The function can be an ActionScript built-in function (such as parseInt), 
        /// a user-defined ActionScript function, or a native function. For more information, see ActionNewObject.
        /// </summary>
        public ActionCallFunction()
        {
            _StackOps = new StackChange[ 1 ];
            // FIXME: variable stack change
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
