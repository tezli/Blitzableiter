using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Recurity.Swf.AVM1;

using Recurity.Swf.Flowgraph;

namespace Recurity.Swf.AVM1Modifier
{
    /// <summary>
    /// 
    /// </summary>
    public class ModLib
    {
        /// <summary>
        /// 
        /// </summary>
        private List<ModVariable> _Variables;

        /// <summary>
        /// 
        /// </summary>
        private List<Modification> _Modifications;

        /// <summary>
        /// 
        /// </summary>
        private List<Modification> _Functions;

        /// <summary>
        /// 
        /// </summary>
        public ModLib()
        {
            _Variables = new List<ModVariable>();
            _Modifications = new List<Modification>();
            _Functions = new List<Modification>();
        }

        /*
        public bool InitializeFromAppConfig()
        {
            log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

            bool loadedSomething = false;

            if ( null == Configuration.Config.Instance.Settings.Modifications )
                return false;

            for ( int i = 0; i < Configuration.Config.Instance.Settings.Modifications.Count; i++ )
            {
                string actionCode = Configuration.Config.Instance.Settings.Modifications[ i ].ActionCode;
                string modFile = Configuration.Config.Instance.Settings.Modifications[ i ].Mod;

                if ( ( null == actionCode ) || ( null == modFile ) )
                    continue;

                if ( actionCode.Equals( "function", StringComparison.InvariantCultureIgnoreCase ) )
                {
                    //
                    // this is a global function definition
                    //
                    if ( this.AddFunction( modFile ) )
                        loadedSomething = true;
                }
                else
                {
                    //
                    // this is an ActionCode replacement
                    //
                    AVM1Actions action = default( AVM1Actions );

                    try
                    {
                        action = (AVM1Actions) Enum.Parse( typeof( AVM1Actions ), actionCode, false );
                    }
                    catch ( ArgumentException )
                    {
                       Log.ErrorFormat( "Illegal modification configuration: '{0}' is not a valid AVM1 ActionCode", actionCode );
                        continue;
                    }

                    if ( this.AddModification( action, modFile ) )
                        loadedSomething = true;
                }
            }

            return loadedSomething;
        }
         */

        /// <summary>
        /// 
        /// </summary>
        public List<ModVariable> Variables
        {
            get
            {
                return _Variables;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddVariable( string name, string value )
        {
            ModVariable v = new ModVariable(name, value );
            _Variables.Add( v );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddVariableRandomString( string name )
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[ 16 ];
            bool uniq = true;
            StringBuilder sb = null;

            do
            {
                rng.GetBytes( random );

                sb = new StringBuilder();
                sb.Append( "BLITZ" );

                for ( int i = 0; i < random.Length; i++ )
                {
                    sb.AppendFormat( "{0:X02}", random[ i ] );
                }

                for ( int i = 0; i < _Variables.Count; i++ )
                {
                    if ( _Variables[ i ].Value.Equals( sb.ToString() ) )
                    {
                        uniq = false;
                        break;
                    }
                }
            } while ( ! uniq );

            this.AddVariable( name, sb.ToString() );
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Modification> Modifications
        {
            get
            {
                return _Modifications;
            }
        }

        /*
        public bool AddModification( AVM1Actions actionToBeReplaced, string filenameOfModification )
        {
            Modification m = new Modification( actionToBeReplaced );
            bool result = m.Load( filenameOfModification, _Variables );
            if (result)
                _Modifications.Add( m );
            return result;
        }
         */

        /// <summary>
        /// 
        /// </summary>
        public List<Modification> Functions
        {
            get
            {
                return _Functions;
            }
        }

        /*
        public bool AddFunction( string filenameOfModification )
        {
            Modification m = new Modification( AVM1Actions.ActionDefineFunction2 ); // the actionCode is not used here
            bool result = m.Load( filenameOfModification, _Variables );
            if ( result )
                _Functions.Add( m );

            return result;
        }
         */

        /*
        public void ApplyAll( AVM1CodeCFG ccfg )
        {
            for ( int modFuncs = 0; modFuncs < _Functions.Count; modFuncs++ )
            {
                AVM1BasicBlock head = ccfg[ 0 ];
                for ( int i = 0; i < _Functions[ modFuncs ].Code.Count; i++ )
                {
                    head.Instructions.Insert( i, _Functions[ modFuncs ].Code[ i ] );                    
                }
            }

            for ( int modi = 0; modi < _Modifications.Count; modi++ )
            {
                foreach ( UInt32 id in ccfg.Keys )
                {
                    for ( int i = 0; i < ccfg[ id ].Instructions.Count; i++ )
                    {
                        if ( ccfg[ id ].Instructions[ i ].ActionType == _Modifications[ modi ].Victim )
                        {
                            ccfg[ id ].Instructions.RemoveAt( i );
                            for ( int j = 0; j < _Modifications[ modi ].Code.Count; j++ )
                            {
                                ccfg[ id ].Instructions.Insert( i + j, _Modifications[ modi ].Code[ j ] );
                            }
                            i += _Modifications[ modi ].Code.Count;
                        }
                    }
                }
            }
        }
         */        
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeToBePatched"></param>
        public void ApplyAll( AVM1Code codeToBePatched )
        {
            for ( int modFuncs = 0; modFuncs < _Functions.Count; modFuncs++ )
            {
                codeToBePatched.InjectAt( 0, _Functions[ modFuncs ].Code );
            }

            for ( int modIndex = 0; modIndex < _Modifications.Count; modIndex++ )
            {                                    
                codeToBePatched.InjectAt( (int)_Modifications[ modIndex ].IndexOfModification, _Modifications[ modIndex ].Code );
                int removalPoint = (int)_Modifications[modIndex].IndexOfModification + _Modifications[ modIndex ].Code.Count;
                codeToBePatched.RemoveAt( removalPoint );
            }            
        }
        
        /*
        public bool WouldPatch( AVM1Code codeToBePatched )
        {
            for ( int modIndex = 0; modIndex < _Modifications.Count; modIndex++ )
            {
                for ( int codeIndex = 0; codeIndex < codeToBePatched.Count; codeIndex++ )
                {
                    if ( codeToBePatched[ codeIndex ].ActionType == _Modifications[ modIndex ].Victim )
                    {
                        return true;
                    }
                }
            }

            return false;            
        } 
         */
    }
}
