using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>DefineBitsLossless2 extends DefineBitsLossless with support for opacity (alpha values). The</para>
    /// <para>colormap colors in colormapped images are defined using RGBA values, and direct images</para>
    /// <para>store 32-bit ARGB colors for each pixel. The intermediate 15-bit color depth is not available</para>
    /// <para>in DefineBitsLossless2. </para>
    /// </summary>
    public class DefineBitsLossless2 : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _characterID;
        private byte _bitmapFormat;
        private UInt16 _bitmapWidth;
        private UInt16 _bitmapHight;
        private byte _bitmapColorTableSize;
        private byte[] _compressedImageData;

        /// <summary>
        /// DefineBitsLossless2 extends DefineBitsLossless with support for opacity (alpha values).
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineBitsLossless2(byte InitialVersion): base(InitialVersion)
        {
    
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
            }
        }

        /// <summary>
        /// The minimum swf version for using this object.
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
                return this._tag.Length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {

            return true; // not much to do here excepting JPEG image check
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();
            this._bitmapFormat = br.ReadByte();
            this._bitmapWidth = br.ReadUInt16();
            this._bitmapHight = br.ReadUInt16();


            this._bitmapColorTableSize = br.ReadByte();
            this._compressedImageData = br.ReadBytes((Int32)(this._dataStream.Length - this._dataStream.Position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._characterID);
            bw.Write(this._bitmapFormat);
            bw.Write(this._bitmapWidth);
            bw.Write(this._bitmapHight);

            output.WriteByte(this._bitmapColorTableSize);
            output.Write(this._compressedImageData, 0, this._compressedImageData.Length);
        }

    }
}
