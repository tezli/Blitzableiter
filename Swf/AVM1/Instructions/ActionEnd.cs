using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{    
    /// <summary>
    /// Blitzableiter ActionEnd represents the Adobe AVM1 ActionEnd 
    /// </summary>
    public class ActionEnd : AbstractAction
    {
        /// <summary>
        /// Undocumented action.
        /// </summary>
        public ActionEnd()
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
