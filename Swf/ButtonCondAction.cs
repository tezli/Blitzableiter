using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonCondAction : AbstractSwfElement
    {
        internal UInt16 _OffsetToNextCondAction;
        internal bool _CondIdleToOverDown;
        internal bool _CondOutDownToIdle;
        internal bool _CondOutDownToOverDown;
        internal bool _CondOverDownToOutDown;
        internal bool _CondOverDownToOverUp;
        internal bool _CondOverUpToOverDown;
        internal bool _CondOverUpToIdle;
        internal bool _CondIdleToOverUp;
        internal byte _CondKeyPress;
        internal bool _CondOverDownToIdle;
        //internal List<AVM1.AbstractAction> _code;
        /// <summary>
        /// 
        /// </summary>
        protected AVM1.AVM1Code _Code;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ButtonCondAction( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        public AVM1.AVM1Code Code
        {
            get
            {
                return _Code;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxSize"></param>
        public void Parse( Stream input, uint maxSize )
        {
            BinaryReader br = new BinaryReader( input );

            //
            // subtract the OffsetToNextCondAction as well as the Conditions from 
            // the available size
            //
            uint maxSizeInternal = maxSize - 4;

            _OffsetToNextCondAction = br.ReadUInt16();

            BitStream bits = new BitStream( input );
            _CondIdleToOverDown = ( 0 != bits.GetBits( 1 ) );
            _CondOutDownToIdle = ( 0 != bits.GetBits( 1 ) );
            _CondOutDownToOverDown = ( 0 != bits.GetBits( 1 ) );
            _CondOverDownToOutDown = ( 0 != bits.GetBits( 1 ) );
            _CondOverDownToOverUp = ( 0 != bits.GetBits( 1 ) );
            _CondOverUpToOverDown = ( 0 != bits.GetBits( 1 ) );
            _CondOverUpToIdle = ( 0 != bits.GetBits( 1 ) );
            _CondIdleToOverUp = ( 0 != bits.GetBits( 1 ) );
            _CondKeyPress = (byte)bits.GetBits( 7 );
            if ( ( 0 != _CondKeyPress ) && ( this.Version < 4 ) )
            {
                throw new SwfFormatException( "CondKeyPress != 0 with Swf version " + this.Version.ToString( "d" ) );
            }
            if (               
                ( ( _CondKeyPress > 19 ) && ( _CondKeyPress < 32 ) )
                || ( _CondKeyPress > 126 )
                )
            {
                throw new SwfFormatException( "Illegal CondKeyPress " + _CondKeyPress.ToString("d") + " in ButtonCondAction" );
            }
            _CondOverDownToIdle = ( 0 != bits.GetBits( 1 ) );

            uint expectedSize;
            if ( 0 != _OffsetToNextCondAction )
            {
                // 
                // if we have been given an offset, this defines our code
                // size expecations (minus 4 bytes for the previous data)
                //
                expectedSize = (uint)( _OffsetToNextCondAction - 4 );
            }
            else
            {
                // 
                // if we have no offset, the caller must tell us how much 
                // we have left
                //
                expectedSize = maxSizeInternal;
            }

            AVM1.AVM1InstructionSequence bytecode = Helper.SwfCodeReader.GetCode( expectedSize, br, this.Version );
            _Code = new AVM1.AVM1Code( bytecode );
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return (
                    sizeof( UInt16 ) + // CondActionSize
                    sizeof( UInt16 ) + // Conditions
                    _Code.Length
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="version"></param>
        /// <param name="lastAction"></param>
        public void Write( Stream output, byte version, bool lastAction )
        {
            BinaryWriter bw = new BinaryWriter( output );

            if ( !lastAction )
            {
                _OffsetToNextCondAction = (ushort)this.Length;
            }
            else
            {
                _OffsetToNextCondAction = 0;
            }
            bw.Write( _OffsetToNextCondAction );

            BitStream bits = new BitStream( output );
            bits.WriteBits( 1, (_CondIdleToOverDown?1:0)); 
            bits.WriteBits( 1, (_CondOutDownToIdle?1:0));
            bits.WriteBits( 1, (_CondOutDownToOverDown?1:0));
            bits.WriteBits( 1, (_CondOverDownToOutDown?1:0));
            bits.WriteBits( 1, (_CondOverDownToOverUp?1:0));
            bits.WriteBits( 1, (_CondOverUpToOverDown?1:0));
            bits.WriteBits( 1, (_CondOverUpToIdle?1:0));
            bits.WriteBits( 1, (_CondIdleToOverUp?1:0));
            bits.WriteBits( 7, _CondKeyPress );
            bits.WriteBits( 1, ( _CondOverDownToIdle ? 1 : 0 ) );
            bits.WriteFlush();

            for ( int i = 0; i < _Code.Count; i++ )
            {
                _Code[ i ].Write( output );
            }                
            
        }
    }
}
