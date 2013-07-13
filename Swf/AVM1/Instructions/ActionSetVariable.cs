using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionSetVariable represents the Adobe AVM1 ActionSetVariable
    /// </summary>
    public class ActionSetVariable  : AbstractAction
    {
        /// <summary>
        /// Sets a variable
        /// </summary>
        public ActionSetVariable()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // value
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // variable name
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
