using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionLess2 represents the Adobe AVM1 ActionLess2
    /// </summary>
    public class ActionLess2 : AbstractAction
    {
        /// <summary>
        /// Calculates whether arg1 is less than arg2
        /// </summary>
        public ActionLess2()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_boolean );
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
