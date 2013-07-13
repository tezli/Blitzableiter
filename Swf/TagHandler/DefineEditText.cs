using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// The DefineEditText tag defines a dynamic text object, or text field.
    /// </summary>
    /// <remarks>
    /// A text field is associated with an ActionScript variable name where
    /// the contents of the text field are stored. The Swf file can read and
    /// write the contents of the variable, which is always kept in sync
    /// with the text being displayed. If the ReadOnly flag is not set, users 
    /// may change the value of a text field interactively.
    /// </remarks>
    public class DefineEditText : AbstractTagHandler, ISwfCharacter
    {
        private UInt16 _characterID;
        private Rect _bounds;

        #region fields

        private bool _hasText;
        /// <summary>
        /// If the text will be wrapped at the end´of the TextBox
        /// </summary>
        public bool Wordrap { get; set; }
        /// <summary>
        /// If the text can have multiple lines
        /// </summary>
        public bool MultiLine { get; set; }
        private bool _password;
        /// <summary>
        /// If the text in the textbox is readonly text
        /// </summary>
        public bool ReadOnly { get; set; }
        private bool _hasTextColor;
        private bool _hasMaxLength;
        private bool _hasFont;

        private bool _hasFontClass;
        private bool _autoSize;
        private bool _hasLayout;
        /// <summary>
        /// If true the text in the textbox is not selectable
        /// </summary>
        public bool NoSelect { get; set; }
        private bool _border;
        private bool _wasStatic;
        /// <summary>
        /// If the text field contains HTML
        /// </summary>
        public bool Html { get; set; }
        private bool _hasOutlines;

        private UInt16 _fontID;
        private string _fontClass;
        private UInt16 _fontHeight;
        /// <summary>
        /// The color of the text
        /// </summary>
        public Rgba TextColor { get; set; }
        private UInt16 _maxLength;
        private Align _align;

        private UInt16 _leftMargin;
        private UInt16 _rightMargin;
        private UInt16 _indent;
        private Int16 _leading;
        /// <summary>
        /// 
        /// </summary>
        public string VariableName { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string InitialText { get; set; }

        #endregion

        /// <summary>
        /// The DefineEditText tag defines a dynamic text object, or text field.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public DefineEditText(byte InitialVersion) : base(InitialVersion)
        {
            this._bounds = new Rect(this._SwfVersion);
            this.TextColor = new Rgba(this._SwfVersion);
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _characterID;
            }
        }

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
        /// The length of this tag including the header.
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get
            {
                ulong length = 0;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write(this._characterID);
                    this._bounds.Write(ms);
                    BitStream bits = new BitStream(ms);
                    bits.WriteBits(1, Convert.ToInt32(this._hasText));
                    bits.WriteBits(1, Convert.ToInt32(this.Wordrap));
                    bits.WriteBits(1, Convert.ToInt32(this.MultiLine));
                    bits.WriteBits(1, Convert.ToInt32(this._password));
                    bits.WriteBits(1, Convert.ToInt32(this.ReadOnly));
                    bits.WriteBits(1, Convert.ToInt32(this._hasTextColor));
                    bits.WriteBits(1, Convert.ToInt32(this._hasMaxLength));
                    bits.WriteBits(1, Convert.ToInt32(this._hasFont));

                    bits.WriteBits(1, Convert.ToInt32(this._hasFontClass));
                    bits.WriteBits(1, Convert.ToInt32(this._autoSize));
                    bits.WriteBits(1, Convert.ToInt32(this._hasLayout));
                    bits.WriteBits(1, Convert.ToInt32(this.NoSelect));
                    bits.WriteBits(1, Convert.ToInt32(this._border));
                    bits.WriteBits(1, Convert.ToInt32(this._wasStatic));
                    bits.WriteBits(1, Convert.ToInt32(this.Html));
                    bits.WriteBits(1, Convert.ToInt32(this._hasOutlines));

                    if (this._hasFont)
                    {
                        bw.Write(this._fontID);
                    }
                    if (this._hasFontClass)
                    {
                        bw.Write(this._fontClass);
                    }
                    if (this._hasFont)
                    {
                        bw.Write(this._fontHeight);
                    }
                    if (this._hasTextColor)
                    {
                        this.TextColor.Write(ms);
                    }
                    if (this._hasMaxLength)
                    {
                        bw.Write(this._maxLength);
                    }
                    if (this._hasLayout)
                    {
                        ms.WriteByte((byte)this._align);

                        bw.Write(this._leftMargin);
                        bw.Write(this._rightMargin);
                        bw.Write(this._indent);
                        bw.Write(this._leading);
                    }
                    SwfStrings.SwfWriteString(this._SwfVersion, bw, this.VariableName);
                    if (this._hasText)
                    {
                        SwfStrings.SwfWriteString(this._SwfVersion, bw, this.InitialText);
                    }
                    length = (ulong)ms.Position;
                }
                return length;
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
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            //Log.Debug(this, "Offset : " + _dataStream.Position);
            BinaryReader br = new BinaryReader(this._dataStream);

            this._characterID = br.ReadUInt16();

            this._bounds.Parse(this._dataStream);

            BitStream bits = new BitStream(this._dataStream);

            this._hasText = ((0 != bits.GetBits(1)) ? true : false);
            this.Wordrap = ((0 != bits.GetBits(1)) ? true : false);
            this.MultiLine = ((0 != bits.GetBits(1)) ? true : false);
            this._password = ((0 != bits.GetBits(1)) ? true : false);
            this.ReadOnly = ((0 != bits.GetBits(1)) ? true : false);
            this._hasTextColor = ((0 != bits.GetBits(1)) ? true : false);
            this._hasMaxLength = ((0 != bits.GetBits(1)) ? true : false);
            this._hasFont = ((0 != bits.GetBits(1)) ? true : false);

            this._hasFontClass = ((0 != bits.GetBits(1)) ? true : false);
            this._autoSize = ((0 != bits.GetBits(1)) ? true : false);
            this._hasLayout = ((0 != bits.GetBits(1)) ? true : false);
            this.NoSelect = ((0 != bits.GetBits(1)) ? true : false);
            this._border = ((0 != bits.GetBits(1)) ? true : false);
            this._wasStatic = ((0 != bits.GetBits(1)) ? true : false);
            this.Html = ((0 != bits.GetBits(1)) ? true : false);
            this._hasOutlines = ((0 != bits.GetBits(1)) ? true : false);

            if (this._hasFont)
            {
                this._fontID = br.ReadUInt16();
            }
            if (this._hasFontClass)
            {
                this._fontClass = SwfStrings.SwfString(this._SwfVersion, br);
            }

            if (this._hasFont)
            {
                this._fontHeight = br.ReadUInt16();
            }
            if (this._hasTextColor)
            {
                this.TextColor.Parse(this._dataStream);
            }
            if (this._hasMaxLength)
            {
                this._maxLength = br.ReadUInt16();
            }
            if (this._hasLayout)
            {
                this._align = (Align)br.ReadByte();
                this._leftMargin = br.ReadUInt16();
                this._rightMargin = br.ReadUInt16();
                this._indent = br.ReadUInt16();
                this._leading = br.ReadInt16();
            }

            this.VariableName = SwfStrings.SwfString(this._SwfVersion, br);

            if (this._hasText)
            {
                this.InitialText = SwfStrings.SwfString(this._SwfVersion, br);
            }

        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this._characterID);
            
            this._bounds.Write(output);

            BitStream bits = new BitStream(output);


            bits.WriteBits(1, Convert.ToInt32(this._hasText) );
            bits.WriteBits(1, Convert.ToInt32(this.Wordrap) );
            bits.WriteBits(1, Convert.ToInt32(this.MultiLine) );
            bits.WriteBits(1, Convert.ToInt32(this._password) );
            bits.WriteBits(1, Convert.ToInt32(this.ReadOnly) );
            bits.WriteBits(1, Convert.ToInt32(this._hasTextColor) );
            bits.WriteBits(1, Convert.ToInt32(this._hasMaxLength) );
            bits.WriteBits(1, Convert.ToInt32(this._hasFont) );

            bits.WriteBits(1, Convert.ToInt32(this._hasFontClass) );
            bits.WriteBits(1, Convert.ToInt32(this._autoSize) );
            bits.WriteBits(1, Convert.ToInt32(this._hasLayout) );
            bits.WriteBits(1, Convert.ToInt32(this.NoSelect) );
            bits.WriteBits(1, Convert.ToInt32(this._border) );
            bits.WriteBits(1, Convert.ToInt32(this._wasStatic) );
            bits.WriteBits(1, Convert.ToInt32(this.Html) );
            bits.WriteBits(1, Convert.ToInt32(this._hasOutlines) );

            if (this._hasFont)
            {
                bw.Write(this._fontID);
            }
            if (this._hasFontClass)
            {
                bw.Write(this._fontClass);
            }
            if (this._hasFont)
            {
                bw.Write(this._fontHeight);
            }
            if (this._hasTextColor)
            {
                this.TextColor.Write(output);
            }
            if (this._hasMaxLength)
            {
                bw.Write(this._maxLength);
            }
            if (this._hasLayout)
            {
                output.WriteByte((byte)this._align);

                bw.Write(this._leftMargin);
                bw.Write(this._rightMargin);
                bw.Write(this._indent);
                bw.Write(this._leading);
            }

            SwfStrings.SwfWriteString(this._SwfVersion, bw, this.VariableName);


            if (this._hasText)
            {
                SwfStrings.SwfWriteString(this._SwfVersion, bw, this.InitialText);
            }
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            return sb.ToString();
        }
    }
}
