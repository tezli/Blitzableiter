using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGetMember represents the Adobe AVM1 ActionGetMember
    /// </summary>
    public class ActionGetMember : AbstractAction
    {
        /// <summary>
        /// retrieves a named property from an object, and 
        /// pushes the value of the property onto the stack
        /// </summary>
        public ActionGetMember()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // name
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_Object ); // object
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
