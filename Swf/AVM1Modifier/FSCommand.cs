using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.AVM1;
using Recurity.Swf.AVM1.Stack;

namespace Recurity.Swf.AVM1Modifier
{
    /// <summary>
    /// 
    /// </summary>
    public class FSCommand
    {
        /// <summary>
        /// 
        /// </summary>
        protected enum PatchState
        {

            /// <summary>
            /// 
            /// </summary>
            Indeterministic,

            /// <summary>
            /// 
            /// </summary>
            Change,

            /// <summary>
            /// 
            /// </summary>
            Innocent
        }


        /// <summary>
        /// 
        /// </summary>
        protected CheckMachine.Machine _Machine;

        /// <summary>
        /// 
        /// </summary>
        protected AVM1Actions _TriggerAction;

        /// <summary>
        /// 
        /// </summary>
        protected List<BuildingBlocks.AbstractBuildingBlock> _Check;

        /// <summary>
        /// 
        /// </summary>
        public FSCommand()
        {
            _TriggerAction = AVM1Actions.ActionGetURL2;
            _Machine = new CheckMachine.Machine();

            _Check = new List<BuildingBlocks.AbstractBuildingBlock>();
            _Check.Add( new BuildingBlocks.ArgN( 1 ) );
            _Check.Add( new BuildingBlocks.BeginsWith( "FSCommand:", true ) );
            _Check.Add( new BuildingBlocks.ConditionalIfFalseExecOrig() );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="modLibrary"></param>
        /// <returns></returns>
        public bool PatchPrepare( AVM1Code code, ModLib modLibrary )
        {
            List<int> triggerPositions = new List<int>();

            // check if trigger action is found, if not bail
            for ( int i = 0; i < code.Count; i++ )
            {
                if ( code[ i ].ActionType == _TriggerAction )
                {
                    triggerPositions.Add( i );                    
                }
            }

            if ( 0 == triggerPositions.Count )
                return false;

            // check if static analysis can show no need to patch
            List<int> patchPositions = new List<int>();
            List<int> modificationPositions = new List<int>();

            for ( int i = 0; i < triggerPositions.Count; i++ )
            {
                switch ( NeedsPatching( code, triggerPositions[i] ) )
                {
                    case PatchState.Innocent:
                        // ignore
                        break;

                    case PatchState.Indeterministic:
                        patchPositions.Add( triggerPositions[ i ] );
                        break;

                    case PatchState.Change:
                        modificationPositions.Add( triggerPositions[ i ] );
                        break;

                    default:
                        throw new Exception( "FUCKUP, cannot even handle an enum correctly" );
                }
            }

            // now patch the indeterministic ones

            for ( int i = 0; i < patchPositions.Count; i++ )
            {
                Modification mod = this.GetInlineCode( code, patchPositions[ i ] );
                modLibrary.Modifications.Add( mod );
            }

            // now, patch the ones that need to go!
            // ... this.GetRemovalCode( ... );

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <returns></returns>
        protected PatchState NeedsPatching( AVM1Code code, int instructionIndex )
        {
            _Machine.Reset();

            if ( _Machine.Run( code, instructionIndex, _Check ) )
            {
                // the run completed, that's good                

                // check the result
                CheckMachine.MachineStackEntry e = _Machine.MachineResult;

                if ( e.Type == CheckMachine.MachineStackType.KeepInstruction )
                    return PatchState.Innocent;
                else if ( e.Type == CheckMachine.MachineStackType.RemoveInstruction )
                    return PatchState.Change;
                else
                    throw new Exception( "FUCKUP! Final CheckMachine State is wongobongo" );
            }
            else
            {
                return PatchState.Indeterministic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="instructionIndex"></param>
        /// <returns></returns>
        public Modification GetInlineCode( AVM1Code code, int instructionIndex )
        {            
            List<string> all = new List<string>();
            for ( int i = 0; i < _Check.Count; i++ )
            {
                for ( int j = 0; j < _Check[ i ].InlineSource.Count; j++ )
                {
                    all.Add( _Check[ i ].InlineSource[ j ] );
                    
                }
            }            

            Modification m = new Modification( (uint)instructionIndex, code[ instructionIndex ], "OriginalAction" );
            m.Load( all, new List<ModVariable>() );
            return m;
        }
    }
}
