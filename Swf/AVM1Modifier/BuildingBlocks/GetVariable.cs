using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1Modifier.BuildingBlocks
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVariable : AbstractBuildingBlock
    {

        /// <summary>
        /// 
        /// </summary>
        protected string _VariableName;

        /// <summary>
        /// 
        /// </summary>
        public GetVariable()
        {
            _VariableName = this.Identity;
            this.MakeCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public GetVariable( string name )
        {
            _VariableName = name;
            this.MakeCode();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MakeCode()
        {
            _InlineSource = new List<string>();
            _InlineSource.Add( "ActionPush 'String:" + _VariableName + "'" );            
            _InlineSource.Add( "ActionGetVariable" );
        }

        /// <summary>
        /// 
        /// </summary>
        public string VariableName { get { return _VariableName; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="mStack"></param>
        /// <returns></returns>
        public override bool Execute( AVM1.AVM1Code code, int instructionIndex, CheckMachine.MachineStack mStack )
        {
            return true;
        }
    }
}
