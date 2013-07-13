using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public enum HandleTagOversizeBy : byte
    {
        /// <summary>
        /// 
        /// </summary>
        RaiseError = 0,

        /// <summary>
        /// 
        /// </summary>
        ExpandStreamAndWipe = 1,

        /// <summary>
        /// 
        /// </summary>
        ExpandStream = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HandleStreamOversizeBy : byte
    {
        /// <summary>
        /// 
        /// </summary>
        RaiseError = 0,

        /// <summary>
        /// 
        /// </summary>
        Wipe = 1,

        /// <summary>
        /// 
        /// </summary>
        Resize = 2,

        /// <summary>
        /// 
        /// </summary>
        Ignore = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HandleHeadSizeIssueBy : byte
    {
        /// <summary>
        /// 
        /// </summary>
        RaiseError = 0,

        /// <summary>
        /// 
        /// </summary>
        Fix = 1,

        /// <summary>
        /// 
        /// </summary>
        Ignore = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HandleReservedBitsBy : byte
    {
        /// <summary>
        /// 
        /// </summary>
        RaiseError = 0,

        /// <summary>
        /// 
        /// </summary>
        Fix = 1,

        /// <summary>
        /// 
        /// </summary>
        Ignore = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FileParsingMethod : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Discontinuous = 0,

        /// <summary>
        /// 
        /// </summary>
        Continuous = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The Settings to be seeked for in the configuration file. Unknown settings will be ignored. Not
        /// </summary>
        internal Dictionary<string, string> Settings { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        internal Dictionary<string, bool> _TagHandlers { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Config()
        {
            Settings = new Dictionary<string, string>()
            {
               {"AllowAVM2", "True" },
               {"AbcRemoveMetadata", "True"},
               {"AllowUnknownTagTypes", "True"},
               {"RequireFileAttributes", "True"},
               {"AllowMultipleFileAttributes", "True"},
               {"AllowInvalidMethodNames", "True"},
               {"FixMultipleFileAttributes", "True"},         
               {"AVM1DeleteInvalidBytecode", "True"},
               {"VersionMinimum","1"},
               {"VersionMaximum","10"},
               {"MinimumStageSizeX","10"},
               {"MaximumStageSizeX","10"},
               {"MinimumStageSizeY","10"},
               {"MaximumStageSizeY","10"},
               {"WriteVersion","9"},
               {"WriteCompressionLevel","9"},
               {"WriteCompressed", "True"},
               {"DumpTrailingGarbage", "True"},
               {"SwfFileParsingMethod", "Discontinuous"},
               {"HandleTagOversize", "RaiseError"},
               {"HandleStreamOversize", "RaiseError"},
               {"HandleHeadSizeIssue", "RaiseError"},
               {"HandleReservedBitsBy","RaiseError"}
            };

            this._TagHandlers = new Dictionary<string, bool>()
            {
                {"CSMTextSettings",true},
                {"DebugID",true},
                {"DefineBinaryData", true},
                {"DefineBits",true},
                {"DefineBitsJPEG2",true},
                {"DefineBitsJPEG3",true},
                {"DefineBitsLossless",true},
                {"DefineBitsLossless2",true},
                {"DefineButton",true},
                {"DefineButtonSound",true},
                {"DefineButton2",true},
                {"DefineEditText",true},
                {"DefineFont",true},
                {"DefineFont2",true},
                {"DefineFont3",true},
                {"DefineFont4",true},
                {"DefineFontInfo",true},
                {"DefineFontInfo2",true},
                {"DefineFontAlignZones",true},
                {"DefineFontName",true},
                {"DefineMorphShape",true},
                {"DefineMorphShape2",true},
                {"DefineScalingGrid",true},
                {"DefineSceneAndFrameLabelData",true},
                {"DefineShape",true},
                {"DefineShape2",true},
                {"DefineShape3",true},
                {"DefineShape4",true},
                {"DefineSprite",true},
                {"DefineSound",true},
                {"DefineText",true},
                {"DefineText2",true},
                {"DefineVideoStream",true},
                {"DoABC",true},
                {"DoAction",true},
                {"DoInitAction",true},
                {"EnableDebugger",true},
                {"EnableDebugger2",true},
                {"ExportAssets",true},
                {"FileAttributes",true},
                {"FrameLabel",true},
                {"ImportAssets",true},
                {"ImportAssets2",true},
                {"JPEGTables",true},
                {"Metadata",true},
                {"ProductID",true},
                {"Protect",true},
                {"PlaceObject",true},
                {"PlaceObject2",true},
                {"PlaceObject3",true},
                {"RemoveObject",true},
                {"RemoveObject2",true},
                {"VideoFrame",true},
                {"StartSound",true},
                {"ScriptLimits", true},
                {"StartSound2",true},
                {"SoundStreamHead",true},
                {"SoundStreamHead2",true},
                {"SoundStreamBlock",true},
                {"SetBackgroundColor",true},
                {"SetTabIndex",true},
                {"SymbolClass",true},
                {"ShowFrame",true},
                {"End",true}
            };

            System.Reflection.Assembly a = System.Reflection.Assembly.GetCallingAssembly();
            FileInfo assemblyLocation = new FileInfo(a.Location);
            string thisDirectory = Path.Combine(assemblyLocation.DirectoryName, "Configuration");
            string fileName = "blitzableiter.config";
            string configLocation = Path.Combine(thisDirectory, fileName);

            XElement theConfig = XElement.Load(configLocation);
            IEnumerable<XElement> configElements = theConfig.Descendants();

            foreach (XElement s in configElements)
            {
                string keyName = s.Name.LocalName;

                if (Settings.ContainsKey(keyName))
                {
                    string attributeValue = s.Attribute("value").Value;
                    Settings[keyName] = attributeValue;
                }

                if (keyName == "TagHandlers")
                {
                    IEnumerable<XElement> handlers = s.Descendants();

                    foreach (XElement t in handlers)
                    {
                        string handlername = t.Name.LocalName;

                        if (_TagHandlers.ContainsKey(handlername))
                        {
                            string attributeValue = t.Attribute("value").Value;
                            this._TagHandlers[handlername] = Convert.ToBoolean(attributeValue);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowAVM2
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AllowAVM2"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AbcRemoveMetadata
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AbcRemoveMetadata"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowUnknownTagTypes
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AllowUnknownTagTypes"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RequireFileAttributes
        {
            get
            {
                return Convert.ToBoolean(this.Settings["RequireFileAttributes"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowMultipleFileAttributes
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AllowMultipleFileAttributes"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool FixMultipleFileAttributes
        {
            get
            {
                return Convert.ToBoolean(this.Settings["FixMultipleFileAttributes"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AVM1DeleteInvalidBytecode
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AVM1DeleteInvalidBytecode"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte VersionMinimum
        {
            get
            {
                return Convert.ToByte(this.Settings["VersionMinimum"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte VersionMaximum
        {
            get
            {
                return Convert.ToByte(this.Settings["VersionMaximum"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int WriteCompressionLevel
        {
            get
            {
                return Convert.ToInt32(this.Settings["WriteCompressionLevel"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteCompressed
        {
            get
            {
                return Convert.ToBoolean(this.Settings["WriteCompressed"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte WriteVersion
        {
            get
            {
                return Convert.ToByte(this.Settings["WriteVersion"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DumpTrailingGarbage
        {
            get
            {
                return Convert.ToBoolean(this.Settings["DumpTrailingGarbage"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowInvalidMethodNames
        {
            get
            {
                return Convert.ToBoolean(this.Settings["AllowInvalidMethodNames"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MinimumStageSizeX
        {
            get
            {
                return Convert.ToUInt32(this.Settings["MinimumStageSizeX"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MinimumStageSizeY
        {
            get
            {
                return Convert.ToUInt32(this.Settings["MinimumStageSizeY"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MaximumStageSizeX
        {
            get
            {
                return Convert.ToUInt32(this.Settings["MaximumStageSizeX"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MaximumStageSizeY
        {
            get
            {
                return Convert.ToUInt32(this.Settings["MaximumStageSizeY"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FileParsingMethod SwfFileParsingMethod
        {
            get
            {
                if (this.Settings["SwfFileParsingMethod"] == "Continuous")
                {
                    return FileParsingMethod.Continuous;
                }
                else
                {
                    return FileParsingMethod.Discontinuous;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HandleStreamOversizeBy HandleStreamOversize
        {
            get
            {
                if (this.Settings["HandleStreamOversize"] == "Wipe")
                {
                    return HandleStreamOversizeBy.Wipe;
                }
                else if (this.Settings["HandleStreamOversize"] == "Resize")
                {
                    return HandleStreamOversizeBy.Resize;
                }
                else if (this.Settings["HandleStreamOversize"] == "Ignore")
                {
                    return HandleStreamOversizeBy.Ignore;
                }
                else if (this.Settings["HandleStreamOversize"] == "RaiseError")
                {
                    return HandleStreamOversizeBy.RaiseError;
                }
                else
                {
                    return HandleStreamOversizeBy.RaiseError;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HandleTagOversizeBy HandleTagOversize
        {
            get
            {
                if (this.Settings["HandleTagOversize"] == "ExpandStream")
                {
                    return HandleTagOversizeBy.ExpandStream;
                }
                else if (this.Settings["HandleTagOversize"] == "ExpandStreamAndWipe")
                {
                    return HandleTagOversizeBy.ExpandStreamAndWipe;
                }
                else if (this.Settings["HandleTagOversize"] == "RaiseError")
                {
                    return HandleTagOversizeBy.RaiseError;
                }
                else
                {
                    return HandleTagOversizeBy.RaiseError;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HandleHeadSizeIssueBy HandleHeadSizeIssue
        {
            get
            {
                if (this.Settings["HandleHeadSizeIssue"] == "Fix")
                {
                    return HandleHeadSizeIssueBy.Fix;
                }
                else if (this.Settings["HandleHeadSizeIssue"] == "Ignore")
                {
                    return HandleHeadSizeIssueBy.Ignore;
                }
                else if (this.Settings["HandleHeadSizeIssue"] == "RaiseError")
                {
                    return HandleHeadSizeIssueBy.RaiseError;
                }
                else
                {
                    return HandleHeadSizeIssueBy.RaiseError;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HandleReservedBitsBy HandleReservedBits
        {
            get
            {
                if (this.Settings["HandleReservedBits"] == "Fix")
                {
                    return HandleReservedBitsBy.Fix;
                }
                else if (this.Settings["HandleReservedBits"] == "Ignore")
                {
                    return HandleReservedBitsBy.Ignore;
                }
                else if (this.Settings["HandleReservedBits"] == "RaiseError")
                {
                    return HandleReservedBitsBy.RaiseError;
                }
                else
                {
                    return HandleReservedBitsBy.RaiseError;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool TagHandlers(string s)
        {

            return this._TagHandlers[s];
        }

    }
}
