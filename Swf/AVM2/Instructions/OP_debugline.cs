using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_debugline : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _LineNumber;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _LineNumber = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _LineNumber );         
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _LineNumber );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            string ret = name + " (line number: " + _LineNumber.ToString( "d" ) + ")";
            return ret;
        }
	}
}
