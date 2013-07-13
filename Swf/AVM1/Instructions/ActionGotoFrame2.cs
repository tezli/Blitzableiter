using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGotoFrame2 represents the Adobe AVM1 ActionGotoFrame2
    /// </summary>
    public class ActionGotoFrame2 : AbstractAction
    {

        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected bool _sceneBias;

        /// <summary>
        /// 
        /// </summary>
        protected bool _play;

        /// <summary>
        /// 
        /// </summary>
        internal UInt16 _sceneBiasAmount;
        #endregion

        #region constructors:

        /// <summary>
        /// Goes to a frame and is stack based
        /// </summary>
        public ActionGotoFrame2()
        {
            this._sceneBias = false;
            this._play = false;
            this._sceneBiasAmount = 0;

            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // frame number of string
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bias">If the frame offset of the next frame to play</param>
        /// <param name="play">Go to frame and play(true), Go to frame and stop(false)</param>
        /// <param name="amount">If bias is set this is the amount</param>
        public ActionGotoFrame2( bool bias, bool play, UInt16 amount ) : this()
        {
            this._sceneBias = bias;
            this._play = play;
            this._sceneBiasAmount = amount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="play"></param>
        public ActionGotoFrame2( bool play )
        {
            this._sceneBias = false;
            this._play = play;
            this._sceneBiasAmount = 0;
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public bool SceneBias 
        { 
            get 
            { 
                return this._sceneBias; 
            } 
            set 
            { 
                this.SceneBias = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Play 
        { 
            get 
            { 
                return this._play; 
            } 
            set 
            { 
                this._play = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt16 SceneBiasAmount 
        { 
            get 
            { 
                return this._sceneBiasAmount; 
            } 
            set 
            { 
                this.SceneBiasAmount = value; 
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
                return 4; 
            }
        }

        /// <summary>
        /// Parses scene bias and play-flag from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            BitStream bits = new BitStream( sourceStream.BaseStream );

            uint reserved = bits.GetBits( 6 );
            if ( 0 != reserved )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionGotoFrame2 flags use reserved bits" );
            }

            _sceneBias = ( 0 != bits.GetBits( 1 ) );
            _play = ( 0 != bits.GetBits( 1 ) );

            if ( _sceneBias )
            {
                _sceneBiasAmount = sourceStream.ReadUInt16();
            }
        }

        /// <summary>
        /// Renders scene bias and play-flag back to an output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The position of the output stream</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            BitStream bits = new BitStream( outputStream.BaseStream );
            bits.WriteBits( 6, 0 );
            bits.WriteBits( 1, ( _sceneBias ? 1 : 0 ) );
            bits.WriteBits( 1, ( _play ? 1 : 0 ) );
            bits.WriteFlush();

            if ( _sceneBias )
            {
                outputStream.Write( _sceneBiasAmount );
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
            if ( _sceneBias )
                sb.AppendFormat( " SceneBias:{0:d}", _sceneBiasAmount );
            if ( _play )
                sb.Append( " Play" );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            for ( int i = 0; i < token.Length; i++ )
            {
                if ( token[ i ].Contains( "SceneBias:" ) )
                {
                    string num = token[ i ].Substring( token[ i ].IndexOf( ":" ) + 1 );
                    _sceneBias = true;
                    _sceneBiasAmount = UInt16.Parse( num );
                }
                else if ( token[ i ].Equals( "Play", StringComparison.InvariantCulture ) )
                {
                    _play = true;
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
