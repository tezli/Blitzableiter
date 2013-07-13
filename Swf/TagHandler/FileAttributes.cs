using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class FileAttributes : AbstractTagHandler
    {
        /// <summary>
        /// 
        /// </summary>
        internal bool _UseDirectBlit;

        /// <summary>
        /// 
        /// </summary>
        internal bool _UseGPU;

        /// <summary>
        /// 
        /// </summary>
        internal bool _HasMetadata;

        /// <summary>
        /// 
        /// </summary>
        internal bool _ActionScript3;

        /// <summary>
        /// 
        /// </summary>
        internal bool _UseNetwork;

        /// <summary>
        /// 
        /// </summary>
        private bool _ReservedBitsDetectedWhileParsing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public FileAttributes(byte InitialVersion)
            : base(InitialVersion)
        {
            _tag = new Tag();
            _tag.TagType = TagTypes.FileAttributes;
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                //
                // Minimum Version is 8
                // or 9 when it signals AS3 
                // or 10 when using DirectBlit or GPU
                //
                // However, the documentation also states that it's legal
                // to include a FileAttributes Tag in any version of Swf.
                //
                if (_UseDirectBlit || _UseGPU)
                {
                    return 10;
                }
                else if (_ActionScript3)
                {
                    return 9;
                }
                else
                {
                    return 8;
                }
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get { return sizeof(UInt32); }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            if (_ReservedBitsDetectedWhileParsing)
            {
                Log.Error(this, "Verification failed, reserved Bits of FileAttriutes not 0");
                return false;
            }

            //Log.Debug(this, "Verification succeeded");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            if (_tag.Length < 4)
            {
                SwfFormatException e = new SwfFormatException("FileAttributes Tag too short.");
                Log.Error(this, e);
                throw e;
            }

            _ReservedBitsDetectedWhileParsing = false;
            BitStream bits = new BitStream(_dataStream);

            if (0 != bits.GetBits(1))
            {
                _ReservedBitsDetectedWhileParsing = true;
            }

            _UseDirectBlit = (0 != bits.GetBits(1));
            _UseGPU = (0 != bits.GetBits(1));
            _HasMetadata = (0 != bits.GetBits(1));
            _ActionScript3 = (0 != bits.GetBits(1));

            if (0 != bits.GetBits(2))
            {
                _ReservedBitsDetectedWhileParsing = true;
            }

            _UseNetwork = (0 != bits.GetBits(1));

            if (0 != bits.GetBits(24))
            {
                _ReservedBitsDetectedWhileParsing = true;
            }

            if (_ReservedBitsDetectedWhileParsing)
            {
                Log.Warn(this, "Reserved Bits used in FileAttributes");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Attributes:");
            sb.Append(" UseDirectBlit:"); sb.Append(_UseDirectBlit);
            sb.Append(" UseGPU:"); sb.Append(_UseGPU);
            sb.Append(" HasMetadata:"); sb.Append(_HasMetadata);
            sb.Append(" ActionScript3:"); sb.Append(_ActionScript3);
            sb.Append(" UseNetwork:"); sb.Append(_UseNetwork);
            //Log.Debug(this, sb.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            //Log.Debug(this, "Write called");

            WriteTagHeader(output);

            BitStream bits = new BitStream(output);
            bits.WriteBits(1, 0);
            bits.WriteBits(1, (_UseDirectBlit ? 1 : 0));
            bits.WriteBits(1, (_UseGPU ? 1 : 0));
            bits.WriteBits(1, (_HasMetadata ? 1 : 0));
            bits.WriteBits(1, (_ActionScript3 ? 1 : 0));
            bits.WriteBits(2, 0);
            bits.WriteBits(1, (_UseNetwork ? 1 : 0));
            bits.WriteBits(24, 0);
            bits.WriteFlush();
        }
    }
}
