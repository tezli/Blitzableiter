using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1Modifier.BuildingBlocks
{
    /// <summary>
    /// Checks if top of the stack (string) begins with a certain string.
    /// Leaves boolean result on top of stack (true=matches, false otherwise)
    /// </summary>
    public class BeginsWith : AbstractBuildingBlock
    {
        /// <summary>
        /// 
        /// </summary>
        protected string _Match;

        /// <summary>
        /// 
        /// </summary>
        protected bool _CaseSensitive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <param name="caseSensitive"></param>
        public BeginsWith( string match, bool caseSensitive )
        {
            if ( ! caseSensitive )
                throw new NotImplementedException( "sucks, but AVM1 doesn't have anything for us, so we would need to use the String. Class of Flash!" );
            _Match = match;
            _CaseSensitive = caseSensitive;
            this.MakeCode();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MakeCode()
        {
            _InlineSource = new List<string>();
            _InlineSource.Add( "# " + this.GetType().ToString() + " for '" + _Match + "'" );
            // strlen( match )
            _InlineSource.Add( "ActionPushDuplicate" );
            _InlineSource.Add( "ActionPush 'String:" + _Match + "'" );
            _InlineSource.Add( "ActionStringLength" );
            // substr( arg, 0, strlen( match ) )
            _InlineSource.Add( "ActionPush String:0" );
            _InlineSource.Add( "ActionStackSwap" );
            _InlineSource.Add( "ActionStringExtract" );
            // strcmp( arg, substr )
            _InlineSource.Add( "ActionPush 'String:" + _Match + "'" );
            _InlineSource.Add( "ActionStringEquals" );            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="mStack"></param>
        /// <returns></returns>
        public override bool Execute( AVM1.AVM1Code code, int instructionIndex, CheckMachine.MachineStack mStack )
        {
            if ( mStack.Count < 1 )
                return false;

            CheckMachine.MachineStackEntry e = mStack.Pop();

            if ( e.Type != CheckMachine.MachineStackType.String )
                return false;

            string testValue = (string)e.Value;

            CheckMachine.MachineStackEntry res = new CheckMachine.MachineStackEntry();
            res.Type = CheckMachine.MachineStackType.Boolean;

            if ( testValue.StartsWith( _Match, ( _CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase ) ) )
            {
                res.Value = ( bool )true;
            }
            else
            {
                res.Value = ( bool )false;
            }
            mStack.Push( res );

            return true;
        }
    }
}
