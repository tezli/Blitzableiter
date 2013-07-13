using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.AVM1;
using Recurity.Swf.Flowgraph;
using Recurity.Swf.AVM1Modifier;

namespace Recurity.Swf.AVM1Modifier.BuildingBlocks
{

    /// <summary>
    /// 
    /// </summary>
    public class ArgN : AbstractBuildingBlock
    {

        /// <summary>
        /// 
        /// </summary>
        private uint _ArgumentNumber;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentNumber"></param>
        public ArgN( uint argumentNumber )
        {
            _ArgumentNumber = argumentNumber;

            _InlineSource = new List<string>();
            _InlineSource.Add( "# " + this.GetType().ToString() + " for " + argumentNumber.ToString( "d" ) );

            if ( 0 == argumentNumber )
            {
                _InlineSource.Add( "ActionPushDuplicate" );
            }
            else
            {
                for ( int argc = (int)argumentNumber; argc >= 0; argc-- )
                {
                    _InlineSource.AddRange( new SetVariable( this.Identity+"arg"+argc.ToString("d") ).InlineSource );
                }
                for ( int argc2 = 0; argc2 <= (int)argumentNumber; argc2++ )
                {
                    _InlineSource.AddRange( new GetVariable( this.Identity + "arg" + argc2.ToString( "d" ) ).InlineSource );
                }
                _InlineSource.AddRange( new GetVariable( this.Identity + "arg0" ).InlineSource );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="mStack"></param>
        /// <returns></returns>
        public override bool Execute( AVM1Code code, int instructionIndex, CheckMachine.MachineStack mStack )
        {
            AVM1DataElement e = new AVM1DataElement();
            // check if we can resolve the argument within the basic block of the instruction
            if ( AVM1.Stack.Trace.TraceArgument( code, instructionIndex, null, _ArgumentNumber, out e ) )
            {
                if ( e.DataType != AVM1DataTypes.AVM_String )
                {
                    return false;
                }
                else
                {
                    CheckMachine.MachineStackEntry se = new CheckMachine.MachineStackEntry();
                    se.Type = CheckMachine.MachineStackType.String;
                    se.Value = e.Value;
                    mStack.Push( se );
                    return true;
                }                                       
            }            

            return false;
        }
    }
}
