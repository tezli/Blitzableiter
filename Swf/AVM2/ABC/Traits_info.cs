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
    public enum TraitType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Trait_Slot = 0,
        /// <summary>
        /// 
        /// </summary>
        Trait_Method = 1,
        /// <summary>
        /// 
        /// </summary>
        Trait_Getter = 2,
        /// <summary>
        /// 
        /// </summary>
        Trait_Setter = 3,
        /// <summary>
        /// 
        /// </summary>
        Trait_Class = 4,
        /// <summary>
        /// 
        /// </summary>
        Trait_Function = 5,
        /// <summary>
        /// 
        /// </summary>
        Trait_Const = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public class TraitsData_Slot
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 SlotID;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 TypeName;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 Vindex;
        /// <summary>
        /// 
        /// </summary>
        public OptionType Vkind;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return
                    VariableLengthInteger.EncodedLengthU30(SlotID) +
                    VariableLengthInteger.EncodedLengthU30(TypeName) +
                    VariableLengthInteger.EncodedLengthU30(Vindex) +
                    (uint)(Vindex == 0 ? 0 : 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (!abc.VerifyMultinameIndex(TypeName))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid TypeName: " + TypeName.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            if (0 == Vindex)
            {
                return;
            }
            else
            {
                if ((!Enum.IsDefined(typeof(OptionType), Vkind)) || (Vkind == OptionType.TotallyInvalid))
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid Vkind: " + Vkind.ToString("d"));
                    Log.Error(this, ave);
                    throw ave;
                }

                bool valid = false;
                switch (Vkind)
                {
                    case OptionType.Option_Int:
                        valid = (Vindex < abc.ConstantPool.Integers.Count);
                        break;
                    case OptionType.Option_UInt:
                        valid = (Vindex < abc.ConstantPool.UIntegers.Count);
                        break;
                    case OptionType.Option_Double:
                        valid = (Vindex < abc.ConstantPool.Doubles.Count);
                        break;
                    case OptionType.Option_Utf8:
                        valid = (Vindex < abc.ConstantPool.Strings.Count);
                        break;
                    case OptionType.Option_True:
                    case OptionType.Option_False:
                    case OptionType.Option_Null:
                    case OptionType.Option_Undefined:
                        valid = true;
                        break;
                    case OptionType.Option_Namespace:
                    case OptionType.Option_PackageNamespace:
                    case OptionType.Option_PackageInternalNs:
                    case OptionType.Option_ProtectedNamespace:
                    case OptionType.Option_ExplicitNamespace:
                    case OptionType.Option_StaticProtectedNs:
                    case OptionType.Option_PrivateNs:
                        valid = (Vindex < abc.ConstantPool.Namespaces.Count);
                        break;
                    default:
                        valid = false;
                        break;
                }

                if (!valid)
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid Vindex " + Vindex.ToString("d") + " for vkind " + Vkind.ToString("d"));
                    Log.Error(this, ave);
                    throw ave;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, SlotID);
            VariableLengthInteger.WriteU30(destination, TypeName);
            VariableLengthInteger.WriteU30(destination, Vindex);
            if (0 != Vindex)
            {
                VariableLengthInteger.WriteU8(destination, (byte)Vkind);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct TraitsData_Class
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 SlotID;

        /// <summary>
        /// 
        /// </summary>
        public UInt32 ClassI;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return
                    VariableLengthInteger.EncodedLengthU30(SlotID) +
                    VariableLengthInteger.EncodedLengthU30(ClassI);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (ClassI >= abc.Classes.Count)
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid Classi: " + ClassI.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, SlotID);
            VariableLengthInteger.WriteU30(destination, ClassI);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct TraitsData_Function
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 SlotID;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 Function;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return
                    VariableLengthInteger.EncodedLengthU30(SlotID) +
                    VariableLengthInteger.EncodedLengthU30(Function);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (Function >= abc.Methods.Count)
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid Function: " + Function.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, SlotID);
            VariableLengthInteger.WriteU30(destination, Function);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct TraitsData_Method
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 DispID;

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Method;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return
                    VariableLengthInteger.EncodedLengthU30(DispID) +
                    VariableLengthInteger.EncodedLengthU30(Method);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (Method >= abc.Methods.Count)
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid Method: " + Method.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, DispID);
            VariableLengthInteger.WriteU30(destination, Method);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Traits_info
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 Name { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public TraitType Type { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AttribFinal { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AttribOverride { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AttribMetadata { get; internal set; }
        //
        // This sucks badly, but the abc file format leaves little choice
        //
        /// <summary>
        /// 
        /// </summary>
        private TraitsData_Slot _Data_Slot; // may be null
        /// <summary>
        /// 
        /// </summary>
        private TraitsData_Class _Data_Class; // may be null
        /// <summary>
        /// 
        /// </summary>
        private TraitsData_Function _Data_Function; // may be null
        /// <summary>
        /// 
        /// </summary>
        private TraitsData_Method _Data_Method;
        //
        // End sucking
        //
        /// <summary>
        /// 
        /// </summary>
        private List<UInt32> _Metadata;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30(Name);
                accumulator += sizeof(byte); // kind, kindType, kindAttr
                switch (Type)
                {
                    case TraitType.Trait_Const:
                    case TraitType.Trait_Slot:
                        accumulator += _Data_Slot.Length;
                        break;

                    case TraitType.Trait_Class:
                        accumulator += _Data_Class.Length;
                        break;

                    case TraitType.Trait_Function:
                        accumulator += _Data_Function.Length;
                        break;

                    case TraitType.Trait_Method:
                    case TraitType.Trait_Getter:
                    case TraitType.Trait_Setter:
                        accumulator += _Data_Method.Length;
                        break;

                    default:
                        Exception e = new Exception("Internal error: invalid _Type 0x" + Type.ToString("X02") + " found");
                        Log.Error(this, e);
                        throw e;
                }
                if (AttribMetadata)
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30((uint)_Metadata.Count);
                    for (int i = 0; i < _Metadata.Count; i++)
                    {
                        accumulator += VariableLengthInteger.EncodedLengthU30(_Metadata[i]);
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
            // UNTESTED
            Name = VariableLengthInteger.ReadU30(source);

            byte kind = VariableLengthInteger.ReadU8(source);
            byte kindType = (byte)(kind & 0x0F);
            byte kindAttr = (byte)((kind & 0xF0) >> 4);

            if (!Enum.IsDefined(typeof(TraitType), kindType))
            {
                AbcFormatException fe = new AbcFormatException("Invalid Traits_info kind " + kindType.ToString("d"));
                Log.Error(this, fe);
                throw fe;
            }
            Type = (TraitType)kindType;

            //
            // "Any other combination of attribute with kind is ignored."
            // - so we test for it.
            //
            AttribFinal = (kindAttr & 0x01) != 0 ? true : false;
            AttribOverride = (kindAttr & 0x02) != 0 ? true : false;
            AttribMetadata = (kindAttr & 0x04) != 0 ? true : false;

            if (AttribFinal && (!((Type == TraitType.Trait_Getter) || (Type == TraitType.Trait_Setter) || (Type == TraitType.Trait_Method))))
            {
                AbcFormatException fe = new AbcFormatException("ATTR_Final with trait type " + Enum.GetName(typeof(TraitType), Type));
                Log.Error(this, fe);
                throw fe;
            }
            if (AttribOverride && (!((Type == TraitType.Trait_Getter) || (Type == TraitType.Trait_Setter) || (Type == TraitType.Trait_Method))))
            {
                AbcFormatException fe = new AbcFormatException("ATTR_Override with trait type " + Enum.GetName(typeof(TraitType), Type));
                Log.Error(this, fe);
                throw fe;
            }


            //
            // Just to point out how this works: only one of them 
            // is going to be valid 
            //            
            switch (Type)
            {
                case TraitType.Trait_Const:
                case TraitType.Trait_Slot:
                    _Data_Slot = ParseSlot(source);
                    break;

                case TraitType.Trait_Class:
                    _Data_Class = ParseClass(source);
                    break;

                case TraitType.Trait_Function:
                    _Data_Function = ParseFunction(source);
                    break;

                case TraitType.Trait_Method:
                case TraitType.Trait_Getter:
                case TraitType.Trait_Setter:
                    _Data_Method = ParseMethod(source);
                    break;

                default:
                    Exception e = new Exception("Internal error: invalid _Type 0x" + Type.ToString("X02") + " reached end of switch");
                    Log.Error(this, e);
                    throw e;
            }

            if (AttribMetadata)
            {
                UInt32 metadataCount = VariableLengthInteger.ReadU30(source);
                _Metadata = new List<UInt32>((int)metadataCount);
                for (uint i = 0; i < metadataCount; i++)
                {
                    UInt32 metav = VariableLengthInteger.ReadU30(source);
                    _Metadata.Add(metav);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (!abc.VerifyMultinameIndex(Name))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid name: " + Name.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }
            if (abc.ConstantPool.Multinames[(int)Name].Type != MultinameType.QName)
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid name type");
                Log.Error(this, ave);
                throw ave;
            }

            if (AttribMetadata)
            {
                foreach (UInt32 ind in _Metadata)
                {
                    if (ind >= abc.Metadata.Count)
                    {
                        AbcVerifierException ave = new AbcVerifierException("Invalid metadata index: " + ind.ToString("d"));
                        Log.Error(this, ave);
                        throw ave;
                    }
                }
            }

            switch (Type)
            {
                case TraitType.Trait_Const:
                case TraitType.Trait_Slot:
                    _Data_Slot.Verify(abc);
                    break;

                case TraitType.Trait_Class:
                    _Data_Class.Verify(abc);
                    break;

                case TraitType.Trait_Function:
                    _Data_Function.Verify(abc);
                    break;

                case TraitType.Trait_Method:
                case TraitType.Trait_Getter:
                case TraitType.Trait_Setter:
                    _Data_Method.Verify(abc);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraitsData_Slot Slot
        {
            get
            {
                if ((Type != TraitType.Trait_Slot) && (Type != TraitType.Trait_Const))
                    throw new InvalidCastException("Instance does not hold Slot");
                else
                    return _Data_Slot;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraitsData_Class Class
        {
            get
            {
                if (Type != TraitType.Trait_Class)
                    throw new InvalidCastException("Instance does not hold Class");
                else
                    return _Data_Class;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraitsData_Function Function
        {
            get
            {
                if (Type != TraitType.Trait_Function)
                    throw new InvalidCastException("Instance does not hold Function");
                else
                    return _Data_Function;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TraitsData_Method Method
        {
            get
            {
                if ((Type != TraitType.Trait_Method) && (Type != TraitType.Trait_Getter) && (Type != TraitType.Trait_Setter))
                    throw new InvalidCastException("Instance does not hold Method");
                else
                    return _Data_Method;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private TraitsData_Slot ParseSlot(Stream source)
        {
            TraitsData_Slot s = new TraitsData_Slot();

            s.SlotID = VariableLengthInteger.ReadU30(source);
            s.TypeName = VariableLengthInteger.ReadU30(source);
            s.Vindex = VariableLengthInteger.ReadU30(source);

            if (0 != s.Vindex)
            {
                byte kind = VariableLengthInteger.ReadU8(source);
                if (!Enum.IsDefined(typeof(OptionType), kind))
                {
                    AbcFormatException fe = new AbcFormatException("TraitsData Slot invalid constant type 0x" + kind.ToString("X02"));
                    Log.Error(this, fe);
                    throw fe;
                }

                s.Vkind = (OptionType)kind;
            }
            else
            {
                s.Vkind = OptionType.TotallyInvalid;
            }

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private TraitsData_Class ParseClass(Stream source)
        {
            TraitsData_Class c = new TraitsData_Class();
            c.SlotID = VariableLengthInteger.ReadU30(source);
            c.ClassI = VariableLengthInteger.ReadU30(source);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private TraitsData_Function ParseFunction(Stream source)
        {
            TraitsData_Function f = new TraitsData_Function();
            f.SlotID = VariableLengthInteger.ReadU30(source);
            f.Function = VariableLengthInteger.ReadU30(source);
            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private TraitsData_Method ParseMethod(Stream source)
        {
            TraitsData_Method m = new TraitsData_Method();
            m.DispID = VariableLengthInteger.ReadU30(source);
            m.Method = VariableLengthInteger.ReadU30(source);
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            byte kindAndAttr = (byte)Type;
            kindAndAttr = (byte)(kindAndAttr | (AttribFinal ? 0x10 : 0));
            kindAndAttr = (byte)(kindAndAttr | (AttribOverride ? 0x20 : 0));
            kindAndAttr = (byte)(kindAndAttr | (AttribMetadata ? 0x40 : 0));

            VariableLengthInteger.WriteU32(destination, Name);
            VariableLengthInteger.WriteU8(destination, kindAndAttr);

            switch (Type)
            {
                case TraitType.Trait_Const:
                case TraitType.Trait_Slot:
                    _Data_Slot.Write(destination);
                    break;

                case TraitType.Trait_Class:
                    _Data_Class.Write(destination);
                    break;

                case TraitType.Trait_Function:
                    _Data_Function.Write(destination);
                    break;

                case TraitType.Trait_Method:
                case TraitType.Trait_Getter:
                case TraitType.Trait_Setter:
                    _Data_Method.Write(destination);
                    break;

                default:
                    Exception e = new Exception("Internal error: invalid _Type 0x" + Type.ToString("X02") + " found");
                    Log.Error(this, e);
                    throw e;
            }

            if (AttribMetadata)
            {
                VariableLengthInteger.WriteU30(destination, (uint)_Metadata.Count);
                for (int i = 0; i < _Metadata.Count; i++)
                {
                    VariableLengthInteger.WriteU30(destination, _Metadata[i]);
                }
            }
        }
    }

}
