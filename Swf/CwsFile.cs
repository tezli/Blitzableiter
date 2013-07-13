using System;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A compressed Swf file
    /// </summary>
    public class CwsFile : FwsFile
    {
        /// <summary>
        /// 
        /// </summary>
        public int CompressionLevel { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public CwsFile()
        {
            Compressed = true;
            this.CompressionLevel = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override Stream Read(Stream input)
        {
            ReadHeader(input);

            if (Compressed)
            {
                //Log.Debug(this, "CWSFile:Read() called for compressed input");
                return Uncompress(input);
            }
            else
            {
                //Log.Debug(this, "CWSFile:Read() called for uncompressed input");
                input.Seek(0, SeekOrigin.Begin);
                return input;
            }
        }

        /// <summary>
        /// Uncompresses ZLIB compressed files
        /// </summary>
        /// <param name="input">The ZLIB compressed file as stream</param>
        /// <returns>A uncompressed Stream</returns>
        public Stream Uncompress(Stream input)
        {
            MemoryStream returnStream = new MemoryStream();

            byte[] zipData = new byte[input.Length - 8];
            input.Read(zipData, 0, (int)(input.Length - 8));

            byte[] buffer = new byte[input.Length * 4];

            Inflater inflater = new Inflater(false);
            inflater.SetInput(zipData);

            try
            {
                int bytesInflated = inflater.Inflate(buffer);
                WriteHeader(returnStream);
                returnStream.Write(buffer, 0, bytesInflated);
                return returnStream;
            }
            catch (Exception e)
            {
                SwfFormatException s = new SwfFormatException(e.Message);
                Log.Error(this, s.Message);
                throw s;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            MemoryStream finalContent = new MemoryStream();


            this.FrameHeader.Write(finalContent);
            this.WriteContent.WriteTo(finalContent);
            this.Length = (UInt32)finalContent.Length + 8;

            this.Signature = "CWS";
            byte[] compressedOutput = Compress(finalContent);
            WriteHeader(output);
            output.Write(compressedOutput, 0, compressedOutput.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] Compress(Stream input)
        {
            String s = String.Format("Compressing output with level {0:d}", CompressionLevel);
            //Log.Debug(this, s);

            Deflater deflater = new Deflater(CompressionLevel, false);
            byte[] uncompressedData = new byte[(Int32)input.Length];
            input.Seek(0, SeekOrigin.Begin);
            input.Read(uncompressedData, 0, uncompressedData.Length);
            byte[] buffer = new byte[input.Length];

            try
            {
                deflater.SetInput(uncompressedData);
                deflater.Finish();
                int bytesDeflated = deflater.Deflate(buffer, 0, buffer.Length);


                byte[] compressedData = new byte[bytesDeflated];
                Array.Copy(buffer, compressedData, bytesDeflated);

                //Log.Debug(this, "Compression completed.");

                return compressedData;
            }
            catch (Exception e)
            {
                Log.Error(this, e);
                throw e;
            }

        }
    }
}
