using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.TagHandler
{

    class SetBackgroundColor : AbstractTagHandler 
    {

        private Rgb _color;

        public SetBackgroundColor(byte init) : base(init)
        {
            this._color = new Rgb(init);
        }

        /// <summary>
        /// Color to set the Background to
        /// </summary>
        public Rgb Color
        {
            get
            {
                return _color;
            }

        }

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
                return _color.Length;
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
            _color.Parse(_dataStream);
        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);
            _color.Write(output);
        }


    }
}
