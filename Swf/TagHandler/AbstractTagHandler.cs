using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractTagHandler : AbstractSwfElement
    {
        /// <summary>
        /// The original Tag content, except the Tag header.
        /// </summary>
        protected MemoryStream _dataStream;

        /// <summary>
        /// The original Tag header.
        /// </summary>
        protected Tag _tag;

        /// <summary>
        /// 
        /// </summary>
        protected SwfFile _SourceFileReference;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public AbstractTagHandler(byte InitialVersion) : base(InitialVersion) { }


        /// <summary>
        /// 
        /// </summary>
        public Tag Tag
        {
            get
            {
                return _tag;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual byte WriteVersion
        {
            get
            {
                return this.MinimumVersionRequired;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputTag"></param>
        /// <param name="sourceFile"></param>
        /// <param name="input"></param>
        public void Read(Tag inputTag, SwfFile sourceFile, Stream input)
        {
            _tag = inputTag;
            _SourceFileReference = sourceFile;
            _dataStream = inputTag.ReadContent(input);

            String s = String.Format("0x{0:X08}: reading tag content for {1}",
                _tag.OffsetData,
                (inputTag.IsTagTypeKnown ? inputTag.TagTypeName : inputTag.TagType.ToString()));
            // Log.Debug(this, s);

            Parse();

        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] TagAndLengthEncoded
        {
            get
            {
                UInt16 tagType = (UInt16)_tag.TagType;
                UInt16 shortTag = (UInt16)(tagType << 6);
                byte[] result = null;

                if (this.Length < 0x3F)
                {
                    //
                    // Short tag
                    //
                    shortTag = (UInt16)(shortTag | (UInt16)this.Length);
                    result = BitConverter.GetBytes(shortTag);
                }
                else
                {
                    //
                    // Long Tag required
                    //
                    shortTag = (UInt16)(shortTag | (UInt16)0x3F);
                    List<byte> l = new List<byte>();
                    l.AddRange(BitConverter.GetBytes(shortTag));
                    l.AddRange(BitConverter.GetBytes((UInt32)(this.Length)));
                    result = l.ToArray();
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                    sb.AppendFormat("{0:X02} ", result[i]);
                //Log.Debug(this, 
                //     "Tag "
                //     + (_tag.TagTypeName == null ? _tag.TagType.ToString("d") : _tag.TagTypeName)
                //     + " encoded as " + sb.ToString() + "(" + this.Length.ToString("d") + " bytes)");



                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected void WriteTagHeader(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(this.TagAndLengthEncoded);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Recurity.Swf.AVM1.AVM1Code[] Code
        {
            get
            {
                AVM1.AVM1Code[] code = new Recurity.Swf.AVM1.AVM1Code[0];
                return code;
            }
            set
            {
                throw new ArgumentException(this.GetType().ToString() + " cannot carry AVM1Code");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ReParseCode()
        {
            return;
        }

        //
        // abstract properties
        //

        /// <summary>
        /// 
        /// </summary>
        public abstract byte MinimumVersionRequired { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract ulong Length { get; }

        //
        // abstract methods
        //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool Verify();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Parse();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public abstract void Write(Stream output);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="s"></param>
        ///// <param name="start"></param>
        //public virtual void DumpThisOnRead(Stream s, long start, Tag t)
        //{
        //    long startPos = s.Position;
        //    byte[] buffer = new byte[s.Position - start];
        //    this._dataStream.Seek(start, SeekOrigin.Begin);
        //    this._dataStream.Read(buffer, 0, buffer.Length);
        //    DumpEngine.Dump(this, new DumpEventArgs(buffer, (long)t.Offset, Method.Read));
        //    this._dataStream.Seek(startPos, SeekOrigin.Begin);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="s"></param>
        ///// <param name="start"></param>
        //public virtual void DumpThisOnWrite(Stream s, long start, Tag t)
        //{
        //    byte[] buffer = new byte[s.Position];
        //    this._dataStream.Seek(start, SeekOrigin.Begin);
        //    this._dataStream.Read(buffer, 0, buffer.Length);
        //    DumpEngine.Dump(this, new DumpEventArgs(buffer, (long)t.Offset, Method.Write));
        //    this._dataStream.Seek(start, SeekOrigin.Begin);
        //}
    }
}
