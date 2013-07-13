using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionOr represents the Adobe AVM1 ActionOr
    /// </summary>
    public class ActionOr : AbstractAction
    {
        /// <summary>
        /// Performs a logical OR of two numbers
        /// </summary>
        public ActionOr()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String );
            // default version >=5
            _StackOps[ 2 ] = new StackPush( AVM1DataTypes.AVM_boolean );
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
                else
                {
                    _StackOps[ 2 ].DataType = AVM1DataTypes.AVM_boolean;
                }
                return _StackOps;
            }
        }
    }
}
