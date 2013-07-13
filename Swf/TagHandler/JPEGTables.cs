using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>This tag defines the JPEG encoding table (the Tables/Misc segment) for all JPEG images</para>
    /// <para>defined using the DefineBits tag. There may only be one JPEGTables tag in a Swf file.</para>
    /// <para>The data in this tag begins with the JPEG SOI marker 0xFF, 0xD8 and ends with the EOI</para>
    /// <para>marker 0xFF, 0xD9. Before version 8 of the Swf file format, Swf files could contain</para>
    /// </summary>
    public class JPEGTables : AbstractTagHandler
    {
        private byte[] _jpegTables;

        /// <summary>
        /// <para>This tag defines the JPEG encoding table for all JPEG images defined using</para>
        /// <para>the DefineBits tag. There may only be one JPEGTables tag in a Swf file.</para>
        /// </summary>
        /// <param name="InitialVersion">The Swf version of the file using this tag.</param>
        public JPEGTables( byte InitialVersion ) : base( InitialVersion ) 
        {
            this._jpegTables = new byte[ 0 ];
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 1 ;
            }

        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (UInt64)this._jpegTables.Length;
            }
        }

        /// <summary>
        /// Verifies this tag and its component for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public override bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            this.WriteTagHeader( output );
            output.Write( _jpegTables, 0, _jpegTables.Length );
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            Stream input = this._dataStream;
            BinaryReader br = new BinaryReader( input );
            this._jpegTables = br.ReadBytes( (Int32)( input.Length - input.Position) );
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( base.ToString() );
            sb.AppendFormat( " Tables data size: {1:d}", this._jpegTables.Length );
            return sb.ToString();
        }
    }
}
