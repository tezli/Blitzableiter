using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionBitRShift represents the Adobe AVM1 ActionBitRShift
    /// </summary>
    public class ActionBitRShift : AbstractAction
    {
        /// <summary>
        /// Performs a bitwise right shift
        /// </summary>
        public ActionBitRShift()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_integer ); // arg
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_integer ); // shift value
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
