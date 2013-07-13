using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter AActionStopSounds represents the Adobe AVM1 ActionStopSounds
    /// </summary>
    public class ActionStopSounds : AbstractAction
    {
        /// <summary>
        /// Instructs Flash Player to stop playing all sounds
        /// </summary>
        public ActionStopSounds()
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
