using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionEnumerate2 represents the Adobe AVM1 ActionEnumerate2
    /// </summary>
    public class ActionEnumerate2 : AbstractAction
    {
        /// <summary>
        /// ActionEnumerate2 is similar to ActionEnumerate, 
        /// but uses a stack argument of object type
        /// rather than using a string to specify its name
        /// </summary>
        public ActionEnumerate2()
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
                throw new StackException( "ActionEnumerate2 cannot determine stack operations" );
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
