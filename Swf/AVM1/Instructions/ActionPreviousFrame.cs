using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionPreviousFrame represents the Adobe AVM1 ActionPreviousFrame
    /// </summary>
    public class ActionPreviousFrame : AbstractAction
    {
        /// <summary>
        /// Instructs Flash Player to go to the previous frame of the current file
        /// </summary>
        public ActionPreviousFrame()
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
                return 3; 
            }
        }
    }
}
