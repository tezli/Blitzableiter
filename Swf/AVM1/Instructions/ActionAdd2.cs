using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionAdd2 represents the Adobe AVM1 ActionAdd2
    /// </summary>
    public class ActionAdd2 : AbstractAction
    {
        /// <summary>
        /// Is similar to ActionAdd, but performs the addition differently, 
        /// according to the data types of the arguments. The addition 
        /// operator algorithm in ECMA-262 Section 11.6.1 is used. 
        /// If string concatenation is applied, the concatenated string 
        /// is arg2 followed by arg1
        /// </summary>
        public ActionAdd2()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_ANY );
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
