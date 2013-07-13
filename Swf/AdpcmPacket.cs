using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class AdpcmPacket
    {

        /// <summary>
        /// 
        /// </summary>
        public abstract void Parse(Stream input);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Write(Stream output);
    }
}
