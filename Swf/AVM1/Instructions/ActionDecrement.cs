using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDecrement represents the Adobe AVM1 ActionDecrement
    /// </summary>
    public class ActionDecrement : AbstractAction
    {
        /// <summary>
        /// Pops a value from the stack, converts it to number type, decrements it by 1, and pushes it back to the stack
        /// </summary>
        public ActionDecrement()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_integer );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_integer );
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
