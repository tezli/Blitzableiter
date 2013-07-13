using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class SetTabIndex : AbstractTagHandler
    {

        private UInt16 _characterDepth;
        private UInt16 _tabIndex;

        public SetTabIndex(byte init) : base(init) { }

        /// <summary>
        /// Depth of the character
        /// </summary>
        public UInt16 CharacterDepth
        {
            get
            {
                return _characterDepth;
            }
        }

        /// <summary>
        /// Value of the tab order
        /// </summary>
        public UInt16 TabIndex
        {
            get
            {
                return _tabIndex;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return 2 * sizeof(UInt16);
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
            _characterDepth = br.ReadUInt16();
            _tabIndex = br.ReadUInt16();

            String s = String.Format("0x{0:X08}: reading SetTabIndex-Tag: Depth {1}; Index: {2}",
                Tag.OffsetData,
                _characterDepth,
                _tabIndex);
             Log.Debug(this, s);
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(_characterDepth);
            bw.Write(_tabIndex);
        }

    }
}
