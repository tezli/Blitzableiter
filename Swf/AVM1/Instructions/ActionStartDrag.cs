using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStartDrag represents the Adobe AVM1 ActionStartDrag
    /// </summary>
    public class ActionStartDrag : AbstractAction
    {
        /// <summary>
        /// Starts dragging a movie clip
        /// </summary>
        public ActionStartDrag()
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
                return 4; 
            }
        }
    }
}
