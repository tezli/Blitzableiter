using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionToggleQuality represents the Adobe AVM1 ActionToggleQuality
    /// </summary>
    public class ActionToggleQuality : AbstractAction
    {
        /// <summary>
        /// Toggles the display between high and low quality
        /// </summary>
        public ActionToggleQuality()
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
