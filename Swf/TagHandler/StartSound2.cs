using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class StartSound2 : AbstractTagHandler
    {

        private string _soundclassname;
        private SoundInfo _soundinfo;

        public StartSound2(byte InitialVersion)
            : base(InitialVersion)
        {
        }

        /// <summary>
        /// Name of the sound class to play
        /// </summary>
        public string SoundClassName
        {
            get
            {
                return _soundclassname;
            }
        }

        /// <summary>
        /// Informations about the sound to play
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
                return (ulong)_soundclassname.Length + (ulong)_soundinfo.Length;
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

            this._soundclassname = SwfStrings.SwfString(this._SwfVersion, br);
            this._soundinfo = SoundInfo.Parse(_dataStream);

        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._soundclassname);
            this._soundinfo.Write(output);
        }

    }
}
