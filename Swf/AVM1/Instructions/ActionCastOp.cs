using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCastOp represents the Adobe AVM1 ActionCastOp
    /// </summary>
    public class ActionCastOp : AbstractAction
    {
        /// <summary>
        /// Implements the ActionScript cast operator, which allows the casting from 
        /// one data type to another. ActionCastOp pops an object off the stack and 
        /// attempts to convert the object to an instance of the class or to the 
        /// interface represented by the constructor function.
        /// </summary>
        public ActionCastOp()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_Object );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_Function );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_Object );
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
