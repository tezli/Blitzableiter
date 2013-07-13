using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionWaitForFrame represents the Adobe AVM1 ActionWaitForFrame
    /// </summary>
    public class ActionWaitForFrame : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _frame;

        /// <summary>
        /// 
        /// </summary>
        protected byte _skipCount;

        #endregion

        #region constructors:

        /// <summary>
        /// Waits for a frame to be loaded and is stack based
        /// </summary>
        public ActionWaitForFrame()
        {
            _frame = 0;
            _skipCount = 0;

            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Waits for a frame to be loaded and is stack based
        /// </summary>
        /// <param name="frame">The number of the frame to wait for</param>
        /// <param name="skip">The number of actions to skip</param>
        public ActionWaitForFrame( UInt16 frame, byte skip ) :this()
        {
            _frame = frame;
            _skipCount = skip;
        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public UInt16 Frame 
        { 
            get 
            { 
                return this._frame; 
            } 
            set 
            { 
                this._frame = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Skip 
        { 
            get 
            { 
                return this._skipCount; 
            } 
            set 
            { 
                this._skipCount = value; 
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
        /// Read the frame to be wait for and the number of skipped actions
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            if ( 3 != _length )
            {
                throw new AVM1ExceptionByteCodeFormat( "illegal length" );
            }

            _frame = sourceStream.ReadUInt16();
            _skipCount = sourceStream.ReadByte();
        }

        /// <summary>
        /// Writes the frame to be waiting for and the number of actions to skip back to the output stream
        /// </summary>
        /// <param name="outputStream">The outpustream to write to</param>
        /// <returns>The size of an unsigned 16 bit integer + a byte</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            outputStream.Write( _frame );
            outputStream.Write( _skipCount );

            return ( sizeof( UInt16 ) + sizeof( byte ) );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " Frame:{0:d}, Skip:{1:d}", _frame, _skipCount );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            if ( token.Length != 2 )
                return false;
            _frame = UInt16.Parse( token[ 0 ] );
            _skipCount = Byte.Parse( token[ 1 ] );
            return true;
        }
        #endregion
    }
}
