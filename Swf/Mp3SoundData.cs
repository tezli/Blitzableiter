using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.TagHandler;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class Mp3SoundData : SoundData
    {
        /// <summary>
        /// 
        /// </summary>
        private Int16 _seekSamples;

        // private List<MP3Frame> _mp3Frames;

        /// <summary>
        /// 
        /// </summary>
        private byte[] _mp3FrameBuffer;

        /// <summary>
        /// 
        /// </summary>
        public override SoundEncoding Encoding
        {
            get
            {
                return SoundEncoding.MP3;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._seekSamples = br.ReadInt16();

            _mp3FrameBuffer = new byte[input.Length - input.Position];
            input.Read(_mp3FrameBuffer, 0, _mp3FrameBuffer.Length);
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get { return (ulong)this._mp3FrameBuffer.Length + 2; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(_seekSamples);
            output.Write(_mp3FrameBuffer, 0, _mp3FrameBuffer.Length);
        }
    }
}
