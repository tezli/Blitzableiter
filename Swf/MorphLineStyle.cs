using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// The format of a line style value within the file
    /// </summary>
    public class MorphLineStyle : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _startWidth;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _endWidth;

        /// <summary>
        /// 
        /// </summary>
        protected Rgba _startColor;

        /// <summary>
        /// 
        /// </summary>
        protected Rgba _endColor;

        /// <summary>
        /// The format of a line style value within the file
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphLineStyle(byte InitialVersion) : base(InitialVersion)
        {
            this._startColor = new Rgba(this._SwfVersion);
            this._endColor = new Rgba(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object in bytes.
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
        /// <param name="input">The input stream.</param>
        public virtual void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._startWidth = br.ReadUInt16();
            this._endWidth = br.ReadUInt16();
            this._startColor.Parse(input);
            this._endColor.Parse(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public virtual void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._startWidth);
            bw.Write(this._endWidth);

            this._startColor.Write(output);
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
