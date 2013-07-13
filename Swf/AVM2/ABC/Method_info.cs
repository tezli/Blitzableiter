using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Recurity.Swf.AVM2.Static;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class Method_info
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 ReturnType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UInt32> ParamType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Name { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagNeedArguments { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagActivation { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagNeedRest { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagHasOptional { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagSetDxns { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FlagHasParamNames { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Option_detail> Option { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UInt32> ParamNames { get; internal set; }       // index into string table, not used by AVM2

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30((uint)ParamType.Count);
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)ReturnType);
                for (int i = 0; i < ParamType.Count; i++)
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30(ParamType[i]);
                }
                accumulator += VariableLengthInteger.EncodedLengthU30(Name);
                accumulator += 1; // Flags
                if (FlagHasOptional)
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30((uint)Option.Count);
                    for (int i = 0; i < Option.Count; i++)
                    {
                        accumulator += Option[i].Length;
                    }
                }
                if (FlagHasParamNames)
                {
                    for (int i = 0; i < ParamType.Count; i++)
                    {
                        accumulator += VariableLengthInteger.EncodedLengthU30(ParamNames[i]);
                    }
                }

                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse(Stream source)
        {
            Log.Debug(this, "Offset : " + source.Position); 
            UInt32 paramCount = VariableLengthInteger.ReadU30(source);
            ReturnType = VariableLengthInteger.ReadU30(source);

            ParamType = new List<UInt32>();
            for (uint i = 0; i < paramCount; i++)
            {
                ParamType.Add(VariableLengthInteger.ReadU30(source));
            }

            Name = VariableLengthInteger.ReadU30(source);

            byte flags = VariableLengthInteger.ReadU8(source);
            FlagNeedArguments = ((flags & 0x01) != 0);
            FlagActivation = ((flags & 0x02) != 0);
            FlagNeedRest = ((flags & 0x04) != 0);
            FlagHasOptional = ((flags & 0x08) != 0);
            FlagSetDxns = ((flags & 0x40) != 0);
            FlagHasParamNames = ((flags & 0x80) != 0);

            if ((flags & 0x30) != 0)
            {
                AbcFormatException fe = new AbcFormatException("Reserved flags used in Method_info");
                Log.Error(this, fe);
                throw fe;
            }

            if (FlagHasOptional)
            {
                UInt32 optionCount = VariableLengthInteger.ReadU30(source);

                if (optionCount > paramCount)
                {
                    AbcFormatException fe = new AbcFormatException("optionCount (" + optionCount.ToString("d") + ") > paramCount (" + paramCount.ToString("d") + ") in Method_info");
                    Log.Error(this, fe);
                    throw fe;
                }

                Option = new List<Option_detail>();
                for (uint i = 0; i < optionCount; i++)
                {
                    Option_detail od = new Option_detail();
                    od.Parse(source);
                    Option.Add(od);
                }
            }

            if (FlagHasParamNames)
            {
                ParamNames = new List<UInt32>();
                for (uint i = 0; i < paramCount; i++)
                {
                    ParamNames.Add(VariableLengthInteger.ReadU30(source));
                }
            }

            if (FlagNeedRest && FlagNeedArguments)
            {
                AbcFormatException fe = new AbcFormatException("NeedRest and NeedArguments both set");
                Log.Error(this, fe);
                throw fe;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            for (int i = 0; i < ParamType.Count; i++)
            {
                if (!abc.VerifyMultinameIndex(ParamType[i]))
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid param type in method_info: " + ParamType[i].ToString("d"));
                    Log.Error(this, ave);
                    throw ave;
                }
            }

            if (!abc.VerifyNameIndex(Name))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid method name : " + Name.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            if (!abc.VerifyMultinameIndex(ReturnType))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid return type: " + ReturnType.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            if (FlagHasOptional)
            {
                for (int i = 0; i < Option.Count; i++)
                {
                    Option[i].Verify(abc);
                }
            }

            if (FlagHasParamNames)
            {
                for (int i = 0; i < ParamNames.Count; i++)
                {
                    if (!abc.VerifyNameIndex(ParamNames[i]))
                    {
                        AbcVerifierException ave = new AbcVerifierException("Invalid param name: " + ParamNames[i].ToString("d"));
                        Log.Error(this, ave);
                        throw ave;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, (uint)ParamType.Count);
            VariableLengthInteger.WriteU30(destination, ReturnType);
            for (int i = 0; i < ParamType.Count; i++)
            {
                VariableLengthInteger.WriteU30(destination, ParamType[i]);
            }
            VariableLengthInteger.WriteU30(destination, Name);

            byte flags = (byte)(
                (FlagNeedArguments ? 0x01 : 0) |
                (FlagActivation ? 0x02 : 0) |
                (FlagNeedRest ? 0x04 : 0) |
                (FlagHasOptional ? 0x08 : 0) |
                (FlagSetDxns ? 0x40 : 0) |
                (FlagHasParamNames ? 0x80 : 0));
            destination.WriteByte(flags);

            if (FlagHasOptional)
            {
                VariableLengthInteger.WriteU30(destination, (uint)Option.Count);
                for (int i = 0; i < Option.Count; i++)
                {
                    Option[i].Write(destination);
                }
            }
            if (FlagHasParamNames)
            {
                for (int i = 0; i < ParamNames.Count; i++)
                {
                    VariableLengthInteger.WriteU30(destination, ParamNames[i]);
                }
            }
        }
    }
}
