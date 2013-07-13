using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStrictEquals represents the Adobe AVM1 ActionStrictEquals
    /// </summary>
    public class ActionStrictEquals : AbstractAction
    {
        /// <summary>
        /// is similar to ActionEquals2, but the two arguments must be of the same 
        /// type in order to be considered equal. Implements the ‘===’ operator 
        /// from the ActionScript language.
        /// </summary>
        public ActionStrictEquals()
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
