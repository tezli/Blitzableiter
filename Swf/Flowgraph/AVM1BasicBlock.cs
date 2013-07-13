using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.AVM1;

namespace Recurity.Swf.Flowgraph
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1BasicBlockEdge
    {

        /// <summary>
        /// 
        /// </summary>
        public AVM1BasicBlock Neighbor;

        /// <summary>
        /// 
        /// </summary>
        public EdgeType EType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbor"></param>
        /// <param name="eType"></param>
        public AVM1BasicBlockEdge( AVM1BasicBlock neighbor, EdgeType eType )
        {
            Neighbor = neighbor;
            EType = eType;
        }       
    }

    /// <summary>
    /// 
    /// </summary>
    public class AVM1BasicBlock
    {

        /// <summary>
        /// 
        /// </summary>
        private AVM1InstructionSequence _Instructions;

        /// <summary>
        /// 
        /// </summary>
        private List<AVM1BasicBlockEdge> _Blocks;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _ID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public AVM1BasicBlock( UInt32 id )
        {
            _ID = id;
            _Instructions = new AVM1InstructionSequence();
            _Blocks = new List<AVM1BasicBlockEdge>();
        }        

        /// <summary>
        /// 
        /// </summary>
        public AVM1InstructionSequence Instructions
        {
            get
            {
                return _Instructions;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<AVM1BasicBlockEdge> OutEdges
        {
            get
            {
                return _Blocks;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 ID
        {
            get
            {
                return _ID;
            }
        }
    }
}
