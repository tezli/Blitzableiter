using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionDefineFunction2 represents the Adobe AVM1 ActionDefineFunction2
    /// </summary>
    public class ActionDefineFunction2 : AbstractAction
    {

        #region fields:

        /// <summary>
        /// 
        /// </summary>
        public struct RegisterParam
        {

            /// <summary>
            /// 
            /// </summary>
            public byte Register;

            /// <summary>
            /// 
            /// </summary>
            public string ParamName;
        }

        /// <summary>
        /// 
        /// </summary>
        protected string _functionName;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numParams;

        /// <summary>
        /// 
        /// </summary>
        protected byte _numRegister;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadParentFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadRootFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _SuppressSuperFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadSuperFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _SuppressArgumentsFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadArgumentsFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _SuppressThisFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadThisFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _PreloadGlobalFlag;

        /// <summary>
        /// 
        /// </summary>
        protected List<RegisterParam> _Parameters;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _codeSize;

        #endregion

        #region constructors:

        /// <summary>
        /// Defines a function
        /// </summary>
        public ActionDefineFunction2()
        {
            this._functionName = "";
            this._numParams = 0;
            this._numRegister = 0;
            this._PreloadParentFlag = false;
            this._PreloadRootFlag = false;
            this._SuppressSuperFlag = false;
            this._PreloadSuperFlag = false;
            this._SuppressArgumentsFlag = false;
            this._PreloadArgumentsFlag = false;
            this._SuppressThisFlag = false;
            this._PreloadThisFlag = false;
            this._PreloadGlobalFlag = false;            
            this._Parameters = new List<RegisterParam>();
            this._codeSize = 0;

            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Defines a function
        /// </summary>
        /// <param name="name">The function name</param>
        /// <param name="registers">Number of registers used by the function</param>
        /// <param name="parameters">The function parameters</param>
        /// <param name="codeSize">The size of the code</param>
        /// <param name="preloadGlobalFlag">True - Preload _global into registers. False - Do not preload _global into registers.</param>
        /// <param name="preloadArgumentsFlag">True - Preload arguments into registers. False - Do not preload arguments into registers.</param>
        /// <param name="preloadParentFlag">True - Preload parent variables into registers. False - Do not preload parent variables into registers.</param>
        /// <param name="preloadRootFlag">True - Preload global variables into registers. False - Do not preload global variables into registers.</param>
        /// <param name="preloadSuperFlag">True - Preload super variables into registers. False - Do not preload super variables into registers.</param>
        /// <param name="preloadThisFlag">True - Preload super variables into registers. False - Do not preload super variables into registers.</param>
        /// <param name="suppressSuperFlag">True - Do NOT create super variable. False - Create super variable</param>
        /// <param name="suppressArgumentsFlag">True - Do NOT create arguments variable. False - Create arguments variable</param>
        /// <param name="suppressThisFlag">True - Do NOT create this variable. False - Create this variable</param>
        public ActionDefineFunction2( string name, byte registers, List<RegisterParam> parameters, UInt16 codeSize,
                                     bool preloadGlobalFlag, bool preloadArgumentsFlag, bool preloadParentFlag, bool preloadRootFlag,
                                     bool preloadSuperFlag, bool preloadThisFlag, bool suppressSuperFlag, bool suppressArgumentsFlag, bool suppressThisFlag ) : this()
        {
            if ( suppressSuperFlag == true && preloadSuperFlag == true )
            {
                throw new SwfFormatException( "suppressSuperFlag and preloadSuperFlag can not be both true!" );
            }
            else if ( suppressArgumentsFlag == true && preloadArgumentsFlag == true )
            {
                throw new SwfFormatException( "suppressArgumentsFlag and preloadArgumentsFlag can not be both true!" );
            }
            else if ( suppressThisFlag == true && preloadThisFlag == true )
            {
                throw new SwfFormatException( "suppressThisFlag and preloadThisFlag can not be both true!" );
            }
            else
            {
                this._functionName = name;
                this._numParams = ( UInt16 )parameters.Count;
                this._numRegister = registers;
                this._PreloadParentFlag = preloadParentFlag;
                this._PreloadRootFlag = preloadRootFlag;
                this._SuppressSuperFlag = suppressSuperFlag;
                this._PreloadSuperFlag = preloadSuperFlag;
                this._SuppressArgumentsFlag = suppressArgumentsFlag;
                this._PreloadArgumentsFlag = preloadArgumentsFlag;
                this._SuppressThisFlag = suppressThisFlag;
                this._PreloadThisFlag = preloadThisFlag;
                this._PreloadGlobalFlag = preloadGlobalFlag;
                this._Parameters = parameters;
                this._codeSize = codeSize;
            }
        }
        #endregion:

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
        public UInt16 ParamCount 
        { 
            get 
            { 
                return this._numParams; 
            } 
            set 
            { 
                this._numParams = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public byte RegisterCount 
        { 
            get 
            { 
                return this._numRegister; 
            } 
            set 
            { 
                this._numRegister = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadParentFlag 
        { 
            get 
            { 
                return this._PreloadParentFlag; 
            } 
            set 
            { 
                this._PreloadParentFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadRootFlag 
        { 
            get 
            { 
                return this._PreloadRootFlag; 
            } 
            set 
            { 
                this._PreloadRootFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SuppressSuperFlag 
        { 
            get 
            { 
                return this._SuppressSuperFlag; 
            } 
            set 
            { 
                this._SuppressSuperFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadSuperFlag 
        { 
            get 
            { 
                return this._PreloadSuperFlag; 
            } 
            set 
            { 
                this._PreloadSuperFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SuppressArgumentsFlag 
        { 
            get 
            { 
                return this._SuppressArgumentsFlag; 
            } 
            set 
            { 
                this._SuppressArgumentsFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadArgumentsFlag 
        { 
            get 
            { 
                return this._PreloadArgumentsFlag; 
            } 
            set 
            { 
                this._PreloadArgumentsFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SuppressThisFlag 
        { 
            get 
            { 
                return this._SuppressThisFlag; 
            } 
            set 
            { 
                this._SuppressThisFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadThisFlag 
        { 
            get 
            { 
                return this._PreloadThisFlag; 
            } 
            set 
            { 
                this._PreloadThisFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PreloadGlobalFlag 
        { 
            get 
            { 
                return this._PreloadGlobalFlag; 
            } 
            set 
            { 
                this._PreloadGlobalFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public List<RegisterParam> Params 
        { 
            get 
            { 
                return this._Parameters; 
            } 
            set 
            { 
                this._Parameters = value; 
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        public UInt16 CodeSize 
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
                return 7; 
            }
        }

        /// <summary>
        /// Parses all neccessary information from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _functionName = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
            _numParams = sourceStream.ReadUInt16();
            _numRegister = sourceStream.ReadByte();

            BitStream bits = new BitStream( sourceStream.BaseStream );
            _PreloadParentFlag = ( 0 != bits.GetBits( 1 ) );
            _PreloadRootFlag = ( 0 != bits.GetBits( 1 ) );
            _SuppressSuperFlag = ( 0 != bits.GetBits( 1 ) );
            _PreloadSuperFlag = ( 0 != bits.GetBits( 1 ) );
            _SuppressArgumentsFlag = ( 0 != bits.GetBits( 1 ) );
            _PreloadArgumentsFlag = ( 0 != bits.GetBits( 1 ) );
            _SuppressThisFlag = ( 0 != bits.GetBits( 1 ) );
            _PreloadThisFlag = ( 0 != bits.GetBits( 1 ) );
            uint reserved = bits.GetBits( 7 );
            _PreloadGlobalFlag = ( 0 != bits.GetBits( 1 ) );

            if ( 0 != reserved )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionDefineFunction2 with reserved flags used" );
            }

            if ( _SuppressArgumentsFlag && _PreloadArgumentsFlag )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionDefineFunction2 Supress+Preload Arguments flag" );
            }

            if ( _SuppressSuperFlag && _PreloadSuperFlag )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionDefineFunction2 Supress+Preload Super flag" );
            }

            if ( _SuppressThisFlag && _PreloadThisFlag )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionDefineFunction2 Supress+Preload This flag" );
            }

            // TODO: make sure this is correct
            _Parameters = new List<RegisterParam>();
            for ( int i = 0; i < _numParams; i++ )
            {
                RegisterParam rp = new RegisterParam();
                rp.Register = sourceStream.ReadByte();
                rp.ParamName = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
                _Parameters.Add( rp );
            }

            _codeSize = sourceStream.ReadUInt16();

            //
            // Please see comment at ActionDefineFunction
            //
            // _code = Helper.SwfCodeReader.GetCode( _codeSize, sourceStream, sourceTag, sourceFile );
        }

        /// <summary>
        /// Renders the DefineFuntion parameters back to a output stream
        /// </summary>
        /// <param name="outputStream">The outputstream</param>
        /// <returns>The output stream position</returns>
        protected override ulong Render( BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _functionName );
            _numParams = ( ushort )_Parameters.Count;
            outputStream.Write( _numParams );
            outputStream.Write( _numRegister );

            BitStream bits = new BitStream( outputStream.BaseStream );
            bits.WriteBits( 1, ( _PreloadParentFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _PreloadRootFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _SuppressSuperFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _PreloadSuperFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _SuppressArgumentsFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _PreloadArgumentsFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _SuppressThisFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _PreloadThisFlag ? 1 : 0 ) );
            bits.WriteBits( 7, 0 );
            bits.WriteBits( 1, ( _PreloadGlobalFlag ? 1 : 0 ) );
            bits.WriteFlush();

            for ( int i = 0; i < _Parameters.Count; i++ )
            {
                outputStream.Write( _Parameters[ i ].Register );
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _Parameters[ i ].ParamName );
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
            sb.AppendFormat( "'{0}' (", _functionName );
            for ( int i = 0; i < _Parameters.Count; i++ )
            {
                sb.AppendFormat( "{0}->Reg{1:d}{2}", _Parameters[ i ].ParamName, _Parameters[ i ].Register, ( ( i + 1 ) == _Parameters.Count ? "" : "," ) );
            }
            sb.Append( ") " );
            sb.Append( _PreloadParentFlag ? "PreloadParent " : "" );
            sb.Append( _PreloadRootFlag ? "PreloadRoot " : "" );
            sb.Append( _SuppressSuperFlag ? "SuppressSuper " : "" );
            sb.Append( _PreloadSuperFlag ? "PreloadSuper " : "" );
            sb.Append( _SuppressArgumentsFlag ? "SuppressArguments " : "" );
            sb.Append( _PreloadArgumentsFlag ? "PreloadArguments " : "" );
            sb.Append( _SuppressThisFlag ? "SuppressThis " : "" );
            sb.Append( _PreloadThisFlag ? "PreloadThis " : "" );
            sb.Append( _PreloadGlobalFlag ? "PreloadGlobal " : "" );
            sb.AppendFormat( "// next {0:d} bytes", _codeSize );
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

            _Parameters = new List<RegisterParam>();
            for ( int i = 2; i < token.Length; i++ )
            {
                //
                // Parameter register preload
                //
                if ( token[ i ].Contains( "->" ) )
                {
                    string param = token[ i ].Substring( 0, token[ i ].IndexOf( "->" ) );
                    string reg = token[ i ].Substring( token[ i ].LastIndexOf( "->" ) + 1 );
                    byte regNum = Byte.Parse( reg );
                    RegisterParam p = new RegisterParam();
                    p.ParamName = param;
                    p.Register = regNum;
                    _Parameters.Add( p );
                }
                else if ( token[ i ].Equals( "PreloadParent", StringComparison.InvariantCulture ) )
                {
                    _PreloadParentFlag = true;
                }
                else if ( token[ i ].Equals( "PreloadRoot", StringComparison.InvariantCulture ) )
                {
                    _PreloadRootFlag = true;
                }
                else if ( token[ i ].Equals( "SuppressSuper", StringComparison.InvariantCulture ) )
                {
                    _SuppressSuperFlag = true;
                }
                else if ( token[ i ].Equals( "PreloadSuper", StringComparison.InvariantCulture ) )
                {
                    _PreloadSuperFlag = true;
                }
                else if ( token[ i ].Equals( "SuppressArguments", StringComparison.InvariantCulture ) )
                {
                    _SuppressArgumentsFlag = true;
                }
                else if ( token[ i ].Equals( "PreloadArguments", StringComparison.InvariantCulture ) )
                {
                    _PreloadArgumentsFlag = true;
                }
                else if ( token[ i ].Equals( "SuppressThis", StringComparison.InvariantCulture ) )
                {
                    _SuppressThisFlag = true;
                }
                else if ( token[ i ].Equals( "PreloadThis", StringComparison.InvariantCulture ) )
                {
                    _PreloadThisFlag = true;
                }
                else if ( token[ i ].Equals( "PreloadGlobal", StringComparison.InvariantCulture ) )
                {
                    _PreloadGlobalFlag = true;
                }
                else
                {
                    // unknown option
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
