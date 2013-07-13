using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDefineFunction represents the Adobe AVM1 ActionDefineFunction
    /// </summary>
    public class ActionDefineFunction : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected string _functionName;

        /// <summary>
        /// 
        /// </summary>
        protected List<string> _parameterName;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _codeSize;

        #endregion

        #region constructors:

        /// <summary>
        /// ActionDefineFunction defines a function with a given name and body size.
        /// </summary>
        public ActionDefineFunction()
        {
            this._functionName = "";
            this._parameterName = new List<string>();
            this._codeSize = 0;

            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// ActionDefineFunction defines a function with a given name and body size.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="size">Number of bytes of code that follow</param>
        public ActionDefineFunction( string name, UInt16 size ) : this()
        {
            this._functionName = name;
            this._parameterName = new List<string>();
            this._codeSize = size;
        }

        /// <summary>
        /// ActionDefineFunction defines a function with a given name and body size.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="parameters">The function parameters</param>
        /// <param name="size">Number of bytes of code that follow</param>
        public ActionDefineFunction( string name, List<string> parameters, UInt16 size ) : this()
        {
            this._functionName = name;
            this._parameterName = parameters;
            this._codeSize = size;
        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public string Name 
        { 
            get 
            { 
                return this._functionName; 
            } 
            set 
            { 
                this._functionName = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Target 
        { 
            get 
            { 
                return this._parameterName; 
            } 
            set 
            { 
                this._parameterName = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 Size 
        { 
            get 
            { 
                return this._codeSize; 
            } 
            set 
            { 
                this._codeSize = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTarget 
        { 
            get 
            { 
                return ( int )_codeSize; 
            } 
            set 
            { 
                _codeSize = ( ushort )value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTargetAdjusted 
        { 
            get 
            { 
                return ( int )( _codeSize + this.ActionLengthRendered ); 
            } 
            set 
            { 
                _codeSize = ( ushort )( value - this.ActionLengthRendered ); 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsFunction
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region code:

        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 5; 
            }
        }

        /// <summary>
        /// Parses function name and parameters of a function from a stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _functionName = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );

            UInt16 numParams = sourceStream.ReadUInt16();

            _parameterName = new List<string>();
            for ( int i = 0; i < numParams; i++ )
            {
                string adder = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
                _parameterName.Add( adder );
            }

            //
            // ActionDefineFunction defines but does not carry the code. In contrast 
            // to intuitive behavior, the code that follows ActionDefineFunction is 
            // not part of the Action itself.
            //
            _codeSize = sourceStream.ReadUInt16();
        }

        /// <summary>
        /// Writes function name and parameters to the output stream
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _functionName );

            outputStream.Write( ( UInt16 )_parameterName.Count );

            for ( int i = 0; i < _parameterName.Count; i++ )
            {
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _parameterName[ i ] );
            }

            outputStream.Write( _codeSize );

            return ( ulong )( outputStream.BaseStream.Position - pos );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " '{0}'( ", _functionName );
            for ( int i = 0; i < _parameterName.Count; i++ )
            {
                sb.AppendFormat( "{0}{1}", _parameterName[ i ], ( ( i + 1 ) == _parameterName.Count ? "" : "," ) );
            }
            sb.AppendFormat( ") // next {0:d} bytes", _codeSize );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            if ( token.Length < 2 )
                return false;

            _functionName = token[ 0 ];
            _codeSize = UInt16.Parse( token[ 1 ] );
            _parameterName = new List<string>();
            for ( int i = 2; i < token.Length; i++ )
            {
                _parameterName.Add( token[ i ] );
            }
            return true;
        }

        #endregion
    }
}
