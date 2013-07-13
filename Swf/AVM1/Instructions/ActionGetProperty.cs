using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGetProperty represents the Adobe AVM1 ActionGetProperty
    /// </summary>
    public class ActionGetProperty : AbstractAction
    {
        /// <summary>
        /// Gets a file property
        /// </summary>
        public ActionGetProperty()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // index (as string)                
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // target
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_ANY );
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
