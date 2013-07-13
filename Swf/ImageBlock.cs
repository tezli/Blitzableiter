using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// The image block represents one block in a frame.
    /// </summary>
    public class ImageBlock : AbstractSwfElement
    {
        private UInt16 _dataSize;
        private byte[] _data;

        /// <summary>
        /// The image block represents one block in a frame.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public ImageBlock(byte InitialVersion) : base(InitialVersion)
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

            BitStream bits = new BitStream(input);

            this._dataSize = (UInt16)bits.GetBits(16);

            if (this._dataSize > 0)
            {
                this._data = new byte[this._dataSize];
                int read = input.Read(this._data, 0, this._dataSize);
            }
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            BitStream bits = new BitStream(output);

            bits.WriteBits(16, (Int32)this._dataSize);

            if(this._dataSize > 0)
            {
                output.Write(this._data, 0, this._data.Length);
            }
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
