using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionCall represents the Adobe AVM1 ActionCall
    /// </summary>
    public class ActionCall : AbstractAction
    {
        #region fields: 
       
        #endregion

        #region constructors:

        /// <summary>
        /// Calls a subroutine
        /// </summary>
        public ActionCall()
        {
            _StackOps = new StackChange[ 1 ];
            _StackOps[ 0 ] = new StackPop( AVM1DataTypes.AVM_String );
        }

        #endregion

        #region accessors:

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
        /// While ActionCall can have arguments, it operates on the Frame name / number
        /// on the stack and hence always has a length field of 0x0000. Therefore, if 
        /// we end up here, it's already a format violation.
        /// </summary>
        /// <param name="sourceStream">The source stream to read from</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            throw new AVM1ExceptionByteCodeFormat( "Argument to ActionCall detected (hell no, it should not be!)" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            return 0;
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            _length = 0;
            return true;
        }
        #endregion
    }
}
