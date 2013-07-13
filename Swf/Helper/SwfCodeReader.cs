using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.AVM1;
using Recurity.Swf.AVM2;

namespace Recurity.Swf.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class SwfCodeReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sourceStream"></param>
        /// <param name="sourceVersion"></param>
        /// <returns></returns>
        public static AVM1InstructionSequence GetCode( UInt32 size, BinaryReader sourceStream, byte sourceVersion )
        {
            AVM1InstructionSequence retVal = new AVM1InstructionSequence();

            using ( MemoryStream memStream = new MemoryStream( sourceStream.ReadBytes( (int)size ) ) )
            {
                BinaryReader2 brInner = new BinaryReader2( memStream );
                while ( brInner.BaseStream.Position < size )
                {
                    if ( 0 == brInner.PeekByte() )
                    {
                        //
                        // ActionEndFlag found
                        //                                                
                        AbstractAction innerAction = AVM1Factory.Create( brInner, sourceVersion );
                        retVal.Add( innerAction );

                        //
                        // Verify that the entire MemoryStream (i.e. "size" bytes) were consumed
                        //                                                
                        if ( brInner.BaseStream.Position != size ) 
                        {
                            Log.Warn(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Code reading for size " + size.ToString("d") + 
                                " terminated prematurely at position 0x" + brInner.BaseStream.Position.ToString( "X08" )
                            );                         
                        }

                        break;
                    }
                    else
                    {
                        AbstractAction innerAction = AVM1Factory.Create( brInner, sourceVersion );
                        retVal.Add( innerAction );
                    }
                }
            }

            return retVal;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inList"></param>
        /// <returns></returns>
        public static UInt32 CodeLength( AVM1InstructionSequence inList )
        {
            UInt32 sum = 0;
            
            for ( int i = 0; i < inList.Count; i++ )
            {
                sum += inList[ i ].ActionLength;
            }

            return sum;
        }        
    }
}
