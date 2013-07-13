using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class SwfStrings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Version"></param>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int SwfStringLength( byte Version, string inputString )
        {
            if ( Version <= 5 )
            {
                return ( ASCIIEncoding.ASCII.GetBytes( inputString.ToCharArray() ).Length + 1 );
            }
            else
            {
                return ( ASCIIEncoding.UTF8.GetBytes( inputString.ToCharArray() ).Length + 1 );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Version"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SwfString( byte Version, BinaryReader source )
        {
            if ( Version <= 5 )
            {
                return Swf5String( source );
            }
            else
            {
                return Swf6Sting( source );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Swf5String( BinaryReader source )
        {            
            return ASCIIEncoding.ASCII.GetString( ReadZeroTerminated( source ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Swf6Sting( BinaryReader source )
        {
            return ASCIIEncoding.UTF8.GetString( ReadZeroTerminated( source ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static byte[] ReadZeroTerminated( BinaryReader source )
        {
            List<byte> bufferA = new List<byte>();

            byte b;
            while ( 0x00 != ( b = source.ReadByte() ) )
            {
                bufferA.Add( b );
            }

            return bufferA.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Version"></param>
        /// <param name="destination"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static uint SwfWriteString( byte Version, BinaryWriter destination, string str )
        {
            long pos = destination.BaseStream.Position;

            byte termination = 0x00;

            if ( Version <= 5 )
            {
                byte[] ascii = ASCIIEncoding.ASCII.GetBytes( str.ToCharArray() );

                destination.Write( ascii );
                destination.Write( termination );
            }
            else
            {
                byte[] utf = ASCIIEncoding.UTF8.GetBytes( str.ToCharArray() );

                destination.Write( utf );
                destination.Write( termination );
            }

            return (uint)( destination.BaseStream.Position - pos );
        }
    }
}
