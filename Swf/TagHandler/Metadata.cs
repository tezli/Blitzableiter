using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class Metadata : AbstractTagHandler
    {

        private string _xmlData;

        public Metadata(byte init)
            : base(init)
        {

        }

        /// <summary>
        /// Embedded metadata in xml format
        /// </summary>
        public string Data
        {
            get
            {
                return _xmlData;
            }
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
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (ulong)SwfStrings.SwfStringLength(_SwfVersion, _xmlData);
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
            BinaryReader br = new BinaryReader(this._dataStream);
            this._xmlData = SwfStrings.SwfString(this._SwfVersion, br);
            
            String s = String.Format("0x{0:X08}: Metadata ({1} bytes) {2}...", Tag.OffsetData, _xmlData.Length, _xmlData.Substring(0, 12 > _xmlData.Length ? _xmlData.Length : 12));
            Log.Debug(this, s);

        }

        public override void Write(Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._xmlData);
        }

    }
}
