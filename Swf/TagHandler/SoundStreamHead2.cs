using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class SoundStreamHead2 : SoundStreamHead
    {

        public SoundStreamHead2(byte initial) : base(initial) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            //TODO: in difference to SoundStreamHead "all" soundformats are allowed to encode the Stream
            return true;
        }
    }
}
