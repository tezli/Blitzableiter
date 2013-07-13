using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// The end shape record simply indicates the end of the shape record array.
    /// </summary>
    public class EndShapeRecord : ShapeRecord
    {

        /// <summary>
        /// The end shape record simply indicates the end of the shape record array.
        /// </summary>
        /// <param name="InitialVersion">The version of the swf file using this object.</param>
        public EndShapeRecord(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        public override void Parse(Stream input, BitStream bits)
        {
            bits.GetBits(6);
            bits.WriteFlush();
        }

        /// <summary>
        /// The length of this object in BITS!.
        /// </summary>
        public override uint Length
        {
            get
            {
                return 6;
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
        /// <param name="output"></param>
        /// <param name="bits"></param>
        public override void Write(Stream output, BitStream bits)
        {
            bits.WriteBits(6, 0);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("ENDSHAPERECORD found");
            return sb.ToString();
        }
    }
}
