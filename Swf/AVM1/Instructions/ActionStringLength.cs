using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStringLength represents the Adobe AVM1 ActionStringLength
    /// </summary>
    public class ActionStringLength : AbstractAction
    {
        /// <summary>
        /// Computes the length of a string
        /// </summary>
        public ActionStringLength()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String ); // bool
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
