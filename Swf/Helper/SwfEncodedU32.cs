using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// This class offers access to the EncodedU32 format of the Swf specification.
    /// It defines an unsigned Iteger as a sequence of 1 to 5 bytes. The most significant bit
    /// of a byte indicates if the next byte is also part of the value. The byte are ordered
    /// low to high.
    /// </summary>
    class SwfEncodedU32
    {
        /// <summary>
        /// Calculates the number of bytes used to encode a ulong value
        /// </summary>
        /// <param name="value">value to calculate the size of</param>
        /// <returns>Number of bytes need to encode the value</returns>
        public static int SwfEncodedSizeOf(ulong value)
        {
            int numBytes = 0;
            while (value > 0)                       // While the size of the value is greater than the
            {                                       // current number of bytes can represent
                value = value >> 7;                 // Reduce the representation of the value by 7 bit
                numBytes++;                         // that are stored is one byte
                if (value == 1)                     // If the value of the current byte is one
                    break;                          // no other byte needed
            }

            return numBytes > 0 ? numBytes : 1;     // At least one byte is used to represent a value
        }

        /// <summary>
        /// Encodes a value of type ulong to a byte array.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>byte respesentation of the value</returns>
        private static byte[] SwfEncodeU32(ulong value)
        {
            byte[] result = new byte[SwfEncodedSizeOf(value)];                              // allocate a byte array to save the encoded values
            for (int i = 0; i < result.Length - 1; i++)                                     // for each byte of the encoded value
                result[i] = (byte)(128 + ((value >> (i * 7)) & 127));                       // the msb(128) must be set but it is part of the value
            result[result.Length - 1] = (byte)((value >> ((result.Length - 1) * 7)) & 255); // the last byte contains 8 bit of the value
            return result;
        }

        /// <summary>
        /// Decodes the byte representation of an ulong value to the ulong value itself
        /// </summary>
        /// <param name="data">byte representation</param>
        /// <returns>ulong value</returns>
        private static ulong SwfDecodeU32(byte[] data)
        {
            ulong result = 0;
            for (int i = 0; i < data.Length - 1; i++)                           // for each byte of the encoded value
                result = result + (((ulong)data[i] & 127) << (i * 7));          // seven bit are added to the result
            result += (ulong)data[data.Length - 1] << ((data.Length - 1) * 7);  // except the last byte which contains eight bits                                                               
            return result;                                                      // of the result
        }

        /// <summary>
        /// Reads an encoded ulong from a given stream
        /// </summary>
        /// <param name="source">Stream to read the value from</param>
        /// <returns>byte representation of the value (1-5 bytes)</returns>
        private static byte[] SwfReadEncodedU32Data(BinaryReader source)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            do
            {
                b = source.ReadByte();      // Read one byte from the stream
                bytes.Add(b);               // and add it to the result
                if (bytes.Count == 5)       // if the max. Limit of five bytes is reached
                    break;                  // stop reading any further bytes
            } while ((b & 128) == 128);     // if the msb(128) is set the next byte is part
                                            // of the value
            return bytes.ToArray();         // return the bytes read
        }

        /// <summary>
        /// Reads an ulong value encoded as EncodedU32 from stream
        /// </summary>
        /// <param name="source">Stream to read the data</param>
        /// <returns>decoded value</returns>
        public static ulong SwfReadEncodedU32(BinaryReader source)
        {
            byte[] encodedData = SwfReadEncodedU32Data(source);
            return SwfDecodeU32(encodedData);
        }

        /// <summary>
        /// Writes an encoded ulong value to a given Stream
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <param name="target">Stream to write the value to</param>
        public static void SwfWriteEncodedU32(ulong value, BinaryWriter target)
        {
            byte[] data = SwfEncodeU32(value);
            target.Write(data);
        }

    }
}
