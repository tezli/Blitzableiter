using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// OpenType CFF font, as defined in the OpenType specification
    /// </summary>
    public class FontData : AbstractSwfElement
    {
        /// <summary>
        /// OpenType CFF font, as defined in the OpenType specification
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public FontData(byte InitialVersion): base(InitialVersion)
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write(Stream output)
        {

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
