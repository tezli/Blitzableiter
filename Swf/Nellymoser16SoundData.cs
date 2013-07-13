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
    public class Nellymoser16SoundData : NellymoserSoundData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        public Nellymoser16SoundData(byte[] rawData) : base(rawData)
        {
            this._rawSoundData = rawData;
        }

        /// <summary>
        /// 
        /// </summary>
        public override SoundEncoding Encoding
        {
            get
            {
                return SoundEncoding.Nellymoser16kHz;
            }
        }
    }
}
