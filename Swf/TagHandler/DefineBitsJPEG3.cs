using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.AVM1;
using Recurity.Swf.Helper;
using System.Drawing;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>This tag defines a bitmap character with JPEG compression. This tag extends</para>
    /// <para>DefineBitsJPEG2, adding alpha channel (opacity) data. Opacity/transparency information is</para>
    /// <para>not a standard feature in JPEG images, so the alpha channel information is encoded separately</para>
    /// <para>from the JPEG data, and compressed using the ZLIB standard for compression. The data</para>
    /// <para>format used by the ZLIB library is described by Request for Comments (RFCs) documents</para>
    /// <para>1950 to 1952.</para>
    /// <para>The data in this tag begins with the JPEG SOI marker 0xFF, 0xD8 and ends with the EOI</para>
    /// <para>marker 0xFF, 0xD9. Before version 8 of the Swf file format, Swf files could contain an</para>
    /// <para>erroneous header of 0xFF, 0xD9, 0xFF, 0xD8 before the JPEG SOI marker.</para>
    /// <para>In addition to specifying JPEG data, DefineBitsJPEG2 can also contain PNG image data and</para>
    /// <para>non-animated GIF89a image data.</para>
    /// <para>If ImageData begins with the eight bytes 0x89 0x50 0x4E 0x47 0x0D 0x0A 0x1A 0x0A, the</para>
    /// <para>ImageData contains PNG data.</para>
    /// <para>If ImageData begins with the six bytes 0x47 0x49 0x46 0x38 0x39 0x61, the ImageData</para>
    /// <para>contains GIF89a data.</para>
    /// <para>If ImageData contains PNG or GIF89a data, the optional BitmapAlphaData is not</para>
    /// <para>supported.</para>
    /// <para>The minimum file format version for this tag is Swf 3. The minimum file format version for</para>
    /// <para>embedding PNG of GIF89a data is Swf 8</para>
    /// </summary>
    public class DefineBitsJPEG3 : AbstractTagHandler, ISwfCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt16 _characterID = 0;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _alphaDataOffset;

        /// <summary>
        /// 
        /// </summary>
        private byte[] _imageData;

        /// <summary>
        /// 
        /// </summary>
        private byte[] _bitmapAlphaData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DefineBitsJPEG3(byte InitialVersion)
            : base(InitialVersion)
        {
            this._characterID = 0;
            this._alphaDataOffset = 0;
            this._imageData = new byte[0];
            this._bitmapAlphaData = new byte[0];
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
                return this.Tag.Length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            //We do not use System.Drawing.Image here since it is nested in GDI
            MemoryStream ms = new MemoryStream();

            //if jpeg contains erroneous header we fix it
            if (this._imageData[0].Equals(0xff) && this._imageData[1].Equals(0xd9) && this._imageData[2].Equals(0xff) && this._imageData[3].Equals(0xd8))
            {
                ms.Write(this._imageData, 4, this._imageData.Length - 4);
                ms.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                ms.Write(this._imageData, 0, this._imageData.Length);
                ms.Seek(0, SeekOrigin.Begin);
            }
            return true;

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
        /// 
        /// </summary>
        protected override void Parse()
        {
            Stream input = this._dataStream;
            BinaryReader br = new BinaryReader(input);

            this._characterID = br.ReadUInt16();
            this._alphaDataOffset = br.ReadUInt32();

            this._imageData = new byte[this._alphaDataOffset - (UInt32)this._dataStream.Position];
            this._imageData = br.ReadBytes(this._imageData.Length);

            this._bitmapAlphaData = new byte[input.Length - this._alphaDataOffset];
            this._bitmapAlphaData = br.ReadBytes(this._bitmapAlphaData.Length);
        }

        /// <summary>
        /// Writes a tag back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(System.IO.Stream output)
        {
            this.WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._characterID);
            bw.Write(this._alphaDataOffset);

            output.Write(this._imageData, 0, _imageData.Length);
            output.Write(this._bitmapAlphaData, 0, _bitmapAlphaData.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat(" Character ID : {0:d} , image data size: {1:d}, bitmap alpha offset: {2:d}, bitmap alpha length: {3:d}", this._characterID, this._imageData.Length, this._alphaDataOffset, this._bitmapAlphaData.Length);
            return sb.ToString();
        }

        /// <summary>
        /// Verifies this object and its component for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        private bool VerifyGIF98a()
        {
            if (this._imageData.Length > 6)
            {
                return false;
            }
            byte[] GIF98aMagic = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
            byte[] comparer = { this._imageData[0], this._imageData[1], this._imageData[2], 
                                this._imageData[3], this._imageData[4], this._imageData[5] };

            if (GIF98aMagic.Equals(comparer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies this object and its component for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        private bool VerifyPNG()
        {
            if (this._imageData.Length > 8)
            {
                return false;
            }

            byte[] PNGMagic = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            byte[] comparer = { this._imageData[ 0 ], this._imageData[ 1 ], this._imageData[ 2 ], 
                                this._imageData[ 3 ], this._imageData[ 4 ], this._imageData[ 5 ], 
                                this._imageData[ 6 ], this._imageData[ 7 ] };

            if (PNGMagic.Equals(comparer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies this object and its component for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        private bool VerifyJPEG()
        {
            if (this._imageData.Length > 4)
            {
                return false;
            }
            byte[] JPEGMagic = { 0xFF, 0xD8, 0xFF, 0xD9 };
            byte[] comparer = { this._imageData[ 0 ], this._imageData[ 1 ], this._imageData[ this._imageData.Length - 2 ], 
                                this._imageData[ this._imageData.Length - 1 ] };

            if (JPEGMagic.Equals(comparer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
