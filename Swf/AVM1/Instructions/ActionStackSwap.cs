using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStackSwap represents the Adobe AVM1 ActionStackSwap
    /// </summary>
    public class ActionStackSwap : AbstractAction
    {
        /// <summary>
        /// Swaps the top two ScriptAtoms on the stack
        /// </summary>
        public ActionStackSwap()
        {
            _StackOps = new StackChange[ 4 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // a
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // b
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_ANY ); // a
            _StackOps[ 3 ] = new StackPush( AVM1DataTypes.AVM_ANY ); // b
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
