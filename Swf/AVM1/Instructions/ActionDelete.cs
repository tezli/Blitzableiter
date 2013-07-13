using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDelete represents the Adobe AVM1 ActionDelete
    /// </summary>
    public class ActionDelete : AbstractAction
    {
        /// <summary>
        /// deletes a named property from a ScriptObject
        /// </summary>
        public ActionDelete()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // name
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_Object ); // where
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
