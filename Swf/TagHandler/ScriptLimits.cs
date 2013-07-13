using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class ScriptLimits : AbstractTagHandler
    {

        private UInt16 _maxRecursionDepth;
        private UInt16 _scriptTimeOutSeconds;

        public ScriptLimits(byte init) : base(init) { }

        /// <summary>
        /// Maximum depth of recursive calls in scripts
        /// </summary>
        public UInt16 MaximumRecursionDepth
        {
            get
            {
                return _maxRecursionDepth;
            }

        }

        /// <summary>
        /// Maximum ActionScript processing time before script stuck dialog box displays
        /// </summary>
        public UInt16 ScriptTimeOut
        {
            get
            {
                return _scriptTimeOutSeconds;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 7;
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
            _maxRecursionDepth = br.ReadUInt16();
            _scriptTimeOutSeconds = br.ReadUInt16();

           String s = String.Format("0x{0:X08}: reading ScriptLimits-Tag: RecursionDepth {1}; TimeOut: {2}sec",
                Tag.OffsetData,
                _maxRecursionDepth,
                _scriptTimeOutSeconds);
            Log.Debug(this, s);
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(_maxRecursionDepth);
            bw.Write(_scriptTimeOutSeconds);
        }

    }
}
