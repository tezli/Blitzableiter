using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_pushint : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Index;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _Index = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public override void Verify( ABC.AbcFile abc )
        {
            if ( _Index >= abc.ConstantPool.Integers.Count )
            {
                throw new AbcVerifierException( "Int index out of bounds: " + _Index.ToString( "d" ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _Index );         
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _Index );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            string ret = name + " (index: " + _Index.ToString( "d" ) + ")";
            return ret;
        }
	}
}
