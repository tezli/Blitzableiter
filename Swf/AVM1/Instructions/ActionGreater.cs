using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGreater represents the Adobe AVM1 ActionGreater
    /// </summary>
    public class ActionGreater : AbstractAction
    {
        /// <summary>
        /// Compares if arg2 > arg1
        /// </summary>
        public ActionGreater()
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
                return 6; 
            }
        }
    }
}
