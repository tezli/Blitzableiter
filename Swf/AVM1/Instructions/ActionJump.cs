using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionJump represents the Adobe AVM1 ActionJump 
    /// </summary>
    public class ActionJump : AbstractAction
    {
        #region fields:
        internal Int16 _offset;
        #endregion

        #region constructors:
        /// <summary>
        /// Creates an unconditional branch.
        /// </summary>
        public ActionJump()
        {
            this._offset = 0;
            _StackOps = new StackChange[ 0 ];
        }
        /// <summary>
        /// Creates an unconditional branch.
        /// </summary>
        /// <param name="offset">The offset is a signed quantity, enabling 
        /// branches from –32,768 bytes to 32,767 bytes. An offset of 0 
        /// points to the action directly after the ActionJump action.</param>
        public ActionJump( Int16 offset ) : this()
        {
            this._offset = offset;
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public Int16 Offset 
        { 
            get 
            { 
                return this._offset; 
            } 
            set 
            { 
                this._offset = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsBranch 
        { 
            get 
            { 
                return true; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsConditional 
        { 
            get 
            { 
                return false; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTarget 
        { 
            get 
            { 
                return ( int )_offset; 
            } 
            set 
            { 
                _offset = ( short )value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTargetAdjusted 
        { 
            get 
            { 
                return ( int )( _offset + this.ActionLengthRendered ); 
            } 
            set 
            { 
                _offset = ( short )( value - this.ActionLengthRendered ); 
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
        /// Reads an offset from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream to read from</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _offset = sourceStream.ReadInt16();
        }

        /// <summary>
        /// Writes the offset back to the output stream
        /// </summary>
        /// <param name="outputStream">The stream to write to</param>
        /// <returns>The lenght of a 16bit unsigned integer</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            outputStream.Write( _offset );
            return sizeof( Int16 );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " {0:d}", _offset );
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
            _offset = Int16.Parse( token[ 0 ] );
            return true;
        }

        #endregion
    }
}
