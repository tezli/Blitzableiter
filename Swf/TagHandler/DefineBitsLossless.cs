using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>Defines a lossless bitmap character that contains RGB bitmap data compressed with ZLIB.</para>
    /// <para>The data format used by the ZLIB library is described by Request for Comments (RFCs)</para>
    /// <para>documents 1950 to 1952.</para>
    /// <para>Two kinds of bitmaps are supported. Colormapped images define a colormap of up to 256</para>
    /// <para>colors, each represented by a 24-bit RGB value, and then use 8-bit pixel values to index into</para>
    /// <para>the colormap. Direct images store actual pixel color values using 15 bits (32,768 colors) or 24</para>
    /// <para>bits (about 17 million colors).</para>
    /// </summary>
    public class DefineBitsLossless : AbstractTagHandler, ISwfCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _characterID;

        /// <summary>
        /// 
        /// </summary>
        protected byte _bitmapFormat;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _bitmapWidth;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _bitmapHight;

        /// <summary>
        /// 
        /// </summary>
        protected byte _bitmapColorTableSize;

        /// <summary>
        /// 
        /// </summary>
        protected byte[] _zlibBitmapData;

        /// <summary>
        /// Defines a lossless bitmap character that contains RGB bitmap data compressed with ZLIB.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public DefineBitsLossless(byte InitialVersion) : base(InitialVersion)
        {
            this._characterID = 0;
            this._bitmapFormat = 0;
            this._bitmapWidth = 0;
            this._bitmapHight = 0;
            this._bitmapColorTableSize = 0;
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
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 2;

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
                ulong ret = 0;

                using (MemoryStream m = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(m);

                    bw.Write(this._characterID);
                    bw.Write(this._bitmapFormat);
                    bw.Write(this._bitmapWidth);
                    bw.Write(this._bitmapHight);

                    m.Write(this._zlibBitmapData, 0, this._zlibBitmapData.Length);
                    ret = (ulong)m.Position;
                }

                return ret;
            }
        }

        /// <summary>
        /// Verifies this tag and its component for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public override bool Verify()
        {
            // TODO : implement a check. we could check for ZLIB compression in _zlibBitmapDataC or _zlibBitmapDataB;
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();
            this._bitmapFormat = br.ReadByte();
            this._bitmapWidth = br.ReadUInt16();
            this._bitmapHight = br.ReadUInt16();

            this._zlibBitmapData = new Byte[this._dataStream.Length - this._dataStream.Position];
            this._dataStream.Read(this._zlibBitmapData, 0, this._zlibBitmapData.Length);
        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            this.WriteTagHeader(output);
            
            bw.Write(this._characterID);
            bw.Write(this._bitmapFormat);
            bw.Write(this._bitmapWidth);
            bw.Write(this._bitmapHight);

            output.Write(this._zlibBitmapData, 0, this._zlibBitmapData.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            if (this._bitmapFormat == 0x03)
            {
                sb.Append(" BitmapFormat is : 8Bit ");
            }
            else if (this._bitmapFormat == 0x04)
            {
                sb.Append(" BitmapFormat is 15Bit ");
            }
            else if (this._bitmapFormat == 0x05)
            {
                sb.Append(" BitmapFormat is : 24Bit ");
            }
            else
            {
                sb.Append(" Unknown BitmapFormat");
            }

            return sb.ToString();
        }
    }
}
