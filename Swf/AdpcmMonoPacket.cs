using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{

    /// <summary>
    /// 
    /// </summary>
    public class AdpcmMonoPacket : AdpcmPacket
    {

        /// <summary>
        /// 
        /// </summary>
        private Int16 _initialSample;

        /// <summary>
        /// 
        /// </summary>
        private Byte _initialIndex;

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

            this._initialSample = br.ReadInt16();
            this._initialIndex = (Byte)bits.GetBits(6);

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

            bw.Write(this._initialSample);
            bits.WriteBits(6, (Int32)this._initialIndex);
            bits.WriteFlush();

            output.Write(this._adpcmCodeData, 0, this._adpcmCodeData.Length);
        }
    }
}
