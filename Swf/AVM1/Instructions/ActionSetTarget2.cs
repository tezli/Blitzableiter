using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionSetTarget2 represents the Adobe AVM1 ActionSetTarget2
    /// </summary>
    public class ActionSetTarget2 : AbstractAction
    {
        /// <summary>
        /// Sets the current context and is stack based
        /// </summary>
        public ActionSetTarget2()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
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
