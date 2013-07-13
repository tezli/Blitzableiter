using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// A string terminated by a null character.
    /// </summary>
    public class SWFString : AbstractSwfElement
    {
        private List<byte> _string;

        /// <summary>
        /// A string trminated by a null character.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public SWFString(byte InitialVersion) : base(InitialVersion)
        {
            this._string = new List<byte>();
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return sizeof(byte) * (UInt64)this._string.Count;
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

            BinaryReader br = new BinaryReader(input);

            byte tempByte = 0;

            while (0 != (tempByte = br.ReadByte()))
            {
                this._string.Add(tempByte);
            }

            this._string.Add(0);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="outputFile">The file to write to.</param>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            output.Write(this._string.ToArray(), 0, this._string.Count);
        }

        /// <summary>
        /// Converts the objects value to a System.String
        /// </summary>
        /// <returns>A string representing the value of the object</returns>
        public String AsString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _string.Count-1 && _string[i]!=0; i++)
                sb.Append((char)_string[i]);

            return sb.ToString();
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
