using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// Gradient information.
    /// </summary>
    public class MorphGradRecord : AbstractSwfElement
    {
        private byte _startRatio;
        private Rgba _startColor;
        private byte _endRatio;
        private Rgba _endColor;     
   
        /// <summary>
        /// <para>Gradient information</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphGradRecord(byte InitialVersion) : base(InitialVersion) 
        {
            this._startColor = new Rgba(this._SwfVersion);
            this._endColor = new Rgba(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object
        /// </summary>
        public virtual ulong Length
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
        public virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The stream to read from</param>
        public void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._startRatio = br.ReadByte();
            this._startColor.Parse(input);
            this._endRatio = br.ReadByte();
            this._endColor.Parse(input);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to</param>
        public void Write(Stream output)
        {
            output.WriteByte(this._startRatio);
            this._startColor.Write(output);
            output.WriteByte(this._endRatio);
            this._endColor.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }
    }
}
