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
    public enum OptionType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Option_Int = 0x03, // integer table
        /// <summary>
        /// 
        /// </summary>
        Option_UInt = 0x04, // uinteger table
        /// <summary>
        /// 
        /// </summary>
        Option_Double = 0x06, // double table
        /// <summary>
        /// 
        /// </summary>
        Option_Utf8 = 0x01, //string
        /// <summary>
        /// 
        /// </summary>
        Option_True = 0x0B, // -
        /// <summary>
        /// 
        /// </summary>
        Option_False = 0x0A, // -
        /// <summary>
        /// 
        /// </summary>
        Option_Null = 0x0C, // -
        /// <summary>
        /// 
        /// </summary>
        Option_Undefined = 0x00, // -
        /// <summary>
        /// 
        /// </summary>
        Option_Namespace = 0x08, // namespace
        /// <summary>
        /// 
        /// </summary>
        Option_PackageNamespace = 0x16, // namespace
        /// <summary>
        /// 
        /// </summary>
        Option_PackageInternalNs = 0x17, // Namespace
        /// <summary>
        /// 
        /// </summary>
        Option_ProtectedNamespace = 0x18, // Namespace
        /// <summary>
        /// 
        /// </summary>
        Option_ExplicitNamespace = 0x19, // Namespace
        /// <summary>
        /// 
        /// </summary>
        Option_StaticProtectedNs = 0x1A, // Namespace
        /// <summary>
        /// 
        /// </summary>
        Option_PrivateNs = 0x05, // namespace
        /// <summary>
        /// 
        /// </summary>
        TotallyInvalid = 0x7F
    }

    /// <summary>
    /// 
    /// </summary>
    public class Option_detail
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _Value;
        /// <summary>
        /// 
        /// </summary>
        private OptionType _Type;

        /// <summary>
        /// 
        /// </summary>
        public OptionType OptionType { get { return _Type; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse(Stream source)
        {
            Log.Debug(this, "Offset : " + source.Position);
            _Value = VariableLengthInteger.ReadU30(source);
            byte type = VariableLengthInteger.ReadU8(source);

            if (!Enum.IsDefined(typeof(OptionType), type))
            {
                AbcFormatException fe = new AbcFormatException("Option_detail entry type 0x" + type.ToString("X02") + " is invalid");
                Log.Error(this, fe);
                throw fe;
            }

            _Type = (OptionType)type;
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return VariableLengthInteger.EncodedLengthU30(_Value) + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            try
            {
                object test = this.GetValue(abc);
            }
            catch (IndexOutOfRangeException ioor)
            {
                AbcVerifierException ave = new AbcVerifierException("Option_info out of range value", ioor);
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
            VariableLengthInteger.WriteU30(destination, _Value);
            destination.WriteByte((byte)_Type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        /// <returns></returns>
        public object GetValue(AbcFile abc)
        {
            object ret;

            switch (_Type)
            {
                case OptionType.Option_Int:
                    Int32 v = abc.ConstantPool.Integers[(int)_Value];
                    ret = v;
                    break;

                case OptionType.Option_UInt:
                    UInt32 u = abc.ConstantPool.UIntegers[(int)_Value];
                    ret = u;
                    break;

                case OptionType.Option_Double:
                    Double d = abc.ConstantPool.Doubles[(int)_Value];
                    ret = d;
                    break;

                case OptionType.Option_Utf8:
                    string ut = abc.ConstantPool.Strings[(int)_Value];
                    ret = ut;
                    break;

                case OptionType.Option_True:
                    bool bt = true;
                    ret = bt;
                    break;

                case OptionType.Option_False:
                    bool bf = false;
                    ret = bf;
                    break;

                case OptionType.Option_Null:
                    string sn = "NULL";
                    ret = sn;
                    break;

                case OptionType.Option_Undefined:
                    string su = "UNDEFINED";
                    ret = su;
                    break;

                case OptionType.Option_Namespace:
                case OptionType.Option_PackageNamespace:
                case OptionType.Option_PackageInternalNs:
                case OptionType.Option_ProtectedNamespace:
                case OptionType.Option_ExplicitNamespace:
                case OptionType.Option_StaticProtectedNs:
                case OptionType.Option_PrivateNs:
                    string ns = abc.ConstantPool.Namespaces[(int)_Value].ToString(abc);
                    ret = ns;
                    break;

                default:
                    Exception e = new Exception("Invalid option type value 0x" + _Type.ToString("X02"));
                    Log.Error(this, e);
                    throw e;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public string OptionTypeName
        {
            get
            {
                return Enum.GetName(typeof(OptionType), _Type);
            }
        }
    }
}
