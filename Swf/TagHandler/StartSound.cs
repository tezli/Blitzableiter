using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    class StartSound : AbstractTagHandler
    {

        private UInt16 _soundID;
        private SoundInfo _soundinfo;

        public StartSound(byte InitialVersion) : base(InitialVersion)
        {
        }

        /// <summary>
        /// Identifier of the soundtag
        /// </summary>
        public UInt16 SoundID
        {
            get
            {
                return _soundID;
            }
        }

        /// <summary>
        /// Information of the sound
        /// </summary>
        public SoundInfo Info
        {
            get
            {
                return _soundinfo;
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
                return 2 + _soundinfo.Length;
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

            _soundID = br.ReadUInt16();
            _soundinfo = SoundInfo.Parse(_dataStream);

        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);
            byte[] id = BitConverter.GetBytes(this._soundID);
            output.Write(id, 0, 2);

            _soundinfo.Write(output);
        }

    }
}
