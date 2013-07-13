using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.Instructions
{
    /// <summary>
    /// 
    /// </summary>
    public class OP_debug : AbstractInstruction
    {
        /// <summary>
        /// 
        /// </summary>
        protected byte _DebugType;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Index;

        /// <summary>
        /// 
        /// </summary>
        protected byte _Reg;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Extra;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected override void Parse(BinaryReader sourceStream)
        {
            _DebugType = AVM2.Static.VariableLengthInteger.ReadU8(sourceStream);
            _Index = AVM2.Static.VariableLengthInteger.ReadU30(sourceStream);
            _Reg = AVM2.Static.VariableLengthInteger.ReadU8(sourceStream);
            _Extra = AVM2.Static.VariableLengthInteger.ReadU30(sourceStream);

            if (0 != _Extra)
            {
                // Of course, Adobe Flash CS4 makes use of this field
                AbcFormatException abcfe = new AbcFormatException("Extra (reserved) field in Debug instruction used: 0x" + _Extra.ToString("X"));
                Log.Warn(this, abcfe);
                //throw abcfe;
                //Log.Warn(this, "Extra (reserved) field in Debug instruction used: 0x" + _Extra.ToString("X"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public override void Verify(ABC.AbcFile abc)
        {
            if (!abc.VerifyStringIndex(_Index))
            {
                throw new AbcVerifierException("Invalid string index");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected override void WriteArgs(Stream destination)
        {
            AVM2.Static.VariableLengthInteger.WriteU8(destination, _DebugType);
            AVM2.Static.VariableLengthInteger.WriteU30(destination, _Index);
            AVM2.Static.VariableLengthInteger.WriteU8(destination, _Reg);
            //
            // Should be
            AVM2.Static.VariableLengthInteger.WriteU30(destination, _Extra);
            // but we don't want to write things that are undocumented!
            //
            //TODO : Change back after length checking implemented : AVM2.Static.VariableLengthInteger.WriteU30( destination, (uint)0 );
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                return 1 + sizeof(byte) * 2 + AVM2.Static.VariableLengthInteger.EncodedLengthU30(_Index) + AVM2.Static.VariableLengthInteger.EncodedLengthU30(_Extra);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(base.ToString());
        //    sb.AppendFormat("(debug type: 0x{0:X}, index:{1:d}, reg:{2:d})", _DebugType, _Index, _Reg);
        //    return sb.ToString();
        //}
    }
}
