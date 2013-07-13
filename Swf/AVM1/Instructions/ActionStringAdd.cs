using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStringAdd represents the Adobe AVM1 ActionStringAdd
    /// </summary>
    public class ActionStringAdd : AbstractAction
    {
        /// <summary>
        /// Concatenates two strings
        /// </summary>
        public ActionStringAdd()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_String );
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
