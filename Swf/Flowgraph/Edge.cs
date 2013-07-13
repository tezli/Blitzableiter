using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.Flowgraph
{
    /// <summary>
    /// 
    /// </summary>
    public enum EdgeType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        UNDEFINED = 0,
        /// <summary>
        /// 
        /// </summary>
        Unconditional = 1,
        /// <summary>
        /// 
        /// </summary>
        UnconditionalBranch = 2,
        /// <summary>
        /// 
        /// </summary>
        ConditionalTrue = 3,
        /// <summary>
        /// 
        /// </summary>
        ConditionalFalse = 4,
        /// <summary>
        /// 
        /// </summary>
        Try = 5,
        /// <summary>
        /// 
        /// </summary>
        Catch = 6,
        /// <summary>
        /// 
        /// </summary>
        Finally = 7,
        /// <summary>
        /// 
        /// </summary>
        TryEnd = 8,
        /// <summary>
        /// 
        /// </summary>
        FunctionDeclaration = 9,
        /// <summary>
        /// 
        /// </summary>
        FunctionBody = 10
    }

    /// <summary>
    /// 
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _SourceIndex;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _DestinationIndex;

        /// <summary>
        /// 
        /// </summary>
        private EdgeType _EType;

        /*
        public Edge( UInt32 from, UInt32 to )
        {
            _SourceIndex = from;
            _DestinationIndex = to;
        }
         */
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="typeOfEdge"></param>
        public Edge( UInt32 from, UInt32 to, EdgeType typeOfEdge )
        {
            _SourceIndex = from;
            _DestinationIndex = to;
            _EType = typeOfEdge;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public EdgeType EdgeType
        {
            get
            {
                return ( _EType );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 FromIndex
        {
            get
            {
                return _SourceIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 ToIndex
        {
            get
            {
                return _DestinationIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _SourceIndex.ToString( "d" ) + " -> " + _DestinationIndex.ToString( "d" );
        }
    }
}
