using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineBinaryData tag permits arbitrary binary data to be embedded in a Swf file.
    /// </summary>
    /// <remarks>
    /// <para>The DefineBinaryData tag permits arbitrary binary data to be embedded in a Swf file.</para>
    /// <para>DefineBinaryData is a definition tag, like DefineShape and DefineSprite. It associates a blob</para>
    /// <para>of binary data with a standard Swf 16-bit character ID. The character ID is entered into the</para>
    /// <para>Swf file's character dictionary.</para>
    /// <para>DefineBinaryData is intended to be used in conjunction with the SymbolClass tag. The</para>
    /// <para>SymbolClass tag can be used to associate a DefineBinaryData tag with an AS3 class definition.</para>
    /// <para>The AS3 class must be a subclass of ByteArray. When the class is instantiated, it will be</para>
    /// <para>populated automatically with the contents of the binary data resource.</para>
    /// </remarks>
    public class DefineBinaryData : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _tagID;
        private byte[] _data;

        /// <summary>
        /// The DefineBinaryData tag permits arbitrary binary data to be embedded in a Swf file.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineBinaryData(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                return this.Tag.Length;
            }
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _tagID;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            Recurity.Swf.Configuration.Config conf = (Recurity.Swf.Configuration.Config)ConfigurationManager.GetSection("SwfEngineConfig");

            if (SwfFile.Configuration.AllowAVM2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);

            this._tagID = br.ReadUInt16();
            UInt32 reserved = br.ReadUInt32(); // reserved (must be 0);

            if (0 != reserved)
            {
                SwfFormatException e = new SwfFormatException("Reserved byte is not null.");
               Log.Error(this, e.Message);
                throw e;
            }

            this._data = new byte[this.Length - 6];
            int read = this._dataStream.Read(this._data, 0, this._data.Length);
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            WriteTagHeader(output);

            UInt32 reserved = 0;

            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this._tagID);
            bw.Write(reserved);
            output.Write(this._data, 0, this._data.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            return sb.ToString();
        }

    }
}
