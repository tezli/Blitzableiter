using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionTry represents the Adobe AVM1 ActionTry
    /// </summary>
    public class ActionTry : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected bool _CatchInRegisterFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _FinallyBlockFlag;

        /// <summary>
        /// 
        /// </summary>
        protected bool _CatchBlockFlag;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _trySize;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _catchSize;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _finallySize;

        /// <summary>
        /// 
        /// </summary>
        protected string _catchName; // may be null

        /// <summary>
        /// 
        /// </summary>
        protected byte _catchRegister; // may be invalid
        #endregion

        #region constructors
        /// <summary>
        /// Try defines handlers for exceptional conditions, 
        /// implementing the ActionScript try, catch, and finally keywords.
        /// </summary>
        public ActionTry()
        {
            this._CatchInRegisterFlag = false;
            this._FinallyBlockFlag = false;
            this._trySize = 0;
            this._catchSize = 0;
            this._catchName = "";
            this._catchRegister = 0;

            _StackOps = new StackChange[ 0 ];
        }
        /// <summary>
        /// Try defines handlers for exceptional conditions, 
        /// implementing the ActionScript try, catch, and finally keywords.
        /// </summary>
        /// <param name="storeObejct">False - Do not put caught object into register (instead, store in named variable) True - Put caught object into register (do not store in named variable)</param>
        /// <param name="finallyBlock">True - The statement has a finally block. False The statement has no finally block</param>
        /// <param name="catchBlock">True - The statement has a catch block. False The statement has no catch block</param>
        /// <param name="trySize">Length of the try block</param>
        /// <param name="catchSize">Length of the catch block</param>
        /// <param name="finallySize">Length of the finally block</param>
        /// <param name="catchVarName">Name of the catch variable</param>
        /// <param name="catchRegister">Register to catch into</param>
        public ActionTry( bool storeObejct, bool finallyBlock, bool catchBlock, UInt16 trySize, UInt16 catchSize, UInt16 finallySize, string catchVarName, byte catchRegister ) : this()
        {
            this._CatchInRegisterFlag = storeObejct;
            this._FinallyBlockFlag = finallyBlock;
            this._CatchBlockFlag = catchBlock;
            this._trySize = trySize;
            this._catchSize = catchSize;
            this._finallySize = finallySize;
            this._catchName = catchVarName;
            this._catchRegister = catchRegister;

        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public bool CatchInRegisterFlag 
        { 
            get 
            { 
                return this._CatchInRegisterFlag; 
            } 
            set 
            { 
                this._CatchInRegisterFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool FinallyBlockFlag 
        { 
            get 
            { 
                return this._FinallyBlockFlag; 
            } 
            set 
            { 
                this._FinallyBlockFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CatchBlockFlag 
        { 
            get 
            { 
                return this._CatchBlockFlag; 
            } 
            set 
            { 
                this._CatchBlockFlag = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 TrySize 
        { 
            get 
            { 
                return this._trySize; 
            } 
            set 
            { 
                this._trySize = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 CatchSize 
        { 
            get 
            { 
                return this._catchSize; 
            } 
            set 
            { 
                this._catchSize = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 Finally 
        { 
            get 
            { 
                return this._finallySize; 
            } 
            set 
            { 
                this._finallySize = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public string CatchName 
        { 
            get 
            { 
                return this._catchName; 
            } 
            set 
            { 
                this._catchName = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public byte CatchRegister 
        { 
            get 
            { 
                return this._catchRegister; 
            } 
            set 
            { 
                this._catchRegister = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasFinally 
        { 
            get 
            { 
                return _FinallyBlockFlag; 
            } 
        }

        /// <summary>
        /// This is the catch(){} element and points the the first instruction AFTER the catch block
        /// </summary>
        public UInt16 CatchTargetAdjusted
        {
            get
            {
                if ( _CatchBlockFlag )
                {
                    return ( UInt16 )( _catchSize + _trySize + this.ActionLengthRendered );
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ( _CatchBlockFlag )
                {
                    _catchSize = ( UInt16 )( value - ( this.ActionLengthRendered + _trySize ) );
                }
                else
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 FinallyTargetAdjusted
        {
            //
            // This is the finally(){} element and point to the first instruction
            // AFTER the finally block (and hence at the first instruction outside try/catch/finally)
            //
            get
            {
                if ( _FinallyBlockFlag )
                {
                    return ( UInt16 )( _finallySize + ( _trySize + _catchSize + this.ActionLengthRendered ) );
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if ( _FinallyBlockFlag )
                {
                    _finallySize = ( UInt16 )( value - ( this.ActionLengthRendered + _trySize + _catchSize ) );
                }
                else
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// This is the try{} element. It points AFTER the try block
        /// </summary>
        public override int BranchTarget 
        { 
            get 
            { 
                return ( int )_trySize; 
            } 
            set 
            { _trySize = ( ushort )value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCatch 
        { 
            get 
            { 
                return _CatchBlockFlag; 
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
        /// Parses try statement from a source stream. 
        /// Quote from Swf spec:
        /// "The try, catch, and finally blocks do not use end tags to mark the end of their respective
        /// blocks. Instead, the length of a block is set by the TrySize, CatchSize, and FinallySize
        /// values."
        /// Additionally, see comment in ActionDefineFunction on why the code isn't read here.
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            BitStream bits = new BitStream( sourceStream.BaseStream );

            uint reserved = bits.GetBits( 5 );
            if ( 0 != reserved )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionTry uses reserved bits in flags" );
            }
            _CatchInRegisterFlag = ( 0 != bits.GetBits( 1 ) );
            _FinallyBlockFlag = ( 0 != bits.GetBits( 1 ) );
            _CatchBlockFlag = ( 0 != bits.GetBits( 1 ) );

            _trySize = sourceStream.ReadUInt16();
            _catchSize = sourceStream.ReadUInt16();
            _finallySize = sourceStream.ReadUInt16();

            if ( _CatchInRegisterFlag )
            {
                _catchRegister = sourceStream.ReadByte();
                _catchName = null;
            }
            else
            {
                _catchRegister = 0xFF;
                _catchName = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
            }

        }

        /// <summary>
        /// Renders a try statement back to an output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The output stream position</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            BitStream bits = new BitStream( outputStream.BaseStream );
            bits.WriteBits( 5, 0 );
            bits.WriteBits( 1, ( _CatchInRegisterFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _FinallyBlockFlag ? 1 : 0 ) );
            bits.WriteBits( 1, ( _CatchBlockFlag ? 1 : 0 ) );
            bits.WriteFlush();

            outputStream.Write( _trySize );
            outputStream.Write( _catchSize );
            outputStream.Write( _finallySize );

            if ( _CatchInRegisterFlag )
            {
                outputStream.Write( _catchRegister );
            }
            else
            {
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _catchName );
            }

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

            sb.AppendFormat( " Try:{0:d} bytes", _trySize );

            if ( _CatchInRegisterFlag )
                sb.AppendFormat( " CatchInRegister:{0:d}", _catchRegister );
            else
                sb.AppendFormat( " CatchIn:{0}", _catchName );

            if ( _CatchBlockFlag )
                sb.AppendFormat( " CatchBlock:{0:d} bytes", _catchSize );
            else
                sb.Append( " (no CatchBlock)" );

            if ( _FinallyBlockFlag )
                sb.AppendFormat( " FinallyBlock:{0:d} bytes", _finallySize );
            else
                sb.Append( " (no FinallyBlock)" );

            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            throw new NotSupportedException( "ActionTry not supported (by Label resolving)" );
        }
        #endregion
    }
}
