using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{
    class End : AbstractTagHandler
    {

        public End(byte init) : base(init) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return 0;
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

        protected override void Parse()
        {
            String s = String.Format("0x{0:X08}: reading End-Tag", this.Tag.OffsetData);
            //Log.Debug(this, s);
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
        }

    }
}
