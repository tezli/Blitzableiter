using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionNot represents the Adobe AVM1 ActionNot
    /// </summary>
    public class ActionNot : AbstractAction
    {
        /// <summary>
        /// Performs a logical NOT of a number
        /// </summary>
        public ActionNot()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );  
            // default to version >=5   
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_boolean );
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
                    _StackOps[ 1 ].DataType = AVM1DataTypes.AVM_String;
                }
                else
                {
                    _StackOps[ 1 ].DataType = AVM1DataTypes.AVM_boolean;
                }
                return _StackOps;
            }
        }
    }
}
