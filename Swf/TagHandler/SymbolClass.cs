using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class SymbolClass : AbstractTagHandler
    {

        private UInt16 _numSymbols;
        private UInt16[] _tagIDs;
        private string[] _classNames;

        public SymbolClass(byte init) : base(init) { }

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
                ulong result = sizeof(UInt16);

                for (UInt16 i = 0; i < _numSymbols; i++)
                    result += sizeof(UInt16) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _classNames[i]);

                return result;
            }
        }

        private static bool isBinaryData(AbstractTagHandler handler)
        {
            return (handler.GetType().Equals(typeof(DefineBinaryData)));
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
            String s = String.Format("0x{0:X08}: reading SymbolClass", this.Tag.OffsetData);
            Log.Debug(this, s);

            BinaryReader br = new BinaryReader(_dataStream);

            _numSymbols = br.ReadUInt16();
            _tagIDs = new UInt16[_numSymbols];
            _classNames = new string[_numSymbols];

            for (UInt16 i = 0; i < _numSymbols; i++)
            {
                _tagIDs[i] = br.ReadUInt16();
                this._classNames[i] = SwfStrings.SwfString(this._SwfVersion, br);

                String s1 = String.Format("0x{0:X08}: {1} SymbolClass: {2} \"{3}\"", this.Tag.OffsetData, i, _tagIDs[i], _classNames[i]);
                Log.Debug(this, s1);
            }

        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(_numSymbols);

            for (UInt16 i = 0; i < _numSymbols; i++)
            {
                bw.Write(_tagIDs[i]);
                SwfStrings.SwfWriteString(_SwfVersion, bw, _classNames[i]);
            }

        }

    }
}
