using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The DefineFontName tag contains the name and copyright information for a font embedded in the Swf file.</para>
    /// </summary>
    public class DefineFontName : AbstractTagHandler
    {
        private UInt16 _fontID;
        private string _fontName;
        private string _fontCopyRight;

        /// <summary>
        /// <para>The DefineFontName tag contains the name and copyright information for a font embedded in the Swf file.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFontName(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return sizeof(UInt16) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, this._fontName) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, this._fontCopyRight);
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

            this._fontID = br.ReadUInt16();
            this._fontName = SwfStrings.SwfString(this._SwfVersion, br);
            this._fontCopyRight = SwfStrings.SwfString(this._SwfVersion, br);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._fontID);
            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._fontName);
            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._fontCopyRight);
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

