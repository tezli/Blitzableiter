using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionModulo represents the Adobe AVM1 ActionModulo
    /// </summary>
    public class ActionModulo : AbstractAction
    {
        /// <summary>
        /// calculates x modulo y. If y is 0, then NaN (0x7FC00000) 
        /// is pushed to the stack.
        /// </summary>
        public ActionModulo()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_integer );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_integer );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_integer );
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
