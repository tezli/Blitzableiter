using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class ExportAssets : AbstractTagHandler
    {

        private UInt16 _count;
        private UInt16[] _tagIDs;
        private string[] _tagNames;

        public ExportAssets(byte init) : base(init) { }

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
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                ulong result = sizeof(UInt16);
                for (UInt16 i = 0; i < _count; i++)
                    result += sizeof(UInt16) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _tagNames[i]); // additional String terminal

                return result;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            return checkExportIDs();
        }

        private void ListSoFarCharacters()
        {          
            for (int i = 0; i < _SourceFileReference.TagHandlers.Count; i++)
                if (_SourceFileReference.TagHandlers[i] is ISwfCharacter)
                {
                    ISwfCharacter character = (ISwfCharacter)_SourceFileReference.TagHandlers[i];
                    //SwfFile.log.DebugFormat("Character {0}: {1}", character.CharacterID, character);
                }
    
        }

        private ISwfCharacter getCharacterTag(UInt16 characterID)
        {
            for (int i = 0; i < _SourceFileReference.TagHandlers.Count; i++)
                if (_SourceFileReference.TagHandlers[i] is ISwfCharacter)
                    if (characterID == ((ISwfCharacter)(_SourceFileReference.TagHandlers[i])).CharacterID)
                        return (ISwfCharacter)_SourceFileReference.TagHandlers[i];
            return null;
        }

        private bool checkExportIDs()
        {
            for (UInt16 i = 0; i < _count; i++)
            {
                ISwfCharacter tag = getCharacterTag(_tagIDs[i]);
                if (tag == null)
                {
                   // SwfFormatException exception = new SwfFormatException("SwfFile exports an invalid character.");
                   // Log.Error(this, exception);
                   // throw exception;
                   // return false;
                }
            }

            return true;
        }

        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(_dataStream);

            _count = br.ReadUInt16();

            _tagIDs = new UInt16[_count];
            _tagNames = new string[_count];

            for (UInt16 i = 0; i < _count; i++)
            {
                _tagIDs[i] = br.ReadUInt16();
                _tagNames[i] = SwfStrings.SwfString(this._SwfVersion, br);

                //SwfFile.log.DebugFormat("0x{0:X08}:\t{1} ExportAsset-Tag: {2:X08} {3}", this.Tag.OffsetData, i, _tagIDs[i], _tagNames[i]);

            }

            ListSoFarCharacters();
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write((UInt16)_count);

            for (UInt16 i = 0; i < _count; i++)
            {
                bw.Write((UInt16)_tagIDs[i]);
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._tagNames[i]);
            }
        }
    }
}
