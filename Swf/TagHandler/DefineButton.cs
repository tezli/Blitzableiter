using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The DefineButton tag defines a button character for later use by control tags such as</para>
    /// <para>PlaceObject.</para>
    /// <para>DefineButton includes an array of Button records that represent the four button shapes: an up</para>
    /// <para>character, a mouse-over character, a down character, and a hit-area character. It is not</para>
    /// <para>necessary to define all four states, but at least one button record must be present. For example,</para>
    /// <para>if the same button record defines both the up and over states, only three button records are</para>
    /// <para>required to describe the button.</para>
    /// <para>More than one button record per state is allowed. If two button records refer to the same state,</para>
    /// <para>both are displayed for that state.</para>
    /// <para>DefineButton also includes an array of ACTIONRECORDs, which are performed when the</para>
    /// <para>button is clicked and released</para>
    /// </summary>
    public class DefineButton : AbstractTagCodeHandler, ISwfCharacter
    {
        internal UInt16 _buttonID;
        internal List<ButtonRecord> _characters;

        /// <summary>
        /// The DefineButton tag defines a button character for later use by control tags such as
        /// PlaceObject
        /// </summary>
        /// <param name="InitialVersion">The minimum version of the Swf file using this tag.</param>
        public DefineButton( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._buttonID = 0;
            this._characters = new List<ButtonRecord>();
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
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 1; }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get 
            {
                return this._tag.Length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            throw new Exception( "The method or operation is not implemented." );
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            // TODO: Completely Untested!!!
            long initialPosition = this._dataStream.Position;

            BinaryReader2 br = new BinaryReader2(this._dataStream );

            this._buttonID = br.ReadUInt16();

            while ( 0 != br.PeekByte() )
            {
                ButtonRecord buttonR = new ButtonRecord( this.Version );
                buttonR.Parse( this._dataStream );
                if ( buttonR._ButtonHasBlendMode && ( this.Version < 8 ) )
                {
                    throw new SwfFormatException( "DefineButton with ButtonHasBlendMode requires Swf 8 (file: " + this.Version.ToString( "d" ) );
                }
                if ( buttonR._ButtonHasFilterList && ( this.Version < 8 ) )
                {
                    throw new SwfFormatException( "DefineButton with ButtonHasFilterList requires Swf 8 (file: " + this.Version.ToString( "d" ) );
                }
               this._characters.Add( buttonR );
            }
            // read the CharacterEndFlag
            br.ReadByte();
            
            //
            // Now read the ActionRecords that might be there
            //
            ParseCode(this._tag.Length - (uint)(this._dataStream.Position - initialPosition ) );

            if (this._dataStream.Position != this._dataStream.Length )
            {
                Log.Warn(this,  "Trailing garbage within DefineButton: " + this._dataStream.Position.ToString( "d" ) + " of " + this._dataStream.Length.ToString( "d" ) + " bytes used." );
            }
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>        
        /// <param name="output">The output file.</param>
        public override void Write( Stream output )        
        {
            this.WriteTagHeader(output);

            byte[] id = BitConverter.GetBytes( this._buttonID );
            output.Write( id, 0, 2 );
            
            for ( int i = 0; i < this._characters.Count; i++ )
            {
                this._characters[ i ].Write( output, this.Version );
            }
            
            output.WriteByte( 0 ); // character end flag
            
            for ( int i = 0; i < this._code.Count; i++ )
            {
                this._code[ i ].Write( output );
            }

            output.WriteByte( 0 ); // action end flag
        }        
    }
}
