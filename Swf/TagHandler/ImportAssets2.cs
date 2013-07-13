using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class ImportAssets2 : AbstractTagHandler
    {

        private string _url;

        private UInt16 _count;
        private UInt16[] _tagIDs;
        private string[] _tagNames;

        public ImportAssets2(byte init) : base(init) { }

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
                return 8;
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
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(ms);

                    SwfStrings.SwfWriteString(this._SwfVersion, bw, this._url);

                    bw.Write(0x01);       // reserved: 1 by spec
                    bw.Write(0x00);       // reserved: 0 by spec
                    bw.Write(_count);

                    for (UInt16 i = 0; i < _count; i++)
                    {
                        bw.Write(_tagIDs[i]);
                        SwfStrings.SwfWriteString(this._SwfVersion, bw, this._tagNames[i]);
                    }
                    return (ulong)ms.Length;
                }
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
            BinaryReader br = new BinaryReader(_dataStream);

            this._url = SwfStrings.SwfString(this._SwfVersion, br);

            byte _reserved1 = br.ReadByte();
            byte _reserved2 = br.ReadByte();

            if (_reserved1 != 1)
                throw new SwfFormatException("reserved field in ImportAssets2 must be set to one");

            if (_reserved2 != 0)
                throw new SwfFormatException("reserved field in ImportAssets2 must be set to zero");

            _count = br.ReadUInt16();
            _tagIDs = new UInt16[_count];
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

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._url);

            bw.Write(0x01);       // reserved: 1 by spec
            bw.Write(0x00);       // reserved: 0 by spec
            bw.Write(_count);

            for (UInt16 i = 0; i < _count; i++)
            {
                bw.Write(_tagIDs[i]);
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._tagNames[i]);
            }
        }
    }
}
