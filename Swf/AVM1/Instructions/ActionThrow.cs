using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionThrow represents the Adobe AVM1 ActionThrow
    /// </summary>
    public class ActionThrow : AbstractAction
    {
        /// <summary>
        /// ActionThrow implements the ActionScript throw keyword
        /// </summary>
        public ActionThrow()
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
                return 7; 
            }
        }
    }
}
