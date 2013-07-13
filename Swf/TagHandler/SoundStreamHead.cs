using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class SoundStreamHead : AbstractTagHandler
    {
        private SoundRate _playbacksoundrate;
        private SoundSize _playbacksoundsize;
        private SoundType _playbacksoundtype;
        private SoundEncoding _streamsoundcompression;
        private SoundRate _streamsoundrate;
        private SoundSize _streamsoundsize;
        private SoundType _streamsoundtype;
        private UInt16 _streamsoundsamplecount;
        private Int16 _latencyseek;

        public SoundStreamHead(byte initial) : base(initial) {  }

        /**<summary>
         * Format of the stream
         *</summary>
         */
        public SoundEncoding StreamCompression
        {
            get
            {
                return _streamsoundcompression;
            }
        }

        /**<summary>
         * indicates the streamingrate
         *</summary>
         */
        public SoundRate StreamRate
        {
            get
            {
                return _streamsoundrate;
            }
        }

        /**<summary>
         * indicates the samplesize of the Stream
         *</summary>
         */
        public SoundSize StreamSize
        {
            get
            {
                return _streamsoundsize;
            }
        }

        /**<summary>
         * indicates the type of the stream (mono/stereo)
         *</summary>
         */
        public SoundType StreamType
        {
            get
            {
                return _streamsoundtype;
            }
        }

        /** <summary>
         * Average number of samples in each SoundStreamBlock. Not
         * affected by mono/stereo setting; for stereo sounds this
         * is the number of sample pairs.</summary>
         */
        public UInt16 AvarageCount
        {
            get
            {
                return _streamsoundsamplecount;
            }
        }

        /**<summary>
         * indicates the streamingrate of the playback
         *</summary>
         */
        public SoundRate PlaybackRate
        {
            get
            {
                return _playbacksoundrate;
            }
        }

        /**<summary>
         * indicates the sizeof the samples of the playback
         *</summary>
         */
        public SoundSize PlaybackSize
        {
            get
            {
                return _playbacksoundsize;
            }
        }

        /**<summary>
         * indicates the type of the playback (mono/stereo)
         *</summary>
         */
        public SoundType PlaybackType
        {
            get
            {
                return _playbacksoundtype;
            }
        }

        /**<summary>
         * if the format if mp3 this indicates the latency seek of the stream
         *</summary>
         */
        public Int16 LatencySeek
        {
            get
            {
                return StreamCompression == SoundEncoding.MP3 ? _latencyseek : (Int16)0;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return 4+(_streamsoundcompression == SoundEncoding.MP3?2UL:0UL);            }
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
            BinaryReader br = new BinaryReader(this._dataStream);
            BitStream bits = new BitStream(this._dataStream);

            ushort _reserved = (ushort)bits.GetBits(4);
            this._playbacksoundrate = DefineSound.getSoundRate(bits.GetBits(2));
            this._playbacksoundsize = (bits.GetBits(1) == 0) ? SoundSize.snd_8bit : SoundSize.snd_16bit;
            this._playbacksoundtype = (bits.GetBits(1) == 0) ? SoundType.mono : SoundType.stereo;

            this._streamsoundcompression = DefineSound.getFormat(bits.GetBits(4));
            this._streamsoundrate = DefineSound.getSoundRate(bits.GetBits(2));
            this._streamsoundsize = (bits.GetBits(1) == 0) ? SoundSize.snd_8bit : SoundSize.snd_16bit;
            this._streamsoundtype = (bits.GetBits(1) == 0) ? SoundType.mono : SoundType.stereo;
            
            this._streamsoundsamplecount = br.ReadUInt16();

            if (_streamsoundcompression == SoundEncoding.MP3)
                this._latencyseek = br.ReadInt16();

            if (_reserved != 0)
            {
                Exception e = new SwfFormatException("Reserved bits are set");
                Log.Warn(this, e);
            }
            String s1 = String.Format("0x{0:X08}: reading soundstreamhead. Avarage samples {1}", this.Tag.OffsetData, this.AvarageCount);
            Log.Debug(this, s1);

           String s2 = String.Format("0x{0:X08}:\tPlayback: {1} {2} {3}",
                this.Tag.OffsetData,
                this.PlaybackRate,
                this.PlaybackSize,
                this.PlaybackType);
           Log.Debug(this, s2);

           String s3 = String.Format("0x{0:X08}:\tStream:   {1} {2} {3} {4}",
                this.Tag.OffsetData,
                this.StreamCompression,
                this.StreamRate,
                this.StreamSize,
                this.StreamType);
           Log.Debug(this, s3);

        }

        public override void Write(Stream output)
        {
            WriteTagHeader(output);

            BitStream bits = new BitStream(output);
            bits.WriteBits(4, 0);
            bits.WriteBits(2, DefineSound.getSoundRateID(PlaybackRate));
            bits.WriteBits(1, PlaybackSize == SoundSize.snd_8bit ? 0 : 1);
            bits.WriteBits(1, PlaybackType == SoundType.mono ? 0 : 1);

            bits.WriteBits(4, DefineSound.getFormatID(StreamCompression));
            bits.WriteBits(2, DefineSound.getSoundRateID(StreamRate));
            bits.WriteBits(1, StreamSize == SoundSize.snd_8bit ? 0 : 1);
            bits.WriteBits(1, StreamType == SoundType.mono ? 0 : 1);

            bits.WriteFlush();

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(AvarageCount);

            if(StreamCompression == SoundEncoding.MP3)
                bw.Write(this.LatencySeek);
        }


    }
}
