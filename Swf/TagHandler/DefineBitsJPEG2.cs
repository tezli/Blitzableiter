using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>This tag defines a bitmap character with JPEG compression.</para> 
    /// <para>It differs from DefineBits in that it contains both the JPEG </para>
    /// <para>encoding table and the JPEG image data. This tag allows multiple</para>
    /// <para>JPEG images with differing encoding tables to be defined within a </para>
    /// <para>single Swf file. The data in this tag begins with the JPEG SOI </para>
    /// <para>marker 0xFF, 0xD8 and ends with the EOI</para>
    /// <para>marker 0xFF, 0xD9. Before version 8 of the Swf file format,</para> 
    /// <para>Swf files could contain an erroneous header of 0xFF, 0xD9, 0xFF, 0xD8 </para>
    /// <para>before the JPEG SOI marker. In addition to specifying JPEG data, </para>
    /// <para>DefineBitsJPEG2 can also contain PNG image data and non-animated </para>
    /// <para>GIF89a image data. If ImageData begins with the eight bytes </para>
    /// <para>0x89 0x50 0x4E 0x47 0x0D 0x0A 0x1A 0x0A, the ImageData contains </para>
    /// <para>PNG data. If ImageData begins with the six bytes </para>
    /// <para>0x47 0x49 0x46 0x38 0x39 0x61, the ImageData contains GIF89a data.</para>
    /// </summary>
    public class DefineBitsJPEG2 : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _characterID;
        private byte[] _imageData;

        /// <summary>
        /// This tag defines a bitmap character with JPEG compression.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this tag.</param>
        public DefineBitsJPEG2( byte InitialVersion ) : base( InitialVersion )
        {
    
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return  2;
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

            BinaryReader br = new BinaryReader( input );

            this._characterID = br.ReadUInt16();
            this._imageData = new byte[(Int32)(input.Length - input.Position)];
            this._imageData = br.ReadBytes(this._imageData.Length);

            String s = String.Format("Reading DefineBitsJPEG2. Character ID: {0:d}, data length {1:d}", this._characterID, this._imageData.Length);
            //Log.Debug(this, s);
        }

        /// <summary>
        /// Writes a tag back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write( Stream output )
        {
            this.WriteTagHeader( output );

            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this._characterID );

            String s = String.Format("Writing DefineBitsJPEG2. Character ID: {0:d}, data length {1:d}", this._characterID, this._imageData.Length);
            //Log.Debug(this, s);

            output.Write(this._imageData, 0, this._imageData.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendFormat(" Character ID : {0:d} , image data size: {1:d}", this._characterID, this._imageData.Length);
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

            if ( GIF98aMagic.Equals( comparer ) )
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

            if ( PNGMagic.Equals( comparer ) )
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

            if ( JPEGMagic.Equals( comparer ) )
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