using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Recurity.Swf.AVM2.Static
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Read( Stream source )
        {
            UInt32 length = VariableLengthInteger.ReadU30( source );
            string value;

            if ( 0 != length )
            {

                BinaryReader br = new BinaryReader( source );
                byte[] supposedString = br.ReadBytes( ( int )length );

                value = ASCIIEncoding.UTF8.GetString( supposedString );
            }
            else
            {
                value = "";
            }                

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint Length( string value )
        {
            uint lenEncoded = (uint)ASCIIEncoding.UTF8.GetByteCount( value );
            uint lenField = VariableLengthInteger.EncodedLengthU30( lenEncoded );

            return lenEncoded + lenField;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="svalue"></param>
        public static void Write( Stream destination, string svalue )
        {
            uint lenEncoded = ( uint )ASCIIEncoding.UTF8.GetByteCount( svalue );
            VariableLengthInteger.WriteU30( destination, lenEncoded );
            byte[] encoded = ASCIIEncoding.UTF8.GetBytes( svalue );
            destination.Write( encoded, 0, encoded.Length );
        }
    }
}
