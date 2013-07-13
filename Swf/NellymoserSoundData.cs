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
    public class NellymoserSoundData : RawSoundData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        public NellymoserSoundData(byte[] rawData) : base(rawData)
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
                return SoundEncoding.Nellymoser;
            }
        }
    }
}
