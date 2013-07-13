using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonRecord : AbstractSwfElement
    {
        internal bool _ButtonHasBlendMode;
        internal bool _ButtonHasFilterList;
        internal bool _ButtonStateHitTest;
        internal bool _ButtonStateDown;
        internal bool _ButtonStateOver;
        internal bool _ButtonStateUp;
        internal UInt16 _CharacterID;
        internal UInt16 _PlaceDepth;
        internal Matrix _PlaceMatrix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ButtonRecord( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public virtual void Parse( Stream input )
        {
            BitStream bits = new BitStream( input );

            uint reserved = bits.GetBits( 2 );
            if ( 0 != reserved )
            {
                throw new SwfFormatException( "ButtonRecord reserved bits used" );
            }

            _ButtonHasBlendMode = ( 0 != bits.GetBits( 1 ) );
            _ButtonHasFilterList = ( 0 != bits.GetBits( 1 ) );
            _ButtonStateHitTest = ( 0 != bits.GetBits( 1 ) );
            _ButtonStateDown = ( 0 != bits.GetBits( 1 ) );
            _ButtonStateOver = ( 0 != bits.GetBits( 1 ) );
            _ButtonStateUp = ( 0 != bits.GetBits( 1 ) );

            BinaryReader br = new BinaryReader( input );

            _CharacterID = br.ReadUInt16();
            _PlaceDepth = br.ReadUInt16();
            _PlaceMatrix = new Matrix( this.Version );
            _PlaceMatrix.Parse( input );            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual uint Length
        {
            get
            {
                return (
                    sizeof( byte ) +
                    ( 2 * sizeof( UInt16 ) ) +
                    _PlaceMatrix.Length
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="version"></param>
        public virtual void Write( Stream output, byte version )
        {
            BitStream bits = new BitStream( output );
            // reserved
            bits.WriteBits( 2, 0 );
            bits.WriteBits( 1, ( _ButtonHasBlendMode ? 1 : 0 ) );
            bits.WriteBits( 1, ( _ButtonHasFilterList ? 1 : 0 ) );
            bits.WriteBits( 1, ( _ButtonStateHitTest ? 1 : 0 ) );
            bits.WriteBits( 1, ( _ButtonStateDown ? 1 : 0 ) );
            bits.WriteBits( 1, ( _ButtonStateOver ? 1 : 0 ) );
            bits.WriteBits( 1, ( _ButtonStateUp ? 1 : 0 ) );
            bits.WriteFlush();

            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( _CharacterID );
            bw.Write( _PlaceDepth );
            _PlaceMatrix.Write( output );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append( base.ToString() );
            sb.Append( ( _ButtonHasBlendMode ? " BlendMode" : "" ) );
            sb.Append( ( _ButtonHasFilterList ? " FilterList" : "" ) );
            sb.Append( ( _ButtonStateHitTest ? " StateHitTest" : "" ) );
            sb.Append( ( _ButtonStateDown ? " StateDown" : "" ) );
            sb.Append( ( _ButtonStateOver ? " StateOver" : "" ) );
            sb.Append( ( _ButtonStateUp ? " StateUp" : "" ) );

            sb.AppendFormat(" CharacterID=0x{0:X}", _CharacterID );
            sb.AppendFormat(" Depth=0x{0:X} ", _PlaceDepth);
            sb.Append( _PlaceMatrix.ToString() );
            return sb.ToString();
        }
    }
}
