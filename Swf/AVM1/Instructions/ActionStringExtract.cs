using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStringExtract represents the Adobe AVM1 ActionStringExtract
    /// </summary>
    public class ActionStringExtract : AbstractAction
    {
        /// <summary>
        /// Extracts a substring from a string
        /// </summary>
        public ActionStringExtract()
        {
            _StackOps = new StackChange[ 4 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // count
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // index
            _StackOps[ 2 ] = new StackPop( AVM1DataTypes.AVM_String ); // the string
            _StackOps[ 3 ] = new StackPush( AVM1DataTypes.AVM_String ); // the substring (result)
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
