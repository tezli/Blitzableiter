using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionPlay represents the Adobe AVM1 ActionPlay
    /// </summary>
    public class ActionPlay : AbstractAction
    {
        /// <summary>
        /// Instructs Flash Player to start playing at the current frame
        /// </summary>
        public ActionPlay()
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
