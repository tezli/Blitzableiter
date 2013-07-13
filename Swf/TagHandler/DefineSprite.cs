using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// Defines a sprite character
    /// </summary>
    /// <remarks>
    /// <para>The DefineSprite tag defines a sprite character. It consists of a </para>
    /// <para>character ID and a frame count, followed by a series of control </para>
    /// <para>tags. The sprite is terminated with an End tag. The length specified </para>
    /// <para>in the Header reflects the length of the entire DefineSprite tag, including</para>
    /// <para>the ControlTags field. Definition tags (such as DefineShape) are not </para>
    /// <para>allowed in the DefineSprite tag. All of the characters that control tags</para>
    /// </remarks>
    public class DefineSprite : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _spriteID;
        private UInt16 _frameCount;
        private List<AbstractTagHandler> _controlTags;
        
        private readonly List<TagTypes> _allowedTags = new List<TagTypes>()
        {
            TagTypes.DoAction,
            TagTypes.End,
            TagTypes.FrameLabel,
            TagTypes.PlaceObject,
            TagTypes.PlaceObject2,
            TagTypes.PlaceObject3,
            TagTypes.RemoveObject,
            TagTypes.RemoveObject2,
            TagTypes.ShowFrame,
            TagTypes.StartSound,
            TagTypes.SoundStreamHead,
            TagTypes.SoundStreamHead2,
            TagTypes.SoundStreamBlock,
            TagTypes.VideoFrame

        };

        /// <summary>
        /// Defines a sprite character
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public DefineSprite(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// Id of the defined Character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _spriteID;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this._spriteID);
                bw.Write(this._frameCount);
                for (int i = 0; i < this._controlTags.Count; i++)
                {
                    this._controlTags[i].Write(ms);
                }
                return (ulong)ms.Length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            foreach (AbstractTagHandler t in this._controlTags)
            {
                if (!_allowedTags.Contains(t.Tag.TagType))
                {
                    //TODO: Log.Warn(this, "DefineSprite contains illegal tag(s).");
                }

            }
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._spriteID = br.ReadUInt16();
            this._frameCount = br.ReadUInt16();

            this._controlTags = new List<AbstractTagHandler>();

            Tag t;

            MemoryStream tempstream = new MemoryStream();
            long position = this._dataStream.Position;
            this._dataStream.WriteTo(tempstream);
            tempstream.Seek(position, SeekOrigin.Begin);

            do
            {
                t = new Tag();

                try
                {
                    tempstream = (MemoryStream)t.Read(tempstream);

                    this._controlTags.Add(TagHandlerFactory.Create(t, this._SourceFileReference, this._dataStream));

                    if (!this._allowedTags.Contains(t.TagType))
                    {
                        if (TagTypes.PlaceObject3 != t.TagType)
                        {
                            SwfFormatException e = new SwfFormatException("DefineSprite contains illegal tag.(" + t.TagType + ")");
                            Log.Warn(this, e.Message);
                            throw e;
                        }
                    }
                }
                catch ( IOException ioe )
                {
                   Log.Error(this, ioe );
                    SwfFormatException e = new SwfFormatException( "Tag list incomplete, does not end with END tag" );
                    Log.Warn(this,  e );
                    //throw e;
                }
            }
            while (!t.TagType.Equals(TagTypes.End));
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this._spriteID);
            bw.Write(this._frameCount);

            for (int i = 0; i < this._controlTags.Count; i++)
            {
                this._controlTags[i].Write(output);
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._spriteID);
            return sb.ToString();
        }

    }
}
