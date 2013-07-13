using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionEnumerate represents the Adobe AVM1 ActionEnumerate
    /// </summary>
    public class ActionEnumerate : AbstractAction
    {
        /// <summary>
        /// ActionEnumerate obtains the names of all “slots” in use 
        /// in an ActionScript object—that is, for an object obj, 
        /// all names X that could be retrieved with the syntax obj.X. 
        /// ActionEnumerate is used to implement the for..in statement 
        /// in ActionScript
        /// </summary>
        public ActionEnumerate()
        {
            _StackOps = null;
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

        /// <summary>
        /// 
        /// </summary>
        public override StackChange[] StackOperations
        {
            get
            {
                throw new StackException( "ActionEnumerate cannot determine stack operations" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsStackPredictable
        {
            get
            {
                return false;
            }
        }
    }
}
