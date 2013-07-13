using System.IO;
using System.Text;
using System;

namespace Recurity.Swf
{
    /// <summary>
    /// The GRADRECORD defines a gradient control point.
    /// </summary>
    public class GradRecord : AbstractSwfElement
    {
        private byte _ratio;
        private Rgb _color;

        /// <summary>
        /// The GRADRECORD defines a gradient control point.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public GradRecord( byte InitialVersion ) : base( InitialVersion ) 
        {

        }

        /// <summary>
        /// The length of this object
        /// </summary>
        public ulong Length
        {
            get
            {
                return sizeof(byte) * 4;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true; // nothing to verify here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public void Parse( Stream input, TagTypes caller )
        {
            BinaryReader br = new BinaryReader(input);
            this._ratio = br.ReadByte();

            if (caller.Equals(TagTypes.DefineShape3) || caller.Equals(TagTypes.DefineShape4))
            {
                this._color = new Rgba(this._SwfVersion);
                this._color.Parse(input);
            }
            else
            {
                this._color = new Rgb(this._SwfVersion);
                this._color.Parse(input);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            output.WriteByte( this._ratio );
            this._color.Write( output );
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.ToString() );
            return sb.ToString();
        }

    }
}
