using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// DefineButton2 extends the capabilities of DefineButton by allowing any state transition to
    /// trigger actions.
    /// </summary>
    public class DefineButton2 : AbstractTagCodeHandler, ISwfCharacter
    {
        internal UInt16 _buttonID;
        internal bool _trackAsMenu;
        internal UInt16 _actionOffset;
        internal List<ButtonRecord2> _characters;
        internal List<ButtonCondAction> _actions;

        /// <summary>
        /// DefineButton2 extends the capabilities of DefineButton by allowing any state transition to
        /// trigger actions.
        /// </summary>
        /// <param name="InitialVersion">The minimum version of the Swf file using this tag.</param>
        public DefineButton2( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._buttonID = 0;
            this._trackAsMenu = false;
            this._actionOffset = 0;
            this._characters = new List<ButtonRecord2>();
            this._actions = new List<ButtonCondAction>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override byte WriteVersion
        {
            get
            {
                byte version = this.MinimumVersionRequired;
                for ( int i = 0; i < this._actions.Count; i++ )
                {
                    if ( null != this._actions[ i ].Code )
                    {
                        for ( int j = 0; j < this._actions[ i ].Code.Count; j++ )
                        {
                            version = this._actions[ i ].Code[ j ].MinimumVersionRequired > version ? this._actions[ i ].Code[ j ].MinimumVersionRequired : version;
                        }
                    }
                }
                return version;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 3; }
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _buttonID;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                uint characterSum = 0;
                for ( int i = 0; i < this._characters.Count; i++ )
                {
                    characterSum += this._characters[ i ].Length;
                }
                characterSum++; //CharacterEnd

                uint actionSum = 0;
                for ( int i = 0; i < this._actions.Count; i++ )
                {
                    actionSum += this._actions[ i ].Length;
                }

                return (
                    sizeof( UInt16 ) + // ButtonID
                    sizeof( byte ) + // flags
                    sizeof( UInt16 ) + // ActionOffset
                    characterSum +
                    actionSum
                );
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            // 
            // Is there anything else to verify with Buttons?
            //
            return VerifyAllCode();
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            // We cannot parse code using the ParseCode() method, due to the 
            // ButtonCondAction format
            BinaryReader2 br = new BinaryReader2( this._dataStream );
            long rememberStreamBegin = this._dataStream.Position;

            this._buttonID = br.ReadUInt16();
            BitStream bits = new BitStream( this._dataStream );
            uint reserved = bits.GetBits( 7 );
            if ( 0 != reserved )
            {
                throw new SwfFormatException( "DefineButton2 uses reserved bits" );
            }
           this._trackAsMenu = ( 0 != bits.GetBits( 1 ) );
            // Quote form spec: "Offset in bytes from start of this
            // field to the first BUTTONCONDACTION, or 0 if no actions occur"
           long rememberActionOffset = this._dataStream.Position;
            this._actionOffset = br.ReadUInt16();

            while ( 0 != br.PeekByte() )
            {
                ButtonRecord2 b = new ButtonRecord2( this.Version );
                b.Parse( this._dataStream );
                this._characters.Add( b );
            }
            // CharacterEndFlag
            br.ReadByte();

            //
            // Now, we can check the ActionOffset value
            //
            this._actions = new List<ButtonCondAction>();
            if ( 0 != this._actionOffset )
            {
                if ( ( rememberActionOffset + this._actionOffset ) != this._dataStream.Position )
                {
                    SwfFormatException e = new SwfFormatException(
                        "ActionOffset was 0x" + this._actionOffset.ToString( "X02" ) + " at stream position 0x" + rememberActionOffset.ToString( "X08" ) +
                        " but stream is now at 0x" + this._dataStream.Position.ToString( "X08" ) );
                   Log.Error(this,  e );
                    throw e;
                }

                ButtonCondAction cond = null;
                do
                {
                    cond = new ButtonCondAction( this.Version );
                    try
                    {
                        uint bytesLeft = ( uint )( this._tag.Length - ( this._dataStream.Position - rememberStreamBegin ) );
                        cond.Parse( this._dataStream, bytesLeft );
                    }
                    catch ( AVM1.AVM1Exception be )
                    {
                       Log.Error(this,  be );

                        if ( SwfFile.Configuration.AVM1DeleteInvalidBytecode )
                        {
                            cond.Code.Clear();
                        }
                        else
                        {
                            SwfFormatException swfe = new SwfFormatException( "DefineButton2 contains invalid ActionCode", be );                            
                            throw swfe;
                        }
                    }

                    this._actions.Add( cond );
                }
                while ( cond._OffsetToNextCondAction != 0 );
            }

            //
            // Scan the _SwfFile for a FileAttributes tag and see if it 
            // specifies AS3. Spec quote: "Starting with Swf 9, if the ActionScript3 field 
            // of the FileAttributes tag is 1, there must be no BUTTONCONDACTION fields in 
            // the DefineButton2 tag. ActionOffset must be 0."
            //
            // TODO: implement me

            if ( this._dataStream.Position != this._dataStream.Length )
            {
                SwfFormatException e = new SwfFormatException(
                    "DefineButton2 has not fully consumed stream (" + this._dataStream.Position.ToString( "d" ) +
                    " of " + this._dataStream.Length.ToString( "d" )
                );
               Log.Error(this,  e );
                throw e;
            }

           //Log.Debug(this,  this.ToString() );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
           //Log.Debug(this,  "0x" + output.Position.ToString( "X08" ) + ": writing..." );            
                                   
            MemoryStream rendered = this.Render( this.Version );
            
            WriteTagHeader( output );

            long pos = output.Position;

            rendered.WriteTo( output );

           //Log.Debug(this,  "0x" + output.Position.ToString( "X08" ) + ": " + ((long)( output.Position - pos)).ToString("d") + " bytes written (" + this.Length.ToString("d") + " calculated)" );
        }

        //
        // We have to do the rendering inside this class, as we are not 
        // inhereting from AbstractTagCodeHandler 
        //
        private MemoryStream Render( byte version )
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter( ms );

            bw.Write( this._buttonID );
            BitStream bits = new BitStream( ms );
            // reserved Bits and flags
            bits.WriteBits( 7, 0 );
            bits.WriteBits( 1, ( this._trackAsMenu ? 1 : 0 ) );
            bits.WriteFlush();

            if ( 0 == this._actions.Count )
            {
                //
                // resetting this value. It could be that we used to have
                // actions in this button but they got nuked when an exception
                // was raised during their parsing.
                //
                this._actionOffset = 0;
            }
            else
            {
                int charactersSum = 0;
                for ( int i = 0; i < this._characters.Count; i++ )
                {
                    charactersSum += ( int )_characters[ i ].Length;
                }
                charactersSum++; // EndCharacter

                this._actionOffset = ( ushort )charactersSum;
                this._actionOffset += sizeof( ushort ); // The ActionOffset field itself
            }
            bw.Write( this._actionOffset );

            for ( int i = 0; i < this._characters.Count; i++ )
            {
                this._characters[ i ].Write( ms, version );
            }
            bw.Write( ( byte )0x00 ); // EndCharacter

            for ( int i = 0; i < this._actions.Count; i++ )
            {
                this._actions[ i ].Write( ms, version, ( ( i + 1 ) == this._actions.Count ? true : false ) );
            }

            return ms;
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "ButtonID:{0:d} / TrackAsMenue: {1}, {2:d} actions, {3:d} Characters",
                this._buttonID, this._trackAsMenu, this._actions.Count, this._characters.Count );

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override Recurity.Swf.AVM1.AVM1Code[] Code
        {
            get
            {
                AVM1.AVM1Code[] code = new Recurity.Swf.AVM1.AVM1Code[ this._actions.Count ];
                for ( int i = 0; i < this._actions.Count; i++ )
                {
                    code[ i ] = this._actions[ i ].Code;
                }
                return code;
            }
        }
    }
}
