using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// The macro block is the next layer down in the video structure. It corresponds to the macro
    /// block layer in H.263
    /// </summary>
    public class MacroBlock : AbstractSwfElement
    {
        private bool _codedMacroblockFlag;
        private object _macroblockType;
        private object _blockPattern;
        private byte _quantizerInformation;
        private object[] _motionVectorData;
        private object[] _extraMotionVectorData;
        private List<BlockData> _blockData;


        /// <summary>
        /// The macro block is the next layer down in the video structure.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public MacroBlock(byte InitialVersion): base(InitialVersion)
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
