using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// The video packet is the top-level structural element in a screen video packet.
    /// </summary>
    /// <remarks>
    /// <para>The video packet is the top-level structural element in a screen video packet.</para> 
    /// <para>This structure is included within the VideoFrame tag in the SWF file format, and </para> 
    /// <para>also within the VIDEODATA structure in the FLV file format.</para> 
    /// </remarks>
    public class ScreenVideoPacket : AbstractSwfElement, IVideoPacket
    {
        private byte _blockWidth;
        private UInt16 _imageWidth;
        private byte _blockHeight;
        private UInt16 _imageHeight;
        private List<ImageBlock> _imageBlocks;

        /// <summary>
        /// The video packet is the top-level structural element in a screen video packet.
        /// </summary>
        /// <param name="InitialVersion">The version of the SWF file using this object.</param>
        public ScreenVideoPacket(byte InitialVersion) : base(InitialVersion)
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

            this._blockWidth = (byte)bits.GetBits(4);
            this._imageWidth = (byte)bits.GetBits(12);
            this._blockHeight = (byte)bits.GetBits(4);
            this._imageHeight = (byte)bits.GetBits(12);

            ImageBlock temp = null;

            this._imageBlocks = new List<ImageBlock>();

            while (input.Length - input.Position > sizeof(UInt16))
            {
                temp = new ImageBlock(this._SwfVersion);
                temp.Parse(input);
                this._imageBlocks.Add(temp);
            }
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
