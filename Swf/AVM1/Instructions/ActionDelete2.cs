using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDelete2 represents the Adobe AVM1 ActionDelete2
    /// </summary>
    public class ActionDelete2 : AbstractAction
    {
        /// <summary>
        /// ActionDelete2 deletes a named property. Flash Player 
        /// first looks for the property in the current scope, 
        /// and if the property cannot be found, continues to 
        /// search in the encompassing scopes.
        /// </summary>
        public ActionDelete2()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // name
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
