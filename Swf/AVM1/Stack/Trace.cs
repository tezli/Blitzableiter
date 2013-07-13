using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.Flowgraph;

namespace Recurity.Swf.AVM1.Stack
{
    /// <summary>
    /// 
    /// </summary>
    public static class Trace
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <param name="stackIn"></param>
        /// <param name="argc"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool TraceArgument( AVM1Code code, int instructionIndex, AVM1Stack stackIn, uint argc, out AVM1DataElement entry )
        {
            if ( null == stackIn)
                stackIn = new AVM1Stack();
                
            int indexInBlock;
            BasicBlock home = code.Flowgraph.BlockOfInstruction( ( uint )instructionIndex, out indexInBlock );

            if ( null == home )
            {
                throw new Exception( "FUCKUP: instuction index " + instructionIndex.ToString( "d" ) + " not found in AVM1Code graph" );
            }

            for ( int i = 0; i < indexInBlock; i++ )
            {
                if ( code[ ( int )home.Indices[ i ] ].IsStackPredictable )
                {
                    code[ ( int )home.Indices[ i ] ].PerformStackOperations( stackIn );
                }
                else
                {
                    entry = default( AVM1DataElement );
                    return false;
                }
            }

            if ( stackIn.Count > argc )
            {
                entry = stackIn[ argc ];
                return true;
            }
            else
            {
                entry = default(AVM1DataElement);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="terminalInstructionIndex"></param>
        /// <param name="stackIn"></param>
        /// <param name="blocks"></param>
        /// <returns></returns>
        public static AVM1Stack TraceBlock( AVM1Code code, int terminalInstructionIndex, AVM1Stack stackIn, List<uint> blocks )
        {
            AVM1Stack newStack = new AVM1Stack();        
            //stackIn.CopyTo( newStack );

            // *** make me work ****

            return newStack;
        }

    }
}
