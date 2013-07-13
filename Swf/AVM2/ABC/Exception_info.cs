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
    public class Exception_info
    {
        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _From;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _To;

        /// <summary>
        /// /
        /// </summary>
        internal UInt32 _Target;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _ExceptionType;

        /// <summary>
        /// 
        /// </summary>
        internal UInt32 _VariableName;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return
                    VariableLengthInteger.EncodedLengthU30(_From) +
                    VariableLengthInteger.EncodedLengthU30(_To) +
                    VariableLengthInteger.EncodedLengthU30(_Target) +
                    VariableLengthInteger.EncodedLengthU30(_ExceptionType) +
                    VariableLengthInteger.EncodedLengthU30(_VariableName)
                    ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse(Stream source)
        {
            //Log.Debug(this, "Offset : " + source.Position); 
            _From = VariableLengthInteger.ReadU30(source);
            _To = VariableLengthInteger.ReadU30(source);
            _Target = VariableLengthInteger.ReadU30(source);
            _ExceptionType = VariableLengthInteger.ReadU30(source);
            _VariableName = VariableLengthInteger.ReadU30(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write(Stream destination)
        {
            VariableLengthInteger.WriteU30(destination, _From);
            VariableLengthInteger.WriteU30(destination, _To);
            VariableLengthInteger.WriteU30(destination, _Target);
            VariableLengthInteger.WriteU30(destination, _ExceptionType);
            VariableLengthInteger.WriteU30(destination, _VariableName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        /// <param name="code"></param>
        public void Verify(AbcFile abc, AVM2Code code)
        {
            if (!abc.VerifyMultinameIndex(_ExceptionType))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid Exception type: " + _ExceptionType.ToString("d"));
                Log.Error(this, ave);
                throw ave;
            }

            if (!abc.VerifyMultinameIndex(_VariableName))
            {
                AbcVerifierException ave = new AbcVerifierException("Invalid exception variable name: " + _VariableName.ToString("d")); ;
                Log.Error(this, ave);
                throw ave;
            }

            try
            {
                uint dummy = code.Address2Index(_From) + code.Address2Index(_To) + code.Address2Index(_Target);
                dummy++;
            }
            catch (ArgumentOutOfRangeException aoor)
            {
                AbcVerifierException ave = new AbcVerifierException("Exception to/from/target not on a instruction", aoor);
                Log.Error(this, ave);
                throw ave;
            }
        }
    }
}
