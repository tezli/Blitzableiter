using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Blitzableiter.SWF
{
    /// <summary>
    /// This class offers access to the EncodedU32 format of the SWF specification.
    /// It defines an unsigned Iteger as a sequence of 1 to 5 bytes. The most significant bit
    /// of a byte indicates if the next byte is also part of the value. The byte are ordered
    /// low to high.
    /// </summary>
    class EncodedU32
    {
        /// <summary>
        /// Calculates the number of bytes used to encode a ulong value
        /// </summary>
        /// <param name="value">value to calculate the size of</param>
        /// <returns>Number of bytes need to encode the value</returns>
        public static int EncodedSizeOf(ulong value)
        {
            int numBytes = 0;
            while (value > 0)
            {
                value = value >> 7;
                numBytes++;
                if (value == 1)
                    break;
            }

            return numBytes > 0 ? numBytes : 1;
        }

        /// <summary>
        /// Decodes the byte representation of an ulong value to the ulong value itself
        /// </summary>
        /// <param name="data">byte representation</param>
        /// <returns>ulong value</returns>
        public static ulong Decode(byte[] data)
        {
            ulong result = 0;

            for (int i = 0; i < data.Length - 1; i++)
                result = result + (((ulong)data[i] & 127) << (i * 7));
            result += (ulong)data[data.Length - 1] << ((data.Length - 1) * 7);

            return result;
        }

        /// <summary>
        /// Reads an encoded ulong from a given stream
        /// </summary>
        /// <param name="source">Stream to read the value from</param>
        /// <returns>byte representation of the value (1-5 bytes)</returns>
        public static byte[] Read(Stream source)
        {
            List<byte> bytes = new List<byte>();

            BinaryReader br = new BinaryReader(source);
            byte b;

            do
            {
                b = br.ReadByte();
                bytes.Add(b);
                if (bytes.Count == 5)
                    break;
            } while ((b & 128) == 128);

            return bytes.ToArray();
        }

        /// <summary>
        /// Encodes a value of type ulong to a byte array.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>byte respesentation of the value</returns>
        public static byte[] Encode(ulong value)
        {
            byte[] result = new byte[EncodedSizeOf(value)];
            for (int i = 0; i < result.Length - 1; i++)
                result[i] = (byte)(128 + ((value >> (i * 7)) & 127));
            result[result.Length - 1] = (byte)((value >> ((result.Length - 1) * 7)) & 255);
            return result;
        }

        /// <summary>
        /// Writes an encoded ulong value to a given Stream
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <param name="target">Stream to write the value to</param>
        public static void Write(ulong value, Stream target)
        {
            byte[] data = Encode(value);
            for (int i = 0; i < data.Length; i++)
                target.WriteByte(data[i]);
        }


    }
}
