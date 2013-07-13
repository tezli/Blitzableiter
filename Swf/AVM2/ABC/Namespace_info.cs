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
    public enum NamespaceKind : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Namespace = 0x08,
        /// <summary>
        /// 
        /// </summary>
        PackageNamespace = 0x16,
        /// <summary>
        /// 
        /// </summary>
        PackageInternalNs = 0x17,
        /// <summary>
        /// 
        /// </summary>
        ProtectedNamespace = 0x18,
        /// <summary>
        /// 
        /// </summary>
        ExplicitNamespace = 0x19,
        /// <summary>
        /// 
        /// </summary>
        StaticProtectedNs = 0x1A,
        /// <summary>
        /// 
        /// </summary>
        PrivateNs = 0x05
    }

    /// <summary>
    /// 
    /// </summary>
    public class Namespace_info
    {
        /// <summary>
        /// 
        /// </summary>
        private NamespaceKind _Kind;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _Name;

        /// <summary>
        /// 
        /// </summary>
        public Namespace_info()
        {
            _Kind = NamespaceKind.Namespace;
            _Name = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse(Stream source)
        {

            BinaryReader br = new BinaryReader(source);
            byte kindId = br.ReadByte();

            if (!Enum.IsDefined(typeof(NamespaceKind), kindId))
            {
                AbcFormatException fe = new AbcFormatException("Namespace kind 0x" + kindId.ToString("X2") + " is undefined");
                Log.Error(this, fe);
                throw fe;
            }

            _Kind = (NamespaceKind)kindId;
            _Name = VariableLengthInteger.ReadU30(source);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            string name = null;

            if (!Enum.IsDefined(typeof(NamespaceKind), _Kind))
            {
                AbcVerifierException ave = new AbcVerifierException("Namespace with invalid kind type " + _Kind.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            try
            {
                name = abc.ConstantPool.Strings[(int)_Name];
            }
            catch (Exception e)
            {
                AbcVerifierException ave = new AbcVerifierException("Namespace with invalid name index " + _Name.ToString("d"), e);
                Log.Error(this, ave);
                throw ave;
            }

            if (!IsValidNamespace(name))
            {
                AbcVerifierException ave = new AbcVerifierException("Namespace name is invalid: >>>" + name + "<<<");
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
            destination.WriteByte((byte)_Kind);
            VariableLengthInteger.WriteU30(destination, _Name);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return 1 + // kindId
                    VariableLengthInteger.EncodedLength(_Name)
                    ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NamespaceKind Kind
        {
            get
            {
                return _Kind;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 NameIndex
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        /// <returns></returns>
        public string ToString(AbcFile abc)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(Enum.GetName(typeof(NamespaceKind), _Kind));
            sb.Append("]");
            if (0 == _Name)
            {
                sb.Append("*");
            }
            else
            {
                sb.Append(abc.ConstantPool.Strings[(int)_Name]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidNamespace(string name)
        {
            bool charTest = true;

            char[] chars = name.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (
                    ((chars[i] >= 'A') && (chars[i] <= 'Z'))
                    ||
                    ((chars[i] >= 'a') && (chars[i] <= 'z'))
                    ||
                    ((chars[i] >= '0') && (chars[i] <= '9'))
                    || (chars[i] == '-')
                    || (chars[i] == '_')
                    || (chars[i] == '.')
                    || (chars[i] == ':')
                    || (chars[i] == '$')
                    )
                {
                    // this is a legal name char 
                }
                else
                {
                    charTest = false;
                }
            }

            //
            // they may also be URIs like "http://adobe.com/AS3/2006/builtin"
            //
            if (!charTest)
            {
                try
                {
                    UriBuilder tempB = new UriBuilder(name);
                }
                catch (UriFormatException)
                {
                    return false;
                }
            }
            return true;
        }
    }


}
