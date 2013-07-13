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
    public class AdpcmSoundData : SoundData
    {

        /// <summary>
        /// 
        /// </summary>
        private Byte _ADPCMCodeSize;

        /// <summary>
        /// 
        /// </summary>
        private AdpcmPacket _ADPCMPacket;

        /// <summary>
        /// 
        /// </summary>
        public override SoundEncoding Encoding
        {
            get
            {
                return TagHandler.SoundEncoding.ADPCM;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        public void Parse(Stream input, SoundType type)
        {
            BitStream bits = new BitStream(input);

            this._ADPCMCodeSize = (Byte)bits.GetBits(2);

            if (type.Equals(SoundType.mono))
            {
                AdpcmMonoPacket packet = new AdpcmMonoPacket();
                packet.Parse(input);
                this._ADPCMPacket = packet;
            }
            else if (type.Equals(SoundType.stereo))
            {
                AdpcmStereoPacket packet = new AdpcmStereoPacket();
                packet.Parse(input);
                this._ADPCMPacket = packet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override UInt64 Length
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
