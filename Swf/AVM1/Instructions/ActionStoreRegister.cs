using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionStoreRegister represents the Adobe AVM1 ActionStoreRegister
    /// </summary>
    public class ActionStoreRegister : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected byte _registerNum;
        #endregion

        #region constructors:
        /// <summary>
        /// ActionStoreRegister reads the next object from the stack (without popping it) 
        /// and stores it in one of four registers.
        /// </summary>
        public ActionStoreRegister()
        {
            this._registerNum = 0;
            _StackOps = new StackChange[ 0 ]; // that's not exactly true, but let's see
        }
        /// <summary>
        /// ActionStoreRegister reads the next object from the stack (without popping it) 
        /// and stores it in one of four registers.
        /// </summary>
        /// <param name="register">The number of the register</param>
        public ActionStoreRegister( byte register ) : this()
        {
            this._registerNum = register;
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public byte RegisterNum 
        { 
            get 
            { 
                return this._registerNum; 
            } 
            set 
            { 
                this._registerNum = value; 
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
        /// The minimum version that is required for the action
        /// </summary>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _registerNum = sourceStream.ReadByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            outputStream.Write( _registerNum );
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
            sb.AppendFormat( " Reg:{0:d}", _registerNum );
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
            _registerNum = Byte.Parse( token[ 0 ] );
            return true;
        }
        #endregion
    }
}
