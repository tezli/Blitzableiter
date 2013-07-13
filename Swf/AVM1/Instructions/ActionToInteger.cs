using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionToInteger represents the Adobe AVM1 ActionToInteger
    /// </summary>
    public class ActionToInteger : AbstractAction
    {
        /// <summary>
        /// Converts a value to an integer
        /// </summary>
        public ActionToInteger()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_ANY );
            // default version >= 5
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_integer );
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
                if ( this.Version >= 5 )
                {
                    _StackOps[ 1 ].DataType = AVM1DataTypes.AVM_integer;
                }
                else
                {
                    _StackOps[ 1 ].DataType = AVM1DataTypes.AVM_String;
                }
                return _StackOps;
            }
        }
    }
}
