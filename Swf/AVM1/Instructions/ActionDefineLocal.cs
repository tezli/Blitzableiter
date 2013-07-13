using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDefineLocal represents the Adobe AVM1 ActionDefineLocal
    /// </summary>
    public class ActionDefineLocal : AbstractAction
    {
        /// <summary>
        /// Defines a local variable and sets its value. If the variable 
        /// already exists, the value is set to the newly specified value.
        /// </summary>
        public ActionDefineLocal()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // value
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // name
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
