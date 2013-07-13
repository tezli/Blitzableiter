using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// The video packet is the top-level structural element in a Sorenson H.263 video packet.
    /// </summary>
    public class H263VideoPacket : AbstractSwfElement, IVideoPacket
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
        private H263TypeInformation _pictureStuffing;

        /// <summary>
        /// The video packet is the top-level structural element in a Sorenson H.263 video packet.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public H263VideoPacket(byte InitialVersion): base(InitialVersion)
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
/*
 5.1.3 Type Information (PTYPE) (Variable Length)
Information about the complete picture:
– Bit 1: Always "1", in order to avoid start code emulation.
– Bit 2: Always "0", for distinction with ITU-T Rec. H.261.
– Bit 3: Split screen indicator, "0" off, "1" on.
– Bit 4: Document camera indicator, "0" off, "1" on.
– Bit 5: Full Picture Freeze Release, "0" off, "1" on.
– Bits 6-8: Source Format, "000" forbidden, "001" sub-QCIF, "010" QCIF, "011" CIF,
"100" 4CIF, "101" 16CIF, "110" reserved, "111" extended PTYPE.
If bits 6-8 are not equal to "111", which indicates an extended PTYPE (PLUSPTYPE), the
following five bits are also present in PTYPE:
– Bit 9: Picture Coding Type, "0" INTRA (I-picture), "1" INTER (P-picture).
– Bit 10: Optional Unrestricted Motion Vector mode (see Annex D), "0" off, "1" on.
– Bit 11: Optional Syntax-based Arithmetic Coding mode (see Annex E), "0" off,
"1" on.
– Bit 12: Optional Advanced Prediction mode (see Annex F), "0" off, "1" on.
– Bit 13: Optional PB-frames mode (see Annex G), "0" normal I- or P-picture, "1"
PB-frame.
Split screen indicator is a signal that indicates that the upper and lower half of the decoded picture
could be displayed side by side. This bit has no direct effect on the encoding or decoding of the
picture.
Full Picture Freeze Release is a signal from an encoder which responds to a request for packet
retransmission (if not acknowledged) or fast update request (see also Annex C) or picture freeze
request (see also Annex L) and allows a decoder to exit from its freeze picture mode and display the
decoded picture in the normal manner.
If bits 6-8 indicate a different source format than in the previous picture header, the current picture
shall be an I-picture, unless an extended PTYPE is indicated in bits 6-8 and the capability to use the
optional Reference Picture Resampling mode (see Annex P) has been negotiated externally
(for example, ITU-T Rec. H.245).
Bits 10-13 refer to optional modes that are only used after negotiation between encoder and decoder
(see also Annexes D, E, F and G, respectively). If bit 9 is set to "0", bit 13 shall be set to "0" as
well.
Bits 6-8 shall not have a value of "111" which indicates the presence of an extended PTYPE
(PLUSPTYPE) unless the capability has been negotiated externally (for example, ITU-T
Rec. H.245) to allow the use of a custom source format or one or more of the other optional modes
available only by the use of an extended PTYPE (see Annexes I through K and M through T).
Whenever bit 6-8 do not have a value of "111", all of the additional modes available only by the use
of an extended PTYPE shall be considered to have been set to an
 */