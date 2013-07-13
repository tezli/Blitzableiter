using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionNextFrame represents the Adobe AVM1 ActionNextFrame
    /// </summary>
    public class ActionNextFrame : AbstractAction
    {
        /// <summary>
        /// Instructs Flash Player to go to the next frame in the current file
        /// </summary>
        public ActionNextFrame()
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
