using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
	public class OP_ifgt : AbstractInstruction
	{
        /// <summary>
        /// 
        /// </summary>
        protected Int32 _Offset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse( BinaryReader sourceStream )
        {
            _Offset = AVM2.Static.VariableLengthInteger.ReadS24( sourceStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs( Stream destination )
        {
            AVM2.Static.VariableLengthInteger.WriteS24( destination, _Offset );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 4;
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
                Int32[] dst = new Int32[ 1 ];
                dst[ 0 ] = ( int )( _Offset + this.Length );
                return dst;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string name = base.ToString();
            return name + " " + _Offset.ToString( "d" );
        }
	}
}
