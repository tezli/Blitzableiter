using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGetTime represents the Adobe AVM1 ActionGetTime
    /// </summary>
    public class ActionGetTime : AbstractAction
    {
        /// <summary>
        /// Reports the milliseconds since Adobe Flash Player started.
        /// </summary>
        public ActionGetTime()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPush( AVM1DataTypes.AVM_String );
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
