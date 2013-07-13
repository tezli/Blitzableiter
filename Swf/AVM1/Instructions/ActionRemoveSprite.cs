using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionRemoveSprite represents the Adobe AVM1 ActionRemoveSprite
    /// </summary>
    public class ActionRemoveSprite : AbstractAction
    {
        /// <summary>
        /// Removes a clone sprite
        /// </summary>
        public ActionRemoveSprite()
        {
             _StackOps = new StackChange[1];
             _StackOps[0] = new StackPop( AVM1DataTypes.AVM_String ); // target
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
