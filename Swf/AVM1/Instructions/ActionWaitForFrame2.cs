using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionWaitForFrame2 represents the Adobe AVM1 ActionWaitForFrame2
    /// </summary>
    public class ActionWaitForFrame2 : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected byte _skipCount;
        #endregion

        #region constructors:
        /// <summary>
        /// Waits for a frame to be loaded and is stack based
        /// </summary>
        public ActionWaitForFrame2()
        {
            this._skipCount = 0;

            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String ); // frame
        }
        /// <summary>
        /// Waits for a frame to be loaded and is stack based
        /// </summary>
        /// <param name="skip">The number of actions to skip</param>
        public ActionWaitForFrame2( byte skip ) : this()
        {
            this._skipCount = skip;
        }
        #endregion

        #region accessors:

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
                return 4; 
            }
        }

        /// <summary>
        /// Parses the number of actions to skip from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            if ( 1 != _length )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionWaitForFrame2 length mismatch" );
            }
            _skipCount = sourceStream.ReadByte();
        }

        /// <summary>
        /// Renders the number of actions to skip back to an output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The size of a byte</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            outputStream.Write( _skipCount );
            return sizeof( byte );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " Skip:{0:d}", _skipCount );
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
            _skipCount = Byte.Parse( token[ 0 ] );
            return true;
        }
        #endregion
    }
}
