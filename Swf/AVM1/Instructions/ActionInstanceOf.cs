using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionInstanceOf represents the Adobe AVM1 ActionInstanceOf
    /// </summary>
    public class ActionInstanceOf : AbstractAction
    {
        /// <summary>
        /// Implements the ActionScript instanceof() operator
        /// </summary>
        public ActionInstanceOf()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_Object );
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_boolean );
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
