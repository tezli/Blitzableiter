using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionNewObject represents the Adobe AVM1 ActionNewObject
    /// </summary>
    public class ActionNewObject : AbstractAction
    {
        /// <summary>
        /// Invokes a constructor function
        /// </summary>
        public ActionNewObject()
        {
            // FIXME: variable stack
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
