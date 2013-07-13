using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_constructsuper : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _ArgCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _ArgCount = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _ArgCount );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return base.Length + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _ArgCount );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            string ret = name + " (" + _ArgCount.ToString( "d" ) + " args)";
            return ret;
        }
	}
}
