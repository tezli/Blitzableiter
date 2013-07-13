using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_callsuperid : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Index;

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
            throw new NotImplementedException( "UNDOCUMENTED" );
            //_Index = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
            //_ArgCount = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public override void Verify( ABC.AbcFile abc )
        {
            if ( !abc.VerifyMultinameIndex( _Index ) )
            {
                throw new AbcVerifierException( "Invalid multiname index" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _Index );
            AVM2.Static.VariableLengthInteger.WriteU30( destination, _ArgCount );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _ArgCount ) + AVM2.Static.VariableLengthInteger.EncodedLengthU30( _Index );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendFormat( " (index {0:d}, {1:d} args)", _Index, _ArgCount );
            return sb.ToString();
        }
	}
}
