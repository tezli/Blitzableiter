using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionEquals represents the Adobe AVM1 ActionEquals
    /// </summary>
    public class ActionEquals : AbstractAction
    {
        /// <summary>
        /// Tests two numbers for equality
        /// </summary>
        public ActionEquals()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String );
            // defaults to version 5 or higher
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
