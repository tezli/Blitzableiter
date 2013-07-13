using System;
using System.IO;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public enum BlendMode : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Normal0 = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Normal1 = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Layer = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Multiply = 0x03,
        /// <summary>
        /// 
        /// </summary>
        Screen = 0x04,
        /// <summary>
        /// 
        /// </summary>
        Lighten = 0x05,
        /// <summary>
        /// 
        /// </summary>
        Darken = 0x06,
        /// <summary>
        /// 
        /// </summary>
        Difference = 0x07,
        /// <summary>
        /// 
        /// </summary>
        Add = 0x08,
        /// <summary>
        /// 
        /// </summary>
        Substract = 0x09,
        /// <summary>
        /// 
        /// </summary>
        Invert = 0x10,
        /// <summary>
        /// 
        /// </summary>
        Aplpha = 0x11,
        /// <summary>
        /// 
        /// </summary>
        Earse = 0x12,
        /// <summary>
        /// 
        /// </summary>
        Overlay = 0x13,
        /// <summary>
        /// 
        /// </summary>
        Hardlight = 0x14

    }

    /// <summary>
    /// 
    /// </summary>
    public class PlaceObject3 : PlaceObject2
    {
        /// <summary>
        /// 
        /// </summary>
        private Boolean _PlaceFlagHasImage;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _PlaceFlagHasClassName;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _PlaceFlagHasCacheAsBitmap;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _PlaceFlagHasBlendMode;
        /// <summary>
        /// 
        /// </summary>
        private Boolean _PlaceFlagHasFilterList;
        /// <summary>
        /// 
        /// </summary>
        private String _ClassName;
        /// <summary>
        /// 
        /// </summary>
        private FilterList _SurfaceFilterList;
        /// <summary>
        /// 
        /// </summary>
        private BlendMode _BlendMode;
        /// <summary>
        /// 
        /// </summary>
        private Byte _BitmapCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public PlaceObject3(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                ulong ret = 0;

                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(ms);
                    BitStream bs = new BitStream(ms);

                    this.WriteFlags(bw, bs);
                    this.WriteFields(bw, bs, ms);

                    ret = (ulong)ms.Position;
                }

                return ret;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            return base.Verify();
        }

        /// <summary>
        /// Parses the flags of this object out of a stream.
        /// </summary>
        /// <param name="br">The BinaryReader</param>
        /// <param name="bs">The Bitstream</param>
        private void ParseFlags(BinaryReader br, BitStream bs)
        {
            this._PlaceFlagHasClipActions = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasClipDepth = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasName = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasRatio = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasColorTransform = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasMatrix = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasCharacter = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagMove = 0 != bs.GetBits(1) ? true : false;
            bs.GetBits(3); // Reserved
            this._PlaceFlagHasImage = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasClassName = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasCacheAsBitmap = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasBlendMode = 0 != bs.GetBits(1) ? true : false;
            this._PlaceFlagHasFilterList = 0 != bs.GetBits(1) ? true : false;
            bs.Reset();

            if (!(_PlaceFlagMove || _PlaceFlagHasCharacter))
            {
                SwfFormatException e = new SwfFormatException("Object is neither a creation (PlaceFlagHasCharacter) nor an update (PlaceFlagMove).");
                Log.Error(this, e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);
            BitStream bs = new BitStream(this._dataStream);

            this.ParseFlags(br, bs);

            this._Depth = br.ReadUInt16();

            if (this._PlaceFlagHasClassName)
            {
                this._ClassName = SwfStrings.SwfString(this._SwfVersion, br);
            }

            if (this._PlaceFlagHasCharacter)
            {
                this._CharacterID = br.ReadUInt16();
            }

            if (this._PlaceFlagHasMatrix)
            {
                Matrix m = new Matrix(this._SwfVersion);
                m.Parse(this._dataStream);
                this._TransformMatrix = m;
            }

            if (this._PlaceFlagHasColorTransform)
            {
                CxFormWithAlpha cx = new CxFormWithAlpha(this._SwfVersion);
                cx.Parse(this._dataStream);
                this._CxFormWithAlpha = cx;
            }

            if (this._PlaceFlagHasRatio)
            {
                this._Ratio = br.ReadUInt16();
            }

            if (this._PlaceFlagHasName)
            {
                this._Name = SwfStrings.SwfString(this._SwfVersion, br);
            }

            if (this._PlaceFlagHasClipDepth)
            {
                this._Depth = br.ReadUInt16();
            }

            if (this._PlaceFlagHasFilterList)
            {
                FilterList fl = new FilterList(this._SwfVersion);
                fl.Parse(this._dataStream);
                this._SurfaceFilterList = fl;
            }

            if (this._PlaceFlagHasBlendMode)
            {
                this._BlendMode = (BlendMode)br.ReadByte();
            }

            if (this._PlaceFlagHasCacheAsBitmap)
            {
                this._BitmapCache = br.ReadByte();
            }

            if (this._PlaceFlagHasClipActions)
            {
                ClipActions ca = new ClipActions(this._SwfVersion);
                ca.Parse(this._dataStream);
                this._ClipActions = ca;
            }
        }

        /// <summary>
        /// Writes the flags of this object to a stream.
        /// </summary>
        /// <param name="bw">A binary writer</param>
        /// <param name="bs">A bitstream</param>
        private void WriteFlags(BinaryWriter bw, BitStream bs)
        {
            bs.WriteBits(1, false == this._PlaceFlagHasClipActions ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasClipDepth ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasName ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasRatio ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasColorTransform ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasMatrix ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasCharacter ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagMove ? 0 : 1);
            bs.WriteBits(3, 0);//Reserved
            bs.WriteBits(1, false == this._PlaceFlagHasImage ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasClassName ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasCacheAsBitmap ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasBlendMode ? 0 : 1);
            bs.WriteBits(1, false == this._PlaceFlagHasFilterList ? 0 : 1);
        }

        /// <summary>
        /// Writes the fields of this object to a stream.
        /// </summary>
        /// <param name="bw">A binary writer</param>
        /// <param name="bs">A bitstream</param>
        /// <param name="output">The output stream to write to</param>
        private void WriteFields(BinaryWriter bw, BitStream bs, Stream output)
        {
            bw.Write(this._Depth);

            if (this._PlaceFlagHasClassName)
            {
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._ClassName);
            }

            if (this._PlaceFlagHasCharacter)
            {
                bw.Write(this._CharacterID);
            }

            if (this._PlaceFlagHasMatrix)
            {
                this._TransformMatrix.Write(output);
            }

            if (this._PlaceFlagHasColorTransform)
            {
                this._CxFormWithAlpha.Write(output);
            }

            if (this._PlaceFlagHasRatio)
            {
                bw.Write(this._Ratio);
            }

            if (this._PlaceFlagHasName)
            {
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._Name);
            }

            if (this._PlaceFlagHasClipDepth)
            {
                bw.Write(this._Depth);
            }

            if (this._PlaceFlagHasFilterList)
            {
                this._SurfaceFilterList.Write(output);
            }

            if (this._PlaceFlagHasBlendMode)
            {
                bw.Write((Byte)this._BlendMode);
            }

            if (this._PlaceFlagHasCacheAsBitmap)
            {
                bw.Write(this._BitmapCache);
            }

            if (this._PlaceFlagHasClipActions)
            {
                this._ClipActions.Write(output);
            }
        }

        /// <summary>
        /// Writes this object to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            BitStream bs = new BitStream(output);

            this.WriteTagHeader(output);

            this.WriteFlags(bw, bs);

            this.WriteFields(bw, bs, output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._CharacterID);
            return sb.ToString();
        }
    }
}
