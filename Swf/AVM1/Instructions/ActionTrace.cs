using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionTrace represents the Adobe AVM1 ActionTrace 
    /// </summary>
    public class ActionTrace : AbstractAction
    {
        /// <summary>
        /// Trace sends a debugging output string
        /// </summary>
        public ActionTrace()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
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
