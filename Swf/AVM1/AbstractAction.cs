using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    abstract public class AbstractAction : Interfaces.ISwfElement
    {

        /// <summary>
        /// 
        /// </summary>
        protected StackChange[] _StackOps;

        /// <summary>
        /// 
        /// </summary>
        private byte _SwfVersion;

        /// <summary>
        /// 
        /// </summary>
        protected byte _actionCode;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _length;        

        /// <summary>
        /// 
        /// </summary>
        public UInt16 ActionLength
        {
            get
            {
                if ( 0 == ( _actionCode & 0x80 ) )
                {
                    return (UInt16)( 1 );
                }
                else
                {
                    return (UInt16)( 3 + _length );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 ActionLengthRendered
        {
            get
            {
                UInt16 len = 0;

                if ( 0 != ( _actionCode & 0x80 ) )
                {
                    // FIXME: (fx) that approach is way too slow IMHO
                    using ( MemoryStream ms = new MemoryStream() )
                    {
                        BinaryWriter bw = new BinaryWriter( ms );
                        this.Render( bw );
                        len = ( UInt16 )ms.Length;
                    }
                    len += 3; // type and length field, as these are not counted for length
                }

                return len;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AVM1Actions ActionType
        {
            get
            {
                return ( AVM1Actions )_actionCode;
            }
            set
            {
                _actionCode = (byte)value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="sourceVersion"></param>
        public void Read( BinaryReader sourceStream, byte sourceVersion )
        {
            _SwfVersion = sourceVersion;

            _actionCode = sourceStream.ReadByte();

            //
            // Verify that the ActionCode can be used legally in this version of a Swf file
            //
            if ( sourceVersion < MinimumVersionRequired )
            {
                string msg = "Action " + _actionCode.ToString( "X02" ) +
                    " version mismatch (action:" + MinimumVersionRequired.ToString() +
                    " file:" + sourceVersion.ToString() +
                    ") - Stream Offset 0x" + sourceStream.BaseStream.Position.ToString( "X08" );

                Log.Warn(this,  msg );
            }



            // determine length, if any
            if ( ( 0x80 & _actionCode ) == 0x80 )
            {
                _length = sourceStream.ReadUInt16();
            }
            else
            {
                _length = 0;
            }

            // only parse if there is anything to parse
            if ( 0 != _length )
            {
                // safety net: remember the stream positon before
                // parsing and later compare it to the _length declared
                long before = sourceStream.BaseStream.Position;

                Parse( sourceStream, sourceVersion );

                // safety net: now checking (see above)
                if ( sourceStream.BaseStream.Position != ( before + _length ) )
                {
                    string msg = "AbstractAction (for code " + _actionCode.ToString( "X02" ) +
                        " - " + ( AVM1Factory.ActionName( ( AVM1Actions )_actionCode ) == null ? "UNKNOWN" : AVM1Factory.ActionName( ( AVM1Actions )_actionCode ) ) +
                        ") consumed more/less than the Action's length (" +
                        ( ( long )( sourceStream.BaseStream.Position - before ) ).ToString() + " consumed, " +
                        _length.ToString() + " declared as length)";

                    throw new AVM1ExceptionByteCodeFormat( msg );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            long pos = output.Position;

            BinaryWriter bw = new BinaryWriter( output );

            bw.Write( _actionCode );

            //
            // When the MSB of the action is set, a length field MUST follow
            if ( 0 != ( _actionCode & 0x80 ) )
            {                
                //
                // fancy action, let it do the rendering into a MemoryStream
                //
                using ( MemoryStream ms = new MemoryStream() )
                {
                    BinaryWriter mbw = new BinaryWriter( ms );
                    ulong innerLength = Render( mbw );

                    if ( innerLength > 0xFFFF )
                    {
                        AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat( "Rendered action is larger than 0xFFFF bytes" );
                       Log.Error(this,  e );
                        throw e;
                    }

                    UInt16 writeLength = ( UInt16 )innerLength;
                    bw.Write( writeLength );

                    //
                    // Yes, this check makes sense. See ActionCall.
                    //
                    if ( innerLength > 0 )
                    {
                        ms.WriteTo( output );
                    }
                    
                    if ( innerLength != _length )
                    {
                        AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat( "Rendered action size differs from _length (" + innerLength.ToString( "d" ) + " vs. " + _length.ToString( "d" ) );
                       Log.Error(this,  e );
                        throw e;
                    }
                }                
            }

           //Log.Debug(this,  this.ToString() + " (" + ( output.Position - pos ).ToString("d") + " bytes)" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool ParseFrom( string line )
        {
            char[] delims = { ' ', '\t' };
            string[] token = line.Split( delims, StringSplitOptions.RemoveEmptyEntries );

            if ( 0 == token.Length )
                return false;

            try
            {
                AVM1Actions action = (AVM1Actions)Enum.Parse( typeof( AVM1Actions ), token[ 0 ] );
                _actionCode = ( byte )action;
            }
            catch ( ArgumentException )
            {
                return false;
            }
            
            //
            // Sanity check
            //
            // It is possible that the parser instantiated a wrong ActionXXX Class. Make sure
            // our Type equals the action determined.
            //
            if ( !this.GetType().ToString().EndsWith( token[ 0 ] ) )
            {
                throw new ArgumentException( "'" + line + "' passed into " + this.GetType().ToString() );
            }

            bool res = true;
            if ( 0 != ( _actionCode & 0x80 ) )
            {
                //
                // fancy ActionCode, let it do the argument parsing
                //
                try
                {
                    string[] arguments = ParseFromTokenize( line );
                    res = ParseFrom( arguments );
                    // 
                    // we now set the length, as its former meaning is gone
                    // after repopulating all class members through parsing
                    //
                    // Note: the -3 is because ActionLength* will add them (ACTIONRECORDHEADER)
                    _length = (ushort)( this.ActionLengthRendered - 3 );
                    //
                    // FIXME: we set the SwfVersion to an arbitrary value here
                    // 
                    _SwfVersion = 10;
                }
                catch ( ArgumentException )
                {
                    res = false;
                }
                catch ( FormatException )
                {
                    res = false;
                }
            }
            else
            {
                _length = 0;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string[] ParseFromTokenize( string line )
        {
            List<string> l = new List<string>();
            StringBuilder accumulator = new StringBuilder();
            bool inString = false;

            for ( int i = 0; i < line.Length; i++ )
            {
                if ( line[ i ].Equals( ' ' ) )
                {
                    if ( !inString )
                    {
                        if ( accumulator.Length > 0 )
                        {
                            l.Add( accumulator.ToString() );
                            accumulator = new StringBuilder();
                        }
                    }
                    else
                    {
                        accumulator.Append( line[ i ] );
                    }
                }
                else if ( line[ i ].Equals( '\'' ) )
                {
                    inString = !inString;
                }
                else
                {
                    accumulator.Append( line[ i ] );
                }
            }
            l.Add( accumulator.ToString() );
            l.RemoveAt( 0 );
            return l.ToArray();
        }

        //
        // virtual Methods (only overwritten by more complex actions)
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected virtual ulong Render( BinaryWriter outputStream )
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual bool ParseFrom( params string[] token )
        {
            throw new NotImplementedException( "ParseFrom not overwritten by Action class for actionCode " + _actionCode.ToString( "d" ) );            
        }

        //
        // Direct access to branch targets (ActionJump, ActionIf, etc) 
        //
        /// <summary>
        /// 
        /// </summary>
        public virtual Int32 BranchTarget
        {
            get
            {
                return 0;
            }
            set
            {
                // ignore
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        public virtual Int32 BranchTargetAdjusted
        {
            get
            {
                return 0;                
            }
            set
            {
                // ignore
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsBranch
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsConditional
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFunction
        {
            get
            {
                return false;
            }
        }

        //
        // Stack tracking
        //

        /// <summary>
        /// 
        /// </summary>
        public virtual StackChange[] StackOperations
        {
            get
            {
                return _StackOps;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int StackDifference
        {
            get
            {
                int accumulator = 0;
                for ( int i = 0; i < this.StackOperations.Length; i++ )
                {
                    accumulator = accumulator + this.StackOperations[ i ].Change;
                }
                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsStackPredictable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Stack.AVM1Stack PerformStackOperations( Stack.AVM1Stack sourceStack )
        {
            for ( int i = 0; i < _StackOps.Length; i++ )
            {
                if ( _StackOps[ i ].Change < 0 )
                {
                    sourceStack.Pop();
                }
                else if ( _StackOps[ i ].Change > 0 )
                {
                    AVM1DataElement e = new AVM1DataElement();
                    e.DataType = _StackOps[ i ].DataType;
                    e.Value = null;
                    sourceStack.Push( e );
                }
            }

            return sourceStack;            
        }

        //
        // ToString overwritten for AVM1 code output 
        //

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return this.GetType().Name;
        }
        
        //
        // abstract Methods
        //

        /// <summary>
        /// 
        /// </summary>
        public abstract byte MinimumVersionRequired { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="sourceVersion"></param>
        protected virtual void Parse( BinaryReader sourceStream, byte sourceVersion ) {}

        #region ISwfElement Members


        /// <summary>
        /// 
        /// </summary>
        public byte Version
        {
            get
            {
                return _SwfVersion;
            }
            set
            {
                _SwfVersion = value;
            }
        }

        #endregion
    }
}
