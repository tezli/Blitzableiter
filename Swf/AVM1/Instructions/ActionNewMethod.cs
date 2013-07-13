using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionNewMethod represents the Adobe AVM1 ActionNewMethod
    /// </summary>
    public class ActionNewMethod : AbstractAction
    {
        /// <summary>
        /// Invokes a constructor function to create a new object
        /// </summary>
        public ActionNewMethod()
        {
            // FIXME: variable
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
