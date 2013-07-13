using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// The video packet is the top-level structural element in a Sorenson H.263 video packet.
    /// </summary>
    public class H236VideoPacket : AbstractSwfElement, IVideoPacket
    {
        private UInt32 _pictureStartCode;
        private byte _version;
        private byte _temporalReference;
        private byte _pictureSize;
        private UInt16 _customWidth;
        private UInt16 _customHeight;
        private byte _pictureType;
        private bool _deblockingFlag;
        private byte _quantizer;
        private bool _extraInformationFlag;
        private List<byte> _extraInformation;
        private MacroBlock _macroBlock;
        private object _pictureStuffing;

        /// <summary>
        /// The video packet is the top-level structural element in a Sorenson H.263 video packet.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public H236VideoPacket(byte InitialVersion): base(InitialVersion)
        {

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        public void Parse(Stream input)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            BitStream bits = new BitStream(input);

            this._pictureStartCode = bits.GetBits(17);
            this._version = (byte)bits.GetBits(5);
            this._temporalReference = (byte)bits.GetBits(8);
            this._pictureSize = (byte)bits.GetBits(3);

            if (this._pictureSize.Equals(0))
            {
                this._customWidth = (UInt16)bits.GetBits(8);
                this._customHeight = (UInt16)bits.GetBits(8);
            }
            else if (this._pictureSize.Equals(1))
            {
                this._customWidth = (UInt16)bits.GetBits(16);
                this._customHeight = (UInt16)bits.GetBits(16);
            }
            else
            {
                SwfFormatException e = new SwfFormatException("Not supported picture size.");
                log.Error(e.Message);
                throw e;
            }
            this._pictureType = (byte)bits.GetBits(2);
            this._deblockingFlag = Convert.ToBoolean(bits.GetBits(1));
            this._quantizer = (byte)bits.GetBits(5);
            this._extraInformationFlag = Convert.ToBoolean(bits.GetBits(1));
            bits.Reset();

            BinaryReader br = new BinaryReader(input);
            byte tempByte = 0;

            if (this._extraInformationFlag)
            {
                this._extraInformation = new List<byte>();

                while (0 != (tempByte = br.ReadByte()))
                {
                    this._extraInformation.Add(tempByte);
                }
            }
            this._extraInformation.Add(0);

            this._macroBlock = new MacroBlock(this._SwfVersion);
            this._macroBlock.Parse(input);
            //this._pictureStuffing.Parse(input);

        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            BitStream bits = new BitStream(output);

            bits.WriteBits(17, (Int32)this._pictureStartCode);
            bits.WriteBits(5, (Int32)this._version);
            bits.WriteBits(8, (Int32)this._temporalReference);
            bits.WriteBits(3, (Int32)this._pictureSize);

            if (this._pictureSize.Equals(0))
            {
                bits.WriteBits(8, (Int32)this._customWidth);
                bits.WriteBits(8, (Int32)this._customHeight);
            }
            else if (this._pictureSize.Equals(1))
            {
                bits.WriteBits(16, (Int32)this._customWidth);
                bits.WriteBits(16, (Int32)this._customHeight);
            }
            else
            {
                SwfFormatException e = new SwfFormatException("Not supported picture size.");
                log.Error(e.Message);
                throw e;
            }

            bits.WriteBits(2, (Int32)this._pictureType);
            bits.WriteBits(1, Convert.ToInt32(this._deblockingFlag));
            bits.WriteBits(5, (Int32)this._quantizer);
            bits.WriteBits(1, Convert.ToInt32(this._extraInformationFlag));
            bits.WriteFlush();

            if (this._extraInformationFlag)
            {
                for (int i = 0; i < this._extraInformation.Count; i++)
                {
                    output.WriteByte(this._extraInformation[i]);
                }
            }

            this._macroBlock.Write(output);
           // this._pictureStuffing.Write(output);
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
    }
}
