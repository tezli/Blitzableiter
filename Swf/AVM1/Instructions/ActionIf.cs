using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionIf represents the Adobe AVM1 ActionIf
    /// </summary>
    public class ActionIf : AbstractAction
    {
        #region fields:
        internal Int16 _offset;
        #endregion

        #region constructors:
        /// <summary>
        /// Creates a conditional test and branch.
        /// </summary>
        public ActionIf()
        {
            _StackOps = new StackChange[ 1 ];
            // default to version > 4
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_boolean );
        }
        /// <summary>
        /// Creates a conditional test and branch.
        /// </summary>
        /// <param name="offset">The offset is a signed quantity, enabling 
        /// branches from –32768 bytes to 32767 bytes. An offset of 0 points 
        /// to the action directly after the ActionIf action.</param>
        public ActionIf( Int16 offset ) : this()
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
            { this._offset = value; 
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
                _offset = ( Int16 )( value - this.ActionLengthRendered ); 
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
                _offset = ( Int16 )value; 
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
                return true; 
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
        /// Reads the offset from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The Version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _offset = sourceStream.ReadInt16();
        }

        /// <summary>
        /// Renders the offset back to an output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The size of an unsigned 16bit integer</returns>
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

        /// <summary>
        /// 
        /// </summary>
        public override StackChange[] StackOperations
        {
            get
            {                
                if ( this.Version < 5 )
                {
                    _StackOps[ 0 ].DataType = AVM1DataTypes.AVM_String;
                }
                else
                {
                    _StackOps[ 0 ].DataType = AVM1DataTypes.AVM_boolean;
                }
                return _StackOps;
            }
        }
    }
}
