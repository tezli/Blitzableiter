using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericTag : AbstractTagHandler
    {
        private byte[] _Contents;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public GenericTag( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 3; }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get 
            { 
                return (ulong)_Contents.Length; 
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader( _dataStream );
            _Contents = br.ReadBytes( (int)_tag.Length );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            WriteTagHeader( output );

            BinaryWriter bw = new BinaryWriter( output );            
            bw.Write( _Contents );
            //SwfFile.log.Debug( "0x" + output.Position.ToString("X08") + ": " + _Contents.Length.ToString( "d" ) + " bytes written" );
        }
    }
}
