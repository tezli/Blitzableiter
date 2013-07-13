using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    class Protect : AbstractTagHandler
    {

        private string _md5password;     // Requires version >=5
        private bool _hasPassword;

        public Protect(byte init) : base(init)
        {

        }

        /// <summary>
        /// Returns true if the tag contains a password
        /// </summary>
        public bool HasPassword
        {
            get
            {
                return _hasPassword;
            }
        }

        /// <summary>
        /// return the password of the tag if exists otherwise null
        /// </summary>
        public string Password
        {
            get
            {
                if (_hasPassword)
                    return _md5password;
                else
                    return null;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return _hasPassword ? (ulong)SwfStrings.SwfStringLength(_SwfVersion, _md5password) : 0;
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
            String s = String.Format("0x{0:X08}: reading Protect-Tag", this.Tag.OffsetData);
            Log.Debug(this, s);

            BinaryReader br = new BinaryReader(this._dataStream);

            _hasPassword = Tag.Length != 0;

            if (_hasPassword)
            {
                _md5password = SwfStrings.SwfString(this._SwfVersion, br);
            }

            if (_hasPassword)
            {
                String s1 = String.Format("0x{0:X08}:\tPassword", this.Tag.OffsetData, _md5password);
                Log.Debug(this, s1);
            }

        }

        public override void Write(System.IO.Stream output)
        {
            WriteTagHeader(output);

            BinaryWriter bw = new BinaryWriter(output);

            if (_hasPassword)
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this._md5password);
        }
    }
}
