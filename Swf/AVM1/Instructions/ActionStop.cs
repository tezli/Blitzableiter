using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStop represents the Adobe AVM1 ActionStop
    /// </summary>
    public class ActionStop : AbstractAction
    {
        /// <summary>
        /// Instructs Flash Player to stop playing the file at the current frame
        /// </summary>
        public ActionStop()
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
