using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Recurity.Swf.AVM1;

namespace Recurity.Swf.AVM1Modifier
{
    /// <summary>
    /// TODO : Documentation
    /// </summary>
    public class Modification
    {
        private class CodePoint
        {
            /// <summary>
            /// 
            /// </summary>
            public string Source;

            /// <summary>
            /// 
            /// </summary>
            public string Label;

            /// <summary>
            /// 
            /// </summary>
            public AbstractAction Code;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ModVariable> _Variables;

        /// <summary>
        /// 
        /// </summary>
        private List<CodePoint> _InnerCode;

        /// <summary>
        /// 
        /// </summary>
        private List<string> _SourceCode;

        /// <summary>
        /// 
        /// </summary>
        private AVM1Code _Code;

        /// <summary>
        /// 
        /// </summary>
        private uint _IndexOfModification;
        // for instruction preservation:

        /// <summary>
        /// 
        /// </summary>
        private AbstractAction _OriginalInstruction;

        /// <summary>
        /// 
        /// </summary>
        private string _OriginalInstructionMarker;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexOfMod"></param>
        public Modification( uint indexOfMod )
        {
            _IndexOfModification = indexOfMod;
            _OriginalInstruction = null;
            _OriginalInstructionMarker = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexOfMod"></param>
        /// <param name="originalInstruction"></param>
        /// <param name="origInstructionMarker"></param>
        public Modification( uint indexOfMod, AbstractAction originalInstruction, string origInstructionMarker )
            : this( indexOfMod )
        {
            _OriginalInstruction = originalInstruction;
            _OriginalInstructionMarker = origInstructionMarker;
        }            

        /// <summary>
        /// 
        /// </summary>
        public uint IndexOfModification
        {
            get
            {
                return _IndexOfModification;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AVM1Code Code
        {
            get
            {
                return _Code;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public bool Load( string filename, List<ModVariable> variables )
        {
            _Variables = variables;

            FileInfo fi = new FileInfo( filename );
            if ( !fi.Exists )
            {
               Log.Error(this,  "File not found: " + fi.FullName );
                return false;
            }

            StreamReader reader;
            try
            {
                Stream sourceS = new FileStream( filename, FileMode.Open, FileAccess.Read );
                reader = new StreamReader( sourceS );

                _SourceCode = new List<string>();
                string oneLine;
                while ( ( oneLine = reader.ReadLine() ) != null )
                {
                    _SourceCode.Add( oneLine );
                }
            }
            catch ( IOException e )
            {
               Log.Error(this, e);
                return false;
            }

            reader.Close();

            return this.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceCode"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public bool Load( List<string> SourceCode, List<ModVariable> variables )
        {
            _Variables = variables;
            _SourceCode = SourceCode;

            return this.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Load()
        {
            bool result;

            RemoveComments();
            result = Populate();

            AVM1InstructionSequence bytecode;
            //
            // Now fill the _Code list for quick access
            //
            if ( result )
            {
                bytecode = new AVM1InstructionSequence();
                for ( int i = 0; i < _InnerCode.Count; i++ )
                {
                    bytecode.Add( _InnerCode[ i ].Code );
                }

                _Code = new AVM1Code( bytecode );

                String s = String.Format("Modification with {0:d} instructions loaded", _Code.Count );
                //Log.Debug(this, s);
            }
            else
            {
               //Log.Debug(this,  "Error loading modification" );
            }            

            return result;
        }

        /// <summary>
        /// TODO : Documentation
        /// </summary>
        /// <returns></returns>
        private bool Populate()
        {
            bool result = true;
            _InnerCode = new List<CodePoint>();

            //
            // Add labels and source code 
            //
            CodePoint p = new CodePoint();
            for ( int i = 0; i < _SourceCode.Count; i++ )
            {                
                if ( _SourceCode[ i ].EndsWith( ":" ) && ( ! _SourceCode[i].Contains(" ") ) )
                {
                    p.Label = _SourceCode[ i ];
                }
                else
                {
                    p.Source = _SourceCode[ i ];
                    _InnerCode.Add( p );
                    p = new CodePoint();
                }
            }
            // Support for Label-at-the-end-of-code special case
            // Here: the CodePoint with label but without code is added
            if ( null != p.Label )
            {
                _InnerCode.Add( p );
            }


            for ( int i = 0; i < _InnerCode.Count; i++ )
            {
                string actionName;
                string statement = null;

                // Support for Label-at-the-end-of-code special case
                // Here: Don't try to render last label into code as it doesn't have any
                if ( null == _InnerCode[ i ].Source )
                    continue;

                try
                {
                    statement = PrepareAndReplaceLabel( _InnerCode[ i ].Source, out actionName );
                }
                catch ( AVM1ExceptionSourceFormat e )
                {
                    String s = String.Format( "Preparation failed: {0}, Error was: {1}", _InnerCode[ i ].Source, e.Message );
                    Log.Error(this, s);
                    result = false;
                    break;
                }
                
                try
                {
                    AbstractAction a;
                    if ( ( null != _OriginalInstructionMarker) && ( actionName.Equals( _OriginalInstructionMarker, StringComparison.InvariantCulture ) ) )
                    {
                        a = _OriginalInstruction;
                    }
                    else
                    {
                        a = AVM1.AVM1Factory.Create( actionName, statement );
                    }
                    _InnerCode[ i ].Code = a;
                   //Log.Debug(this,  a.ToString() );
                }
                catch ( AVM1ExceptionSourceFormat )
                {
                   Log.Error(this,  "Syntax error: " + _InnerCode[ i ].Source );
                    result = false;
                    break;
                }
            }

            if ( false == result )
                return result;

            // Support for Label-at-the-end-of-code special case
            // Here: remove the _InnerCode element, as it's no longer needed since the 
            //       branch targets have been resolved to indices
            if ( null == _InnerCode[ _InnerCode.Count - 1 ].Source )
                _InnerCode.RemoveAt( _InnerCode.Count - 1 );

            //
            // resolve the branch targets (which are still indices) into actual
            // byte addresses
            //
            for ( int i = 0; i < _InnerCode.Count; i++ )
            {
                // TODO: make this better!
                if ( ( _InnerCode[ i ].Code.IsBranch ) 
                    || ( _InnerCode[ i ].Code.ActionType == AVM1Actions.ActionDefineFunction )
                    || ( _InnerCode[ i ].Code.ActionType == AVM1Actions.ActionDefineFunction2 ) )
                {
                    if ( _InnerCode[ i ].Code.BranchTarget > _InnerCode.Count )
                    {
                       Log.Error(this,  _InnerCode[ i ].Source + " branch target out of range (" + 
                            _InnerCode[ i ].Code.BranchTarget.ToString( "d" ) + " > " + 
                            _InnerCode.Count.ToString( "d" ) + ")" );
                        result = false;
                        break;
                    }

                    int replacementTarget = _InnerCode[ i ].Code.BranchTarget;
                    int begin = i < replacementTarget ? i : replacementTarget;
                    int end = i > replacementTarget ? i : replacementTarget;                    

                    uint codesize = 0;
                    for ( int j = begin; j < end; j++ )
                    {
                        codesize += _InnerCode[ j ].Code.ActionLength;
                    }
                    _InnerCode[ i ].Code.BranchTargetAdjusted = (int)(i < replacementTarget? codesize : ( -1 * codesize ));
                }
            }            

            return result;
        }
        /// <summary>
        /// TODO : Documentation
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private string PrepareAndReplaceLabel( string statement, out string actionName )
        {
            actionName = null;

            char[] delims = { ' ', '\t' };
            string[] token = statement.Split( delims, StringSplitOptions.RemoveEmptyEntries );

            if ( 0 == token.Length )
                return null;

            actionName = token[ 0 ];

            for ( int i = 0; i < token.Length; i++ )
            {
                //
                // replace Label in statement (token ending in ':') with 
                // index of instruction referenced. Index is resolved in Populate().
                //
                if ( token[ i ].EndsWith( ":" ) )
                {
                    //
                    // find label
                    //
                    for ( int j = 0; j < _InnerCode.Count; j++ )
                    {
                        if ( null != _InnerCode[ j ].Label )
                        {
                            if ( _InnerCode[ j ].Label.Equals( token[ i ], StringComparison.InvariantCulture ) )
                            {
                                token[ i ] = j.ToString( "d" );
                            }
                        }
                    }
                }

                //
                // replace variables
                //
                if ( token[ i ].Contains( "$" ) )
                {
                    Match m = Regex.Match( token[i], @"(\$[A-Z]+)" );
                    if ( m.Success )
                    {
                        string vName = m.Value;
                        bool replacementDone = false;

                        for ( int j = 0; j < _Variables.Count; j++ )
                        {
                            if ( vName.Equals( _Variables[ j ].Name, StringComparison.InvariantCulture ) )
                            {
                                token[ i ] = token[ i ].Replace( vName, _Variables[ j ].Value );
                                replacementDone = true;
                                break;
                            }
                        }

                        if ( !replacementDone )
                        {
                            AVM1ExceptionSourceFormat e = new AVM1ExceptionSourceFormat( "Variable " + vName + " not found" );
                            throw e;
                        }
                    }
                }
            }

            return String.Join( " ", token );            
        }

        /// <summary>
        /// TODO : Documentation
        /// </summary>
        private void RemoveComments()
        {
            List<string> n = new List<string>();

            for ( int i = 0; i < _SourceCode.Count; i++ )
            {
                if (
                    ( ! ( _SourceCode[ i ].StartsWith( "#" ) ) )
                    &&
                    ( ! ( _SourceCode[i].Length < 2 ) )
                    )
                {
                    n.Add( _SourceCode[ i ].Trim() );
                }                
            }            
            _SourceCode = n;
        }       
    }
}
