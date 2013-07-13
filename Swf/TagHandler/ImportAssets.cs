using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class ImportAssets : AbstractTagHandler
    {

        private UInt16 _count;
        private string _url;
        private UInt16[] _tagIDs;
        private string[] _tagNames;

        public ImportAssets(byte init) : base(init) { }

        /// <summary>
        /// URL the importreferes to
        /// </summary>
        public string URL
        {
            get
            {
                return _url;
            }
        }

        /// <summary>
        /// Number of assets in tag
        /// </summary>
        public UInt16 Count
        {
            get
            {
                return _count;
            }
        }

        /// <summary>
        /// Ids of the assets
        /// </summary>
        public UInt16[] TagIDs
        {
            get
            {
                return _tagIDs;
            }
        }

        /// <summary>
        /// names of the assets
        /// </summary>
        public string[] Names
        {
            get
            {
                return _tagNames;
            }
        }

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
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                ulong result = sizeof(UInt16);

                for (UInt16 i = 0; i < _count; i++)
                    result += sizeof(UInt16) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _tagNames[i]);

                return result;
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

            this._url = SwfStrings.SwfString(this._SwfVersion, br);
            this._count = br.ReadUInt16();
            this._tagIDs = new UInt16[this._count];
            this._tagNames = new string[this._count];

            String s = String.Format("Importing Assets From : " + _url);
            Log.Debug(this, s);
            for (UInt16 i = 0; i < _count; i++)
            {
                _tagIDs[i] = br.ReadUInt16();
                _tagNames[i] = SwfStrings.SwfString(this._SwfVersion, br);

                Log.Debug(this, "Tag ID : " + _tagIDs[i]);
            }

        }

        public override void Write(Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._url);

            bw.Write(_count);

            for (UInt16 i = 0; i < _count; i++)
            {
                bw.Write(_tagIDs[i]);
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._tagNames[i]);
            }
        }
    }
}
