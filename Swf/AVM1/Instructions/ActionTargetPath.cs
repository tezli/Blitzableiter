using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionTargetPath represents the Adobe AVM1 ActionTargetPath
    /// </summary>
    public class ActionTargetPath : AbstractAction
    {
        /// <summary>
        /// If the object in the stack is of type MovieClip, the object’s 
        /// target path is pushed on the stack in dot notation. If the object 
        /// is not a MovieClip, the result is undefined rather than the movie 
        /// clip target path.
        /// </summary>
        public ActionTargetPath()
        {
            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_Object ); // must be movie-clip
            _StackOps[ 1 ] = new StackPush( AVM1DataTypes.AVM_String ); // target path
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
