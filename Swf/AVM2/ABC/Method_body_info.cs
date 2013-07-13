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
    public class Method_body_info
    {
        internal byte[] dataOnRead;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 Method { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 MaxStack { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 LocalCount { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 InitScopeDepth { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 MaxScopeDepth { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public AVM2Code Code { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Exception_info> Exceptions { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Traits_info> Traits { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30(Method);
                accumulator += VariableLengthInteger.EncodedLengthU30(MaxStack);
                accumulator += VariableLengthInteger.EncodedLengthU30(LocalCount);
                accumulator += VariableLengthInteger.EncodedLengthU30(InitScopeDepth);
                accumulator += VariableLengthInteger.EncodedLengthU30(MaxScopeDepth);
                accumulator += VariableLengthInteger.EncodedLengthU30(Code.Length);
                accumulator += Code.Length;
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Exceptions.Count);
                for (int i = 0; i < Exceptions.Count; i++)
                {
                    accumulator += Exceptions[i].Length;
                }
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)Traits.Count);
                for (int i = 0; i < Traits.Count; i++)
                {
                    accumulator += Traits[i].Length;
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
            long positionBefore = source.Position;
            Method = VariableLengthInteger.ReadU30(source);
            MaxStack = VariableLengthInteger.ReadU30(source);
            LocalCount = VariableLengthInteger.ReadU30(source);
            InitScopeDepth = VariableLengthInteger.ReadU30(source);
            MaxScopeDepth = VariableLengthInteger.ReadU30(source);

            if (InitScopeDepth > MaxScopeDepth)
            {
                String s = String.Format("Method_body_info InitScopeDepth ({0:d}) > MaxScopeDepth ({1:d})", InitScopeDepth, MaxScopeDepth);
                Log.Warn(this, s);
            }

            UInt32 byteCodeLength = VariableLengthInteger.ReadU30(source);

            String s1 = String.Format("{0:d} bytes AVM2 code found, parsing now", byteCodeLength);
            //Log.Debug(this, s1);

            AVM2InstructionSequence avm2code = new AVM2InstructionSequence();
            byte[] rawBuffer = new byte[(int)byteCodeLength];
            source.Read(rawBuffer, 0, (int)byteCodeLength);
            MemoryStream ms = new MemoryStream(rawBuffer);
            BinaryReader br = new BinaryReader(ms);
            while (br.BaseStream.Position < (long)byteCodeLength)
            {
                AbstractInstruction ai = AVM2Factory.Create(br, Method);
                avm2code.Add(ai);
            }
            Code = new AVM2Code(avm2code, Method);
            if (Code.Length != byteCodeLength)
            {
                String s2 = String.Format("AVM2Code calculated length {0:d} differs from declared length {1:d}", Code.Length, byteCodeLength);
                Log.Warn(this, s2);
                throw new Exception("This is likely to be a bug!");
            }


            UInt32 exceptionCount = VariableLengthInteger.ReadU30(source);
            Exceptions = new List<Exception_info>((int)exceptionCount);
            for (uint i = 0; i < exceptionCount; i++)
            {
                Exception_info ei = new Exception_info();
                ei.Parse(source);
                Exceptions.Add(ei);
            }

            String s3 = String.Format("{0:d} Exceptions", Exceptions.Count);
            //Log.Debug(this, s3);

            UInt32 traitsCount = VariableLengthInteger.ReadU30(source);
            Traits = new List<Traits_info>((int)traitsCount);
            for (uint i = 0; i < traitsCount; i++)
            {
                Traits_info ti = new Traits_info();
                ti.Parse(source);
                Traits.Add(ti);
            }

            String s4 = String.Format("{0:d} Traits", Traits.Count);
            //Log.Debug(this, s4);

            long positionAfter = source.Position;
            source.Seek(positionBefore, SeekOrigin.Begin);
            this.dataOnRead = new byte[positionAfter - positionBefore];
            source.Read(dataOnRead, 0, dataOnRead.Length);
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

            Code.Verify(abc);

            foreach (Exception_info ex in Exceptions)
            {
                ex.Verify(abc, Code);
            }

            foreach (Traits_info t in Traits)
            {
                t.Verify(abc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, Method);
            VariableLengthInteger.WriteU30(destination, MaxStack);
            VariableLengthInteger.WriteU30(destination, LocalCount);
            VariableLengthInteger.WriteU30(destination, InitScopeDepth);
            VariableLengthInteger.WriteU30(destination, MaxScopeDepth);
            VariableLengthInteger.WriteU30(destination, Code.Length);
            Code.Write(destination);
            VariableLengthInteger.WriteU30(destination, (uint)Exceptions.Count);
            for (int i = 0; i < Exceptions.Count; i++)
            {
                Exceptions[i].Write(destination);
            }
            VariableLengthInteger.WriteU30(destination, (uint)Traits.Count);
            for (int i = 0; i < Traits.Count; i++)
            {
                Traits[i].Write(destination);
            }
        }
    }
}
