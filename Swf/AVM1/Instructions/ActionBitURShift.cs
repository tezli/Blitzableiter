using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionBitURShift represents the Adobe AVM1 ActionBitURShift
    /// </summary>
    public class ActionBitURShift : AbstractAction
    {
        /// <summary>
        /// Pops the value and shift count arguments from 
        /// the stack. The value argument is converted to 
        /// 32-bit signed integer and only the least significant 
        /// 5 bits are used as the shift count.
        /// </summary>
        public ActionBitURShift()
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
