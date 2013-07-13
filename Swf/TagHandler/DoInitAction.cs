using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class DoInitAction : DoAction
    {
        internal UInt16 _SpriteID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DoInitAction( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 6; }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            return VerifyAllCode();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader( _dataStream );
            _SpriteID = br.ReadUInt16();
            
            base.Parse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write( Stream output )
        {
            WriteTagHeader( output );

            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( _SpriteID );

            WriteCodeAndCheck( output );
        }

        /// <summary>
        /// 
        /// </summary>
        protected override long CodeOffset
        {
            get
            {
                return 2;
            }
        }
    }
}
