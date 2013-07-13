using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A morph line style array enumerates a number of fill styles.
    /// </summary>
    public class MorphLineStyleArray : LineStyleArray
    {
        private new byte _lineStyleCount;
        private new UInt16 _lineStyleCountExtended;
        private new List<MorphLineStyle> _lineStyles;

        /// <summary>
        /// A morph line style array enumerates a number of fill styles.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphLineStyleArray(byte InitialVersion)
            : base(InitialVersion)
        {
            this._lineStyles = new List<MorphLineStyle>();
        }

        /// <summary>
        /// The number of MorphLineStyles
        /// </summary>
        public override UInt16 Count
        {
            get
            {
                return (UInt16)this._lineStyles.Count;
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

            this._lineStyleCount = br.ReadByte();

            if (caller.Equals(TagTypes.DefineMorphShape2))
            {
                if (this._lineStyleCount.Equals(0xFF))
                {
                    this._lineStyleCountExtended = br.ReadUInt16();

                    for (UInt16 i = 0; i < this._lineStyleCountExtended; i++)
                    {
                        MorphLineStyle2 temp = new MorphLineStyle2(this._SwfVersion);
                        temp.Parse(input);
                        this._lineStyles.Add(temp);
                    }
                }
                else
                {

                    for (byte i = 0; i < this._lineStyleCount; i++)
                    {
                        MorphLineStyle2 temp = new MorphLineStyle2(this._SwfVersion);
                        temp.Parse(input);
                        this._lineStyles.Add(temp);
                    }

                }
            }
            else if (caller.Equals(TagTypes.DefineMorphShape))
            {
                if (this._lineStyleCount.Equals(0xFF))
                {
                    this._lineStyleCountExtended = br.ReadUInt16();

                    for (UInt16 i = 0; i < this._lineStyleCountExtended; i++)
                    {
                        MorphLineStyle temp = new MorphLineStyle(this._SwfVersion);
                        temp.Parse(input);
                        this._lineStyles.Add(temp);
                    }
                }
                else
                {
                    MorphLineStyle temp = null;

                    for (byte i = 0; i < this._lineStyleCount; i++)
                    {
                        temp = new MorphLineStyle(this._SwfVersion);
                        temp.Parse(input);
                        this._lineStyles.Add(temp);
                    }
                }
            }
            else
            {
                Exception e = new Exception(" Only DefineMorphShape2 and DefineMorphShape can access MorphLineStyleArray ");
               Log.Error(this, e.Message);
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            output.WriteByte(this._lineStyleCount);

            if (this._lineStyleCount.Equals(0xFF))
            {
                bw.Write(this._lineStyleCountExtended);
            }

            for (int i = 0; i < this._lineStyles.Count; i++)
            {
                this._lineStyles[i].Write(output);
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
