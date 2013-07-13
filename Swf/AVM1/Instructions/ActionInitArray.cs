using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionInitArray represents the Adobe AVM1 ActionInitArray 
    /// </summary>
    public class ActionInitArray : AbstractAction
    {
        /// <summary>
        /// Initializes an array in a ScriptObject.
        /// </summary>
        public ActionInitArray()
        {
            _StackOps = new StackChange[ 0 ];
            // FIXME: variable length
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
