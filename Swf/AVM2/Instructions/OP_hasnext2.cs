using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_hasnext2 : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _ObjectReg;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _IndexReg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _ObjectReg = AVM2.Static.VariableLengthInteger.ReadU32( sourceStream );
            _IndexReg = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU32( destination, _ObjectReg );
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _IndexReg );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _ObjectReg ) + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _IndexReg );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "{0} (object register: {1:d}, index register: {2:d}", base.ToString(), _ObjectReg, _IndexReg );
            return sb.ToString();            
        }
	}
}
