using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public enum SoundType
    {
        /// <summary>
        /// 
        /// </summary>
        mono = 0,
        /// <summary>
        /// 
        /// </summary>
        stereo = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SoundSize
    {

        /// <summary>
        /// 
        /// </summary>
        snd_8bit = 0,

        /// <summary>
        /// 
        /// </summary>
        snd_16bit = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SoundRate
    {

        /// <summary>
        /// 
        /// </summary>
        snd_5kHz = 0,

        /// <summary>
        /// 
        /// </summary>
        snd_11kHz = 1,

        /// <summary>
        /// 
        /// </summary>
        snd_22kHz = 2,

        /// <summary>
        /// 
        /// </summary>
        snd_44kHz = 3,

        /// <summary>
        /// 
        /// </summary>
        snd_Unknown
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SoundEncoding
    {

        /// <summary>
        /// 
        /// </summary>
        uncompressed_native = 0,

        /// <summary>
        /// 
        /// </summary>
        ADPCM = 1,

        /// <summary>
        /// 
        /// </summary>
        MP3 = 2,

        /// <summary>
        /// 
        /// </summary>
        Uncompressed_little_endian = 3,

        /// <summary>
        /// 
        /// </summary>
        Nellymoser16kHz = 4,

        /// <summary>
        /// 
        /// </summary>
        Nellymoser8kHz = 5,

        /// <summary>
        /// 
        /// </summary>
        Nellymoser = 6,

        /// <summary>
        /// 
        /// </summary>
        Speex = 11,

        /// <summary>
        /// 
        /// </summary>
        Unknown = 255

    }
    
    /// <summary>
    /// 
    /// </summary>
    public class DefineSound : AbstractTagHandler, ISwfCharacter
    {
        private byte _requiredVersion = 1;
        private UInt16 _soundID;
        private SoundEncoding _soundFormat;
        private SoundRate _soundRate;
        private SoundSize _soundSize;
        private SoundType _soundType;
        private UInt32 _soundSampleCount;
        private byte[] _soundData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DefineSound(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte getRequiredVersion(SoundEncoding format)
        {
            if (format == SoundEncoding.uncompressed_native || format == SoundEncoding.ADPCM)
                return 1;

            else if (format == SoundEncoding.Uncompressed_little_endian || format == SoundEncoding.MP3)
                return 4;

            else if (format == SoundEncoding.Nellymoser16kHz || format == SoundEncoding.Nellymoser8kHz || format == SoundEncoding.Speex)
                return 10;

            else if (format == SoundEncoding.Nellymoser)
                return 6;

            else
                return 255; // Unknown format
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SoundEncoding getFormat(uint type)
        {
            if (type == 0)
                return SoundEncoding.uncompressed_native;

            else if(type == 1)
                return SoundEncoding.ADPCM;

            else if(type == 2)
                return SoundEncoding.MP3;

            else if(type == 3)
                return SoundEncoding.Uncompressed_little_endian;

            else if(type == 4)
                return SoundEncoding.Nellymoser8kHz;

            else if(type == 5)
                return SoundEncoding.Nellymoser16kHz;

            else if(type == 6)
                return SoundEncoding.Nellymoser;

            else if(type == 11)
                return SoundEncoding.Speex;

            SwfFormatException sfe = new SwfFormatException("coding format is not defined");
            Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.DeclaringType, sfe);
            throw sfe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static uint getFormatID(SoundEncoding format)
        {
            if (format == SoundEncoding.uncompressed_native)
                return 0;

            else if (format == SoundEncoding.ADPCM)
                return 1;

            else if (format == SoundEncoding.MP3)
                return 2;

            else if (format == SoundEncoding.Uncompressed_little_endian)
                return 3;

            else if (format == SoundEncoding.Nellymoser8kHz)
                return 4;

            else if (format == SoundEncoding.Nellymoser16kHz)
                return 5;

            else if (format == SoundEncoding.Nellymoser)
                return 6;

            else if (format == SoundEncoding.Speex)
                return 11;

            else
                return 255; // Invalid Value
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static SoundRate getSoundRate(uint sr)
        {
            if (sr == 0)
                return SoundRate.snd_5kHz;

            else if (sr == 1)
                return SoundRate.snd_11kHz;

            else if (sr == 2)
                return SoundRate.snd_22kHz;

            else if (sr == 3)
                return SoundRate.snd_44kHz;

            else
                return SoundRate.snd_Unknown;
        }

        //

        /// <summary>
        /// Determines the number indicates the given soundrate
        /// </summary>
        /// <param name="sr">The sound rate</param>
        /// <returns>The sound rate ID</returns>
        public static uint getSoundRateID(SoundRate sr)
        {
            if (sr == SoundRate.snd_5kHz)
                return 0;

            else if (sr == SoundRate.snd_11kHz)
                return 1;

            else if (sr == SoundRate.snd_22kHz)
                return 2;

            else if (sr == SoundRate.snd_44kHz)
                return 3;

            else
                return 255;  //Invalid Value
        }

        /// <summary>
        /// Calculates the number of bytes used by the Sound
        /// </summary>
        public UInt32 SampleCount
        {
            get
            {
                return _soundSampleCount;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                // 1 since no format is given
                return _requiredVersion;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (ulong)7 + (ulong)_soundData.Length;
            }
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _soundID;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            //TODO: initial version >= requiredVersion
            //TODO: format value is correct
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);
            BitStream bits = new BitStream(this._dataStream);

            this._soundID = br.ReadUInt16();
            this._soundFormat = getFormat(bits.GetBits(4));
            this._soundRate = getSoundRate(bits.GetBits(2));
            this._soundSize = (bits.GetBits(1) == 0) ? SoundSize.snd_8bit : SoundSize.snd_16bit;
            this._soundType = (bits.GetBits(1) == 0) ? SoundType.mono : SoundType.stereo;
            this._soundSampleCount = br.ReadUInt32();

            this._requiredVersion = getRequiredVersion(this._soundFormat);

            // TODO: check buffersize
            // Casting from UInt32 to int !!!!
            this._soundData = br.ReadBytes((int)Tag.Length - 7);

            String s = String.Format("\t{0} {1}", Tag.Length, Length);
            //Log.Debug(this, s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
           //Log.Debug(this, "Writing DefineSound Tag");

            WriteTagHeader(output);

            byte[] id = BitConverter.GetBytes(this._soundID);
            output.Write(id, 0, 2);
            output.Flush();
            BitStream bits = new BitStream(output);

            bits.WriteBits(4, getFormatID(this._soundFormat));
            bits.WriteBits(2, getSoundRateID(this._soundRate));
            bits.WriteBits(1, this._soundSize == SoundSize.snd_8bit ?0 :1);
            bits.WriteBits(1, this._soundType == SoundType.mono ?0 :1);

            bits.WriteFlush();

            byte[] size = BitConverter.GetBytes(this._soundSampleCount);
            output.Write(size, 0, 4);
            output.Write(this._soundData, 0, this._soundData.Length);
            output.Flush();
        }

    }
}
