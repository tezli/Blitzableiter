using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionWith represents the Adobe AVM1 ActionWith
    /// </summary>
    public class ActionWith : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _size;
        #endregion

        #region constructors:
        /// <summary>
        /// With block of script
        /// </summary>
        public ActionWith()
        {
            this._size = 0;

            _StackOps = new StackChange[ 0 ];
        }
        /// <summary>
        /// With block of script
        /// </summary>
        /// <param name="size">Number of bytes of code that follow</param>
        public ActionWith( UInt16 size ) : this()
        {
            this._size = size;
        }
        #endregion

        #region accessors:


        /// <summary>
        /// 
        /// </summary>
        public UInt16 Size 
        { 
            get 
            { 
                return this._size; 
            } 
            set 
            { 
                this._size = value; 
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
                return 5; 
            }
        }
        /// <summary>
        /// Reads 2 bytes from the stream to get the size
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version of the source</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _size = sourceStream.ReadUInt16();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            outputStream.Write( _size );
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
            sb.AppendFormat( " Size:{0:d}", _size );
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
            _size = UInt16.Parse( token[ 0 ] );
            return true;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UInt16 CodeSize
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTargetAdjusted
        {
            get
            {
                return ( int )( _size + this.ActionLengthRendered );
            }
            set
            {
                _size = ( ushort )( value - this.ActionLengthRendered );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int BranchTarget
        {
            get
            {
                return ( int )_size;
            }
            set
            {
                _size = ( ushort )value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsFunction
        {
            get
            {
                return true;
            }
        }
    }
}
