using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// TODO : Documentation
    /// </summary>
    public class Rect : AbstractSwfElement
    {
        private UInt32 _bits_per_entry;     // max: 11111 = 31 
        private Int32 _x_min;
        private Int32 _y_min;
        private Int32 _x_max;
        private Int32 _y_max;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Rect( byte InitialVersion ) : base( InitialVersion ) { }

        /*
        public UInt32 NBits
        {
            get { return _bits_per_entry; }
            set { _bits_per_entry = value; }
        }
         */

        /// <summary>
        /// 
        /// </summary>
        public Int32 Xmin
        {
            get { return _x_min; }
            set { _x_min = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Ymin
        {
            get { return _y_min; }
            set { _y_min = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Xmax
        {
            get { return _x_max; }
            set { _x_max = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Ymax
        {
            get { return _y_max; }
            set { _y_max = value; }
        }

        /// <summary>
        /// TODO : Documentation
        /// </summary>
        /// <param name="source"></param>
        public void Parse( Stream source )
        {
            BitStream bits = new BitStream(source);

            if (source.Length < 1)
            {
                throw new ArgumentException("Source does not contain enough data. Length = " + source.Length.ToString("d"));
            }
            else
            {
                //
                // The first 5 bits declare how many bits per entry are used
                //
                _bits_per_entry = bits.GetBits(5);
            }
            _x_min = bits.GetBitsSigned( _bits_per_entry );
            _x_max = bits.GetBitsSigned( _bits_per_entry );
            _y_min = bits.GetBitsSigned( _bits_per_entry );
            _y_max = bits.GetBitsSigned( _bits_per_entry );            
        }

        /// <summary>
        /// TODO : Documentation
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            BitStream bits = new BitStream( output );

            int bitsNeeded = bits.CountMaximumBits(_x_min, _x_max, _y_min, _y_max);

            bits.WriteBits(5, (ulong)bitsNeeded);
            // reassign, so we don't work with stale data
            _bits_per_entry = (uint)bitsNeeded;

            bits.WriteBits(bitsNeeded, _x_min);
            bits.WriteBits(bitsNeeded, _x_max);
            bits.WriteBits(bitsNeeded, _y_min);
            bits.WriteBits(bitsNeeded, _y_max);
            bits.WriteFlush();
        }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public uint Length
        {
            get
            {
                uint ret = 0;

                using ( MemoryStream temp = new MemoryStream() )
                {
                    this.Write( temp );
                    ret = ( uint )temp.Length;
                }
                return ret;
            }
        }

        /// <summary>
        ///         /// TODO : Documentation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            BitStream bits = new BitStream();
            StringBuilder sb = new StringBuilder();
            sb.Append( "RECT " );
            sb.AppendFormat( "{0:d}Bits/entry, ", bits.CountMaximumBits( _x_min, _x_max, _y_min, _y_max ) );
            sb.AppendFormat( "Xmin {0:d} / Xmax {1:d} / Ymin {2:d} / Ymax {3:d}",
                _x_min, _x_max, _y_min, _y_max );
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            // TODO : implement real check here.
            return true;
        }
    }
}
