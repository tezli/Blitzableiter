using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Interfaces;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Pix : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Pix( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public abstract ulong Length { get; }

        /// <summary>
        /// Verifies this this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public abstract bool Verify();

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        public abstract void Parse( Stream input );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public abstract void Write( Stream output );
    }
}
