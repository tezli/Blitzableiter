using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf
{

    /// <summary>
    /// The type of filling. FILLSTYLEs are byte aligned
    /// </summary>
    public enum FillStyleType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        SolidFill = 0x00,
        /// <summary>
        /// 
        /// </summary>
        LinearGradientFill = 0x10,
        /// <summary>
        /// 
        /// </summary>
        RadialGradientFill = 0x12,
        /// <summary>
        /// 
        /// </summary>
        FocalRadialGradientFill = 0x13,
        /// <summary>
        /// 
        /// </summary>
        RepeatingBitmapFill = 0x40,         // (Swf 8 file format and later only)
        /// <summary>
        /// 
        /// </summary>
        ClippedBitmapFill = 0x41,           // (Swf 8 file format and later only)
        /// <summary>
        /// 
        /// </summary>
        NonSmoothedRepeatingBitmap = 0x42,  // (Swf 8 file format and later only)
        /// <summary>
        /// 
        /// </summary>
        NonSmoothedClippedBitmap = 0x43     // (Swf 8 file format and later only)
    }

    /// <summary>
    /// The type of the gradient
    /// </summary>
    public enum GradientType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Linear = 0x10,
        /// <summary>
        /// 
        /// </summary>
        Radial = 0x12,
    }

    /// <summary>
    /// The type of the bitmap filling
    /// </summary>
    public enum BitmapFillType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Repeating = 0x40,
        /// <summary>
        /// 
        /// </summary>
        Clipped = 0x41,
        /// <summary>
        /// 
        /// </summary>
        NonSmoothedRepeating = 0x42,
        /// <summary>
        /// 
        /// </summary>
        NonSmoothedClipped = 0x43
    }

    /// <summary>
    /// Style of the caps
    /// </summary>
    public enum CapStyle : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Round = 0x00,
        /// <summary>
        /// 
        /// </summary>
        None = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Square = 0x02
    }

    /// <summary>
    /// Style of the joins
    /// </summary>
    public enum JoinStyle : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Round = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Bevel = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Miter = 0x02
    }

    /// <summary>
    /// The spread mode.
    /// </summary>
    public enum SpreadMode : byte
    {
        /// <summary>
        /// 
        /// </summary>
        PadMode = 0x00,
        /// <summary>
        /// 
        /// </summary>
        ReflectMode = 0x01,
        /// <summary>
        /// 
        /// </summary>
        RepeatMode = 0x02
    }

    /// <summary>
    /// Interpolation type.
    /// </summary>
    public enum InterPolation : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Normal = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Linear = 0x01
    }

    /// <summary>
    /// Align of an element.
    /// </summary>
    public enum Align : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Left = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Right = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Center = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Justify = 0x03
    }

    /// <summary>
    /// The language code.
    /// </summary>
    public enum LangCode : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Latin = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Japanese = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Korean = 0x03,
        /// <summary>
        /// 
        /// </summary>
        SimplifiedChinese = 0x04,
        /// <summary>
        /// 
        /// </summary>
        TraditionalChinese = 0x05
    }

    /// <summary>
    /// Video deblocking
    /// </summary>
    public enum VideoFlagsDeblocking : byte
    {
        /// <summary>
        /// Use VIDEOPACKET value
        /// </summary>
        UseVideoPacket = 0,
        /// <summary>
        /// Off
        /// </summary>
        Off = 1,
        /// <summary>
        /// Fast deblocking filter
        /// </summary>
        Level1 = 2,
        /// <summary>
        /// VP6 only, better deblocking filter
        /// </summary>
        Level2 = 3,
        /// <summary>
        /// VP6 only, better deblocking plus fast deringing filter
        /// </summary>
        Level3 = 4,
        /// <summary>
        /// VP6 only, better deblocking plus better deringing filter
        /// </summary>
        Level4 = 5,
    }

    /// <summary>
    /// Video codec
    /// </summary>
    public enum CodecID : byte
    {
        /// <summary>
        /// 
        /// </summary>
        SorensonH263 = 2,
        /// <summary>
        /// 
        /// </summary>
        ScreenVideo = 3,
        /// <summary>
        /// 
        /// </summary>
        VP6 = 4,
        /// <summary>
        /// 
        /// </summary>
        VP6Alpha = 5,
        /// <summary>
        /// 
        /// </summary>
        ScreenV2Video = 6
    }
}
