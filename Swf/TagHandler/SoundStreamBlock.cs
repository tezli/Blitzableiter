using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class SoundStreamBlock : AbstractTagHandler
    {

        private SoundStreamHead _soundstreamhead;
        private SoundData _soundData;

        public SoundStreamBlock(byte initial) : base(initial) { }

        /**<summary>
          * The head of the soundstream.
          *</summary>
          */
        public SoundStreamHead Head
        {
            get
            {
                return _soundstreamhead;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SoundData Sounddata
        {
            get
            {
                return this._soundData;
            }
            set
            {
                this._soundData = value;
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
                return (ulong)this._soundData.Length;
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

        private void TryParseSoundData(Stream input)
        {

            Byte[] b = new Byte[(Int32)(input.Length - input.Position)];
            Int64 position = this._dataStream.Position;
            input.Read(b, 0, b.Length);
            input.Seek(position, SeekOrigin.Begin);

            if (_soundstreamhead != null)
            {
                switch (this._soundstreamhead.StreamCompression)
                {
                    case SoundEncoding.uncompressed_native:
                        RawSoundData rawDataN = new RawSoundData(b, false);
                        this._soundData = rawDataN;
                        break;
                    case SoundEncoding.Uncompressed_little_endian:
                        RawSoundData rawDataL = new RawSoundData(b, true);
                        this._soundData = rawDataL;
                        break;
                    case SoundEncoding.ADPCM:
                        AdpcmSoundData ADPCMData = new AdpcmSoundData();
                        ADPCMData.Parse(input, this._soundstreamhead.StreamType);
                        this._soundData = ADPCMData;
                        break;
                    case SoundEncoding.MP3:
                        Mp3SoundData mp3Data = new Mp3SoundData();
                        mp3Data.Parse(input);
                        this._soundData = mp3Data;
                        break;
                    case SoundEncoding.Nellymoser:
                        NellymoserSoundData nellySound = new NellymoserSoundData(b);
                        this._soundData = nellySound;
                        break;
                    case SoundEncoding.Nellymoser8kHz:
                        Nellymoser8SoundData nelly8Sound = new Nellymoser8SoundData(b);
                        this._soundData = nelly8Sound;
                        break;
                    case SoundEncoding.Nellymoser16kHz:
                        Nellymoser16SoundData nelly16Sound = new Nellymoser16SoundData(b);
                        this._soundData = nelly16Sound;
                        break;
                    default:
                        SwfFormatException e = new SwfFormatException("Unsupported sound encoding found in sound stream block.");
                        Log.Error(this, e.Message);
                        throw e;
                }
            }
            else
            {
                this._soundData = new RawSoundData(b);
            }
        }

        ///<summary>
        /// checks if an AbstractTagHandler is type of SoundStreamHead
        ///</summary>
        private static bool isSoundStreamHead(AbstractTagHandler handler)
        {
            return (handler.GetType().Equals(typeof(SoundStreamHead)));
        }

        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(_dataStream);
            this._soundstreamhead = (SoundStreamHead)(this._SourceFileReference.TagHandlers.FindLast(isSoundStreamHead));

            if (_soundstreamhead == null)
            {
                SwfFormatException s = new SwfFormatException("No SoundStreamHead found");
                Log.Warn(this, s.Message);
                //throw s;
            }

            this.TryParseSoundData(this._dataStream);

            String s1 = String.Format("0x{0:X08}: reading soundstreamblock ({1} bytes)",
                this.Tag.OffsetData,
                this._soundData.Length);
            Log.Debug(this, s1);

            if (this.Head != null)
            {
                String s2 = String.Format("0x{0:X08}\tHead: Playback: {1} {2} {3}",
                    this.Head.Tag.OffsetData,
                    this.Head.PlaybackRate,
                    this.Head.PlaybackSize,
                    this.Head.PlaybackType);

                 Log.Debug(this, s2);

                String s3 = String.Format("0x{0:X08}\tHead: Stream: {1} {2} {3} {4}",
                    this.Head.Tag.OffsetData,
                    this.Head.StreamCompression,
                    this.Head.StreamRate,
                    this.Head.StreamSize,
                    this.Head.StreamType);
                 Log.Debug(this, s3);
            }

        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);
            this._soundData.Write(output);

        }

    }

}
