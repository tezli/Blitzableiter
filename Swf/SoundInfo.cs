using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    public class SoundInfo
    {

//        private bool _reserved1;        // reserved = 0
//        private bool _reserved2;        // reserved = 0
        private bool _syncstop;         // Stop the sound now
        private bool _syncnomultiple;   // Don’t start the sound if already playing.
        private bool _hasEnvelope;      // Has envelope information.
        private bool _hasLoops;         // Has loop information.
        private bool _hasOutPoint;      // Has out-point information.
        private bool _hasInPoint;       // Has in-point information.

        private UInt32 _inPoint;
        private UInt32 _outPoint;
        private UInt16 _loopCount;
        private byte _envelopePoints;
        private SoundEnvelope[] _envelopeRecords;

        public SoundInfo() { }

        public ulong Length
        {
            get
            {
                ulong result = 1;

                if (_hasInPoint)
                    result += 4;

                if (_hasOutPoint)
                    result += 4;

                if (_hasLoops)
                    result += 2;

                if (_hasEnvelope)
                    result += 1 + _envelopePoints * SoundEnvelope.Length;

                return result;
            }
        }

        /**<summary>
         * indicates to stop the sound now 
         *</summary> 
         */
        public bool SyncStop
        {
            get
            {
                return _syncstop;
            }
        }

        /**<summary>
         * indicates not to start the sound if it is playing already 
         *</summary> 
         */
        public bool SyncNoMultiples
        {
            get
            {
                return _syncnomultiple;
            }
        }

        /**<summary>
         * indicates the envelope points are defined for this sound 
         *</summary> 
         */
        public bool hasEnvelope
        {
            get
            {
                return _hasEnvelope;
            }
        }

        /**<summary>
         * indicates the number envelope points
         *</summary> 
         */
        public byte numEnvelopePoints
        {
            get
            {
                return hasEnvelope?_envelopePoints:(byte)0;
            }
        }

        /**<summary>
         * indicates to play the sound in loops 
         *</summary> 
         */
        public bool hasLoops
        {
            get
            {
                return _hasLoops;
            }
        }

        /**<summary>
         * indicates the number of loops this sound should be played
         *</summary> 
         */
        public ushort numLoop
        {
            get
            {
                return hasLoops?_loopCount:(ushort)0;
            }
        }

        /**<summary>
         * indicates the sound has an inPoint
         *</summary> 
         */
        public bool hasInPoint
        {
            get
            {
                return _hasInPoint;
            }
        }

        /**<summary>
         * indicates if the sound has an outPoint 
         *</summary> 
         */
        public bool hasOutPoint
        {
            get
            {
                return _hasOutPoint;
            }
        }

        /**<summary>
         * the Sounds inPoint if exist. 
         *</summary> 
         */
        public UInt32 inPoint
        {
            get
            {
                return _inPoint;
            }
        }

        /**<summary>
         * the Sounds outPoint if exist. 
         *</summary> 
         */
        public UInt32 outPoint
        {
            get
            {
                return _outPoint;
            }
        }

        /**<summary>
         * Envelope Points 
         *</summary> 
         */
        public SoundEnvelope this[ byte i ]
        {
            get
            {
                if (hasEnvelope && i < _envelopePoints)
                    return _envelopeRecords[i];
                else
                    return null; 
            }
        }

        
        public static SoundInfo Parse(System.IO.Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            BitStream bits = new BitStream(input);

            SoundInfo info = new SoundInfo();

            bool _reserved1 = bits.GetBits(1) == 1;
            bool _reserved2 = bits.GetBits(1) == 1;
            info._syncstop = bits.GetBits(1) == 1;
            info._syncnomultiple = bits.GetBits(1) == 1;
            info._hasEnvelope = bits.GetBits(1) == 1;
            info._hasLoops = bits.GetBits(1) == 1;
            info._hasOutPoint = bits.GetBits(1) == 1;
            info._hasInPoint = bits.GetBits(1) == 1;

            if (_reserved1 || _reserved2)
                throw new SwfFormatException("Reserved bits are not set to false");

            if (info._hasInPoint)
                info._inPoint = br.ReadUInt32();

            if (info._hasOutPoint)
                info._outPoint = br.ReadUInt32();

            if (info._hasLoops)
                info._loopCount = br.ReadUInt16();

            if (info._hasEnvelope)
            {
                info._envelopePoints = br.ReadByte();
                info._envelopeRecords = new SoundEnvelope[info._envelopePoints];
                for (uint i = 0; i < info._envelopePoints; i++)
                    info._envelopeRecords[i] = SoundEnvelope.Parse(br);

            }

            return info;
        }

        public void Write(System.IO.Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            BitStream bs = new BitStream(output);

            bs.WriteBits(1, 0);     // reserved
            bs.WriteBits(1, 0);     // reserved
            bs.WriteBits(1, _syncstop ? 1 : 0);
            bs.WriteBits(1, _syncnomultiple ? 1 : 0);
            bs.WriteBits(1, _hasEnvelope ? 1 : 0);
            bs.WriteBits(1, _hasLoops ? 1 : 0);
            bs.WriteBits(1, _hasOutPoint ? 1 : 0);
            bs.WriteBits(1, _hasInPoint ? 1 : 0);

            if (_hasInPoint)
            {
                byte[] data = BitConverter.GetBytes(this._inPoint);
                output.Write(data, 0, 4);
            }

            if (_hasOutPoint)
            {
                byte[] data = BitConverter.GetBytes(this._outPoint);
                output.Write(data, 0, 4);
            }

            if (_hasLoops)
            {
                byte[] data = BitConverter.GetBytes(this._loopCount);
                output.Write(data, 0, 2);
            }

            if (_hasEnvelope)
            {
                output.WriteByte(this._envelopePoints);
                for (int i = 0; i < this._envelopePoints; i++)
                    _envelopeRecords[i].Write(output);
            }


        }

    }

    public class SoundEnvelope
    {

        private UInt32 _pos44;
        private UInt16 _leftLevel;
        private UInt16 _rightLevel;

        public static SoundEnvelope Parse(BinaryReader input)
        {
            SoundEnvelope result = new SoundEnvelope();

            result._pos44 = input.ReadUInt32();
            result._leftLevel = input.ReadUInt16();
            result._rightLevel = input.ReadUInt16();

            return result;
        }

        /**<summary>
         * Determines the position of the envelope point at a rate
         * of 44kHz.
         * </summary>
         */
        public UInt32 PositionAt44kHz
        {
            get
            {
                return _pos44;
            }
        }

        /**<summary>
         * Determines the value of the left channel(stereo) at the envelope point
         * </summary>
         */
        public UInt16 LeftLevel
        {
            get
            {
                return _leftLevel;
            }
        }

        /**<summary>
         * Determines the value of the right channel(stereo) at the envelope point
         * </summary>
         */
        public UInt16 RightLevel
        {
            get
            {
                return _rightLevel;
            }
        }

        public static ulong Length
        {
            get
            {
                return 2 + 2 + 4;
            }
        }

        public void Write(System.IO.Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            byte[] pos = BitConverter.GetBytes(this._pos44);
            output.Write(pos, 0, 4);

            byte[] left = BitConverter.GetBytes(this._leftLevel);
            output.Write(left, 0, 2);

            byte[] right = BitConverter.GetBytes(this._rightLevel);
            output.Write(pos, 0, 2);
 
        }


    }
}
