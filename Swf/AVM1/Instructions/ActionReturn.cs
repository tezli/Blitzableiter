using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionReturn represents the Adobe AVM1 ActionReturn
    /// </summary>
    public class ActionReturn : AbstractAction
    {
        /// <summary>
        /// Forces the return item to be pushed off the stack and returned. 
        /// If a return is not appropriate, the return item is discarded
        /// </summary>
        public ActionReturn()
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
                return 5; 
            }
        }
    }
}
