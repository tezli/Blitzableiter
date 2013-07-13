using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM2Factory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static AbstractInstruction Create( BinaryReader sourceStream, UInt32 method )
        {
            AbstractInstruction product = null;

            byte opCode = sourceStream.ReadByte();

            if ( !Enum.IsDefined( typeof( AVM2OpCodes ), opCode ) )
            {
                AbcFormatException ex = new AbcFormatException( "Illegal opcode 0x" + opCode.ToString( "X02" ) + " at stream position 0x" + sourceStream.BaseStream.Position.ToString( "X" ) );
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, ex);
                throw ( ex );
            }            
            
            string opName = Enum.GetName( typeof( AVM2OpCodes ), opCode );

            product = ( AbstractInstruction )MethodBase.GetCurrentMethod().DeclaringType.Assembly.CreateInstance( "Recurity.Swf.AVM2.Instructions." + opName );

            product.Read( sourceStream, opCode, method );

            //Log.Debug(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, product.ToString());

            return product;
        }
    }
}
