using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDefineLocal2 represents the Adobe AVM1 ActionDefineLocal2
    /// </summary>
    public class ActionDefineLocal2 : AbstractAction
    {
        /// <summary>
        /// defines a local variable without setting its value. 
        /// If the variable already exists, nothing happens. 
        /// The initial value of the local variable is undefined.
        /// </summary>
        public ActionDefineLocal2()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // name
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
