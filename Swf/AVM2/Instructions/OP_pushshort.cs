using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_pushshort : AbstractInstruction
	{
        /// <summary>
        /// WARNING: The Flash CS4 compiler, for example, will encode -1337 as 
        /// VariableLengthInteger: C7 F5 FF FF 0F
        /// which becomes 0x3FFFFAC7
        /// The assumption, however, is, that the pushed value is really a promoted
        /// short (16Bit) so that the value should actually be cast into a Int16.
        /// </summary>
        protected UInt32 _Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            // In order to not trigger detection of MSBits set in U30, we divert
            // from the spec a bit and use U32 here
            // _Value = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
            _Value = AVM2.Static.VariableLengthInteger.ReadU32( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            // Although specified as U30, we will write U32, since that's what it 
            // really is
            AVM2.Static.VariableLengthInteger.WriteU32( destination, _Value );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _Value );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            string ret = name + " 0x" + _Value.ToString( "X02" ) + " (" + unchecked((Int16)_Value).ToString("d")+" corrected)";
            return ret;
        }
	}
}
