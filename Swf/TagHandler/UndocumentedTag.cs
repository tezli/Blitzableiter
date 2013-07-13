using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// Represents an undocumented tag.
    /// </summary>
    public class UndocumentedTag : AbstractTagHandler
    {
        private byte[] _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public UndocumentedTag(byte InitialVersion) : base(InitialVersion)
        {
            this._data = new byte[0];
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                return this.Tag.Length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
                return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._data = new byte[this._dataStream.Length - this._dataStream.Position];
            this._dataStream.Read(this._data, 0, this._data.Length);

        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            Log.Warn(this, "Writing empty tag for undocumented tag");
            
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            output.Write(new byte[this._data.Length], 0, this._data.Length);

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
