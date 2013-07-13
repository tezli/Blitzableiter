using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipEventFlags : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventKeyUp;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventKeyDown;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventMouseUp;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventMouseDown;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventMouseMove;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventUnload;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventEnterFrame;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventLoad;
        // Swf6 and following

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventDragOver;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventRollOut;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventRollOver;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventReleaseOutside;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventRelease;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventPress;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventInitialize;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventData;
        // Swf6, used in Swf7

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventConstruct;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventKeyPress;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipEventDragOut;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ClipEventFlags( byte InitialVersion ) : base( InitialVersion ) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Parse( Stream input )
        {
            BitStream bits = new BitStream( input );

            ClipEventKeyUp = ( 1 == bits.GetBits( 1 ) );            // 0
            ClipEventKeyDown = ( 1 == bits.GetBits( 1 ) );          // 1
            ClipEventMouseUp = ( 1 == bits.GetBits( 1 ) );          // 2
            ClipEventMouseDown = ( 1 == bits.GetBits( 1 ) );        // 3
            ClipEventMouseMove = ( 1 == bits.GetBits( 1 ) );        // 4
            ClipEventUnload = ( 1 == bits.GetBits( 1 ) );           // 5
            ClipEventEnterFrame = ( 1 == bits.GetBits( 1 ) );       // 6
            ClipEventLoad = ( 1 == bits.GetBits( 1 ) );             // 7
            if ( this.Version >= 6 )
            {
                // Swf6 and following
                ClipEventDragOver = ( 1 == bits.GetBits( 1 ) );
                ClipEventRollOut = ( 1 == bits.GetBits( 1 ) );
                ClipEventRollOver = ( 1 == bits.GetBits( 1 ) );
                ClipEventReleaseOutside = ( 1 == bits.GetBits( 1 ) );
                ClipEventRelease = ( 1 == bits.GetBits( 1 ) );
                ClipEventPress = ( 1 == bits.GetBits( 1 ) );
                ClipEventInitialize = ( 1 == bits.GetBits( 1 ) );
                ClipEventData = ( 1 == bits.GetBits( 1 ) );

                uint reserved = bits.GetBits( 5 );
                if ( 0 != reserved )
                {
                    throw new SwfFormatException( "Reserved flags (following ClipEventData) used in ClipEventFlags" );
                }

                // Swf6, used in Swf7
                if ( ( ClipEventConstruct = ( 1 == bits.GetBits( 1 ) ) ) && ( this.Version < 7 ) )
                {
                    throw new SwfFormatException( "ClipEventConstruct in Swf with Version < 7" );
                }
                ClipEventKeyPress = ( 1 == bits.GetBits( 1 ) );
                ClipEventDragOut = ( 1 == bits.GetBits( 1 ) );

                uint reserved2 = bits.GetBits( 8 );
                if ( 0 != reserved2 )
                {
                    throw new SwfFormatException( "Reserved flags (following ClipEventDragOut) used in ClipEventFlags" );
                }
            }
            else
            {
                // 
                // Swf spec mentiones that CLIPEVENTFLAGS is 2 Bytes in 
                // Swf.Version < 6, but doesn't specify a Reserved 8-Bit field
                // explicitly. So we handle it like one.
                //
                uint reservedB46 = bits.GetBits( 8 );
                if ( 0 != reservedB46 )
                {
                    throw new SwfFormatException( "Reserved flags (following ClipEventLoad) used in ClipEventFlags" );
                }
            }            
        }

        /*
        public uint GetLength( byte version )
        {
            if ( version < 6 )
                return 2;
            else
                return 4;
        }
         */

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                if ( this.Version < 6 )
                    return 2;
                else
                    return 4;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            BitStream bits = new BitStream( output );
            
            bits.WriteBits( 1, ( ClipEventKeyUp ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventKeyDown ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventMouseUp ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventMouseDown ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventMouseMove ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventUnload ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventEnterFrame ? 1 : 0 ) );
            bits.WriteBits( 1, ( ClipEventLoad ? 1 : 0 ) );
            if ( this.Version >= 6 )
            {
                // Swf6 and following
                bits.WriteBits( 1, ( ClipEventDragOver ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventRollOut ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventRollOver ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventReleaseOutside ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventRelease ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventPress ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventInitialize ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventData ? 1 : 0 ) );
                bits.WriteBits( 5, 0 ); // reserved
                // Swf6, used in Swf7
                if ( this.Version >= 7 )
                    bits.WriteBits( 1, ( ClipEventConstruct ? 1 : 0 ) );
                else
                    bits.WriteBits( 1, 0 );

                bits.WriteBits( 1, ( ClipEventKeyPress ? 1 : 0 ) );
                bits.WriteBits( 1, ( ClipEventDragOut ? 1 : 0 ) );
            }

            bits.WriteBits( 8, 0 ); // reserved

            bits.WriteFlush();
        }
    }
}
