using System;
using System.Collections.Generic;
using System.Text;

using Recurity.Swf.Flowgraph;


namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1InstructionSequence : List<AbstractAction> { }

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
    /// Primary class to handle AVM1 Code operations, including modifications and flow graph handling.
    /// The class features a couple of performance caches for Address-to-Index and vice versa, as well as 
    /// a cached AVM1Flow instance.
    /// </summary>
    public class AVM1Code
    {

        /// <summary>
        /// 
        /// </summary>
        private AVM1InstructionSequence _Code;

        /// <summary>
        /// 
        /// </summary>
        private List<AVM1Function> _Functions;

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
        private AVM1Flow _Flow;

        /// <summary>
        /// 
        /// </summary>
        private bool _FlowDirty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public AVM1Code(AVM1InstructionSequence code)
        {
            if (null == code)
            {
                ArgumentNullException e = new ArgumentNullException("code is null");
                Log.Error(this, e);
                throw e;
            }

            _Code = code;

            // save CPU cycles
            //_Flow = new AVM1Flow( this );
            _FlowDirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            if (null != _Code)
                _Code.Clear();

            if (null != _Functions)
                _Functions.Clear();

            MarkMapsDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newCode"></param>
        public void ReplaceWith(AVM1Code newCode)
        {
            this.Clear();
            for (int i = 0; i < newCode.Count; i++)
            {
                _Code.Add(newCode[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AbstractAction this[int index]
        {
            get
            {
                return _Code[index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _Code.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Length
        {
            get
            {
                // TODO: this *could* use the _Index2Address map
                return Helper.SwfCodeReader.CodeLength(_Code);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AVM1IndexCFG Flowgraph
        {
            get
            {
                if (_FlowDirty)
                {
                    _Flow = new AVM1Flow(this);
                    _FlowDirty = false;
                }
                return _Flow.CodeFlowGraph;
            }
        }

        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="filename"></param>
        public void WriteFlowgraphGML(string filename)
        {
            if (_FlowDirty)
            {
                _Flow = new AVM1Flow(this);
                _FlowDirty = false;
            }
            _Flow.WriteGML(filename, this);
        }

        #region AVM1 Function View

        /// <summary>
        /// 
        /// </summary>
        public IList<AVM1Function> Functions
        {
            get
            {
                if (null == _Functions)
                {
                    try
                    {
                        this.AssignInstructions2Functions();
                    }
                    catch (AVM1ExceptionByteCodeFormat)
                    {
                        return null;
                    }
                }
                return _Functions;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FunctionInstructionMapTest()
        {
            Dictionary<UInt32, FunctionMapEntry> functionMap = new Dictionary<uint, FunctionMapEntry>();

            for (int i = 0; i < _Code.Count; i++)
            {
                AbstractAction currentInstruction = _Code[i];

                if ((currentInstruction.ActionType == AVM1Actions.ActionDefineFunction) || (currentInstruction.ActionType == AVM1Actions.ActionDefineFunction2))
                {
                    FunctionMapEntry t = new FunctionMapEntry();
                    t.begin = (uint)i;
                    t.end = this.Branch2Index((uint)i) - 1; // Branch2Index points PAST the function
                    uint toasttest = this.Address2Index((uint)((long)this.Index2Address((uint)i) + currentInstruction.BranchTargetAdjusted)) - 1;

                    if (((long)t.end - (long)t.begin) < 0)
                    {
                        AVM1ExceptionByteCodeFormat ae = new AVM1ExceptionByteCodeFormat("Function definition incorrect");
                        Log.Error(this, ae);
                        throw ae;
                    }

                    functionMap.Add((uint)i, t);
                }
            }

            foreach (UInt32 indexA in functionMap.Keys)
            {
                foreach (UInt32 indexB in functionMap.Keys)
                {
                    if (indexA != indexB)
                    {
                        //
                        // test for overlap
                        //

                        // 1) beginA < beginB, endA < beginB
                        if ((functionMap[indexA].begin < functionMap[indexB].begin) && (functionMap[indexA].end < functionMap[indexB].begin))
                        {
                            // A completely before B 
                            continue;
                        }
                        // 2) beginB < beginA, endB < beginA
                        if ((functionMap[indexB].begin < functionMap[indexA].begin) && (functionMap[indexB].end < functionMap[indexA].begin))
                        {
                            // B completely before A
                            continue;
                        }
                        // 3) beginA < beginB, endA >= endB
                        if ((functionMap[indexA].begin < functionMap[indexB].begin) && (functionMap[indexA].end >= functionMap[indexB].end))
                        {
                            // A completely encompasses B
                            continue;
                        }
                        // 4) beginB < begin>, endB >= endA
                        if ((functionMap[indexB].begin < functionMap[indexA].begin) && (functionMap[indexB].end >= functionMap[indexA].end))
                        {
                            // B completely encopasses A
                            continue;
                        }

                        // all other cases should be illegal
                        AVM1ExceptionByteCodeFormat ae = new AVM1ExceptionByteCodeFormat(
                            "Function at index " + functionMap[indexA].begin.ToString("d") + " (to " + functionMap[indexA].end.ToString("d") + ")"
                            + " overlaps with function at index " + functionMap[indexB].begin.ToString("d") + " (to " + functionMap[indexB].end.ToString("d") + ")"
                            );
                        Log.Error(this, ae);
                        throw ae;
                    }
                }
            }

            //Log.Debug(this, "overlap test pass");
        }


        /// <summary>
        /// TODO : documentation 
        /// could it be struct?
        /// </summary>
        private class FunctionMapEntry
        {
            public UInt32 begin;
            public UInt32 end;
            public AVM1Function functionDef;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AssignInstructions2Functions()
        {

            //
            // Algorithm:
            // Due to the beautiful way AVM1 declares functions, this
            // method will first traverse all code in _Code to find function 
            // definitions and store them in functionMap, together with their
            // begin and end instruction index. 
            // Following this, the code takes every instruction and searches
            // backwards in the instruction List<> for a function definition 
            // before this instruction that would encompass the instruction 
            // currently examined.
            // Instructions for which no function definition could be found
            // are assigned to a global entry, which gets added to _Functions
            // at the end of the method.

            _Functions = new List<AVM1Function>();

            Dictionary<UInt32, FunctionMapEntry> functionMap = new Dictionary<uint, FunctionMapEntry>();
            FunctionMapEntry t_all = new FunctionMapEntry();

            for (int i = 0; i < _Code.Count; i++)
            {
                AbstractAction test = _Code[i];

                if ((test.ActionType == AVM1Actions.ActionDefineFunction) || (test.ActionType == AVM1Actions.ActionDefineFunction2))
                {
                    FunctionMapEntry t = new FunctionMapEntry();
                    t.begin = (uint)i;
                    try
                    {
                        t.end = this.Address2Index((uint)((long)this.Index2Address((uint)i) + test.BranchTargetAdjusted)) - 1;
                    }
                    catch (ArgumentOutOfRangeException aor)
                    {
                        Log.Error(this, aor);
                        AVM1ExceptionByteCodeFormat ae = new AVM1ExceptionByteCodeFormat("address is not a valid index");
                        Log.Error(this, ae);
                        throw ae;
                    }

                    if (((long)t.end - (long)t.begin) < 0)
                    {
                        AVM1ExceptionByteCodeFormat ae = new AVM1ExceptionByteCodeFormat("Function definition incorrect");
                        Log.Error(this, ae);
                        throw ae;
                    }

                    functionMap.Add((uint)i, t);
                }
            }

            AVM1Function collectUnassigned = new AVM1Function();
            collectUnassigned.Name = "(AVM1 code block)";

            for (uint i = 0; i < (uint)_Code.Count; i++)
            {
                if (functionMap.ContainsKey((uint)i))
                {
                    // the function definition itself
                    AVM1Function f = new AVM1Function();
                    f.Name = this.GetFunctionName((uint)i);
                    f.Instructions.Add((uint)i);
                    _Functions.Add(f);
                    // update map with reference to function
                    functionMap[i].functionDef = f;
                }
                else
                {
                    bool assignedToFunction = false;
                    for (int backScan = (int)i; backScan >= 0; backScan--)
                    {
                        if (functionMap.ContainsKey((uint)backScan))
                        {
                            if (functionMap[(uint)backScan].end >= i)
                            {
                                functionMap[(uint)backScan].functionDef.Instructions.Add(i);
                                assignedToFunction = true;
                                break;
                            }
                        }
                    }

                    if (!assignedToFunction)
                    {
                        collectUnassigned.Instructions.Add(i);
                    }
                }
            }

            _Functions.Add(collectUnassigned);

            TestFunctionOverlap();

            for (int i = 0; i < _Functions.Count; i++)
            {

                //Log.Debug(this, _Functions[i].ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TestFunctionOverlap()
        {
            Dictionary<UInt32, AVM1Function> indexTest = new Dictionary<uint, AVM1Function>();

            //
            // Build dict with all instruction indicees, checking for collisions.
            // Here, we make sure that every instruction belongs to exactly one function.
            //
            for (int i = 0; i < _Functions.Count; i++)
            {
                for (int j = 0; j < _Functions[i].Instructions.Count; j++)
                {
                    UInt32 instructionIndex = _Functions[i].Instructions[j];
                    if (indexTest.ContainsKey(instructionIndex))
                    {
                        AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat(
                            "Instruction with index " + instructionIndex.ToString("d") +
                            ", address 0x" + this.Index2Address(instructionIndex).ToString("X08") +
                            " supposedly belongs to two functions at the same time");
                        Log.Error(this, e);
                        throw e;
                    }
                    indexTest.Add(instructionIndex, _Functions[i]);
                }
            }

            //
            // Counter-test: check that every instruction is accounted for in a function
            //
            for (int i = 0; i < _Code.Count; i++)
            {
                bool foundInstruction = false;

                for (int j = 0; j < _Functions.Count; j++)
                {
                    for (int jj = 0; jj < _Functions[j].Instructions.Count; jj++)
                    {
                        if (_Functions[j].Instructions[jj] == (uint)i)
                        {
                            foundInstruction = true;
                            break;
                        }
                    }
                    if (foundInstruction)
                        break;
                }

                if (!foundInstruction)
                {
                    AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat(
                        "Instruction with index " + i.ToString("d") +
                            ", address 0x" + this.Index2Address((UInt32)i).ToString("X08") +
                            " belongs to no function at all");
                    Log.Error(this, e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instructionIndex"></param>
        /// <returns></returns>
        private string GetFunctionName(UInt32 instructionIndex)
        {
            AbstractAction test = _Code[(int)instructionIndex];

            string name = "";

            if (test.ActionType == AVM1Actions.ActionDefineFunction)
            {
                name = ((ActionDefineFunction)test).Name;
            }
            else if (test.ActionType == AVM1Actions.ActionDefineFunction2)
            {
                name = ((ActionDefineFunction2)test).Name;
            }

            if (0 == name.Length)
            {
                name = "(unnamed function at index " + instructionIndex.ToString("d") + ")";
            }

            return name;
        }

        #endregion AVM1 Function View

        #region AVM1 Code Modifications
        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public AbstractAction RemoveAt(int where)
        {
            if (where >= _Code.Count)
            {
                IndexOutOfRangeException e = new IndexOutOfRangeException(where.ToString("d") + " outside AVM1 code range (count=" + _Code.Count.ToString("d") + ")");
                Log.Error(this, e);
                throw e;
            }

            MarkMapsDirty();

            AbstractAction ret = _Code[where];
            FixupOffsets(where, (Int16)(-1 * ret.ActionLength));
            _Code.RemoveAt(where);

            return ret;
        }

        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="where"></param>
        /// <param name="what"></param>
        public void InjectAt(int where, AbstractAction what)
        {
            MarkMapsDirty();

            FixupOffsets(where, (Int16)what.ActionLength);
            _Code.Insert(where, what);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="what"></param>
        public void ReplaceAt(int where, AbstractAction what)
        {
            // UNTESTED
            MarkMapsDirty();
            this.InjectAt(where, what);
            this.RemoveAt(where + 1);
        }

        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="where"></param>
        /// <param name="what"></param>
        public void InjectAt(int where, AVM1Code what)
        {
            MarkMapsDirty();

            Int16 length = (Int16)what.Length;
            FixupOffsets(where, length);
            for (int i = 0; i < what.Count; i++)
            {
                _Code.Insert(where + i, what[i]);
            }
        }

        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="where"></param>
        /// <param name="difference"></param>
        private void FixupOffsets(int where, Int16 difference)
        {
            uint bytesBefore = 0;
            for (int i = 0; i < where; i++)
            {
                bytesBefore += _Code[i].ActionLength;
            }

            uint currentOffset = 0;
            for (int i = 0; i < _Code.Count; i++)
            {
                if (0 != _Code[i].BranchTarget)
                {
                    int branch = _Code[i].BranchTargetAdjusted;
                    uint destinationAddress = (uint)((long)branch + (long)currentOffset);
                    int branchChange = this.FixupAdjustment(currentOffset, bytesBefore, destinationAddress, difference);

                    try
                    {
                        branch += branchChange;
                        _Code[i].BranchTargetAdjusted = branch;
                    }
                    catch (OverflowException)
                    {
                        AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat(
                            "Code insertion failed: destination address overflow for " + difference.ToString("d") +
                            " additional bytes ( " + _Code[i].ToString() + ")");
                        Log.Error(this, e);
                        throw e;
                    }

                    //
                    // Special case handling for ActionTry
                    //
                    if (_Code[i].ActionType == AVM1Actions.ActionTry)
                    {
                        try
                        {
                            ushort catchTarget = ((ActionTry)_Code[i]).CatchTargetAdjusted;
                            ushort finallyTarget = ((ActionTry)_Code[i]).FinallyTargetAdjusted;

                            if (0 != catchTarget)
                                catchTarget = (ushort)((int)catchTarget + this.FixupAdjustment(currentOffset, bytesBefore, (uint)((long)catchTarget + (long)currentOffset), difference));
                            if (0 != finallyTarget)
                                finallyTarget = (ushort)((int)finallyTarget + this.FixupAdjustment(currentOffset, bytesBefore, (uint)((long)finallyTarget + (long)currentOffset), difference));

                            ((ActionTry)_Code[i]).CatchTargetAdjusted = catchTarget;
                            ((ActionTry)_Code[i]).FinallyTargetAdjusted = finallyTarget;
                        }
                        catch (OverflowException)
                        {
                            AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat(
                                                        "Code insertion failed: destination address overflow for " + difference.ToString("d") +
                                                        " additional bytes ( " + _Code[i].ToString() + ")");
                            Log.Error(this, e);
                            throw e;
                        }
                    }
                }

                currentOffset += _Code[i].ActionLength;
            }
        }

        /// <summary>
        /// TODO : documentation
        /// </summary>
        /// <param name="currentOffset"></param>
        /// <param name="bytesBefore"></param>
        /// <param name="destination"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        private int FixupAdjustment(uint currentOffset, uint bytesBefore, uint destination, Int16 difference)
        {
            if ((currentOffset < bytesBefore) && (destination > bytesBefore))
            {
                return (int)difference;
            }
            if ((currentOffset >= bytesBefore) && (destination <= bytesBefore))
            {
                return (int)(-difference);
            }
            return 0;
        }

        /*
        private int FixupAdjustment( uint currentOffset, uint changeOffset, uint branchDestination, Int16 sizeOfChange )
        {
            if ( currentOffset < changeOffset )
            {
                if ( branchDestination > changeOffset )
                {
                    return ( int )sizeOfChange;
                }
                else if ( branchDestination == changeOffset )
                {
                    return 0;
                }
                else if ( branchDestination < changeOffset )
                {
                    return 0;
                }
            }
            else if ( currentOffset == changeOffset )
            {
                // doesn't matter for this.RemoveAt(), the instruction gets nuked anyway
                if ( branchDestination < currentOffset )
                {
                    return ( int )( -sizeOfChange );
                }
                else if ( branchDestination == currentOffset )
                {
                    return 0;
                }
                else if ( branchDestination > currentOffset )
                {
                    return 0;
                }
            }
            else if ( currentOffset > changeOffset )
            {
                if ( branchDestination < changeOffset )
                {
                    return ( int )( -sizeOfChange );
                }
                else if ( branchDestination == changeOffset )
                {
                    return ( int )( -sizeOfChange );
                }
                else if ( branchDestination > changeOffset )
                {
                    return 0;
                }
            }

            throw new Exception( "Idiot!" );
        }
         */

        #endregion AVM1 Code Modifications

        #region Instruction Index and Offset

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<UInt32, AbstractAction> MemoryMap
        {
            get
            {
                //
                // Generate a memory map of valid addresses
                //
                Dictionary<UInt32, AbstractAction> memoryMap = new Dictionary<uint, AbstractAction>();
                UInt32 address = 0;

                for (int i = 0; i < _Code.Count; i++)
                {
                    memoryMap.Add(address, _Code[i]);

                    String s = String.Format("{2:d4} 0x{0:X08}: {1}", address, _Code[i].ToString(), i);
                    //Log.Debug(this, s);
                    address += _Code[i].ActionLength;
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

            for (int i = 0; i < _Code.Count; i++)
            {
                _Address2IndexMap.Add(address, i);
                address += _Code[i].ActionLength;
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
            _FlowDirty = true;

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
        /// <param name="branchInstructionIndex"></param>
        /// <returns></returns>
        public UInt32 Branch2Index(UInt32 branchInstructionIndex)
        {
            UInt32 addr = this.Index2Address(branchInstructionIndex);
            addr = (UInt32)((int)addr + (int)_Code[(int)branchInstructionIndex].BranchTargetAdjusted);
            return this.Address2Index(addr);
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

        #region Code Verification

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            if (!this.VerifyBranchTargets())
            {
                //
                // Simple branch target verification failed
                //
                return false;
            }

            /*
            try
            {
                this.AssignInstructions2Functions();
            }
            catch ( AVM1ExceptionByteCodeFormat )
            {
                //
                // Not every instruction is assigned uniquely to a function
                //
                return false;
            } 
             
            Faulty:
             
            if ( !this.VerifyBranchTargetsWithinFunctions() )
            {
                //
                // Second stage branch target verification failed
                //
                return false;
            }
             */

            try
            {
                this.FunctionInstructionMapTest();
            }
            catch (AVM1ExceptionByteCodeFormat)
            {
                //
                // functions overlap
                //
                return false;
            }



            //
            // If this is reached, it looks OK
            // 
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool VerifyBranchTargets()
        {
            Dictionary<UInt32, AbstractAction> memoryMap = new Dictionary<uint, AbstractAction>();

            memoryMap = this.MemoryMap;

            //
            // Verification 
            //
            bool verificationResult = true;

            foreach (UInt32 addr in memoryMap.Keys)
            {
                //
                // BranchTarget != 0 indicates an Action that has a branch 
                //
                if (memoryMap[addr].IsBranch || memoryMap[addr].IsFunction)
                {
                    bool destinationOverflow = false;
                    UInt32 destination = 0;

                    try
                    {
                        destination = (UInt32)(addr + memoryMap[addr].BranchTargetAdjusted);

                        String s = String.Format("[verify] 0x{0:X08}: {1} -> {2:X08}", addr, (null == AVM1Factory.ActionName(memoryMap[addr].ActionType) ? memoryMap[addr].ActionType.ToString("d") : AVM1Factory.ActionName(memoryMap[addr].ActionType)), destination);
                        Log.Debug(this, s);
                    }
                    catch (OverflowException)
                    {
                        // 
                        // this overflow happens when there is a BranchTargetAdjusted that points
                        // before the beginning of our code block (negative branch offset outside)
                        //
                        destinationOverflow = true;
                    }

                    if ((!memoryMap.ContainsKey(destination)) || destinationOverflow)
                    {
                        String s = String.Format("Action {0} branch to invalid target address detected (0x{1:X08}->0x{3:X08} - {2})",
                            (null == AVM1Factory.ActionName(memoryMap[addr].ActionType) ? memoryMap[addr].ActionType.ToString("d") : AVM1Factory.ActionName(memoryMap[addr].ActionType)),
                            addr,
                            memoryMap[addr],
                            destination);
                        Log.Error(this, s);
                        verificationResult = false;
                        break;
                    }
                }

                // Special handling for ActionTry, which has three destinations
                if (memoryMap[addr].ActionType == AVM1Actions.ActionTry)
                {
                    bool specialDestinationOverflow = false;
                    UInt32 destinationTry = 0;
                    UInt32 destinationCatch = 0;
                    UInt32 destinationFinally = 0;

                    try
                    {
                        destinationTry = (UInt32)(addr + ((ActionTry)memoryMap[addr]).BranchTargetAdjusted);

                        String s = String.Format("[verify] 0x{0:X08}: ActionTry.Try -> {1:X08}", addr, destinationTry);
                        //Log.Debug(this, s);
                    }
                    catch (OverflowException)
                    {
                        //
                        // Same check as above for the two special case destinations
                        //
                        specialDestinationOverflow = true;
                    }
                    if ((!memoryMap.ContainsKey(destinationCatch)) || specialDestinationOverflow)
                    {
                        Log.Error(this, "ActionTry.Catch to invalid target address detected");
                        verificationResult = false;
                        break;
                    }

                    if (((ActionTry)memoryMap[addr]).HasCatch)
                    {
                        try
                        {
                            destinationCatch = (UInt32)(addr + ((ActionTry)memoryMap[addr]).CatchTargetAdjusted);

                            String s = String.Format("[verify] 0x{0:X08}: ActionTry.Catch -> {1:X08}", addr, destinationCatch);
                            //Log.Debug(this, s);
                        }
                        catch (OverflowException)
                        {
                            //
                            // Same check as above for the two special case destinations
                            //
                            specialDestinationOverflow = true;
                        }
                        if ((!memoryMap.ContainsKey(destinationCatch)) || specialDestinationOverflow)
                        {
                            Log.Error(this, "ActionTry.Catch to invalid target address detected");
                            verificationResult = false;
                            break;
                        }
                    }

                    if (((ActionTry)memoryMap[addr]).HasFinally)
                    {
                        try
                        {
                            destinationFinally = (UInt32)(addr + ((ActionTry)memoryMap[addr]).FinallyTargetAdjusted);

                            String s = String.Format("[verify] 0x{0:X08}: ActionTry.Finally -> {1:X08}", addr, destinationFinally);
                            //Log.Debug(this, s);
                        }
                        catch (OverflowException)
                        {
                            specialDestinationOverflow = true;
                        }
                        if ((!memoryMap.ContainsKey(destinationFinally)) || specialDestinationOverflow)
                        {
                            Log.Error(this, "ActionTry.Finally to invalid target address detected");
                            verificationResult = false;
                            break;
                        }
                    }
                }
            }

            //Log.Debug(this, "[verify] Branch target verification result: " + verificationResult.ToString());

            return verificationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool VerifyBranchTargetsWithinFunctions()
        {
            for (int funcIt = 0; funcIt < _Functions.Count; funcIt++)
            {
                for (int instIt = 0; instIt < _Functions[funcIt].Instructions.Count; instIt++)
                {
                    UInt32 currentIndex = _Functions[funcIt].Instructions[instIt];

                    if (_Code[(int)currentIndex].IsBranch)
                    {
                        UInt32 brTarget = this.Branch2Index(currentIndex);

                        //
                        // Check if the branch instruction will land within the same
                        // function. It may be surprising, but it could legally not be the 
                        // case, since the Flash compiler will generate branch targets 
                        // AFTER THE LAST INSTRUCTION of the function if, e.g. the whole 
                        // function was a single if(...){...} statement. 
                        //
                        if (!_Functions[funcIt].ContainsInstruction(brTarget))
                        {
                            Log.Warn(this,  "[verify] instruction index " + currentIndex.ToString( "d" ) + " branches to outside function - " + _Code[ ( int )currentIndex ].ToString() );

                            //
                            // However, if the branch target is 0, that's suspicious
                            //
                            if (brTarget == 0)
                            {
                                Log.Error(this, "[verify] instruction index " + currentIndex.ToString("d") + " branches to index 0 - " + _Code[(int)currentIndex].ToString());
                                return false;
                            }
                            //
                            // In the case of the beautiful "branch past the function" code,
                            // we want at least that the instruction before the branch target
                            // is part of our function.
                            //
                            if (!_Functions[funcIt].ContainsInstruction(brTarget - 1))
                            {
                                Log.Error(this, "[verify] instruction index " + currentIndex.ToString("d") + " branch isn't even close to the function - " + _Code[(int)currentIndex].ToString());
                                return false;
                            }
                        }
                    }

                    //
                    // we don't need to check the .IsConditional case extra here, since
                    // 1) it may branch, in which case the check above is deciding already
                    // 2) it may not branch, in which case the function or AVM1 code block just terminates,
                    //    since we already checked the ActionEnd is where it needs to be
                    //

                    //
                    // The beloved Try-Catch-Finally case
                    //
                    if (_Code[(int)currentIndex].ActionType == AVM1Actions.ActionTry)
                    {
                        ActionTry t = (ActionTry)_Code[(int)currentIndex];
                        UInt32 tryOffset = (UInt32)((long)this.Index2Address(currentIndex) + (long)t.BranchTargetAdjusted);
                        UInt32 catchOffset = (UInt32)((long)this.Index2Address(currentIndex) + (long)t.CatchTargetAdjusted);
                        UInt32 finallyOffset = (UInt32)((long)this.Index2Address(currentIndex) + (long)t.FinallyTargetAdjusted);

                        UInt32 tryIndex = this.Address2Index(tryOffset);
                        UInt32 catchIndex = this.Address2Index(catchOffset);
                        UInt32 finallyIndex = this.Address2Index(finallyOffset);

                        if (!_Functions[funcIt].ContainsInstruction(tryIndex))
                        {
                            Log.Error(this, "[verify] instruction index " + currentIndex.ToString("d") + " try range to outside function - " + _Code[(int)currentIndex].ToString());
                            return false;
                        }
                        if (!_Functions[funcIt].ContainsInstruction(catchIndex))
                        {
                            Log.Error(this, "[verify] instruction index " + currentIndex.ToString("d") + " catch range to outside function - " + _Code[(int)currentIndex].ToString());
                            return false;
                        }
                        if (!_Functions[funcIt].ContainsInstruction(finallyIndex))
                        {
                            Log.Error(this, "[verify] instruction index " + currentIndex.ToString("d") + " finally range to outside function - " + _Code[(int)currentIndex].ToString());
                            return false;
                        }
                    }
                }
            }

            //Log.Debug(this, "[verify] Branch target within function result: true");

            return true;
        }

        #endregion Code Verification

    }
}
