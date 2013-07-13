using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    public interface IVideoPacket
    {

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        ulong Length
        {
            get;
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        bool Verify();

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        void Parse(Stream input);

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        void Write(Stream output);
    }
}
