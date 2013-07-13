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
    public class AbcFile
    {
        private UInt16 _MajorVersion;
        private UInt16 _MinorVersion;
        private Cpool_info _ConstantPool;
        private List<Method_info> _Method;
        private List<Metadata_info> _Metadata;
        private List<Instance_info> _Instance;
        private List<Class_info> _Class;
        private List<Script_info> _Script;
        private Dictionary<UInt32, Method_body_info> _MethodBody;

        /// <summary>
        /// 
        /// </summary>
        public IList<Method_info> Methods { get { return _Method; } }

        /// <summary>
        /// 
        /// </summary>
        public IList<Metadata_info> Metadata { get { return _Metadata; } }

        /// <summary>
        /// 
        /// </summary>
        public IList<Instance_info> Instances { get { return _Instance; } }

        /// <summary>
        /// 
        /// </summary>
        public IList<Class_info> Classes { get { return _Class; } }

        /// <summary>
        /// 
        /// </summary>
        public IList<Script_info> Scripts { get { return _Script; } }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<UInt32, Method_body_info> MethodBodies { get { return _MethodBody; } }

        /// <summary>
        /// 
        /// </summary>
        public Cpool_info ConstantPool { get { return _ConstantPool; } }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint debugLast = 0;

                uint accumulator = 0;
                accumulator += 4; // major and minor version

                // 1. Constant Pool
                String s = String.Format("{0:X08} ConstantPool (last: {1:X08})", accumulator, accumulator - debugLast);
                Log.Debug(this, s);

                debugLast = accumulator;
                accumulator += _ConstantPool.Length;
                accumulator += VariableLengthInteger.EncodedLength((uint)_Method.Count);

                // 2. Methods
                for (int i = 0; i < _Method.Count; i++)
                {
                    accumulator += _Method[i].Length;
                }

                // 3. Metadata
                String s1 = String.Format("{0:X08} Metadata (last: {1:X08})", accumulator, accumulator - debugLast);
                Log.Debug(this, s1);

                debugLast = accumulator;

                if (SwfFile.Configuration.AbcRemoveMetadata)
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30((uint)0);
                }
                else
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30((uint)_Metadata.Count);
                    for (int i = 0; i < _Metadata.Count; i++)
                    {
                        accumulator += _Metadata[i].Length;
                    }
                }

                // 4. Instances
                String s2 = String.Format("{0:X08} Instance_info (last: {1:X08})", accumulator, accumulator - debugLast);
                debugLast = accumulator;
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)_Instance.Count);
                Log.Debug(this, s2);

                for (int i = 0; i < _Instance.Count; i++)
                {
                    accumulator += _Instance[i].Length;
                }

                // 5. Classes
                String s3 = String.Format("{0:X08} Class_info (last: {1:X08})", accumulator, accumulator - debugLast );
                debugLast = accumulator;
                Log.Debug(this, s3);

                // uses classCount == instanceCount, therefore, no length field added here
                for (int i = 0; i < _Class.Count; i++)
                {
                    accumulator += _Class[i].Length;
                }

                // 6. Scripts
                String s4 = String.Format("{0:X08} Script_info (last: {1:X08})", accumulator, accumulator - debugLast);
                Log.Debug(this, s4);
                debugLast = accumulator;
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)_Script.Count);

                for (int i = 0; i < _Script.Count; i++)
                {
                    accumulator += _Script[i].Length;
                }

                // 7. Methods
                String s5 = String.Format("{0:X08} MethodBody_info (last: {1:X08})", accumulator, accumulator - debugLast);
                Log.Debug(this, s5);
                debugLast = accumulator;
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)_MethodBody.Count);

                foreach (UInt32 k in _MethodBody.Keys)
                {
                    accumulator += _MethodBody[k].Length;
                }

                String s6 = String.Format("{0:X08} End (last: {1:X08})", accumulator, accumulator - debugLast);
                Log.Debug(this, s6);

                debugLast = accumulator;
                return accumulator;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="source">The source stream.</param>
        public void Parse(Stream source)
        {
            long startPos = source.Position;

            _MinorVersion = VariableLengthInteger.ReadU16(source);
            _MajorVersion = VariableLengthInteger.ReadU16(source);

            String s1 = String.Format("ABC Version {0:d}.{1:d}", _MajorVersion, _MinorVersion);
            Log.Debug(this, s1);
            if (_MajorVersion != 46)
            {
                AbcFormatException fe = new AbcFormatException("ABC major version mismatch: " + _MajorVersion.ToString("d") + " (expected: 46)");
                Log.Error(this, fe);
                throw fe;
            }
            if (_MinorVersion != 16)
            {
                String s2 = String.Format("ABC minor version {d:0} is not equal to expected version 16", _MinorVersion);
                Log.Warn(this, s2);
            }

            // (1) ConstantPool
            String s3 = String.Format("0x{0:X08}: ConstantPool", source.Position - startPos);
            Log.Debug(this, s3);
            _ConstantPool = new Cpool_info();
            _ConstantPool.Parse(source);
            Log.Debug(this, _ConstantPool.ToString(this));

            // (2) Method_Info
            String s4 = String.Format("0x{0:X08}: Method_info", source.Position - startPos);
            Log.Debug(this, s4);
            UInt32 methodCount = VariableLengthInteger.ReadU30(source);
            _Method = new List<Method_info>();
            for (uint i = 0; i < methodCount; i++)
            {
                Method_info mi = new Method_info();
                mi.Parse(source);
                _Method.Add(mi);
            }
            String s5 = String.Format("{0:d} Methods", _Method.Count);
            Log.Debug(this, s5);

            // (3) Metadata
            String s6 = String.Format("0x{0:X08}: Metadata", source.Position - startPos);
           // //Log.Debug(this, s6);
            UInt32 metadataInfoCount = VariableLengthInteger.ReadU30(source);
            _Metadata = new List<Metadata_info>();
            for (uint i = 0; i < metadataInfoCount; i++)
            {
                Metadata_info mdi = new Metadata_info();
                mdi.Parse(source);
                _Metadata.Add(mdi);
            }

            String s7 = String.Format("{0:d} Metadata entries", _Metadata.Count);
            Log.Debug(this, s7);

            // (4) Instance_info
            String s8 = String.Format("0x{0:X08}: Instance_info", source.Position - startPos);
            Log.Debug(this, s8);
            UInt32 classCount = VariableLengthInteger.ReadU30(source);
            _Instance = new List<Instance_info>((int)classCount);
            for (uint i = 0; i < classCount; i++)
            {
                Instance_info ii = new Instance_info();
                ii.Parse(source);
                _Instance.Add(ii);
            }
            String s9 = String.Format("{0:d} Instances", _Instance.Count);
            Log.Debug(this, s9);

            // (5) Class_info
            String s10 = String.Format("0x{0:X08}: Class_info", source.Position - startPos);
            Log.Debug(this, s10);
            _Class = new List<Class_info>((int)classCount);
            for (uint i = 0; i < classCount; i++)
            {
                Class_info ci = new Class_info();
                ci.Parse(source);
                _Class.Add(ci);
            }
            String s11 = String.Format("{0:d} Classes", _Class.Count);
            Log.Debug(this, s11);

            // (6) Script_info
            String s12 = String.Format("0x{0:X08}: Script_info", source.Position - startPos);
            Log.Debug(this, s12);

            UInt32 scriptCount = VariableLengthInteger.ReadU30(source);
            _Script = new List<Script_info>((int)scriptCount);
            for (uint i = 0; i < scriptCount; i++)
            {
                Script_info si = new Script_info();
                si.Parse(source);
                _Script.Add(si);
            }
            String s13 = String.Format("{0:d} Scripts", _Script.Count);
            Log.Debug(this, s13);


            // (7) Method_body_info
            String s14 = String.Format("0x{0:X08}: MethodBody_info", source.Position - startPos);
            Log.Debug(this, s14);

            UInt32 methodBodyCount = VariableLengthInteger.ReadU30(source);

            if (methodBodyCount > methodCount)
            {
                String s15 = String.Format("More method bodies ({0:d}) than methods ({1:d})!", methodBodyCount, methodCount);
                Log.Warn(this, s15);
            }

            _MethodBody = new Dictionary<UInt32, Method_body_info>((int)methodBodyCount);

            for (uint i = 0; i < methodBodyCount; i++)
            {
                Method_body_info mbi = new Method_body_info();
                mbi.Parse(source);
                _MethodBody.Add(mbi.Method, mbi);
            }

            String s16 = String.Format("{0:d} Method bodies", _MethodBody.Count);
            Log.Debug(this, s16);

            if (source.Position != source.Length)
            {
                AbcFormatException fe = new AbcFormatException(
                    "Trailing garbage after reading ABC file: at offset 0x" +
                    source.Position.ToString("X08") + " of 0x" + source.Length.ToString("X08"));
                Log.Error(this, fe);
                throw fe;
            }
            String s17 = String.Format("0x{0:X08}: End", source.Position - startPos);
            Log.Debug(this, s17);
            Log.Debug(this, "Done reading ABC");
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            //
            // All Verify( AbcFile abc ) style methods of sub-elements of the ABC 
            // format implementation do not return a bool but actually throw exceptions
            // in case they find issues with their respective test subjects. The
            // exception type should always be AbcVerifierException, in order to 
            // make sure we intentionally threw it and Blitzableiter crashes if we
            // ran into some other problem (which needs to get fixed).
            //
            try
            {
                _ConstantPool.Verify(this);

                // NOTE: If we ever do performance improvements we should use "for" since it is faster than foreach

                foreach (Method_info m in _Method)
                {
                    m.Verify(this);
                }

                foreach (Metadata_info m in _Metadata)
                {
                    m.Verify(this);
                }

                foreach (Instance_info i in _Instance)
                {
                    i.Verify(this);
                }

                foreach (Class_info c in _Class)
                {
                    c.Verify(this);
                }

                foreach (Script_info s in _Script)
                {
                    s.Verify(this);
                }

                foreach (UInt32 k in _MethodBody.Keys)
                {
                    _MethodBody[k].Verify(this);
                }
            }
            catch (AbcVerifierException)
            {
                // 
                // The exception is already logged when thrown, so we
                // can simply return false here
                //
                return false;
            }

            //
            // All tests passed
            //
            return true;
        }

        /// <summary>
        /// Checks if a string instance is a valid identifier.
        /// </summary>
        /// <param name="name">The actual string</param>
        /// <returns>True if name is a valid identifier.</returns>
        public static bool IsValidIdentifier(string name)
        {
            if (SwfFile.Configuration.AllowInvalidMethodNames)
            {
                return true;
            }
            else
            {
                char[] chars = name.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (
                        ((chars[i] >= 'A') && (chars[i] <= 'Z'))
                        ||
                        ((chars[i] >= 'a') && (chars[i] <= 'z'))
                        ||
                        ((chars[i] >= '0') && (chars[i] <= '9'))
                        || (chars[i] == '_')
                        || (chars[i] == '/')
                        || (chars[i] == ':')
                        || (chars[i] == '.')
                        )
                    {
                        // this is a legal name char 
                    }
                    else
                    {
                        Log.Warn(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType, "Invalid identifier found :" + name);
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Checks if a name can be found at specific position in the constant pool.
        /// </summary>
        /// <param name="nindex">The constant pool index</param>
        /// <returns>True if the index is within the range of the constant pool and if the string is a valid identifier.</returns>
        public bool VerifyNameIndex(uint nindex)
        {
            string testname;
            try
            {
                testname = this.ConstantPool.Strings[(int)nindex];
            }
            catch (IndexOutOfRangeException)
            {
                Log.Error(this, "Constant pool index is out of range.");
                return false;
            }

            if (!AbcFile.IsValidIdentifier(testname))
            {
                Log.Error(this, "\"" + testname + "\" is not valid identifier");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sindex"></param>
        /// <returns></returns>
        public bool VerifyStringIndex(uint sindex)
        {
            return (sindex < (uint)this.ConstantPool.Strings.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool VerifyMultinameIndex(uint index)
        {
            return (index < (uint)this.ConstantPool.Multinames.Count);
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="destination">The stream to write to.</param>
        public void Write(Stream destination)
        {
            long startPos = destination.Position;

            VariableLengthInteger.WriteU16(destination, _MinorVersion);
            VariableLengthInteger.WriteU16(destination, _MajorVersion);

            // (1) ConstantPool
            String s1 = String.Format("0x{0:X08}: ConstantPool", destination.Position - startPos);
            Log.Debug(this, s1);
            _ConstantPool.Write(destination);

            // (2) Method_Info
            String s2 = String.Format("0x{0:X08}: Method_info", destination.Position - startPos);
            Log.Debug(this, s2);

            VariableLengthInteger.WriteU30(destination, (uint)_Method.Count);
            for (int i = 0; i < _Method.Count; i++)
            {
                _Method[i].Write(destination);
            }

            // (3) Metadata
            String s3 = String.Format("0x{0:X08}: Metadata", destination.Position - startPos);
            Log.Debug(this, s3);

            if (SwfFile.Configuration.AbcRemoveMetadata)
            {
                VariableLengthInteger.WriteU30(destination, 0);
            }
            else
            {
                VariableLengthInteger.WriteU30(destination, (uint)_Metadata.Count);
                for (int i = 0; i < _Metadata.Count; i++)
                {
                    _Metadata[i].Write(destination);
                }
            }

            // (4) Instances
            String s4 = String.Format("0x{0:X08}: Instance_info", destination.Position - startPos);
            Log.Debug(this, s4);

            VariableLengthInteger.WriteU30(destination, (uint)_Instance.Count);
            for (int i = 0; i < _Instance.Count; i++)
            {
                _Instance[i].Write(destination);
            }

            // (5) Class_info
            String s5 = String.Format("0x{0:X08}: Class_info", destination.Position - startPos);
            Log.Debug(this, s5);

            // ClassCount == InstanceCount
            for (int i = 0; i < _Class.Count; i++)
            {
                _Class[i].Write(destination);
            }

            // (6) Script_info
            String s6 = String.Format("0x{0:X08}: Script_info", destination.Position - startPos);
            Log.Debug(this, s6);

            VariableLengthInteger.WriteU30(destination, (uint)_Script.Count);
            for (int i = 0; i < _Script.Count; i++)
            {
                _Script[i].Write(destination);
            }

            // (7) Method_body_info
            String s7 = String.Format("0x{0:X08}: MethodBody_info", destination.Position - startPos);
            Log.Debug(this, s7);

            VariableLengthInteger.WriteU30(destination, (uint)_MethodBody.Count);

            foreach (UInt32 k in _MethodBody.Keys)
            {
                _MethodBody[k].Write(destination);
            }

            String s8 = String.Format("0x{0:X08}: End", destination.Position - startPos);
            Log.Debug(this, s8);
        }
    }
}
