using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionExtends represents the Adobe AVM1 ActionExtends
    /// </summary>
    public class ActionExtends : AbstractAction
    {
        /// <summary>
        /// implements the ActionScript extends keyword. ActionExtends 
        /// creates an inheritance relationship between two classes, 
        /// called the subclass and the superclass.
        /// </summary>
        public ActionExtends()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_Function );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_Function );
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
