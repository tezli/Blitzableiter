using System;
using Recurity.Swf;
using System.Collections.Generic;
using System.IO;
using Recurity.Swf.Interfaces;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// A fill style array enumerates a number of fill styles.
    /// </summary>
    public class FillStyleArray : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected byte _fillStyleCount;


        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _fillStyleCountExtended; // DefineShape2 and DefineShape3 only

        /// <summary>
        /// 
        /// </summary>
        protected List<FillStyle> _fillStyles;

        /// <summary>
        /// A fill style array enumerates a number of fill styles.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public FillStyleArray( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._fillStyles = new List<FillStyle>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual List<FillStyle> FillStyles
        {
            get
            {
                return this._fillStyles;
            }
            set
            {
                this._fillStyles = value;
            }
        }

        /// <summary>
        /// Gets the number of FILLSTYLES
        /// </summary>
        public virtual UInt16 Count
        {
            get
            {
                if (this._fillStyleCount == 0xFF)
                {
                    return this._fillStyleCountExtended;
                }
                else
                {
                    return Convert.ToUInt16(this._fillStyleCount);
                }
            }

        }

        /// <summary>
        /// The length of this object.
        /// </summary>
        public virtual ulong Length
        {
            get
            {
                UInt64 length = 0;

                if (this._fillStyleCount.Equals(0xFF))
                {
                    length += sizeof(byte);
                    length += sizeof(UInt16);
                }
                else
                {
                    length += sizeof(byte);
                }

                foreach ( FillStyle f in this._fillStyles )
                {
                    length += f.Length;
                }

                return length;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true; // nothing to do here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public virtual void Parse( Stream input, TagTypes caller )
        {
            BinaryReader br = new BinaryReader(input);

            this._fillStyleCount = br.ReadByte();

            if (this._fillStyleCount.Equals(0xFF)) 
            {
                if (caller.Equals(TagTypes.DefineShape2) || caller.Equals(TagTypes.DefineShape3))
                {
                    this._fillStyleCountExtended = br.ReadUInt16();

                    //Log.InfoFormat("{0}(0x{0:x4}) fillstyles will be parsed", (int)this._fillStyleCountExtended);

                    for (UInt16 i = 0; i < this._fillStyleCountExtended; i++)
                    {
                        FillStyle temp = new FillStyle(this._SwfVersion);
                        try
                        {
                            temp.Parse(input, caller);
                            this._fillStyles.Add(temp);
                        }
                        catch (SwfFormatException e)
                        {
                           Log.Error(this, e.Message);
                        }
                    }
                }
                else
                {
                    SwfFormatException e = new SwfFormatException("Extended count of fill styles supported only for Shape2 and Shape3.");
                   Log.Error(this, e.Message);
                    throw e;
                }
            }
            else
            {
                //Log.InfoFormat("{0}(0x{0:x4}) fillstyles will be parsed", (int)this._fillStyleCount);

                for (byte i = 0; i < this._fillStyleCount; i++)
                {
                    FillStyle temp = new FillStyle(this._SwfVersion);
                    try
                    {
                        temp.Parse(input, caller);
                    }
                    catch (SwfFormatException e)
                    {
                        throw e;
                    }
                    
                    this._fillStyles.Add( temp);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public virtual void Write(Stream output)
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
        /// 
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }

    }
}
