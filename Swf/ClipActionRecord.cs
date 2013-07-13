using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Recurity.Swf.AVM1;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipActionRecord : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool _ParsingFailures;

        /// <summary>
        /// 
        /// </summary>
        internal ClipEventFlags _ClipEventFlags;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _length; // excluding the ClipEventFlags and length field

        /// <summary>
        /// 
        /// </summary>
        internal byte _KeyCode;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _codeSize;
        //internal List<AbstractAction> _code;

        /// <summary>
        /// 
        /// </summary>
        protected AVM1Code _Code;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ClipActionRecord( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        public override byte Version
        {
            get
            {
                return base.Version;
            }
            set
            {
                base.Version = value;

                if ( null != _ClipEventFlags )
                    _ClipEventFlags.Version = base.Version;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Parse( Stream input )
        {
            AVM1InstructionSequence bytecode;
            _ParsingFailures = false;

            _ClipEventFlags = new ClipEventFlags( this.Version );
            _ClipEventFlags.Parse( input );

            BinaryReader br = new BinaryReader( input );
            _length = br.ReadUInt32();
            _codeSize = _length;

            if ( _ClipEventFlags.ClipEventKeyPress )
            {
                if ( 1 > _length )
                {
                    throw new SwfFormatException( "ClipActionRecord length=0 but KeyCode indicated by ClipEventKeyPress" );
                }
                _KeyCode = br.ReadByte();
                _codeSize--;
            }

            long before = br.BaseStream.Position;

            try
            {
                bytecode = Helper.SwfCodeReader.GetCode( _codeSize, br, this.Version );

                if ( br.BaseStream.Position != ( before + _codeSize ) )
                {
                    throw new SwfFormatException( "ClipActionRecord code reader consumed more than length indicated (" +
                        ( ( uint )( br.BaseStream.Position - before ) ).ToString() + " consumed, " +
                        _codeSize + " length)" );
                }
            }
            catch ( AVM1ExceptionByteCodeFormat ave )
            {
               Log.Error(this,  ave );
                _ParsingFailures = true;

                if (SwfFile.Configuration.AVM1DeleteInvalidBytecode)
                {
                    bytecode = new AVM1InstructionSequence();
                }
                else
                {
                    SwfFormatException swfe = new SwfFormatException( "ClipActionRecord parsing error", ave );
                    throw swfe;
                }
            }
            finally
            {
                //
                // make sure that the input stream is at the right position
                // it would have if code reading would have been successful
                //
                long diff = ( before + _codeSize ) - br.BaseStream.Position;
                if ( 0 != diff )
                    br.BaseStream.Seek( diff, SeekOrigin.Current );
            }

            _Code = new AVM1Code( bytecode );

            return _ParsingFailures;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            if ( _ParsingFailures )
                return false;

            return true;
        }        

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint res = 0;

                res += _ClipEventFlags.Length;
                res += 4; // length field
                if ( _ClipEventFlags.ClipEventKeyPress )
                    res += 1; // KeyCode
                res += _Code.Length;

                return res;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            long pos = output.Position;

            _ClipEventFlags.Write( output );

            UInt32 actionRecordSize = _Code.Length;
            // HACK: increase this value to cause Flash Player 10 to crash when handling the ClipAction
            actionRecordSize += (uint)( _ClipEventFlags.ClipEventKeyPress ? 1 : 0 ); // optional KeyCode            

            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( actionRecordSize );

            if ( _ClipEventFlags.ClipEventKeyPress )
                bw.Write( _KeyCode );

            for ( int i = 0; i < _Code.Count; i++ )
            {
                _Code[ i ].Write( output );
            }            

           //Log.Debug(this,  ( ( ulong )( output.Position - pos ) ).ToString( "d" ) + " bytes written, " + this.Length + " calculated" );
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
    }
}
