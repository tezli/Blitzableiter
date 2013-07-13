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
    public abstract class SoundData
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract UInt64 Length { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract SoundEncoding Encoding { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract void Write(Stream output);

    }
}
