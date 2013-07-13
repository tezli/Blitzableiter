using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// DefineVideoStream defines a video character that can later be placed on the display list
    /// </summary>
    public class DefineVideoStream : AbstractTagHandler, Helper.ISwfCharacter
    {
        private UInt16 _characterID;
        private UInt16 _numberOfFrames;
        private UInt16 _width;
        private UInt16 _height;
        private VideoFlagsDeblocking _videoFlagsDeblocking;
        private bool _videoFlagsSmoothing;
        private CodecID _codecID;

        /// <summary>
        /// DefineVideoStream defines a video character that can later be placed on the display list
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineVideoStream(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// Character ID of the defined character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
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
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                return this.Tag.Length;
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

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();
            this._numberOfFrames = br.ReadUInt16();
            this._width = br.ReadUInt16();
            this._height = br.ReadUInt16();

            BitStream bits = new BitStream(this._dataStream);

            UInt32 reserved = bits.GetBits(4); // muste be 0

            if (0 != reserved)
            {
                SwfFormatException e = new SwfFormatException("Reserved bits havae been set");
               Log.Error(this, e.Message);
                throw e;
            }

            this._videoFlagsDeblocking = (VideoFlagsDeblocking)bits.GetBits(3);
            this._videoFlagsSmoothing = (0 != bits.GetBits(1) ? true : false);
            this._codecID = (CodecID)this._dataStream.ReadByte();

        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._characterID);
            bw.Write(this._numberOfFrames);
            bw.Write(this._width);
            bw.Write(this._height);

            BitStream bits = new BitStream(output);

            bits.WriteBits(4, 0);
            bits.WriteBits(3, (Int32)this._videoFlagsDeblocking);
            bits.WriteBits(1, Convert.ToInt32(this._videoFlagsSmoothing));
            output.WriteByte((byte)this._codecID);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// The ID of this object
        /// </summary>
        public UInt16 StreamID
        {
            get
            {
                return this._characterID;
            }
                
        }

        /// <summary>
        /// The ID of the codec
        /// </summary>
        public CodecID CodecId
        {
            get
            {
                return this._codecID;
            }

        }
    }
}
