using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCloneSprite represents the Adobe AVM1 ActionCloneSprite
    /// </summary>
    public class ActionCloneSprite : AbstractAction
    {
        /// <summary>
        /// Clones a sprite.
        /// </summary>
        public ActionCloneSprite()
        {
            _StackOps = new StackChange[ 3 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // depth
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // target
            _StackOps[ 2 ] = new StackPop( AVM1DataTypes.AVM_String );  // source
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
    }
}
