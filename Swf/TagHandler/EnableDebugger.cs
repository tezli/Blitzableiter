using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// This Tag is used in Swf version 5 only. It is depricated in later versions
    /// </summary>
    class EnableDebugger : AbstractTagHandler
    {

        private string _md5Password;

        public EnableDebugger(byte init) : base(init) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Password of the tag
        /// </summary>
        public string Password
        {
            get
            {
                return _md5Password;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (ulong)SwfStrings.SwfStringLength(_SwfVersion, _md5Password);
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

        protected override void Parse()
        {
            String s = String.Format("0x{0:X08}: reading EnableDebugger-Tag", this.Tag.OffsetData);
            //Log.Debug(this, s);
            BinaryReader br = new BinaryReader(_dataStream);

            _md5Password = SwfStrings.SwfString(this._SwfVersion, br);

            String s1 = String.Format("0x{0:X08}:\t{1}", this.Tag.OffsetData, _md5Password);
            //Log.Debug(this, s1);
        }

        public override void Write(Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._md5Password);
        }


    }
}
