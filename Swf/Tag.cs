using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Recurity.Swf
{
    //
    // TagTypes (RECORDHEADER) in the Swf Spec
    //
    /// <summary>
    /// 
    /// </summary>
    public enum TagTypes
    {
        /// <summary>
        /// 
        /// </summary>
        End = 0,
        /// <summary>
        /// 
        /// </summary>
        ShowFrame = 1,
        /// <summary>
        /// 
        /// </summary>
        DefineShape = 2,
        /// <summary>
        /// 
        /// </summary>
        PlaceObject = 4,
        /// <summary>
        /// 
        /// </summary>
        RemoveObject = 5,
        /// <summary>
        /// 
        /// </summary>
        DefineBits = 6,
        /// <summary>
        /// 
        /// </summary>
        DefineButton = 7,
        /// <summary>
        /// 
        /// </summary>
        JPEGTables = 8,
        /// <summary>
        /// 
        /// </summary>
        SetBackgroundColor = 9,
        /// <summary>
        /// 
        /// </summary>
        DefineFont = 10,
        /// <summary>
        /// 
        /// </summary>
        DefineText = 11,
        /// <summary>
        /// 
        /// </summary>
        DoAction = 12,
        /// <summary>
        /// 
        /// </summary>
        DefineFontInfo = 13,
        /// <summary>
        /// 
        /// </summary>
        DefineSound = 14,
        /// <summary>
        /// 
        /// </summary>
        StartSound = 15,
        /// <summary>
        /// 
        /// </summary>
        DefineButtonSound = 17,
        /// <summary>
        /// 
        /// </summary>
        SoundStreamHead = 18,
        /// <summary>
        /// 
        /// </summary>
        SoundStreamBlock = 19,
        /// <summary>
        /// 
        /// </summary>
        DefineBitsLossless = 20,
        /// <summary>
        /// 
        /// </summary>
        DefineBitsJPEG2 = 21,
        /// <summary>
        /// 
        /// </summary>
        DefineShape2 = 22,
        /// <summary>
        /// 
        /// </summary>
        DefineButtonCxform = 23,
        /// <summary>
        /// 
        /// </summary>
        Protect = 24,
        /// <summary>
        /// 
        /// </summary>
        PlaceObject2 = 26,
        /// <summary>
        /// 
        /// </summary>
        RemoveObject2 = 28,
        /// <summary>
        /// 
        /// </summary>
        DefineShape3 = 32,
        /// <summary>
        /// 
        /// </summary>
        DefineText2 = 33,
        /// <summary>
        /// 
        /// </summary>
        DefineButton2 = 34,
        /// <summary>
        /// 
        /// </summary>
        DefineBitsJPEG3 = 35,
        /// <summary>
        /// 
        /// </summary>
        DefineBitsLossless2 = 36,
        /// <summary>
        /// 
        /// </summary>
        DefineEditText = 37,
        /// <summary>
        /// 
        /// </summary>
        DefineSprite = 39,
        //SwfTools uses a tag code 40
        /// <summary>
        /// 
        /// </summary>
        FrameLabel = 43,
        /// <summary>
        /// 
        /// </summary>
        SoundStreamHead2 = 45,
        /// <summary>
        /// 
        /// </summary>
        DefineMorphShape = 46,
        /// <summary>
        /// 
        /// </summary>
        DefineFont2 = 48,
        /// <summary>
        /// 
        /// </summary>
        ExportAssets = 56,
        /// <summary>
        /// 
        /// </summary>
        ImportAssets = 57,
        /// <summary>
        /// 
        /// </summary>
        EnableDebugger = 58,
        /// <summary>
        /// 
        /// </summary>
        DoInitAction = 59,
        /// <summary>
        /// 
        /// </summary>
        DefineVideoStream = 60,
        /// <summary>
        /// 
        /// </summary>
        VideoFrame = 61,
        /// <summary>
        /// 
        /// </summary>
        DefineFontInfo2 = 62,
        /// <summary>
        /// 
        /// </summary>
        EnableDebugger2 = 64,
        /// <summary>
        /// 
        /// </summary>
        ScriptLimits = 65,
        /// <summary>
        /// 
        /// </summary>
        SetTabIndex = 66,
        /// <summary>
        /// 
        /// </summary>
        FileAttributes = 69,
        /// <summary>
        /// 
        /// </summary>
        PlaceObject3 = 70,
        /// <summary>
        /// 
        /// </summary>
        ImportAssets2 = 71,
        /// <summary>
        /// 
        /// </summary>
        DefineFontAlignZones = 73,
        /// <summary>
        /// 
        /// </summary>
        CSMTextSettings = 74,
        /// <summary>
        /// 
        /// </summary>
        DefineFont3 = 75,
        /// <summary>
        /// 
        /// </summary>
        SymbolClass = 76,
        /// <summary>
        /// 
        /// </summary>
        Metadata = 77,
        /// <summary>
        /// 
        /// </summary>
        DefineScalingGrid = 78,
        /// <summary>
        /// 
        /// </summary>
        DoABC = 82,
        /// <summary>
        /// 
        /// </summary>
        DefineShape4 = 83,
        /// <summary>
        /// 
        /// </summary>
        DefineMorphShape2 = 84,
        /// <summary>
        /// 
        /// </summary>
        DefineSceneAndFrameLabelData = 86,
        /// <summary>
        /// 
        /// </summary>
        DefineBinaryData = 87,
        /// <summary>
        /// 
        /// </summary>
        DefineFontName = 88,
        /// <summary>
        /// 
        /// </summary>
        StartSound2 = 89,
        /// <summary>
        /// 
        /// </summary>
        DefineBitsJPEG4 = 90,
        /// <summary>
        /// 
        /// </summary>
        DefineFont4 = 91,

        //
        // mxmlc tag types
        //
        /// <summary>
        /// 
        /// </summary>
        ProductID = 41,
        /// <summary>
        /// 
        /// </summary>
        DebugID = 63,/*,

        //
        // from Flex source
        //
        
        FreeCharacter = 3,
        StopSound = 16,
        PathsArePostScript = 25,
        SyncFrame = 29,
        FreeAll = 31,
        DefineVideo = 38,
        NameCharacter = 40,
        DefineTextFormat = 42,
        DefineBehavior = 44,
        FrameTag = 47,
        GenCommand = 49,
        DefineCommandObj = 50,
        CharacterSet = 51,
        FontRef = 52,
        DefineFunction = 53,
        PlaceFunction = 54,
        GenTagObject = 55,
        DefineShape4 = 67,
        DoABC = 72
        */

        // SwfTools Tags
        /// <summary>
        /// 
        /// </summary>
        ST_REFLEX = 777,
        /// <summary>
        /// 
        /// </summary>
        ST_GLYPHNAMES = 778
    }

    /// <summary>
    /// 
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt16 _type;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _length;

        /// <summary>
        /// 
        /// </summary>
        private UInt64 _offset;

        /// <summary>
        /// 
        /// </summary>
        private bool _longTag;

        /// <summary>
        /// 
        /// </summary>
        public TagTypes TagType
        {
            get
            {
                return (TagTypes)_type;
            }
            set
            {
                _type = (UInt16)value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Length
        {
            get { return _length; }
            set
            {
                _length = value;
                if (_length >= 0x3F)
                    _longTag = true;
                else
                    _longTag = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt64 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LongTag
        {
            get { return _longTag; }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt64 OffsetEnd
        {
            get
            {
                return (_offset + this.LengthTotal);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt64 OffsetData
        {
            get
            {
                if (_longTag)
                {
                    // long tag with 6 byte header
                    return _offset + 6;
                }
                else
                {
                    // short tag with 2 byte header
                    return _offset + 2;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UInt32 LengthTotal
        {
            get
            {
                if (_longTag)
                {
                    // a long tag has a 6 byte header 
                    return _length + 6;
                }
                else
                {
                    // a regular (short) tag has a 2 byte header
                    return _length + 2;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Stream Read(Stream input)
        {
            _offset = (UInt64)input.Position;

            BinaryReader br = new BinaryReader(input);
            UInt16 TagAndCode = br.ReadUInt16();
            _type = (UInt16)(TagAndCode >> 6);

            // A length field of 0x3F indicates that the next 4 bytes 
            // are a 32Bit lenght field (so-called long tag)
            UInt16 len = (UInt16)(TagAndCode & 0x3F);
            if (0x3F == len)
            {
                _length = br.ReadUInt32();
                _longTag = true;
            }
            else
            {
                _length = len;
                _longTag = false;
            }

            if (this.OffsetEnd > (UInt64)input.Length)
            {
                // TODO: Recheck
                // Verify the tag is entirely within the stream
                if (SwfFile.Configuration.HandleTagOversize == Configuration.HandleTagOversizeBy.RaiseError)
                {

                    SwfFormatException e = new SwfFormatException("The end of the tag " + (TagTypes)this._type + " : " + OffsetEnd.ToString() + " is beyond the end of the file stream : " + input.Length.ToString() + ". Header length was : " + SwfFile.HeaderDeclaredLength);
                    Log.Error(this, e);
                    throw e;
                }
                if (SwfFile.Configuration.HandleTagOversize == Configuration.HandleTagOversizeBy.ExpandStreamAndWipe)
                {
                    byte[] buffer = new byte[OffsetEnd];
                    long inputposition = input.Position;

                    input.Seek(0, SeekOrigin.Begin);
                    input.Read(buffer, 0, (int)inputposition);
                    MemoryStream ms = new MemoryStream(buffer);
                    input = ms;
                }

            }

            //if (SwfFile.Configuration.HandleTagOversize == Configuration.HandleTagOversizeBy.ExpandStream)
            //{
            //    throw new NotImplementedException("Implement me!");
            //}
            //
            // now place the stream position at the expected next tag
            //
            if (SwfFile.Configuration.SwfFileParsingMethod == Configuration.FileParsingMethod.Discontinuous)
            {
                input.Seek((long)this.OffsetEnd, SeekOrigin.Begin);
            }

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MemoryStream ReadContent(Stream input)
        {
            // save stream position for later restoration
            long previousPosition = input.Position;

            // go to the tag and read it
            input.Seek((long)this.OffsetData, SeekOrigin.Begin);

            BinaryReader br = new BinaryReader(input);
            byte[] tempBuffer = br.ReadBytes((int)this.Length);

            // restore previous stream position
            input.Seek(previousPosition, SeekOrigin.Begin);
            return new MemoryStream(tempBuffer);
        }

        /// <summary>
        /// 
        /// </summary>
        public string TagTypeName
        {
            get
            {
                foreach (FieldInfo fi in typeof(TagTypes).GetFields())
                {
                    if (fi.IsLiteral)
                    {
                        if (this.TagType == (TagTypes)fi.GetValue(null))
                            return fi.Name;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTagTypeKnown
        {
            get
            {
                return (null != this.TagTypeName);
            }
        }
    }
}
