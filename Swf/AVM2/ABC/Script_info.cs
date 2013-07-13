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
    public class Script_info
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 Init { get; internal set; }

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
                uint accumulator = VariableLengthInteger.EncodedLengthU30(Init);
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
            Init = VariableLengthInteger.ReadU30(source);
            UInt32 traitsCount = VariableLengthInteger.ReadU30(source);
            Traits = new List<Traits_info>((int)traitsCount);
            for (uint i = 0; i < traitsCount; i++)
            {
                Traits_info ti = new Traits_info();
                ti.Parse(source);
                Traits.Add(ti);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (Init >= abc.Methods.Count)
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid init: " + Init.ToString("d"));
                Log.Error(this, ave);
                throw ave;
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
            VariableLengthInteger.WriteU30(destination, Init);
            VariableLengthInteger.WriteU30(destination, (uint)Traits.Count);
            for (int i = 0; i < Traits.Count; i++)
            {
                Traits[i].Write(destination);
            }
        }
    }
}
