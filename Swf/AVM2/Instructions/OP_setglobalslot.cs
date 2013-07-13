using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_setglobalslot : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _SlotIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _SlotIndex = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public override void Verify( ABC.AbcFile abc )
        {
            if ( 0 == _SlotIndex )
            {
                throw new AbcVerifierException( "Slot index is 0" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _SlotIndex );         
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _SlotIndex );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            string ret = name + " (index: " + _SlotIndex.ToString( "d" ) + ")";
            return ret;
        }
	}
}
