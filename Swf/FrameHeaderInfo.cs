using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class FrameHeaderInfo : AbstractSwfElement
    {
        private Rect _FrameSize;
        private float _FrameRate;
        private UInt16 _FrameRateDelay;
        private UInt16 _FrameCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public FrameHeaderInfo(byte version)
            : base(version)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Rect FrameSize
        {
            get
            {
                return this._FrameSize;
            }
            set
            {
                this._FrameSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            _FrameSize = new Rect(this.Version);
            _FrameSize.Parse(input);

            _FrameRateDelay = br.ReadUInt16();
            _FrameRate = (_FrameRateDelay >> 8) + ((_FrameRateDelay & 0xFF) / 100);
            _FrameCount = br.ReadUInt16();

            int x = Math.Abs((this._FrameSize.Xmax - this._FrameSize.Xmin) / 12);
            int y = Math.Abs((this._FrameSize.Ymax - this._FrameSize.Ymin) / 12);

            if (x > SwfFile.Configuration.MaximumStageSizeX)
            {
                Log.Warn(this, "The x value(" + x + ") of the stage exceeds the allowed maximum.");
            }

            if (x < SwfFile.Configuration.MinimumStageSizeX)
            {
                Log.Warn(this, "The x value(" + x + ") of the stage under-runs the allowed minimum.");
            }

            if (y > SwfFile.Configuration.MaximumStageSizeY)
            {
                Log.Warn(this, "The y value(" + y + ") of the stage exceeds the allowed maximum.");
            }
            if (y < SwfFile.Configuration.MinimumStageSizeY)
            {
                Log.Warn(this, "The y value(" + y + ") of the stage under-runs the allowed minimum.");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                // FrameSize, FrameRateDelay, FrameCount
                return (uint)(_FrameSize.Length + 4);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            _FrameSize.Write(output);
            bw.Write(_FrameRateDelay);
            bw.Write(_FrameCount);
        }
    }
}
