using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStringGreater represents the Adobe AVM1 ActionStringGreater
    /// </summary>
    public class ActionStringGreater : AbstractAction
    {
        /// <summary>
        /// Tests to see if a string is longer than another string
        /// </summary>
        public ActionStringGreater()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String );
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
