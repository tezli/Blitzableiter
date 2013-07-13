using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineFont3 tag extends the functionality of DefineFont2 by expressing the SHAPE
    /// coordinates in the GlyphShapeTable at 20 times the resolution.
    /// </summary>
    /// <remarks>
    /// The DefineFont3 tag is introduced along with the DefineFontAlignZones tag in Swf 8. The
    /// DefineFontAlignZones tag is optional but recommended for Swf files using advanced antialiasing,
    /// and it modifies the DefineFont3 tag.
    /// The DefineFont3 tag extends the functionality of DefineFont2 by expressing the SHAPE
    /// coordinates in the GlyphShapeTable at 20 times the resolution. All the EMSquare coordinates
    /// are multiplied by 20 at export, allowing fractional resolution to 1/20 of a unit. This allows for
    /// more precisely defined glyphs and results in better visual quality.
    /// </remarks>
    public class DefineFont3 : DefineFont2
    {
        /// <summary>
        /// <para>The DefineFont3 tag extends the functionality of DefineFont2 by expressing the SHAPE</para>
        /// <para>coordinates in the GlyphShapeTable at 20 times the resolution.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineFont3(byte InitialVersion)
            : base(InitialVersion)
        {

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
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                uint ret = 0;
                using (MemoryStream temp = new MemoryStream())
                {
                    base.WriteFlags(temp);

                    this.WriteCodeTable(temp);

                    base.WriteLayout(temp);

                    ret = (uint)temp.Position;
                }
                return ret;
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
        /// 
        /// </summary>
        protected override void Parse()
        {
            try
            {
                base.ParseFlags(this._dataStream);

            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                base._fontFlagsWideCodes = true; // Must be 1

            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                this.ParseCodeTable(this._dataStream);

            }
            catch (Exception e)
            {
                throw e;
            }
            try
            {
                if (this._dataStream.Position < this._dataStream.Length)
                {
                    base._fontFlagsHasLayout = true;
                    ParseLayout(this._dataStream);

                }
                else
                {
                    this._fontFlagsHasLayout = false;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {

            this.WriteTagHeader(output);

            base.WriteFlags(output);

            this.WriteCodeTable(output);

            base.WriteLayout(output);
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
        /// 
        /// </summary>
        /// <param name="input"></param>
        protected override void ParseCodeTable(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._wideCodeTable = new UInt16[this._numberOfGlyphs];

            for (int i = 0; i < this._numberOfGlyphs; i++)
            {
                this._wideCodeTable[i] = br.ReadUInt16();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected override void WriteCodeTable(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            for (int i = 0; i < this._wideCodeTable.Length; i++)
            {
                bw.Write(this._wideCodeTable[i]);
            }

        }

    }
}
