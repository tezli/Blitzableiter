using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionTypeOf represents the Adobe AVM1 ActionTypeOf
    /// </summary>
    public class ActionTypeOf : AbstractAction
    {
        /// <summary>
        /// pushes the object type to the stack, which is equivalent 
        /// to the ActionScript TypeOf() method. The possible types are:
        /// "number","boolean","string","object","movieclip","null",
        /// "undefined","function"
        /// </summary>
        public ActionTypeOf()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY ); // the thing to inspect
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String ); // type name
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
