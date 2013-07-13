using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionSetMember represents the Adobe AVM1 ActionSetMember
    /// </summary>
    public class ActionSetMember : AbstractAction
    {
        /// <summary>
        /// Sets a property of an object. If the property does not already exist, 
        /// it is created. Any existing value in the property is overwritten
        /// </summary>
        public ActionSetMember()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );     // value
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String );  // object name
            _StackOps[ 2 ] = new StackPop( AVM1DataTypes.AVM_Object );  // object
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
