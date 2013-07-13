using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    public class DefineButtonSound : AbstractTagHandler
    {
        public ushort CharacterID { get; internal set; }
        
        public ushort ButtonSoundChar0 { get; internal set; }
        public SoundInfo ButtonSoundInfo0 { get; internal set; }

        public ushort ButtonSoundChar1 { get; internal set; }
        public SoundInfo ButtonSoundInfo1 { get; internal set; }

        public ushort ButtonSoundChar2 { get; internal set; }
        public SoundInfo ButtonSoundInfo2 { get; internal set; }

        public ushort ButtonSoundChar3 { get; internal set; }
        public SoundInfo ButtonSoundInfo3 { get; internal set; }

        public DefineButtonSound(byte InitialVersion) : base(InitialVersion)
        {

        }

        public override byte MinimumVersionRequired
        {
            get { return 2; }
        }

        public override ulong Length
        {
            get 
            {
                ulong length = 0;
                length += this.ButtonSoundInfo0.Length;
                length += this.ButtonSoundInfo1.Length;
                length += this.ButtonSoundInfo2.Length;
                length += this.ButtonSoundInfo3.Length;
                length += 8;
                return length;
            }
        }

        public override bool Verify()
        {
            return true;
        }

        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this.CharacterID = br.ReadUInt16();
            this.ButtonSoundChar0 = br.ReadUInt16();
            this.ButtonSoundInfo0 = SoundInfo.Parse(this._dataStream);
            this.ButtonSoundChar1 = br.ReadUInt16();
            this.ButtonSoundInfo1 = SoundInfo.Parse(this._dataStream);
            this.ButtonSoundChar2 = br.ReadUInt16();
            this.ButtonSoundInfo2 = SoundInfo.Parse(this._dataStream);
            this.ButtonSoundChar3 = br.ReadUInt16();
            this.ButtonSoundInfo3 = SoundInfo.Parse(this._dataStream);
        }

        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
           
            bw.Write(this.CharacterID);
           
            bw.Write(this.ButtonSoundChar0);
            this.ButtonSoundInfo0.Write(output);
           
            bw.Write(this.ButtonSoundChar1);
            this.ButtonSoundInfo1.Write(output);
           
            bw.Write(this.ButtonSoundChar2);
            this.ButtonSoundInfo2.Write(output);
            
            bw.Write(this.ButtonSoundChar3);
            this.ButtonSoundInfo3.Write(output);
        }
    }
}
