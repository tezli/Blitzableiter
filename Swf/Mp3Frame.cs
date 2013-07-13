using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class Mp3Frame
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt16 _synchWord;
        
        /// <summary>
        /// 
        /// </summary>
        private Byte _MPEGVersion;

        /// <summary>
        /// 
        /// </summary>
        private Byte _layer;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _protectionBit;

        /// <summary>
        /// 
        /// </summary>
        private Byte _bitrate;

        /// <summary>
        /// 
        /// </summary>
        private Byte _samplingRate;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _paddingBit;

        /// <summary>
        /// 
        /// </summary>
        private Byte _channelMode;

        /// <summary>
        /// 
        /// </summary>
        private Byte _modeExtension;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _copyright;

        /// <summary>
        /// 
        /// </summary>
        private Boolean _original;

        /// <summary>
        /// 
        /// </summary>
        private Byte _emphasis;

        /// <summary>
        /// 
        /// </summary>
        private Byte[] _sampleData;

        /// <summary>
        /// 
        /// </summary>
        private UInt16 _sampleDataSize;

        /// <summary>
        /// 
        /// </summary>
        private void CalculateSampleDataSize()
        {
            _sampleDataSize = (UInt16)((((_MPEGVersion == 1 ? 144 : 72) * _bitrate) / _samplingRate) - (_paddingBit == true ? 3 : 4));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse(Stream input)
        {
            BitStream bits = new BitStream(input);

            this._synchWord = (UInt16)bits.GetBits(11);
            this._MPEGVersion = (Byte)bits.GetBits(2);
            this._layer = (Byte)bits.GetBits(2);
            this._protectionBit = 1 == bits.GetBits(1) ? true : false;
            this._bitrate = (Byte)bits.GetBits(4);
            this._samplingRate = (Byte)bits.GetBits(2);
            this._paddingBit = 1 == bits.GetBits(1) ? true : false;
            bits.GetBits(1); //reserved
            this._channelMode = (Byte)bits.GetBits(2);
            this._modeExtension = (Byte)bits.GetBits(2);
            this._copyright = 1 == bits.GetBits(1) ? true : false;
            this._original = 1 == bits.GetBits(1) ? true : false;
            this._emphasis = (Byte)bits.GetBits(2);

            this._sampleData = new Byte[(Int32)(input.Length - input.Position)];
            input.Read(this._sampleData, 0, this._sampleData.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        internal void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            bits.WriteBits(11, this._synchWord);
            bits.WriteBits(2, this._MPEGVersion);
            bits.WriteBits(2, this._layer);
            bits.WriteBits(1, true == this._protectionBit ? 1 : 0);
            bits.WriteBits(4, this._bitrate);
            bits.WriteBits(2, this._samplingRate);
            bits.WriteBits(1, true == this._paddingBit ? 1 : 0);
            bits.WriteBits(1,0);
            bits.WriteBits(2, this._channelMode);
            bits.WriteBits(2, this._modeExtension);
            bits.WriteBits(1, true == this._copyright ? 1 : 0);
            bits.WriteBits(1, true == this._original ? 1 : 0);
            bits.WriteBits(2, this._emphasis);

            output.Write(this._sampleData, 0, this._sampleData.Length);
        }
    }
}
