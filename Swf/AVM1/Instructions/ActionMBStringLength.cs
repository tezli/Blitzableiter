using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionMBStringLength represents the Adobe AVM1 ActionMBStringLength
    /// </summary>
    public class ActionMBStringLength : AbstractAction
    {
        /// <summary>
        /// Computes the length of a string and is multi-byte aware.
        /// </summary>
        public ActionMBStringLength()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String );
        }
        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 4; 
            }
        }
    }
}
