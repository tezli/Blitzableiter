using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionInitObject represents the Adobe AVM1 ActionInitObject
    /// </summary>
    public class ActionInitObject : AbstractAction
    {
        /// <summary>
        /// Initializes an object
        /// </summary>
        public ActionInitObject()
        {
            // FIXME: variable length
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
