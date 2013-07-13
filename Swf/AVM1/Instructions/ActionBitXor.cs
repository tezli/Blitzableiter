using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionBitXor represents the Adobe AVM1 ActionBitXor
    /// </summary>
    public class ActionBitXor : AbstractAction
    {
        /// <summary>
        /// Pops two numbers off of the stack, performs a bitwise XOR, 
        /// and pushes an S32 number to the stack. The arguments are 
        /// converted to 32-bit unsigned integers before performing the 
        /// bitwise operation. The result is a SIGNED 32-bit integer.
        /// </summary>
        public ActionBitXor()
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
