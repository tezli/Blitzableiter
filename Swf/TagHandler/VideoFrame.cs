using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// VideoFrame provides a single frame of video data.
    /// </summary>
    /// <remarks>
    /// <para>VideoFrame provides a single frame of video data for a video character</para> 
    /// <para>that is already definedwith DefineVideoStream.In playback, the time</para> 
    /// <para>sequencing of video frames depends on the Swf frame rate only. When</para>
    /// <para>Swf playback reaches a particular Swf frame, the video images from any</para> 
    /// <para>VideoFrame tags in that Swf frame are rendered. Any timing mechanisms</para> 
    /// <para>built into the video payload are ignored. A VideoFrame tag is not</para> 
    /// <para>needed for every video character in every frame number specified. A</para> 
    /// <para>VideoFrame tag merely sets video data associated with a particular</para> 
    /// <para>frame number; it does not automatically display a video frame.</para> 
    /// <para>To display a video frame, specify the frame number as the Ratio</para> 
    /// <para>field in PlaceObject2 or PlaceObject3.</para>
    /// </remarks>
    public class VideoFrame : AbstractTagHandler
    {
        private UInt16 _streamID;
        private UInt16 _frameNumber;
        private byte[] _videoData;


        /// <summary>
        /// VideoFrame provides a single frame of video data
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public VideoFrame(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 6;
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
                return this.Tag.Length;
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

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);
            
            this._streamID = br.ReadUInt16();
            this._frameNumber = br.ReadUInt16();

            DefineVideoStream streamReference = GetStreamReference(this._streamID);

            this._videoData = new byte[this._tag.Length - (sizeof(UInt16) * 2)];
            int read = this._dataStream.Read(this._videoData, 0, this._videoData.Length);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._streamID);
            bw.Write(this._frameNumber);
            output.Write(this._videoData, 0, this._videoData.Length);

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

        /// <summary>
        /// Verifies is an AbstractTagHandler is a DefineVideoStream tag (for later use)
        /// </summary>
        /// <param name="handler">An AbstractTagHandler.</param>
        /// <returns>True if the AbstractTagHandler is a DefineFonttag</returns>
        /// <remarks>
        /// This method is for later use if we decide to parse the video data as well.
        /// </remarks>
        private static bool VideoStream(AbstractTagHandler handler)
        {
            if (handler.GetType().Equals(typeof(DefineVideoStream)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a reference to the Stream used by this tag.
        /// </summary>
        /// <param name="streamId">The id of the stream</param>
        /// <returns>The VideoStream to which the stream id refers. Returns null if no font matches the ID</returns>
        /// <remarks>
        /// This method is for later use if we decide to parse the video data as well.
        /// </remarks>
        private DefineVideoStream GetStreamReference(UInt16 streamId)
        {
            List<AbstractTagHandler> streams = this._SourceFileReference.TagHandlers.FindAll(VideoStream);

            foreach (DefineVideoStream d in streams)
            {
                if (d.StreamID.Equals(streamId))
                {
                    return d;
                }
            }
            return null;
        }
    }
}
