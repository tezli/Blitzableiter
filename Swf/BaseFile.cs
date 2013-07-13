using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// An abstract class template for Swf files
    /// </summary>
    public abstract class BaseFile : Interfaces.ISwfElement
    {
        private const byte VersionMaximum = 11;
        private const byte VersionMinimum = 1;

        internal string Signature;

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Length { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Compressed { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte Version { get; set; }


        /// <summary>
        /// Reads the header of an Swf file. Defines signature, version and length
        /// </summary>
        /// <param name="input"></param>
        protected void ReadHeader(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            if (!input.CanSeek)
            {
                Exception e = new ArgumentException("input stream can not seek");
                Log.Error(this, e);
                throw e;
            }

            input.Seek(0, SeekOrigin.Begin);

            byte[] sigbytes = br.ReadBytes(3);

            try
            {
                Signature = System.Text.ASCIIEncoding.ASCII.GetString(sigbytes);
                Version = br.ReadByte();
                Length = br.ReadUInt32();
            }
            catch (EndOfStreamException e)
            {
                Log.Error(this, e.Message);
                throw e;
            }

            // Checking the signature
            if (Signature.Equals("FWS", StringComparison.InvariantCulture))
            {
                Compressed = false;
            }
            else if (Signature.Equals("CWS", StringComparison.InvariantCulture))
            {
                Compressed = true;
            }
            else
            {
                Exception e = new SwfFormatException("Invalid Signature: '" + Signature + "'");
                Log.Error(this, e);
                throw e;
            }

            // Checking the version 
            if ((Version > VersionMaximum) || (Version < VersionMinimum))
            {
                Exception e = new SwfFormatException("Invalid / unknown version " + Version.ToString());
                Log.Error(this, e);
                throw e;
            }
            if (Compressed && (Version < 6))
            {
                Log.Warn(this, "Compression is indicated, but version is " + Version.ToString() + " (must at least be 6)");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract Stream Read(Stream input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public abstract void Write(Stream output);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected void WriteHeader(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature.ToCharArray()));
            bw.Write(this.Version);
            bw.Write(this.Length);
        }
    }
}
