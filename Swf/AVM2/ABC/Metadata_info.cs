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
    public class Metadata_info
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _Name;

        /// <summary>
        /// 
        /// </summary>
        private List<Metadata_item_info> _ItemInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse(Stream source)
        {

            // UNTESTED
            _Name = VariableLengthInteger.ReadU30(source);

            if (0 == _Name)
            {
                AbcFormatException fe = new AbcFormatException("Metadata_info with Name == 0");
                Log.Error(this, fe);
                throw fe;
            }

            UInt32 itemCount = VariableLengthInteger.ReadU30(source);
            _ItemInfo = new List<Metadata_item_info>();
            for (uint i = 0; i < itemCount; i++)
            {
                Metadata_item_info mdii = new Metadata_item_info();
                mdii.Parse(source);
                _ItemInfo.Add(mdii);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(AbcFile abc)
        {
            if (!abc.VerifyNameIndex(_Name))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid Name entry " + _Name.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            foreach (Metadata_item_info i in _ItemInfo)
            {
                i.Verify(abc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30(_Name);
                accumulator += VariableLengthInteger.EncodedLengthU30((uint)_ItemInfo.Count);
                for (int i = 0; i < _ItemInfo.Count; i++)
                {
                    accumulator += _ItemInfo[i].Length;
                }
                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, _Name);
            VariableLengthInteger.WriteU30(destination, (uint)_ItemInfo.Count);
            for (int i = 0; i < _ItemInfo.Count; i++)
            {
                _ItemInfo[i].Write(destination);
            }
        }
    }
}
