using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionEquals2 represents the Adobe AVM1 ActionEquals2
    /// </summary>
    public class ActionEquals2 : AbstractAction
    {
        /// <summary>
        /// ActionEquals2 is similar to ActionEquals, but ActionEquals2 
        /// knows about types. The equality comparison algorithm from 
        /// ECMA-262 Section 11.9.3 is applied
        /// </summary>
        public ActionEquals2()
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
                return 5; 
            }
        }
    }
}
