using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionSetProperty represents the Adobe AVM1 ActionSetProperty
    /// </summary>
    public class ActionSetProperty : AbstractAction
    {
        /// <summary>
        /// Sets a file property
        /// </summary>
        public ActionSetProperty()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // value
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // index
            _StackOps[ 2 ] = new StackPop( AVM1DataTypes.AVM_String ); // target 
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
