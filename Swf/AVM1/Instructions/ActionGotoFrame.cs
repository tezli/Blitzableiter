using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGotoFrame represents the Adobe AVM1 ActionGotoFrame
    /// </summary>
    public class ActionGotoFrame : AbstractAction
    {

        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _frameIndex;

        #endregion

        #region constructors:

        /// <summary>
        /// Instructs Flash Player to go to the specified frame in the current file
        /// </summary>
        public ActionGotoFrame()
        {
            this._frameIndex = 0;
            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Goes to a frame and is stack based.
        /// </summary>
        /// <param name="frame">The frame numberof the destination frame.</param>
        public ActionGotoFrame( UInt16 frame ) : this()
        {
            this._frameIndex = frame;
        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public UInt16 FrameIndex 
        { 
            get 
            { 
                return this._frameIndex; 
            } 
            set 
            { 
                this._frameIndex = value; 
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
                return 3; 
            }
        }

        /// <summary>
        /// Parses frame number to go to from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            if ( 2 != _length )
            {
                throw new AVM1ExceptionByteCodeFormat( "length invalid" );
            }

            _frameIndex = sourceStream.ReadUInt16();
        }

        /// <summary>
        /// Writes the frame number to go to back to the output stream
        /// </summary>
        /// <param name="outputStream">The outputstream</param>
        /// <returns>The size of an unsigned 16bit integer</returns>
        protected override ulong Render( BinaryWriter outputStream )
        {
            outputStream.Write( _frameIndex );
            return sizeof( UInt16 );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " Frage:{0:d}", _frameIndex );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            if ( token.Length != 1 )
                return false;
            _frameIndex = UInt16.Parse( token[ 0 ] );
            return true;
        }

        #endregion
    }
}
