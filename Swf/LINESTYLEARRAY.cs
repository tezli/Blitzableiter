using System;
using System.Collections.Generic;
using System.IO;
using Recurity.Swf.Interfaces;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// An array of line styles
    /// </summary>
    public class LineStyleArray : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected byte _lineStyleCount;

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _lineStyleCountExtended;

        /// <summary>
        /// 
        /// </summary>
        protected List<LineStyle> _lineStyles;

        /// <summary>
        /// An array of line styles
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this objct</param>
        public LineStyleArray(byte InitialVersion) : base(InitialVersion)
        {
            this._lineStyles = new List<LineStyle>();
        }


        /// <summary>
        /// 
        /// </summary>
        public List<LineStyle> LineStyles
        {
            get
            {
                return this._lineStyles;
            }
            set
            {
                this._lineStyles = value;
            }
        }

        /// <summary>
        /// Gets the number of FILLSTYLES
        /// </summary>
        public virtual UInt16 Count
        {
            get
            {
                if (this._lineStyleCount == 0xFF)
                {
                    return this._lineStyleCountExtended;
                }
                else
                {
                    return Convert.ToUInt16(this._lineStyleCount);
                }
            }

        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public virtual ulong Length
        {
            get
            {
                UInt64 length = 0;

                if (this._lineStyleCount.Equals(0xFF))
                {
                    length += sizeof(byte);
                    length += sizeof(UInt16);
                }
                else
                {
                    length += sizeof(byte);
                }
                foreach (LineStyle l in this._lineStyles)
                {
                    length += l.Length;
                }

                return length * 8;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public virtual void Parse(Stream input, TagTypes caller)
        {
            BinaryReader br = new BinaryReader(input);

            this._lineStyleCount = br.ReadByte();

            if (caller.Equals(TagTypes.DefineShape4))
            {
                if (this._lineStyleCount.Equals(0xFF))
                {
                    this._lineStyleCountExtended = br.ReadUInt16();
                    LineStyle2 temp = null;

                    for (byte i = 0; i < this._lineStyleCountExtended; i++)
                    {
                        temp = new LineStyle2(this._SwfVersion);
                        try
                        {
                            temp.Parse(input, caller);
                        }
                        catch (SwfFormatException e)
                        {
                            throw e;
                        }
                        this._lineStyles.Add(temp);
                    }
                }
                else
                {
                    LineStyle2 temp = null;

                    for (byte i = 0; i < this._lineStyleCount; i++)
                    {
                        temp = new LineStyle2(this._SwfVersion);

                        try
                        {
                            temp.Parse(input, caller);
                        }
                        catch (SwfFormatException e)
                        {
                            throw e;
                        }

                        this._lineStyles.Add(temp);
                    }
                }
            }
            else
            {
                if (this._lineStyleCount.Equals(0xFF))
                {
                    this._lineStyleCountExtended = br.ReadUInt16();
                    LineStyle temp = null;

                    for (byte i = 0; i < this._lineStyleCountExtended; i++)
                    {
                        temp = new LineStyle(this._SwfVersion);

                        try
                        {
                            temp.Parse(input, caller);
                        }
                        catch (SwfFormatException e)
                        {
                            throw e;
                        }

                        this._lineStyles.Add(temp);
                    }
                }
                else
                {

                    for (byte i = 0; i < this._lineStyleCount; i++)
                    {
                        LineStyle temp = new LineStyle(this._SwfVersion);

                        try
                        {
                            temp.Parse(input, caller);
                        }
                        catch (SwfFormatException e)
                        {
                            throw e;
                        }
                        
                        this._lineStyles.Add(temp);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public virtual void Write(Stream output)
        {
            if (this._lineStyleCount.Equals(0xFF))
            {
                output.WriteByte(this._lineStyleCount);
                byte[] countExtended = BitConverter.GetBytes(this._lineStyleCountExtended);
                output.Write(countExtended, 0, 2);

                for (UInt16 i = 0; i < _lineStyleCountExtended; i++)
                {
                    this._lineStyles[i].Write(output);
                }
            }

            else
            {
                output.WriteByte(this._lineStyleCount);

                for (byte i = 0; i < _lineStyleCount; i++)
                {
                    this._lineStyles[i].Write(output);
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

            if (this._lineStyleCount == 0xFF)
            {
                sb.AppendFormat(" Number of LINETSYLES(counted) : {0:d}, Number of LINETSYLES(parsed) : {1:d} ", this.Count, this._lineStyleCountExtended);
            }
            else
            {
                sb.AppendFormat(" Number of LINETSYLES(counted) : {0:d}, Number of LINETSYLES(parsed) : {1:d} ", this.Count, this._lineStyleCount);
            }

            return sb.ToString();
        }
    }
}
