using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryReader2 : BinaryReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inp"></param>
        public BinaryReader2( Stream inp )
            :
            base( inp )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte PeekByte()
        {
            long position = this.BaseStream.Position;
            byte r = this.ReadByte();
            this.BaseStream.Seek( position, SeekOrigin.Begin );
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UInt16 PeekUInt16()
        {
            long position = this.BaseStream.Position;
            UInt16 r = this.ReadUInt16();
            this.BaseStream.Seek( position, SeekOrigin.Begin );
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UInt32 PeekUInt32()
        {
            long position = this.BaseStream.Position;
            UInt32 r = this.ReadUInt32();
            this.BaseStream.Seek( position, SeekOrigin.Begin );
            return r;
        }
    }
}
