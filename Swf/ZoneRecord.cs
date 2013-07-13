using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// </summary>
    public class ZoneRecord : AbstractSwfElement
    {
        private byte _numZoneData;
        private List<ZoneData> _zoneData;
        private bool _zoneMaskY;
        private bool _zoneMaskX;

        /// <summary>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ZoneRecord(byte InitialVersion) : base(InitialVersion)
        {
            this._zoneData = new List<ZoneData>();
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public ulong Length
        {
            get
            {
                return sizeof(byte) * 2 + (UInt64)this._zoneData.Count * sizeof(double) * 2;
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
            BinaryReader br = new BinaryReader(input);

            this._numZoneData = br.ReadByte();

            ZoneData temp = null;

            for (int i = 0; i < this._numZoneData; i++)
            {
                temp = new ZoneData(this._SwfVersion);
                temp.Parse(input);
                this._zoneData.Add(temp);
            }

            BitStream bits = new BitStream(input);
            bits.GetBits(6); //reserved
            this._zoneMaskY = ((0 != bits.GetBits(1)) ? true : false);
            this._zoneMaskX = ((0 != bits.GetBits(1)) ? true : false);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            if (this._zoneData.Count != (Int32)this._numZoneData)
            {
                SwfFormatException e = new SwfFormatException("The count of List<ZoneData> and the byte value of zone data differs. ");
               Log.Error(this, e.Message);
                throw e;
            }

            output.WriteByte(this._numZoneData);

            for (int i = 0; i < this._zoneData.Count; i++)
            {
                this._zoneData[i].Write(output);
            }

            BitStream bits = new BitStream(output);
            bits.WriteBits(6, 0); //reserved
            bits.WriteBits(1, Convert.ToInt32(this._zoneMaskY));
            bits.WriteBits(1, Convert.ToInt32(this._zoneMaskX));
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
