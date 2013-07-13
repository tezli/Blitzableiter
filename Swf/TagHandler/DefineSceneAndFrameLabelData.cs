using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class DefineSceneAndFrameLabelData : AbstractTagHandler
    {

        private ulong _sceneCount;
        private ulong[] _sceneOffset;
        private string[] _sceneName;

        private ulong _frameNum;
        private ulong[] _frames;
        private string[] _frameLabel;

        public DefineSceneAndFrameLabelData(byte init) : base(init) { }

        /// <summary>
        /// Number of scenes defined within the tag
        /// </summary>
        public ulong SceneCount
        {
            get
            {
                return _sceneCount;
            }
        }

        /// <summary>
        /// Offsets of scenes defined within the tag
        /// </summary>
        public ulong[] SceneOffsets
        {
            get
            {
                return _sceneOffset;
            }
        }

        /// <summary>
        /// Labels of scenes defined within the tag
        /// </summary>
        public string[] SceneLabels
        {
            get
            {
                return _sceneName;
            }
        }

        /// <summary>
        /// Number of Frames defined within the tag
        /// </summary>
        public ulong FrameCount
        {
            get
            {
                return _frameNum;
            }
        }

        /// <summary>
        /// IDs of the frames defined within the tag
        /// </summary>
        public ulong[] FrameIDs
        {
            get
            {
                return _frames;
            }
        }

        /// <summary>
        /// Labels of frames defined within the tag
        /// </summary>
        public string[] FrameLabels
        {
            get
            {
                return _frameLabel;
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
                ulong result = 0;
                result += (ulong)SwfEncodedU32.SwfEncodedSizeOf(_sceneCount);
                result += (ulong)SwfEncodedU32.SwfEncodedSizeOf(_frameNum);

                for (ulong i = 0; i < _sceneCount; i++)
                    result += (ulong)SwfEncodedU32.SwfEncodedSizeOf(_sceneOffset[i]) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _sceneName[i]);

                for (ulong i = 0; i < _frameNum; i++)
                    result += (ulong)SwfEncodedU32.SwfEncodedSizeOf(_frames[i]) + (ulong)SwfStrings.SwfStringLength(_SwfVersion, _frameLabel[i]);

                return result;
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


        protected override void Parse()
        {
            String s = String.Format("0x{0:X08}: reading DefineSceneAndFrameLabelData-Tag", this.Tag.OffsetData);
            Log.Debug(this, s);

            BinaryReader br = new BinaryReader(_dataStream);
            _sceneCount = SwfEncodedU32.SwfReadEncodedU32(br);
            _sceneOffset = new ulong[_sceneCount];
            _sceneName = new string[_sceneCount];

            for (ulong i = 0; i < _sceneCount; i++)
            {
                _sceneOffset[i] = SwfEncodedU32.SwfReadEncodedU32(br);
                _sceneName[i] = SwfStrings.SwfString(this._SwfVersion, br);

                String s1 = String.Format("0x{0:X08}:\tScene {1} {2}", this.Tag.OffsetData, _sceneOffset[i], _sceneName[i]);
                Log.Debug(this, s1);
            }

            _frameNum = SwfEncodedU32.SwfReadEncodedU32(br);
            _frames = new ulong[_frameNum];
            _frameLabel = new string[_frameNum];

            for (ulong i = 0; i < _frameNum; i++)
            {
                _frames[i] = SwfEncodedU32.SwfReadEncodedU32(br);
                _frameLabel[i] = SwfStrings.SwfString(this._SwfVersion, br);

                String s1 = String.Format("0x{0:X08}:\tFrame {1} {2}", this.Tag.OffsetData, _frames[i], _frameLabel[i]);
                Log.Debug(this, s1);
            }
        }

        public override void Write(Stream output)
        {
            WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);

            SwfEncodedU32.SwfWriteEncodedU32(_sceneCount, bw);
            for (ulong i = 0; i < _sceneCount; i++)
            {
                SwfEncodedU32.SwfWriteEncodedU32(_sceneOffset[i], bw);
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._sceneName[i]);
            }

            SwfEncodedU32.SwfWriteEncodedU32(_frameNum, bw);
            for (ulong i = 0; i < _frameNum; i++)
            {
                SwfEncodedU32.SwfWriteEncodedU32(_frames[i], bw);
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._frameLabel[i]);
            }

        }

    }

}
