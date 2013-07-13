using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A morph fill style array enumerates a number of fill styles.
    /// </summary>
    public class MorphFillStyleArray : FillStyleArray
    {
        private new  byte _fillStyleCount;
        private new UInt16 _fillStyleCountExtended;
        private new MorphFillStyle[] _fillStyles;

        /// <summary>
        /// A morph fill style array enumerates a number of fill styles.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphFillStyleArray(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The number of MorphFillStyles
        /// </summary>
        public override UInt16 Count
        {
            get
            {
                return (UInt16)this._fillStyles.Length;
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
                return 0;
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
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public override void Parse(Stream input, TagTypes caller)
        {
            BinaryReader br = new BinaryReader(input);

            this._fillStyleCount = br.ReadByte();

            if (this._fillStyleCount.Equals(0xFF))
            {
                this._fillStyleCountExtended = br.ReadUInt16();

                this._fillStyles = new MorphFillStyle[this._fillStyleCountExtended];

                for (UInt16 i = 0; i < this._fillStyleCountExtended; i++)
                {
                    MorphFillStyle temp = new MorphFillStyle(this._SwfVersion);
                    temp.Parse(input);
                    this._fillStyles[i] = temp;
                }
            }
            else
            {
                this._fillStyles = new MorphFillStyle[this._fillStyleCount];

                for (byte i = 0; i < this._fillStyleCount; i++)
                {
                    MorphFillStyle temp = new MorphFillStyle(this._SwfVersion);
                    temp.Parse(input);
                    this._fillStyles[i] = temp;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            if (this._fillStyleCount.Equals(0xFF))
            {
                output.WriteByte(this._fillStyleCount);

                byte[] countExtended = BitConverter.GetBytes(this._fillStyleCountExtended);
                output.Write(countExtended, 0, 2);

                for (UInt16 i = 0; i < this._fillStyleCountExtended; i++)
                {
                    this._fillStyles[i].Write(output);
                }
            }

            else
            {
                output.WriteByte(this._fillStyleCount);

                for (byte i = 0; i < this._fillStyleCount; i++)
                {
                    this._fillStyles[i].Write(output);
                }
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }
    }
}
