using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM2InstructionSequence : List<AbstractInstruction> { }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    class AddressIndexMap<T, K> : Dictionary<T, K>
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _Dirty;

        /// <summary>
        /// 
        /// </summary>
        public bool Dirty
        {
            get
            {
                return _Dirty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void MarkDirty()
        {
            _Dirty = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AVM2Code
    {

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _MethodId;

        /// <summary>
        /// 
        /// </summary>
        public AVM2InstructionSequence Code { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        private AddressIndexMap<UInt32, int> _Address2IndexMap;

        /// <summary>
        /// 
        /// </summary>
        private AddressIndexMap<int, UInt32> _Index2AddressMap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="method"></param>
        public AVM2Code(AVM2InstructionSequence code, UInt32 method)
        {
            if (null == code)
            {
                ArgumentNullException ane = new ArgumentNullException("code is null");
                Log.Error(this, ane);
                throw ane;
            }
            _MethodId = method;
            Code = code;
            MarkMapsDirty();

            String s = String.Format("initialized with {0:d} instructions ({1:d} bytes) code", Code.Count, this.Length);
            //Log.Debug(this, s);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Method
        {
            get
            {
                return _MethodId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = 0;

                if (null != Code)
                {
                    for (int i = 0; i < Code.Count; i++)
                    {
                        accumulator += Code[i].Length;
                    }
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
            for (int i = 0; i < Code.Count; i++)
            {
                Code[i].Write(destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify(ABC.AbcFile abc)
        {
            for (int i = 0; i < Code.Count; i++)
            {
                // 
                // for code complexity reduction reasons, we are not checking the various
                // indicees in the individual instructions, but catch the OOB Exception here
                //
                try
                {
                    Code[i].Verify(abc);
                }
                catch (AbcVerifierException ave)
                {
                    //
                    // just log and rethrow, so it doesn't have to get logged in the 
                    // individual instructions
                    //
                    Log.Error(this, ave);
                    throw ave;
                }
                catch (IndexOutOfRangeException ioor)
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid instruction argument at instruction #" +
                        i.ToString("d") + " at offset 0x" + this.Index2Address((uint)i).ToString("X08"), ioor);
                    Log.Error(this, ave);
                    throw ave;
                }
                catch (ArgumentOutOfRangeException aoor)
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid instruction argument at instruction #" +
                        i.ToString("d") + " at offset 0x" + this.Index2Address((uint)i).ToString("X08"), aoor);
                    Log.Error(this, ave);
                    throw ave;
                }
            }

            VerifyBranchTargets();
        }


        #region Instruction Index and Offset

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<UInt32, AbstractInstruction> MemoryMap
        {
            get
            {
                //
                // Generate a memory map of valid addresses
                //
                Dictionary<UInt32, AbstractInstruction> memoryMap = new Dictionary<uint, AbstractInstruction>();
                UInt32 address = 0;

                for (int i = 0; i < Code.Count; i++)
                {
                    memoryMap.Add(address, Code[i]);

                    String s1 = String.Format("{2:d4} 0x{0:X08}: {1}", address, Code[i].ToString(), i);
                    //Log.Debug(this, s1);

                    address += Code[i].Length;
                }

                return memoryMap;
            }
        }

        #region Address-Index-Cache

        /// <summary>
        /// 
        /// </summary>
        private void GenerateCacheMaps()
        {
            _Address2IndexMap = new AddressIndexMap<uint, int>();
            UInt32 address = 0;

            for (int i = 0; i < Code.Count; i++)
            {
                _Address2IndexMap.Add(address, i);
                address += Code[i].Length;
            }

            _Index2AddressMap = new AddressIndexMap<int, uint>();
            foreach (UInt32 addr in _Address2IndexMap.Keys)
            {
                _Index2AddressMap.Add(_Address2IndexMap[addr], addr);
            }

            //
            // The _Dirty member of both maps is never set explicitly,
            // as the CLR initialization guarantees it will be FALSE 
            //
        }

        /// <summary>
        /// 
        /// </summary>
        private void MarkMapsDirty()
        {
            if (null != this._Address2IndexMap)
            {
                _Address2IndexMap.MarkDirty();
            }
            if (null != this._Index2AddressMap)
            {
                _Index2AddressMap.MarkDirty();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCacheMaps()
        {
            if ((null == _Index2AddressMap) || (null == _Address2IndexMap))
            {
                GenerateCacheMaps();
            }
            else
            {
                if (_Index2AddressMap.Dirty || _Address2IndexMap.Dirty)
                {
                    GenerateCacheMaps();
                }
            }
        }

        #endregion Address-Index-Cache

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public UInt32 Address2Index(UInt32 address)
        {
            UpdateCacheMaps();

            if (_Address2IndexMap.ContainsKey(address))
            {
                return (UInt32)_Address2IndexMap[address];
            }
            else
            {
                ArgumentOutOfRangeException aor = new ArgumentOutOfRangeException("0x" + address.ToString("X08") + " is an invalid address");
                Log.Error(this, aor);
                throw aor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UInt32 Index2Address(UInt32 index)
        {
            UpdateCacheMaps();

            if (_Index2AddressMap.ContainsKey((int)index))
            {
                return (UInt32)_Index2AddressMap[(int)index];
            }
            else
            {
                ArgumentOutOfRangeException aor = new ArgumentOutOfRangeException("0x" + index.ToString("d") + " is an invalid instruction index");
                Log.Error(this, aor);
                throw aor;
            }
        }

        #endregion Instruction Index and Offset

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchInstructionIndex"></param>
        /// <returns></returns>
        public UInt32 Branch2Index(UInt32 branchInstructionIndex)
        {
            UInt32 addr = this.Index2Address(branchInstructionIndex);
            Int32[] targets = Code[(int)branchInstructionIndex].BranchTarget;
            if (targets.Length != 1)
            {
                throw new ArgumentException("Instruction at " + branchInstructionIndex.ToString("d") + " has more than one target");
            }
            addr = (UInt32)((int)addr + targets[0]);
            return this.Address2Index(addr);
        }

        /// <summary>
        /// 
        /// </summary>
        public void VerifyBranchTargets()
        {
            String s = String.Format("Branch target verification for method #{0:d}", _MethodId);
            //Log.Debug(this, s);

            for (int i = 0; i < Code.Count; i++)
            {
                try
                {
                    if (Code[(int)i].IsBranch)
                    {
                        Int32[] targets = Code[i].BranchTarget;

                        for (int j = 0; j < targets.Length; j++)
                        {
                            UInt32 baseAddr = this.Index2Address((uint)i);
                            UInt32 destAddr = unchecked((UInt32)((long)baseAddr + (long)targets[j]));
                            UInt32 destIndex = this.Address2Index(destAddr);

                            String s1 = String.Format("[{0:X08}] [{1:d3}] {2} -> {3:X08} {4}",
                                 this.Index2Address((uint)i), i, Code[(int)i].ToString(),
                                 destAddr, destIndex);
                            //Log.Debug(this, s1);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    AbcVerifierException ave = new AbcVerifierException("Invalid branch target", e);
                    Log.Error(this, ave);
                    throw ave;
                }
            }
        }
    }
}
