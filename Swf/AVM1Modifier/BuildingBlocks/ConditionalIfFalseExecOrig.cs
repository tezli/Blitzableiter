using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1Modifier.BuildingBlocks
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionalIfFalseExecOrig : AbstractBuildingBlock
    {
        /// <summary>
        /// 
        /// </summary>
        public ConditionalIfFalseExecOrig()
        {
            _InlineSource = new List<string>();
            _InlineSource.Add( "# " + this.GetType().ToString() );

            _InlineSource.Add( "ActionIf " + this.Identity + "True:" );
            _InlineSource.Add( "OriginalAction" );
            _InlineSource.Add( this.Identity + "True:" );
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

            if ( e.Type != CheckMachine.MachineStackType.Boolean )
                return false;

            bool condition = ( bool )e.Value;

            CheckMachine.MachineStackEntry res = new CheckMachine.MachineStackEntry();            
            res.Value = instructionIndex;

            if ( condition )
            {
                res.Type = CheckMachine.MachineStackType.RemoveInstruction;
            }
            else
            {
                res.Type = CheckMachine.MachineStackType.KeepInstruction;
            }
            mStack.Push( res );

            return true;
        }
    }
}
