using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.AVM1;
using Recurity.Swf.AVM1Modifier.BuildingBlocks;

namespace Recurity.Swf.AVM1Modifier.CheckMachine
{
    /// <summary>
    /// 
    /// </summary>
    public enum MachineStackType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Boolean,
        /// <summary>
        /// 
        /// </summary>
        String,
        /// <summary>
        /// 
        /// </summary>
        RemoveInstruction,
        /// <summary>
        /// 
        /// </summary>
        KeepInstruction
    }

    /// <summary>
    /// 
    /// </summary>
    public class MachineStackEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public MachineStackType Type;

        /// <summary>
        /// 
        /// </summary>
        public object Value;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MachineStack : Stack<MachineStackEntry> { }

    /// <summary>
    /// 
    /// </summary>
    public class Machine    
    {
        private MachineStack _MachineStack;

        /// <summary>
        /// 
        /// </summary>
        public Machine()
        {
            _MachineStack = new MachineStack();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            _MachineStack.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="blocks"></param>
        /// <returns></returns>
        public bool Run( AVM1Code code, int instructionIndex, List<AbstractBuildingBlock> blocks )
        {
            bool currentResult = true;

            for ( int i = 0; i < blocks.Count; i++ )
            {
                currentResult = blocks[ i ].Execute( code, instructionIndex, _MachineStack );
                if ( !currentResult )
                    break;
            }

            if ( _MachineStack.Count < 1 )
                throw new Exception("FUCKUP! MachineStack empty! WTF?");

            return currentResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public MachineStackEntry MachineResult
        {
            get
            {
                if ( _MachineStack.Count != 1 )
                    throw new Exception( "FUCKUP! MachineStack must be exactly 1 when this is called! WTF?" );

                return _MachineStack.Peek();
            }
        }
    }
}
