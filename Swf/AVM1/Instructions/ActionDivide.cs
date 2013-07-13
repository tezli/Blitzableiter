using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDivide represents the Adobe AVM1 ActionDivide
    /// </summary>
    public class ActionDivide : AbstractAction
    {
        /// <summary>
        /// Divides two numbers and pushes the result back to the stack.
        /// </summary>
        public ActionDivide()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_float );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_float );
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_float );
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

        /// <summary>
        /// 
        /// </summary>
        public override StackChange[] StackOperations
        {
            get
            {
                if ( this.Version < 5 )
                {
                    _StackOps[ 2 ].DataType = AVM1DataTypes.AVM_String;
                }
                return _StackOps;
            }
        }
    }
}
