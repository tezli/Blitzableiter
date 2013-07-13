using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The advanced text rendering engine uses alignment zones to establish </para>
    /// <para>the borders of a glyph for pixel snapping. </para>
    /// <para>Alignment zones are critical for high-quality display of fonts.</para>
    /// </summary>
    public class ZoneData : AbstractSwfElement
    {
        private double _alignmentCoordinate;
        private double _range;

        /// <summary>
        /// <para>The advanced text rendering engine uses alignment zones to establish </para>
        /// <para>the borders of a glyph for pixel snapping.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ZoneData(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
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
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        public void Parse(Stream input)
        {
            BitStream bits = new BitStream(input);

            bits.GetBitsFB(16, out this._alignmentCoordinate);
            bits.GetBitsFB(16, out this._range);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            bits.WriteBitsFB(16, this._alignmentCoordinate);
            bits.WriteBitsFB(16, this._range);
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
    }
}
