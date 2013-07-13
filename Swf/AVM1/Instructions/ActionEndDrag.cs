using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionEndDrag represents the Adobe AVM1 ActionEndDrag
    /// </summary>
    public class ActionEndDrag : AbstractAction
    {
        /// <summary>
        /// Ends the drag operation in progress, if any.
        /// </summary>
        public ActionEndDrag()
        {
            _StackOps = new StackChange[ 0 ];
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
