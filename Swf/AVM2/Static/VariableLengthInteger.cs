using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recurity.Swf.AVM2.Static
{
    /// <summary>
    /// 
    /// </summary>
    public static class VariableLengthInteger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte ReadU8( Stream source )
        {
            BinaryReader br = new BinaryReader( source );

            return br.ReadByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static byte ReadU8( BinaryReader br )
        {            
            return br.ReadByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public static void WriteU8( Stream destination, byte value )
        {
            destination.WriteByte( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteU8( BinaryWriter bw, byte value )
        {
            bw.Write( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static UInt16 ReadU16( Stream source )
        {
            BinaryReader br = new BinaryReader( source );

            return br.ReadUInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static UInt16 ReadU16( BinaryReader br )
        {            
            return br.ReadUInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destintion"></param>
        /// <param name="value"></param>
        public static void WriteU16( Stream destintion, UInt16 value )
        {
            WriteU16( new BinaryWriter( destintion ), value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteU16( BinaryWriter bw, UInt16 value )
        {
            bw.Write( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static UInt32 ReadU32( Stream source )
        {
            return Read32( source );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static UInt32 ReadU32( BinaryReader br )
        {
            return Read32( br.BaseStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public static void WriteU32( Stream destination, UInt32 value )
        {
            Write32( destination, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteU32( BinaryWriter bw, UInt32 value )
        {
            Write32( bw.BaseStream, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static UInt32 ReadU30( Stream source )
        {            
            UInt32 value = ReadU32( source );
            if ( ( value & 0xC0000000 ) != 0 )
            {
                String s = String.Format("U30 value with most significant 2 bits in use: {0:X08} [cleared]", value);
                Log.Warn(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, s);
            }
            return ( value & 0x3FFFFFFF );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static UInt32 ReadU30( BinaryReader br )
        {
            return ReadU30( br.BaseStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public static void WriteU30( Stream destination, UInt32 value )
        {
            Write32( destination, ( value & 0x3FFFFFFF ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteU30( BinaryWriter bw, UInt32 value )
        {
            Write32( bw.BaseStream, ( value & 0x3FFFFFFF ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int32 ReadS32( Stream source )
        {            
            Int32 retVal = unchecked( (Int32)Read32( source ) );

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static Int32 ReadS32( BinaryReader br )
        {
            Int32 retVal = unchecked( ( Int32 )Read32( br.BaseStream ) );

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public static void WriteS32( Stream destination, Int32 value )
        {            
            UInt32 v = unchecked( ( UInt32 )value );
            Write32( destination, v );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteS32( BinaryWriter bw, Int32 value )
        {
            UInt32 v = unchecked( ( UInt32 )value );
            Write32( bw.BaseStream, v );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int32 ReadS24( Stream source )
        {
            byte[] raw = new byte[ 3 ];
            source.Read( raw, 0, 3 );
            Int32 ret = raw[ 2 ] << 16;
            ret = ret | ( raw[ 1 ] << 8 );
            ret = ret | raw[ 0 ];

            if ( ( raw[ 2 ] & 0x80 ) != 0 )
            {
                ret = unchecked( ( Int32 )( ( UInt32 )ret | 0xFF000000 ) );
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static Int32 ReadS24( BinaryReader br )
        {
            return ReadS24( br.BaseStream );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public static void WriteS24( Stream destination, Int32 value )
        {
            byte b0 = unchecked( (byte)(value & 0xFF) );
            byte b1 = unchecked( ( byte )( ( value >> 8 ) & 0xFF ) );
            byte b2 = unchecked( ( byte )( ( value >> 16 ) & 0xFF ) );
            destination.WriteByte( b0 );
            destination.WriteByte( b1 );
            destination.WriteByte( b2 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteS24( BinaryWriter bw, Int32 value )
        {
            WriteS24( bw.BaseStream, value );
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static UInt32 Read32( Stream source )
        {                
            UInt32 value = 0;
            bool moreToFollow = false;
            int shift = 0;            
            long pos = source.Position;
            UInt32 addition = 0;
            byte b = 0;
            byte[] buf = new byte[ 2 ];
            
            do
            {
                source.Read( buf, 0, 1 );
                b = buf[ 0 ];
                moreToFollow = ( 0 != ( b & 0x80 ) );
                addition = unchecked( ( UInt32 )( ( b & 0x7F ) << shift ) );                
                value = value | addition ;
                shift = shift + 7;
            }
            while ( ( ( source.Position - pos ) < 5 ) && moreToFollow );

            if ( moreToFollow )
            {
                AbcFormatException fe = new AbcFormatException( "Variable length integer indicating > 5 bytes (stream pos: 0x"
                    + pos.ToString( "X08" ) + ", value=0x" + value.ToString( "X08" ) + 
                    ", last byte=0x" + b.ToString( "X02" ) + ")" );
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, fe);
                throw fe;
            }
            if ( ( ( source.Position - pos ) == 5 ) && ( ( b & 0xF0 ) != 0 ) )
            {
                AbcFormatException fe = new AbcFormatException( "Overlong variable length integer detected! (stream pos: 0x"
                    + pos.ToString( "X08" ) + ", value=0x" + value.ToString( "X08" ) +
                    ", last byte=0x" + b.ToString( "X02" ) + ")" );
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, fe);
                throw fe;
            }

            return value;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint EncodedLength( Int32 value )
        {            
            UInt32 v = unchecked( ( UInt32 )value );

            return EncodedLength( v );            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint EncodedLength( UInt32 value )
        {
            UInt32 bit = 1;
            int higestBit = 0;

            for ( int i = 0; i < 31; i++ )
            {
                UInt32 mask = bit << i;
                if ( ( value & mask ) != 0 )
                {
                    higestBit = i;
                }
            }

            return ( uint )( ( higestBit / 7 ) + 1 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint EncodedLengthU30( UInt32 value )
        {
            return EncodedLength( ( uint )( value & 0x3FFFFFFF ) );
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        private static void Write32( Stream destination, UInt32 value )
        {
            uint length = EncodedLength( value );
            uint drain = value;

            for ( int i = 0; i < length; i++ )
            {
                byte bits = ( byte )( drain & 0x7F );
                drain = drain >> 7;

                if ( !( i + 1 == length ) )
                {
                    bits = ( byte )( bits | ( byte )0x80 );
                }

                destination.WriteByte( bits );
            }
        }        
    }
}
