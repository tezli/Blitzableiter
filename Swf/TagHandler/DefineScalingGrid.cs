using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class DefineScalingGrid : AbstractTagHandler, Helper.ISwfCharacter
    {

        private UInt16 _characterID;
        private Rect _splitter;

        public DefineScalingGrid(byte init) : base(init)
        {
            _splitter = new Rect(init);
        }

        /// <summary>
        /// ID of the character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
            }
        }

        /// <summary>
        /// Splitter rectangle defined in the tag. This rectangle defines the center
        /// field of a semiscalable frame
        /// </summary>
        public Rect Splitter
        {
            get
            {
                return _splitter;
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
        /// </summary>
        public override ulong Length
        {
            get
            {
                return sizeof(UInt16) + _splitter.Length;
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

            _characterID = br.ReadUInt16();
            _splitter.Parse(_dataStream);
        }

        public override void Write(System.IO.Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            WriteTagHeader(output);
            bw.Write(_characterID);
            _splitter.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._characterID);
            return sb.ToString();
        }

    }
}
