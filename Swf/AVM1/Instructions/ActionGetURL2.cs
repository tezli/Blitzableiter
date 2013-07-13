using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGetURL2 represents the Adobe AVM1 ActionGetURL2
    /// </summary>
    public class ActionGetURL2 : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        public enum SendVarsMethods
        {

            /// <summary>
            /// 
            /// </summary>
            Method_None = 0,

            /// <summary>
            /// 
            /// </summary>
            Method_GET = 1,

            /// <summary>
            /// 
            /// </summary>
            Method_POST = 2
        }

        /// <summary>
        /// 
        /// </summary>
        protected SendVarsMethods _sendVarsMethod;

        /// <summary>
        /// 
        /// </summary>
        protected bool _loadTargetFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _loadVariablesFlag;

        #endregion

        #region constructors:

        /// <summary>
        /// 
        /// </summary>
        public ActionGetURL2()
        {
            this._sendVarsMethod = SendVarsMethods.Method_GET;
            this._loadTargetFlag = false;
            this._loadVariablesFlag = false;

            _StackOps = new StackChange[ 2 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // target
            _StackOps[ 1 ] = new StackPop( AVM1DataTypes.AVM_String ); // URL
        }

        /// <summary>
        /// Gets a URL
        /// </summary>
        /// <param name="method">One of the the three request methods: None, GET or POST)</param>
        /// <param name="loadTarget">The target. True (Target is a browser window), false (Target is a path to a sprite)</param>
        /// <param name="loadVars">Load the variables. True (variables to load) or false(no variables to load)</param>
        public ActionGetURL2( ActionGetURL2.SendVarsMethods method, bool loadTarget, bool loadVars ) : this()
        {
            this._sendVarsMethod = method;
            this._loadTargetFlag = loadTarget;
            this._loadVariablesFlag = loadVars;

        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public SendVarsMethods Method 
        { 
            get 
            { 
                return this._sendVarsMethod; 
            } 
            set 
            { 
                this._sendVarsMethod = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LoadTargetFlag 
        { 
            get 
            { 
                return this._loadTargetFlag; 
            } 
            set 
            { 
                this._loadTargetFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LoadVariablesFlag { get { return this._loadVariablesFlag; } set { this._loadVariablesFlag = value; } }

        #endregion

        #region code:

        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 4; 
            }
        }

        /// <summary>
        /// Parses the method and the flags from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            if ( 1 != _length )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionGetURL2 length missmatch" );
            }

            BitStream bits = new BitStream( sourceStream.BaseStream );

            uint method = bits.GetBits( 2 );
            if ( 3 == method )
            {
                //
                // This is illegal and other implementations agree (e.g. gnash, 
                // see http://www.mail-archive.com/gnash-commit@gnu.org/msg00141.html)
                //
                throw new AVM1ExceptionByteCodeFormat( "ActionGetURL2 SendVarsMethod = 3 is illegal" );
            }
            _sendVarsMethod = ( SendVarsMethods )method;

            if ( 0 != bits.GetBits( 4 ) )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionGetURL2 reserved bits used in flags" );
            }

            _loadTargetFlag = ( 0 != bits.GetBits( 1 ) );
            _loadVariablesFlag = ( 0 != bits.GetBits( 1 ) );
        }

        /// <summary>
        /// Writes the method and the flags back to an outputstream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>One(1)</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            BitStream bits = new BitStream( outputStream.BaseStream );

            bits.WriteBits( 2, ( int )( ( byte )_sendVarsMethod ) );
            bits.WriteBits( 4, 0 );
            bits.WriteBits( 1, ( _loadTargetFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _loadVariablesFlag ? 1 : 0 ) );
            bits.WriteFlush();

            return 1;
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            if ( _sendVarsMethod == SendVarsMethods.Method_GET )
                sb.Append( " GET " );
            if ( _sendVarsMethod == SendVarsMethods.Method_POST )
                sb.Append( " POST " );
            if ( _sendVarsMethod == SendVarsMethods.Method_None )
                sb.Append( " NONE " );
            if ( _loadVariablesFlag )
                sb.Append( " LoadVariables " );
            if ( _loadTargetFlag )
                sb.Append( " LoadTarget" );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            _sendVarsMethod = SendVarsMethods.Method_None;
            for ( int i = 0; i < token.Length; i++ )
            {
                if ( token[ i ].Equals( "GET", StringComparison.InvariantCulture ) )
                {
                    _sendVarsMethod = SendVarsMethods.Method_GET;
                }
                else if ( token[ i ].Equals( "POST", StringComparison.InvariantCulture ) )
                {
                    _sendVarsMethod = SendVarsMethods.Method_POST;
                }
                else if ( token[ i ].Equals( "NONE", StringComparison.InvariantCulture ) )
                {
                    _sendVarsMethod = SendVarsMethods.Method_None;
                }
                else if ( token[ i ].Equals( "LoadVariables", StringComparison.InvariantCulture ) )
                {
                    _loadVariablesFlag = true;
                }
                else if ( token[ i ].Equals( "LoadTarget", StringComparison.InvariantCulture ) )
                {
                    _loadTargetFlag = true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        #endregion       
    }
}
