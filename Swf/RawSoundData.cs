using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.TagHandler;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class RawSoundData : SoundData
    {
        /// <summary>
        /// 
        /// </summary>
        protected Byte[] _rawSoundData;
        /// <summary>
        /// 
        /// </summary>
        private Boolean isLittleEndian;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        public RawSoundData(byte[] rawData)
        {
            this._rawSoundData = rawData;
            this.isLittleEndian = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="isLittleEndian"></param>
        public RawSoundData(byte[] rawData, Boolean isLittleEndian)
        {
            this._rawSoundData = rawData;
            this.isLittleEndian = isLittleEndian;
        }

        /// <summary>
        /// 
        /// </summary>
        public override SoundEncoding Encoding
        {
            get
            {
                if (isLittleEndian)
                {
                    return SoundEncoding.Uncompressed_little_endian;
                }
                else
                {
                    return SoundEncoding.uncompressed_native;
                }
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        public override UInt64 Length
        {
            get 
            {
                return (UInt64)this._rawSoundData.Length;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            output.Write(this._rawSoundData, 0, this._rawSoundData.Length);
        }
    }
}
