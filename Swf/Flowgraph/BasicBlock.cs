using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.Flowgraph
{    
    /// <summary>
    /// 
    /// </summary>
    public class BasicBlockEdge
    {

        /// <summary>
        /// 
        /// </summary>
        public BasicBlock Neighbor;

        /// <summary>
        /// 
        /// </summary>
        public EdgeType EType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbor"></param>
        /// <param name="eType"></param>
        public BasicBlockEdge( BasicBlock neighbor, EdgeType eType )
        {
            Neighbor = neighbor;
            EType = eType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "-> " + Neighbor.ID + " (" + Enum.GetName( typeof( EdgeType ), EType ) + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BasicBlock
    {

        /// <summary>
        /// 
        /// </summary>
        private List<UInt32> _InstructionIndices;

        /// <summary>
        /// 
        /// </summary>
        private List<BasicBlockEdge> _Blocks;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _ID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public BasicBlock( UInt32 id )
        {
            _ID = id;
            _InstructionIndices = new List<UInt32>();
            _Blocks = new List<BasicBlockEdge>();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<UInt32> Indices
        {
            get
            {
                return _InstructionIndices;
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        public List<BasicBlockEdge> OutEdges
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
