using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class DebugID : AbstractTagHandler
    {
        /// <summary>
        /// 
        /// </summary>
        private byte[] _UUID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialVersion"></param>
        public DebugID( byte initialVersion ) : base( initialVersion ) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                // TODO: we don't actually know
                return 9;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get 
            {
                return 16; // one UUID
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            //
            // Nothing to verify on a UUID
            //
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader( _dataStream );

            _UUID = br.ReadBytes( 16 );

            if ( 16 != _UUID.Length )
            {
                SwfFormatException sfe = new SwfFormatException( "UUID read is not 16 bytes long" );
                Log.Error(this,  sfe );
                throw sfe;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( System.IO.Stream output )
        {
            this.WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( _UUID );
        }
    }
}
