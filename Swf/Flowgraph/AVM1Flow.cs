using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Recurity.Swf.AVM1;

namespace Recurity.Swf.Flowgraph
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1CodeCFG : Dictionary<UInt32, AVM1BasicBlock> { }

    /// <summary>
    /// 
    /// </summary>
    public class AVM1IndexCFG : Dictionary<UInt32, BasicBlock> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instructionIndex"></param>
        /// <param name="indexInBlock"></param>
        /// <returns></returns>
        public BasicBlock BlockOfInstruction( UInt32 instructionIndex, out int indexInBlock )
        {
            foreach ( BasicBlock b in this.Values )
            {
                for ( int i = 0; i < b.Indices.Count; i++ )
                {
                    if ( instructionIndex == b.Indices[ i ] )
                    {
                        indexInBlock = i;
                        return b;
                    }
                }
            }

            indexInBlock = 0;
            return null;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class AVM1Flow
    {
        /// <summary>
        /// 
        /// </summary>
        private AVM1IndexCFG _CachedGraph;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public AVM1Flow( AVM1Code code )
        {
            if ( null == code )
            {
                ArgumentNullException e = new ArgumentNullException( "code is null" );
                Log.Error(this,  e );
                throw e;
            }                                   
                                                
            _CachedGraph = Coalesce( CFG( code ) );
        }

        /// <summary>
        /// 
        /// </summary>
        public AVM1IndexCFG CodeFlowGraph
        {
            get
            {
                return _CachedGraph;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AVM1IndexCFG Update( AVM1Code code )
        {
            _CachedGraph = Coalesce( CFG( code ) );
            return _CachedGraph;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexCFG"></param>
        /// <returns></returns>
        private static AVM1IndexCFG Coalesce( AVM1IndexCFG indexCFG )
        {
            bool graphChanged = false;

            //
            // Algorithm:
            // We iterate through all BasicBlocks and see if they have only
            // one edge going out, as well as the destination block has only one
            // edge going in. This is bound to be the one going out from this
            // block. In such case, the instructions of the neighbor become 
            // part of our block, we throw away our edge (to the neighbor) and
            // import his.
            // Safety check: the edge we remove (from us to the neighbor) must
            // be unconditional, so we don't eat branches or function declarations.
            //
            
            do
            {
                graphChanged = false;                                                

                foreach ( BasicBlock bi in indexCFG.Values )
                {
                    if ( 1 == OutDegree( bi ) )
                    {
                        BasicBlock follow = bi.OutEdges[ 0 ].Neighbor;

                        if ( ! (
                               ( bi.OutEdges[ 0 ].EType != EdgeType.Unconditional )
                            || ( bi.OutEdges[ 0 ].EType != EdgeType.UnconditionalBranch )
                            ) )
                        {
                            Exception up = new Exception(
                                "About to coalesce a flow graph edge of type "
                                + Enum.GetName( typeof( EdgeType ), bi.OutEdges[ 0 ].EType )
                                + " for node ID (parrent) = "
                                + bi.ID.ToString( "d" ) );
                            Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, up);
                            throw up;                            
                        }

                        if ( 1 == InDegree( indexCFG, follow ) )
                        {
                            bi.Indices.AddRange( follow.Indices );
                            bi.OutEdges.Clear();
                            bi.OutEdges.AddRange( follow.OutEdges );
                            indexCFG.Remove( follow.ID );

                            graphChanged = true;
                            break;
                        }
                    }
                }
            }
            while ( graphChanged );

            return indexCFG;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexCFG"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static UInt32 InDegree( AVM1IndexCFG indexCFG, BasicBlock b )
        {
            UInt32 inDeg = 0;

            foreach ( BasicBlock bi in indexCFG.Values )
            {
                for ( int i = 0; i < bi.OutEdges.Count; i++ )
                {
                    if ( bi.OutEdges[ i ].Neighbor.ID == b.ID )
                    {
                        inDeg++;
                    }
                }
            }

            return inDeg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static UInt32 OutDegree( BasicBlock b )
        {
            return (UInt32) b.OutEdges.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private AVM1IndexCFG CFG( AVM1Code code )
        {
            AVM1IndexCFG blocks = new AVM1IndexCFG();
            List<Edge> edges = new List<Edge>();

            //
            // First, get all edges in the code
            //

            for ( uint instIt = 0; instIt < code.Count; instIt++ )
            {
                //
                // Initially, every instruction gets its own basic block. We 
                // use a Dictionary to ensure that every instruction index is only
                // used once (would throw an Exception otherwise, as the key is the 
                // instruction index).
                // The Block's ID is the instruction index when it was created (should
                // be unique).
                //
                BasicBlock b = new BasicBlock( instIt );
                b.Indices.Add( instIt );
                blocks.Add( instIt, b );

                //
                // Instruction we are dealing with
                //
                AbstractAction inst = code[ (int)instIt ];

                if ( inst.ActionType == AVM1Actions.ActionTry )
                {
                    UInt32 endOfConstructIndex = 0;
                    ActionTry t = ( ActionTry )inst;
                    //
                    // the "try" pointer in ActionTry actually points past the try
                    // block to the beginning of the catch block if any
                    //
                    UInt32 tryIndex = code.Address2Index( ( UInt32 )( ( long )code.Index2Address( instIt ) + ( long )t.BranchTargetAdjusted ) );
                    endOfConstructIndex = tryIndex;
                    UInt32 catchIndex = code.Address2Index( ( UInt32 )( ( long )code.Index2Address( instIt ) + ( long )t.CatchTargetAdjusted ) );
                    UInt32 finallyIndex = code.Address2Index( ( UInt32 )( ( long )code.Index2Address( instIt ) + ( long )t.FinallyTargetAdjusted ) );

                    if ( t.HasCatch )
                    {
                        edges.Add( new Edge( instIt, tryIndex, EdgeType.Catch ) );                        
                        endOfConstructIndex = catchIndex;                        
                    }                    

                    //
                    // similar to above, the "catch" pointer points to the finally
                    // block if any is available. A target of 0 indicates there is none.
                    //
                    if ( t.HasFinally )
                    {                        
                        edges.Add( new Edge( instIt, endOfConstructIndex, EdgeType.Finally ) );
                        endOfConstructIndex = finallyIndex;
                    }
                    
                    //
                    // The instruction directly following the ActionTry is the 
                    // first instruction of the try block
                    //
                    if ( ( instIt + 1 ) < code.Count )
                    {
                        edges.Add( new Edge( instIt, instIt + 1, EdgeType.Try ) );
                    }

                    edges.Add( new Edge( instIt, endOfConstructIndex, EdgeType.TryEnd ) );
                }
                else if ( inst.ActionType == AVM1Actions.ActionDefineFunction )
                {
                    //
                    // Function definitions have two edges: 
                    // The next instruction may be part of the function, but the instruction
                    // after the function is unconditionally not.
                    //
                    ActionDefineFunction df = ( ActionDefineFunction )inst;
                    UInt32 endOfFunction = code.Address2Index( ( UInt32 )( ( long )code.Index2Address( instIt ) + ( long )df.BranchTargetAdjusted ) );
                    edges.Add( new Edge( instIt, endOfFunction, EdgeType.FunctionDeclaration ) );
                    if ( ( instIt + 1 ) < code.Count )
                    {
                        edges.Add( new Edge( instIt, instIt + 1, EdgeType.FunctionBody ) );
                    }
                }
                else if ( inst.ActionType == AVM1Actions.ActionDefineFunction2 )
                {
                    //
                    // Function definitions have two edges: 
                    // The next instruction may be part of the function, but the instruction
                    // after the function is unconditionally not.
                    //
                    ActionDefineFunction2 df = ( ActionDefineFunction2 )inst;
                    UInt32 endOfFunction = code.Address2Index( ( UInt32 )( ( long )code.Index2Address( instIt ) + ( long )df.BranchTargetAdjusted ) );
                    edges.Add( new Edge( instIt, endOfFunction, EdgeType.FunctionDeclaration ) );
                    if ( ( instIt + 1 ) < code.Count )
                    {
                        edges.Add( new Edge( instIt, instIt + 1, EdgeType.FunctionBody ) );
                    }
                }
                else if ( ( inst.IsBranch ) && ( ! inst.IsConditional ) )
                {
                    //
                    // Unconditional branch, nothing to see here
                    //
                    edges.Add( new Edge( instIt, code.Branch2Index( instIt ), EdgeType.UnconditionalBranch ) );
                }
                else if ( inst.IsConditional )
                {
                    //
                    // Conditional branch, either taken (when true) or just
                    // skipped (when false). Two edges.
                    //
                    edges.Add( new Edge( instIt, code.Branch2Index( instIt ), EdgeType.ConditionalTrue ) );
                    if ( ( instIt + 1 ) < code.Count )
                    {
                        edges.Add( new Edge( instIt, instIt + 1, EdgeType.ConditionalFalse ) );
                    }
                }
                else
                {
                    //
                    // Any other code falls unconditionally into the following
                    // instruction, if there is any.
                    //
                    if ( ( instIt + 1 ) < code.Count )
                    {
                        edges.Add( new Edge( instIt, instIt + 1, EdgeType.Unconditional ) );
                    }
                }
            }

            //
            // Over all those edges, create the neighbor information for our blocks.
            // Neighbors have a destination (instruction index) and the type.
            //
            for ( int i = 0; i < edges.Count; i++ )
            {
                BasicBlockEdge be = new BasicBlockEdge( blocks[ edges[ i ].ToIndex ], edges[ i ].EdgeType );
                                
                blocks[ edges[ i ].FromIndex ].OutEdges.Add( be );
            }
           
            return blocks;
        }

        #region AVM1 Code Generation from CFG

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AVM1CodeCFG GetAVM1CodeFlowGraph( AVM1Code code )
        {
            return this.GetAVM1CodeFlowGraph( _CachedGraph, code );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexCFG"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public AVM1CodeCFG GetAVM1CodeFlowGraph( AVM1IndexCFG indexCFG, AVM1Code code )
        {
            AVM1CodeCFG newGraph = new AVM1CodeCFG();

            // copy blocks
            foreach ( UInt32 k in indexCFG.Keys )
            {
                AVM1BasicBlock ab = new AVM1BasicBlock( indexCFG[ k ].ID );
                for ( int i = 0; i < indexCFG[ k ].Indices.Count; i++ )
                {
                    ab.Instructions.Add( code[ ( int )indexCFG[ k ].Indices[ i ] ] );
                }
                newGraph.Add( k, ab );
            }
            // copy edges
            foreach ( UInt32 k in indexCFG.Keys )
            {
                for ( int i = 0; i < indexCFG[ k ].OutEdges.Count; i++ )
                {
                    AVM1BasicBlockEdge e = new AVM1BasicBlockEdge( newGraph[ indexCFG[ k ].OutEdges[ i ].Neighbor.ID ], indexCFG[ k ].OutEdges[ i ].EType );
                    newGraph[ k ].OutEdges.Add( e );
                }
            }

            return newGraph;
        }        

        /*
        public AVM1Code CodeFromAVM1FlowGraph( AVM1CodeCFG avm1cfg )
        {
            log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
            AVM1IndexCFG newcfg = new AVM1IndexCFG();

            //
            // TODO: test graph correctness            
            //
            throw new NotImplementedException( "This is not correct and very slow" );
            
            SortedList<UInt32, AVM1BasicBlock> sortedBlocks = new SortedList<uint, AVM1BasicBlock>();
            AVM1InstructionSequence newInstructions = new AVM1InstructionSequence();            

            foreach ( UInt32 k in  avm1cfg.Keys )
            {
                sortedBlocks.Add( k, avm1cfg[ k ] );
            }

            IList<UInt32> blockIdList = sortedBlocks.Keys;                       

            foreach ( UInt32 id in blockIdList )
            {    
                // create basic blocks with corresponding IDs         
                BasicBlock nb = new BasicBlock( id );                

                // copy the instructions into the new code list
                // at the same time, note the instruction indices in the new block
                for ( int i = 0; i < sortedBlocks[ id ].Instructions.Count; i++ )
                {
                    newInstructions.Add( sortedBlocks[ id ].Instructions[ i ] );
                    nb.Indices.Add( (uint)( newInstructions.Count - 1 ) );
                }
                // add the new block
                newcfg.Add( id, nb );
            }
            //
            // now we port the edge information from the sorted list over
            // to the new IndexCFG. The edge information must stay the same
            // or the whole stunt will not work.
            //
            foreach ( UInt32 id in blockIdList )
            {
                for ( int i = 0; i < sortedBlocks[id].OutEdges.Count; i++ )
                {
                    BasicBlockEdge e = new BasicBlockEdge( newcfg[ sortedBlocks[id].OutEdges[i].Neighbor.ID ], sortedBlocks[id].OutEdges[i].EType );
                    newcfg[ id ].OutEdges.Add( e );
                }
            }

            //
            // new AVM1Code instance
            //
            AVM1Code newCode = new AVM1Code( newInstructions );

            foreach ( UInt32 k in newcfg.Keys )
            {
                //
                // Last Action of basic block:
                // By definition, this is the only action of the basic block
                // that will need adjustment now. We perform the adjustment 
                // using the edge type information.
                //
                UInt32 bIndex = newcfg[ k ].Indices[ newcfg[ k ].Indices.Count - 1 ];
                AbstractAction bAction = newCode[ (int)bIndex ];
                UInt32 bAddress = newCode.Index2Address( bIndex );

                for ( int i = 0; i < newcfg[ k ].OutEdges.Count; i++ )
                {                                        
                    if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.ConditionalTrue )
                    {
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        int offset = ( int )( ( long )dest - ( long )bAddress );
                        bAction.BranchTargetAdjusted = offset;
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.ConditionalFalse )
                    {
                        // nothing to be done here
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.FunctionBody )
                    {
                        // nothing
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.FunctionDeclaration )
                    {
                        // ActionDefineFunction
                        // ActionDefineFunction2
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        uint offset = ( uint )( ( long )dest - ( long )bAddress );                        
                        bAction.BranchTargetAdjusted = (int)offset;
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.Unconditional )
                    {
                        // nothing
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.UnconditionalBranch )
                    {
                        // ActionIf
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        int offset = ( int )( ( long )dest - ( long )bAddress );
                        bAction.BranchTargetAdjusted = offset;
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.Try )
                    {
                        // nothing, try block follows directly
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.Catch )
                    {                        
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        uint offset = ( uint )( ( long )dest - ( long )bAddress );
                        bAction.BranchTargetAdjusted = (ushort)offset;
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.Finally )
                    {
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        uint offset = ( uint )( ( long )dest - ( long )bAddress );
                        ((ActionTry)bAction).CatchTargetAdjusted = (ushort)offset;
                    }
                    else if ( newcfg[ k ].OutEdges[ i ].EType == EdgeType.TryEnd )
                    {
                        UInt32 dest = newCode.Index2Address( newcfg[ k ].OutEdges[ i ].Neighbor.Indices[ 0 ] );
                        uint offset = ( uint )( ( long )dest - ( long )bAddress );

                        ActionTry t = ( ActionTry )bAction;
                        if ( t.HasFinally )
                        {
                            t.FinallyTargetAdjusted = ( ushort )offset;
                        }
                        else if ( t.HasCatch )
                        {
                            t.CatchTargetAdjusted = ( ushort )offset;
                        }
                        else
                        {
                            bAction.BranchTargetAdjusted = ( int )offset;
                        }
                    }
                    else
                    {
                        throw new Exception( "Edge type " + newcfg[ k ].OutEdges[ i ].EType.ToString( "X02" ) + " not handled" );
                    }
                }
            }

            if ( ! newCode.Verify() )
            {
                throw new Exception( "Graphidiot" );
            }            

            this.WriteGML( @"c:\Hack\code\SilverSec\out\__NEWCODE.gml", newCode );                                                
            return newCode;
        }
        */

        #endregion AVM1 Code Generation from CFG

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="code"></param>
        public void WriteGML( string filename, AVM1Code code )
        {
            AVM1CodeCFG ccfg = this.GetAVM1CodeFlowGraph( this.CodeFlowGraph, code );
            this.WriteGML( filename, ccfg );
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="avm1FlowGraph"></param>
        public void WriteGML( string filename, AVM1CodeCFG avm1FlowGraph )
        {
            using ( StreamWriter sw = new StreamWriter( filename ) )
            {
                sw.WriteLine( "Creator \"Blitzableiter\"" );
                sw.WriteLine( "Version \"2.7\"" );
                sw.WriteLine( "graph" );
                sw.WriteLine( "[" );
                sw.WriteLine( "\thierarchic\t1" );
                sw.WriteLine( "\tlabel\t\"\"" );
                sw.WriteLine( "\tdirected\t1" );

                foreach ( UInt32 k in avm1FlowGraph.Keys )
                {
                    AVM1BasicBlock b = avm1FlowGraph[ k ];

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat( "ID: {0:d}\n", b.ID );
                    for ( int j = 0; j < b.Instructions.Count; j++ )
                    {
                        sb.AppendFormat( "{0:d}.{1:d} {2}\n", k, j, b.Instructions[j].ToString() );
                    }
                    sb.Replace( "\"", "''" );
                    sb.Replace( "Recurity.Swf.AVM1.", "" );

                    sw.WriteLine( "\tnode" );
                    sw.WriteLine( "\t[" );
                    sw.WriteLine( "\t\tid\t" + k.ToString( "d" ) );
                    sw.Write( "\t\tlabel\t\"" );
                    sw.Write( sb.ToString() );
                    sw.WriteLine( "\"" );

                    sw.WriteLine( "\t\tLabelGraphics" );
                    sw.WriteLine( "\t\t[" );
                    sw.Write( "\t\t\ttext\t\"" );
                    sw.Write( sb.ToString() );
                    sw.WriteLine( "\"" );
                    sw.WriteLine( "\t\t]" );

                    sw.WriteLine( "\t]" );
                }

                foreach ( UInt32 k in avm1FlowGraph.Keys )
                {
                    AVM1BasicBlock b = avm1FlowGraph[ k ];

                    for ( int j = 0; j < b.OutEdges.Count; j++ )
                    {
                        sw.WriteLine( "\tedge" );
                        sw.WriteLine( "\t[" );

                        sw.WriteLine( "\t\tsource\t" + k.ToString( "d" ) );
                        sw.WriteLine( "\t\ttarget\t" + b.OutEdges[ j ].Neighbor.ID.ToString( "d" ) );
                        sw.Write( "\t\tlabel\t\"" );
                        sw.Write( Enum.GetName( typeof( EdgeType ), b.OutEdges[ j ].EType ) );
                        sw.WriteLine( "\"" );
                        sw.WriteLine( "\t\tgraphics" );
                        sw.WriteLine( "\t\t[" );
                        sw.WriteLine( "\t\t\tfill\t\"#000000\"" );
                        sw.WriteLine( "\t\t\ttargetArrow\t\"standard\"" );
                        sw.WriteLine( "\t\t]" );
                        sw.WriteLine( "\t\tLabelGraphics" );
                        sw.WriteLine( "\t\t[" );
                        sw.Write( "\t\t\ttext\t\"" );
                        sw.Write( Enum.GetName( typeof( EdgeType ), b.OutEdges[ j ].EType ) );
                        sw.WriteLine( "\"" );
                        sw.WriteLine( "\t\t]" );

                        sw.WriteLine( "\t]" );
                    }
                }

                sw.WriteLine( "]" );
            }
        }   
    }
}
