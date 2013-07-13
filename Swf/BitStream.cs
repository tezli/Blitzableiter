using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class BitStream
    {

        /// <summary>
        /// 
        /// </summary>
        private Stream _backend;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _bitPos;

        /// <summary>
        /// 
        /// </summary>
        private byte _currentByte;

        /// <summary>
        /// 
        /// </summary>
        public BitStream()
        {
            this._backend = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public BitStream(Stream input)
        {
            this._backend = input;
            this._bitPos = 0;            
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Position
        {
            get
            {
                return this._bitPos;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerOfBits"></param>
        /// <returns></returns>
        public UInt32 GetBits(UInt32 numerOfBits)
        {
            UInt32 ret = 0;

            for (uint i = 0; i < numerOfBits; i++)
            {
                if (0 == _bitPos)
                {
                    int backendInput = _backend.ReadByte();
                    if (-1 == backendInput)
                        throw new EndOfStreamException();
                    _currentByte = (byte)backendInput;
                }

                byte mask = (byte)(0x80 >> (int)_bitPos);
                int shift = (7 - (int)_bitPos);
                byte bit = (byte)((_currentByte & mask) >> shift);
                ret = (ret << 1) | bit;

                _bitPos = ((_bitPos + 1) % 8);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfBits"></param>
        /// <returns></returns>
        public Int32 GetBitsSigned(UInt32 numberOfBits)
        {
            if (0 == numberOfBits)
                return 0;

            UInt32 usig = this.GetBits(numberOfBits);
            UInt32 mask = unchecked((UInt32)(long)(~0) << (int)numberOfBits);
            Int32 ret;

            if (0 != ((long)usig >> (int)(numberOfBits - 1)))
            {
                ret = unchecked((Int32)((long)usig | (long)mask));
            }
            else
            {
                ret = (Int32)usig;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfBits"></param>
        /// <param name="result"></param>
        public void GetBitsFB(UInt32 numberOfBits, out double result)
        {
            //
            // See FBtoInt32 for details
            //
            Int32 raw = this.GetBitsSigned(numberOfBits);
            UInt32 fraction = unchecked((UInt32)raw & 0xFFFF);
            double fract2 = (double)fraction / (double)0xFFFF;

            double h = raw >> 16;
            result = h + fract2;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            _bitPos = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Int32 FBtoInt32(double source)
        {
            //
            // The Swf FB[nBit] Story:
            //
            // FBs are documented as 16.16Bit values with variable length. 
            // The actual implementation uses a signed 32Bit value first. That 
            // value (decoded, in Int32) is split at the 16 Bit boundary. The lower
            // part is the fraction of 1.0, as seen from left to right, e.g. 0x8000 
            // would be 0.5, 0xC000 would be 0.25 and so on.
            // See: http://de.wikipedia.org/wiki/Dualsystem (and don't use the English page)
            //
            // The following code uses a lot of local variables for debugging purposes.
            // The code may get optimized if required.
            //

            double abs = Math.Floor(source);
            double frac = source - abs;
            double frac2 = frac * (double)0xFFFF;
            double frac3 = Math.Round(frac2);
            UInt16 frac4 = (UInt16)frac3;
            Int16 major = (Int16)abs;
            Int32 final = unchecked((major << 16) | frac4);

            return final;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int CountNeededBitsFB(double source)
        {
            Int32 i = FBtoInt32(source);
            int result = this.CountNeededBits(i);
            return result;
        }

        /// <summary>
        /// Signed integer Bit counting
        /// </summary>
        /// <param name="source">A signed int</param>
        /// <returns>Number of bits needed</returns>
        public int CountNeededBits(int source)
        {
            if (0 == source)
            {
                return 0;
            }
            else if (0 > source)
            {
                ulong fuckedUp = (ulong)(~source);

                return (this.CountNeededBits(fuckedUp) + 1);
            }
            else
            {
                ulong s2 = (ulong)source;

                return (this.CountNeededBits(s2) + 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public int CountMaximumBits(params int[] args)
        {
            int largest = 0;
            for (int i = 0; i < args.Length; i++)
            {
                int j = this.CountNeededBits(args[i]);
                if (j > largest)
                    largest = j;
            }
            return largest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int CountNeededBits(ulong source)
        {
            int highestBit = 0;

            if (0 == source)
            {
                return 0;
            }

            for (int i = 0; i < (sizeof(ulong) * 8); i++)
            {
                if (((source >> i) & 0x01) != 0)
                {
                    highestBit = i;
                }
            }

            return (highestBit + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public int CountMaximumBits(params UInt32[] args)
        {
            int largest = 0;
            for (int i = 0; i < args.Length; i++)
            {
                int j = this.CountNeededBits(args[i]);
                if (j > largest)
                    largest = j;
            }
            return largest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfBits"></param>
        /// <param name="value"></param>
        public void WriteBitsFB(int numberOfBits, double value)
        {
            Int32 i = FBtoInt32(value);
            this.WriteBits(numberOfBits, i);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="numberOfBits"></param>
        /// <param name="data"></param>
        public void WriteBits(int numberOfBits, int data)
        {
            ulong dataAsLong = unchecked((ulong)(data));

            this.WriteBits(numberOfBits, dataAsLong);
        }

        /// <summary>
        /// Fails for the following szenario : bits.WriteBits( 3 , 1 ), RGB.Write() RGB is #ff
        /// bits would look like following : 00000000 11111111 11111111 11111111
        /// </summary>
        /// <param name="numberOfBits"></param>
        /// <param name="data"></param>
        public void WriteBits(int numberOfBits, ulong data)
        {

            if (numberOfBits < 0)
                throw new ArgumentOutOfRangeException("numberOfBits < 1");

            if (0 == numberOfBits)
                return;

            for (int i = (numberOfBits - 1); i >= 0; i--)
            {
                byte bit = (byte)((data >> i) & 0x01);
                _currentByte = (byte)((_currentByte << 1) | bit);

                if (7 == _bitPos)
                {
                    _backend.WriteByte(_currentByte);
                    _currentByte = 0;
                }

                _bitPos = ((_bitPos + 1) % 8);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteFlush()
        {
            if (0 != _bitPos)
            {
                _currentByte = (byte)(_currentByte << (int)(8 - _bitPos));
                _backend.WriteByte(_currentByte);
                _bitPos = 0;
            }
            _currentByte = 0;
        }
    }
}
