using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{

    /// <summary>
    /// 
    /// </summary>
    public enum AVM1DataTypes
    {

        /// <summary>
        /// 
        /// </summary>
        AVM_String = 0,

        /// <summary>
        /// 
        /// </summary>
        AVM_float = 1,

        /// <summary>
        /// 
        /// </summary>
        AVM_null = 2,

        /// <summary>
        /// 
        /// </summary>
        AVM_undefined = 3,

        /// <summary>
        /// 
        /// </summary>
        AVM_register = 4,

        /// <summary>
        /// 
        /// </summary>
        AVM_boolean = 5,

        /// <summary>
        /// 
        /// </summary>
        AVM_double = 6,

        /// <summary>
        /// 
        /// </summary>
        AVM_integer = 7,

        /// <summary>
        /// 
        /// </summary>
        AVM_constUInt8 = 8,

        /// <summary>
        /// 
        /// </summary>
        AVM_constUInt16 = 9,
        // Types that do not appear in ActionPush

        /// <summary>
        /// 
        /// </summary>
        AVM_Object = 0x80,

        /// <summary>
        /// 
        /// </summary>
        AVM_Function = 0x81,
        // Pseudo-types

        /// <summary>
        /// 
        /// </summary>
        AVM_ANY = 0xFE,

        /// <summary>
        /// 
        /// </summary>
        AVM_UNKNOWN = 0xFF
    }


    /// <summary>
    /// 
    /// </summary>
    public enum AVM1Actions
    {
        // Swf3 actions

        /// <summary>
        /// 
        /// </summary>
        ActionEnd = 0x00,

        /// <summary>
        /// 
        /// </summary>
        ActionGotoFrame = 0x81,

        /// <summary>
        /// 
        /// </summary>
        ActionGetURL = 0x83,

        /// <summary>
        /// 
        /// </summary>
        ActionNextFrame = 0x04,

        /// <summary>
        /// 
        /// </summary>
        ActionPreviousFrame = 0x05,

        /// <summary>
        /// 
        /// </summary>
        ActionPlay = 0x06,

        /// <summary>
        /// 
        /// </summary>
        ActionStop = 0x07,

        /// <summary>
        /// 
        /// </summary>
        ActionToggleQuality = 0x08,

        /// <summary>
        /// 
        /// </summary>
        ActionStopSounds = 0x09,

        /// <summary>
        /// 
        /// </summary>
        ActionWaitForFrame = 0x8A,

        /// <summary>
        /// 
        /// </summary>
        ActionSetTarget = 0x8B,

        /// <summary>
        /// 
        /// </summary>
        ActionGoToLabel = 0x8C,

        // Swf4 actions

        /// <summary>
        /// 
        /// </summary>
        ActionPush = 0x96,

        /// <summary>
        /// 
        /// </summary>
        ActionPop = 0x17,

        /// <summary>
        /// 
        /// </summary>
        ActionAdd = 0x0A,

        /// <summary>
        /// 
        /// </summary>
        ActionSubtract = 0x0B,

        /// <summary>
        /// 
        /// </summary>
        ActionMultiply = 0x0C,

        /// <summary>
        /// 
        /// </summary>
        ActionDivide = 0x0D,

        /// <summary>
        /// 
        /// </summary>
        ActionEquals = 0x0E,

        /// <summary>
        /// 
        /// </summary>
        ActionLess = 0x0F,

        /// <summary>
        /// 
        /// </summary>
        ActionAnd = 0x10,

        /// <summary>
        /// 
        /// </summary>
        ActionOr = 0x11,

        /// <summary>
        /// 
        /// </summary>
        ActionNot = 0x12,

        /// <summary>
        /// 
        /// </summary>
        ActionStringEquals = 0x13,

        /// <summary>
        /// 
        /// </summary>
        ActionStringLength = 0x14,

        /// <summary>
        /// 
        /// </summary>
        ActionStringAdd = 0x21,

        /// <summary>
        /// 
        /// </summary>
        ActionStringExtract = 0x15,

        /// <summary>
        /// 
        /// </summary>
        ActionStringLess = 0x29,

        /// <summary>
        /// 
        /// </summary>
        ActionMBStringLength = 0x31,

        /// <summary>
        /// 
        /// </summary>
        ActionMBStringExtract = 0x35,

        /// <summary>
        /// 
        /// </summary>
        ActionToInteger = 0x18,

        /// <summary>
        /// 
        /// </summary>
        ActionCharToAscii = 0x32,

        /// <summary>
        /// 
        /// </summary>
        ActionAsciiToChar = 0x33,

        /// <summary>
        /// 
        /// </summary>
        ActionMBCharToAscii = 0x36,

        /// <summary>
        /// 
        /// </summary>
        ActionMBAsciiToChar = 0x37,

        /// <summary>
        /// 
        /// </summary>
        ActionJump = 0x99,

        /// <summary>
        /// 
        /// </summary>
        ActionIf = 0x9D,

        /// <summary>
        /// 
        /// </summary>
        ActionCall = 0x9E,

        /// <summary>
        /// 
        /// </summary>
        ActionGetVariable = 0x1C,

        /// <summary>
        /// 
        /// </summary>
        ActionSetVariable = 0x1D,

        /// <summary>
        /// 
        /// </summary>
        ActionGetURL2 = 0x9A,

        /// <summary>
        /// 
        /// </summary>
        ActionGotoFrame2 = 0x9F,

        /// <summary>
        /// 
        /// </summary>
        ActionSetTarget2 = 0x20,

        /// <summary>
        /// 
        /// </summary>
        ActionGetProperty = 0x22,

        /// <summary>
        /// 
        /// </summary>
        ActionSetProperty = 0x23,

        /// <summary>
        /// 
        /// </summary>
        ActionCloneSprite = 0x24,

        /// <summary>
        /// 
        /// </summary>
        ActionRemoveSprite = 0x25,

        /// <summary>
        /// 
        /// </summary>
        ActionStartDrag = 0x27,

        /// <summary>
        /// 
        /// </summary>
        ActionEndDrag = 0x28,

        /// <summary>
        /// 
        /// </summary>
        ActionWaitForFrame2 = 0x8D,

        /// <summary>
        /// 
        /// </summary>
        ActionTrace = 0x26,

        /// <summary>
        /// 
        /// </summary> 
        ActionGetTime = 0x34,

        /// <summary>
        /// 
        /// </summary>
        ActionRandomNumber = 0x30,

        // Swf5 actions

        /// <summary>
        /// 
        /// </summary>
        ActionCallFunction = 0x3D,

        /// <summary>
        /// 
        /// </summary>
        ActionCallMethod = 0x52,

        /// <summary>
        /// 
        /// </summary>
        ActionConstantPool = 0x88,

        /// <summary>
        /// 
        /// </summary>
        ActionDefineFunction = 0x9B,

        /// <summary>
        /// 
        /// </summary>
        ActionDefineLocal = 0x3C,

        /// <summary>
        /// 
        /// </summary>
        ActionDefineLocal2 = 0x41,

        /// <summary>
        /// 
        /// </summary>
        ActionDelete = 0x3A,

        /// <summary>
        /// 
        /// </summary>
        ActionDelete2 = 0x3B,

        /// <summary>
        /// 
        /// </summary>
        ActionEnumerate = 0x46,

        /// <summary>
        /// 
        /// </summary>
        ActionEquals2 = 0x49,

        /// <summary>
        /// 
        /// </summary>
        ActionGetMember = 0x4E,

        /// <summary>
        /// 
        /// </summary>
        ActionInitArray = 0x42,

        /// <summary>
        /// 
        /// </summary>
        ActionInitObject = 0x43,

        /// <summary>
        /// 
        /// </summary>
        ActionNewMethod = 0x53,

        /// <summary>
        /// 
        /// </summary>
        ActionNewObject = 0x40,

        /// <summary>
        /// 
        /// </summary>
        ActionSetMember = 0x4F,

        /// <summary>
        /// 
        /// </summary>
        ActionTargetPath = 0x45,

        /// <summary>
        /// 
        /// </summary>
        ActionWith = 0x94,

        /// <summary>
        /// 
        /// </summary>
        ActionToNumber = 0x4A,

        /// <summary>
        /// 
        /// </summary>
        ActionToString = 0x4B,

        /// <summary>
        /// 
        /// </summary>
        ActionTypeOf = 0x44,

        /// <summary>
        /// 
        /// </summary>
        ActionAdd2 = 0x47,

        /// <summary>
        /// 
        /// </summary>
        ActionLess2 = 0x48,

        /// <summary>
        /// 
        /// </summary>
        ActionModulo = 0x3F,

        /// <summary>
        /// 
        /// </summary>
        ActionBitAnd = 0x60,

        /// <summary>
        /// 
        /// </summary>
        ActionBitLShift = 0x63,

        /// <summary>
        /// 
        /// </summary>
        ActionBitOr = 0x61,

        /// <summary>
        /// 
        /// </summary>
        ActionBitRShift = 0x64,

        /// <summary>
        /// 
        /// </summary>
        ActionBitURShift = 0x65,

        /// <summary>
        /// 
        /// </summary>
        ActionBitXor = 0x62,

        /// <summary>
        /// 
        /// </summary>
        ActionDecrement = 0x51,

        /// <summary>
        /// 
        /// </summary>
        ActionIncrement = 0x50,

        /// <summary>
        /// 
        /// </summary>
        ActionPushDuplicate = 0x4C,

        /// <summary>
        /// 
        /// </summary>
        ActionReturn = 0x3E,

        /// <summary>
        /// 
        /// </summary>
        ActionStackSwap = 0x4D,

        /// <summary>
        /// 
        /// </summary>
        ActionStoreRegister = 0x87,

        // Swf6 actions

        /// <summary>
        /// 
        /// </summary>
        ActionInstanceOf = 0x54,

        /// <summary>
        /// 
        /// </summary>
        ActionEnumerate2 = 0x55,

        /// <summary>
        /// 
        /// </summary>
        ActionStrictEquals = 0x66,

        /// <summary>
        /// 
        /// </summary>
        ActionGreater = 0x67,

        /// <summary>
        /// 
        /// </summary>
        ActionStringGreater = 0x68,

        // Swf7 actions

        /// <summary>
        /// 
        /// </summary>
        ActionDefineFunction2 = 0x8E,

        /// <summary>
        /// 
        /// </summary>
        ActionExtends = 0x69,

        /// <summary>
        /// 
        /// </summary>
        ActionCastOp = 0x2B,

        /// <summary>
        /// 
        /// </summary>
        ActionImplementsOp = 0x2C,

        /// <summary>
        /// 
        /// </summary>
        ActionTry = 0x8F,

        /// <summary>
        /// 
        /// </summary>
        ActionThrow = 0x2A

    }
}