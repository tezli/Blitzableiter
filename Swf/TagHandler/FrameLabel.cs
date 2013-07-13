using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class FrameLabel : AbstractTagHandler
    {

        private string _name;
        private bool _isNamedAnchor;

        public FrameLabel(byte init) : base(init)
        {

        }

        /// <summary>
        /// idicates the label is an anchor
        /// </summary>
        public bool isNamedAnchor
        {
            get
            {
                return _isNamedAnchor;
            }
        }

        /// <summary>
        /// Name of the label
        /// </summary>
        public String Name
        {
            get
            {
                return _name;
            }

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (ulong)SwfStrings.SwfStringLength(_SwfVersion, _name) + (ulong)(_isNamedAnchor ? 1 : 0);
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
            BinaryReader br = new BinaryReader(this._dataStream);

            this._name = SwfStrings.SwfString(this._SwfVersion, br);

            String s = String.Format("0x{0:X08}: FrameLabel (\"{1}\")", Tag.OffsetData, Name);
            //Log.Debug(this, s);

            if (SwfStrings.SwfStringLength(_SwfVersion, _name) == Tag.Length-1)   // String + anchorflag
            {
                String s1 = String.Format("0x{0:X08}:\t named anchor", Tag.OffsetData);
                //Log.Debug(this, s1);

                byte b = br.ReadByte();
                if (b != 1)
                    throw new SwfFormatException("NamedAnchorFlag must be set to one");
                _isNamedAnchor = true;
            }

        }

        public override void Write(System.IO.Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            WriteTagHeader(output);

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this._name);

            if (_isNamedAnchor)
            {
                output.WriteByte(1);
            }

        }

    }
}
