using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionBitOr represents the Adobe AVM1 ActionBitOr
    /// </summary>
    public class ActionBitOr : AbstractAction
    {
        /// <summary>
        /// Performs a bitwise OR operation.
        /// </summary>
        public ActionBitOr()
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
