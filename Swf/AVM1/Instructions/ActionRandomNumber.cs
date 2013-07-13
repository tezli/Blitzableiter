using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionRandomNumber represents the Adobe AVM1 ActionRandomNumber
    /// </summary>
    public class ActionRandomNumber : AbstractAction
    {
        /// <summary>
        /// Calculates a pseudo random number
        /// </summary>
        public ActionRandomNumber()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_integer );     // maximum
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_integer );    // rnd
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
