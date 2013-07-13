using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Recurity.Swf.AVM2.Static;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// Cpool_info is a block of array-based entries that reflect the constants used by all methods.
    /// </summary>
    public class Cpool_info
    {
        /// <summary>
        /// Holds integer constants referenced by the bytecode
        /// </summary>
        public List<Int32> Integers { get; internal set; }

        /// <summary>
        /// Holds unsigned integer constants referenced by the bytecode.
        /// </summary>
        public List<UInt32> UIntegers { get; internal set; }

        /// <summary>
        /// Holds IEEE double-precision floating point constants referenced by the bytecode
        /// </summary>
        public List<double> Doubles { get; internal set; }

        /// <summary>
        /// Holds UTF-8 encoded strings referenced by the compiled code and by many other parts of the abcFile.
        /// </summary>
        public List<string> Strings { get; internal set; }

        /// <summary>
        /// Describes the namespaces used by the bytecode and also for names of many kinds.
        /// </summary>
        public List<Namespace_info> Namespaces { get; internal set; }

        /// <summary>
        /// Describes namespace sets used in the descriptions of multinames.
        /// </summary>
        public List<Ns_set_info> NsSets { get; internal set; }

        /// <summary>
        /// Describes names used by the bytecode.
        /// </summary>
        public List<AbstractMultinameEntry> Multinames { get; internal set; }

        /// <summary>
        /// An empty string entry
        /// </summary>
        public UInt32 EmptyString { get { return 0; } }

        /// <summary>
        /// The length of this object
        /// </summary>
        public UInt32 Length
        {
            get
            {
                UInt32 accumulator = 0;

                // (1) count Integers
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)(Integers.Count));
                for (int i = 1; i < Integers.Count; i++)
                {
                    accumulator += VariableLengthInteger.EncodedLength(Integers[i]);
                }
                // (2) count UInteger
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)(UIntegers.Count));
                for (int i = 1; i < UIntegers.Count; i++)
                {
                    accumulator += VariableLengthInteger.EncodedLength(UIntegers[i]);
                }
                // (3) double
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Doubles.Count);
                accumulator += (uint)Doubles.Count < 2 ? 0 : (uint)(Doubles.Count - 1) * 8;
                // (4) strings
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Strings.Count);
                for (int i = 1; i < Strings.Count; i++)
                {
                    accumulator += StringInfo.Length(Strings[i]);
                }
                // (5) namespaces
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Namespaces.Count);
                for (int i = 1; i < Namespaces.Count; i++)
                {
                    accumulator += Namespaces[i].Length;
                }
                // (6) nsSet
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)NsSets.Count);
                for (int i = 1; i < NsSets.Count; i++)
                {
                    accumulator += NsSets[i].Length;
                }
                // (7) MultiNames
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Multinames.Count);
                for (int i = 1; i < Multinames.Count; i++)
                {
                    accumulator += Multinames[i].Length;
                }

                return accumulator;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="source">The input stream</param>
        public void Parse(Stream source)
        {
            // "The “0” entry of the integer array is not present in the abcFile; it 
            // represents the zero value for the purposes of providing values for optional 
            // parameters and field initialization."
            //
            UInt32 integerCount = VariableLengthInteger.ReadU30(source);
            // Note: integerCount == 0 seems to be valid        
            Integers = new List<Int32>((int)integerCount);
            Integers.Add((int)0);
            for (uint i = 1; i < integerCount; i++)
            {
                Integers.Add(VariableLengthInteger.ReadS32(source));
            }

            //
            // "The “0” entry of the uinteger array is not present in the abcFile; it 
            // represents the zero value for the purposes of providing values for optional
            // parameters and field initialization."
            //
            UInt32 uIntegerCount = VariableLengthInteger.ReadU30(source);
            // Note: uIntegerCount == 0 seems to be valid
            UIntegers = new List<UInt32>((int)uIntegerCount);
            UIntegers.Add((UInt32)0);
            for (uint i = 1; i < uIntegerCount; i++)
            {
                UIntegers.Add(VariableLengthInteger.ReadU32(source));
            }

            //
            // Double
            //
            UInt32 doubleCount = VariableLengthInteger.ReadU30(source);
            //Note: doubleCount == 0 seems to be valid too
            Doubles = new List<double>();
            Doubles.Add(Double.NaN);
            BinaryReader br = new BinaryReader(source);
            for (uint i = 1; i < doubleCount; i++)
            {
                Doubles.Add(br.ReadDouble());
            }

            //
            // String pool
            //
            UInt32 stringCount = VariableLengthInteger.ReadU30(source);
            //
            // "The value of string_count is the number of entries in the string array, plus one."
            //
            if (1 > stringCount)
            {
                AbcFormatException fe = new AbcFormatException("Constant pool string count < 1");
                Log.Error(this, fe);
                throw fe;
            }
            stringCount--;

            Strings = new List<string>();
            // Money quote: "Entry “0” of the string array is not present in the abcFile; it
            // represents the empty string in most contexts but is also used to represent the 
            // “any” name in others (known as “*” in ActionScript)."            
            Strings.Add("");
            // WARNING: Adobe Flash CS4 often includes an empty string at _String[1] (the first
            //          actually present in the file), _despite_ the fact that _String[0] is
            //          defined to be there for that purpose.
            for (uint i = 0; i < stringCount; i++)
            {
                Strings.Add(StringInfo.Read(source));
            }

            //
            // Namespaces
            //

            UInt32 namespaceCount = VariableLengthInteger.ReadU30(source);
            //
            // "The value of namespace_count is the number of entries in 
            // the namespace array, plus one." 
            //
            if (1 > namespaceCount)
            {
                /*
                AbcFormatException fe = new AbcFormatException( "Name space count < 1" );
               Log.Error(this,  fe );
                throw fe;
                 */
                Log.Warn(this, "Namespace count < 1");
                namespaceCount = 1;
            }
            namespaceCount--;

            Namespaces = new List<Namespace_info>();
            Namespaces.Add(new Namespace_info());
            for (uint i = 0; i < namespaceCount; i++)
            {
                Namespace_info nsi = new Namespace_info();
                nsi.Parse(source);
                Namespaces.Add(nsi);
            }

            //
            // NS Set
            //

            UInt32 nsSetCount = VariableLengthInteger.ReadU30(source);
            // another "plus one" case
            if (1 > nsSetCount)
            {
                /*
                AbcFormatException fe = new AbcFormatException( "Namespace set count < 1" );
               Log.Error(this,  fe );
                throw fe;
                 */
                Log.Warn(this, "NsSetCount < 1");
                nsSetCount = 1;
            }
            nsSetCount--;

            NsSets = new List<Ns_set_info>();
            NsSets.Add(new Ns_set_info()); // dummy entry for array[0] not in file
            for (uint i = 0; i < nsSetCount; i++)
            {
                Ns_set_info nssi = new Ns_set_info();
                nssi.Parse(source);
                NsSets.Add(nssi);
            }

            //
            // Multinames
            //
            UInt32 multinameCount = VariableLengthInteger.ReadU30(source);
            // another "plus one" case, doh!
            if (1 > multinameCount)
            {
                /*
                AbcFormatException fe = new AbcFormatException( "Multiname count < 1" );
               Log.Error(this,  fe );
                throw fe;
                 */
                Log.Warn(this, "Multiname count < 1");
                multinameCount = 1;
            }
            multinameCount--;

            Multinames = new List<AbstractMultinameEntry>();
            Multinames.Add(new MultinameRTQnameL((byte)MultinameType.RTQNameL)); // dummy entry for array[0] not in file
            for (uint i = 0; i < multinameCount; i++)
            {
                AbstractMultinameEntry entry = MultinameEntryFactory(source);
                Multinames.Add(entry);
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <param name="abc">The abc file to verify</param>
        public void Verify(AbcFile abc)
        {
            for (int i = 0; i < Multinames.Count; i++)
            {
                Multinames[i].Verify(abc);
            }
            for (int i = 0; i < NsSets.Count; i++)
            {
                NsSets[i].Verify(abc);
            }
            for (int i = 0; i < Namespaces.Count; i++)
            {
                Namespaces[i].Verify(abc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, (uint)Integers.Count);
            for (int i = 1; i < Integers.Count; i++)
            {
                VariableLengthInteger.WriteS32(destination, Integers[i]);
            }
            VariableLengthInteger.WriteU30(destination, (uint)UIntegers.Count);
            for (int i = 1; i < UIntegers.Count; i++)
            {
                VariableLengthInteger.WriteU32(destination, UIntegers[i]);
            }
            VariableLengthInteger.WriteU30(destination, (uint)Doubles.Count);
            BinaryWriter bw = new BinaryWriter(destination);
            for (int i = 1; i < Doubles.Count; i++)
            {
                bw.Write(Doubles[i]);
            }
            VariableLengthInteger.WriteU30(destination, (uint)Strings.Count);
            for (int i = 1; i < Strings.Count; i++)
            {
                StringInfo.Write(destination, Strings[i]);
            }
            VariableLengthInteger.WriteU30(destination, (uint)Namespaces.Count);
            for (int i = 1; i < Namespaces.Count; i++)
            {
                Namespaces[i].Write(destination);
            }
            VariableLengthInteger.WriteU30(destination, (uint)NsSets.Count);
            for (int i = 1; i < NsSets.Count; i++)
            {
                NsSets[i].Write(destination);
            }
            VariableLengthInteger.WriteU30(destination, (uint)Multinames.Count);
            for (int i = 1; i < Multinames.Count; i++)
            {
                Multinames[i].Write(destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private AbstractMultinameEntry MultinameEntryFactory(Stream source)
        {
            BinaryReader br = new BinaryReader(source);
            byte kind = br.ReadByte();

            if (!Enum.IsDefined(typeof(MultinameType), kind))
            {
                AbcFormatException fe = new AbcFormatException("Invalid multiname type 0x" + kind.ToString("X2"));
                Log.Error(this, fe);
                throw fe;
            }

            AbstractMultinameEntry product = null;

            switch ((MultinameType)kind)
            {
                case MultinameType.QName:
                case MultinameType.QNameA:
                    product = new MultinameQname(kind);
                    product.Namespace = VariableLengthInteger.ReadU30(source);
                    product.NameIndex = VariableLengthInteger.ReadU30(source);
                    break;

                case MultinameType.RTQName:
                case MultinameType.RTQNameA:
                    product = new MultinameRTQname(kind);
                    product.NameIndex = VariableLengthInteger.ReadU30(source);
                    break;

                case MultinameType.RTQNameL:
                case MultinameType.RTQNameLA:
                    product = new MultinameRTQnameL(kind);
                    break;

                case MultinameType.Multiname:
                case MultinameType.MultinameA:
                    product = new MultinameMultiname(kind);
                    product.NameIndex = VariableLengthInteger.ReadU30(source);
                    product.NsSet = VariableLengthInteger.ReadU30(source);
                    break;

                case MultinameType.MultinameL:
                case MultinameType.MultinameLA:
                    product = new MultinameMultinameL(kind);
                    product.NsSet = VariableLengthInteger.ReadU30(source);
                    break;

                case MultinameType.Multiname0x1D:
                    product = new Multiname0x1D(kind);
                    ((Multiname0x1D)product).Parse(source);
                    break;

                default:
                    Exception e = new Exception("Internal error, Multiname type " + kind.ToString("X2") + " reached end of factory!!");
                    Log.Error(this, e);
                    throw e;
            }

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abcfile"></param>
        /// <returns></returns>
        public string ToString(AbcFile abcfile)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name);
            sb.Append(" ");
            sb.Append(Environment.NewLine);
            for (int i = 0; i < Integers.Count; i++)
            {
                sb.AppendFormat(" Integer[#{0:d}] : {1:d} ", i, Integers[i]);
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < UIntegers.Count; i++)
            {
                sb.AppendFormat(" Unsigned Integer[#{0:d}] t: {1:d} ", i, UIntegers[i]);
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < Doubles.Count; i++)
            {
                sb.AppendFormat(" Double[#{0:d}] : {1:g} ", i, Doubles[i]);
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < Strings.Count; i++)
            {
                sb.AppendFormat(" String[#{0:d}] : '{1}' ", i, Strings[i]);
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < Namespaces.Count; i++)
            {
                sb.AppendFormat(" Namespace[#{0:d}] : {1} ", i, Namespaces[i].ToString(abcfile));
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < NsSets.Count; i++)
            {
                sb.AppendFormat(" Namespace Set[#{0:d}] : {1} ", i, NsSets[i].ToString());
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            for (int i = 0; i < Multinames.Count; i++)
            {
                sb.AppendFormat(" Multiname[#{0:d}] : {1} ", i, Multinames[i].ToString(abcfile));
                sb.Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
