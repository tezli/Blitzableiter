using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// Block data is the lowest layer in the video structure.
    /// </summary>
    /// <remarks>
    /// <para>Block data is the lowest layer in the video structure. In version 0</para> 
    /// <para>of the Sorenson H.263 video format, this layer follows H.263 section</para>
    /// <para>5.4 exactly. In version 1 of the Sorenson H.263 video format, escape</para>
    /// <para>codes in transform coefficients (see H.263 section 5.4.2) are encoded</para>
    /// <para>differently. When the ESCAPE code 0000 011 appears, the next bit is</para>
    /// <para>a format bit that indicates the subsequent bit layout for LAST, RUN,</para> 
    /// <para>and LEVEL. In both cases, one bit is used for LAST and six bits are</para>
    /// <para>used for RUN. If the format bit is 0, seven bits are used for LEVEL;</para> 
    /// <para>if the format bit is 1, eleven bits are used for LEVEL.</para>
    /// </remarks>
    public class BlockData : AbstractSwfElement
    {
        /// <summary>
        /// Block data is the lowest layer in the video structure
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public BlockData(byte InitialVersion): base(InitialVersion)
        {

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        public void Parse(Stream input)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            return sb.ToString();
        }
    }
}
