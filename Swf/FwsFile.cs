using Recurity.Swf.Interfaces;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// An uncompressed Swf file
    /// </summary>
    public class FwsFile : BaseFile, ISwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        public FrameHeaderInfo FrameHeader { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public override byte Version { get; set; }

        internal MemoryStream WriteContent;

        /// <summary>
        /// 
        /// </summary>
        public FwsFile()
        {
            Compressed = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override Stream Read( Stream input )
        {
            ReadHeader( input );
            
            //if (input.Length < Length)
            //{
            //    Exception e = new SwfFormatException("Stream length " + input.Length.ToString() + " shorter than header declared length " + Length.ToString());
            //    Log.Error(this, e);
            //    throw e;
            //}
            //else if (input.Length > Length)
            //{
            //    Log.Warn(this, "Stream length " + input.Length.ToString() + " greater than header declared length " + Length.ToString());
            //    Log.Warn(this, "Trailing garbage detected!");
            //}             

            FrameHeader = new FrameHeaderInfo( this.Version );
            FrameHeader.Parse( input );

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            this.Signature = "FWS";
            this.Length = (uint) (
                WriteContent.Length
                + 3 // signature
                + 1 // version
                + 4 // length
                + FrameHeader.Length
                );
            WriteHeader( output );
            FrameHeader.Write( output );
            WriteContent.WriteTo( output );
        }
    }
}
