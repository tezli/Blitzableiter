using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_lookupswitch : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected Int32 _DefaultOffset;
        /// <summary>
        /// 
        /// </summary>
        protected List<Int32> _CaseOffsets;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _DefaultOffset = AVM2.Static.VariableLengthInteger.ReadS24( sourceStream );
            UInt32 caseCount = AVM2.Static.VariableLengthInteger.ReadU30( sourceStream );
            // from spec: "There are case_count+1 case offsets. case_count is a u30."
            _CaseOffsets = new List<int>( ( int )caseCount + 1 );
            for ( long i = 0; i < ( caseCount + 1 ); i++ )
            {
                Int32 offset = AVM2.Static.VariableLengthInteger.ReadS24( sourceStream );
                _CaseOffsets.Add( offset );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteS24( destination, _DefaultOffset );
            AVM2.Static.VariableLengthInteger.WriteU30( destination, ( uint )_CaseOffsets.Count - 1 );
            for ( int i = 0; i < _CaseOffsets.Count; i++ )
            {
                AVM2.Static.VariableLengthInteger.WriteS24( destination, _CaseOffsets[ i ] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return (uint)(
                    1 + // opcode
                    ( ( _CaseOffsets.Count ) * 3 ) + // case offsets
                    3 + // default offset
                    AVM2.Static.VariableLengthInteger.EncodedLengthU30( ( uint )_CaseOffsets.Count - 1 )   // case count U30 field
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsBranch
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int[] BranchTarget
        {
            get
            {
                Int32[] dst = new Int32[ _CaseOffsets.Count + 1 ];
                for ( int i = 0; i < _CaseOffsets.Count; i++ )
                {
                    dst[ i ] = _CaseOffsets[ i ];
                }
                dst[ _CaseOffsets.Count ] = _DefaultOffset;
                return dst;
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
            sb.AppendFormat( " {0:d} cases: ", _CaseOffsets.Count );
            for ( int i = 0; i < _CaseOffsets.Count; i++ )
            {
                sb.AppendFormat(" {0:d}:{1:d}", i, _CaseOffsets[i] );
            }
            sb.AppendFormat( " default:{0:d}", _DefaultOffset );
            return sb.ToString();
        }
	}
}
