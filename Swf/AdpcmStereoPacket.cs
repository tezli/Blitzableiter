using System;
using System.IO;

namespace Recurity.Swf
{

    /// <summary>
    /// 
    /// </summary>
    public class AdpcmStereoPacket : AdpcmPacket
    {

        /// <summary>
        /// 
        /// </summary>
        private Int16 _initialSampleLeft;

        /// <summary>
        /// 
        /// </summary>
        private Byte _initialIndexLeft;

        /// <summary>
        /// 
        /// </summary>
        private Int16 _initialSampleRight;

        /// <summary>
        /// 
        /// </summary>
        private Byte _initialIndexRight;

        /// <summary>
        /// 
        /// </summary>
        private Byte[] _adpcmCodeData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public override void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);
            BitStream bits = new BitStream(input);

            this._initialSampleLeft = br.ReadInt16();
            this._initialIndexLeft = (Byte)bits.GetBits(6);
            this._initialSampleRight = br.ReadInt16();
            this._initialIndexRight = (Byte)bits.GetBits(6);

            this._adpcmCodeData = new Byte[(Int32)(input.Length - input.Position) + 2];
            input.Read(this._adpcmCodeData, 0, this._adpcmCodeData.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            BitStream bits = new BitStream(output);

            bw.Write(this._initialSampleLeft);
            bits.WriteBits(6, (Int32)this._initialIndexLeft);
            bits.WriteFlush();

            bw.Write(this._initialSampleRight);
            bits.WriteBits(6, (Int32)this._initialIndexRight);
            bits.WriteFlush();

            output.Write(this._adpcmCodeData, 0, this._adpcmCodeData.Length);
        }
    }
}
