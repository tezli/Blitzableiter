using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public enum AdobeProductType : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 
        /// </summary>
        MacromediaFlexForJ2EE = 1,
        /// <summary>
        /// 
        /// </summary>
        MacromediaFlexForDotNET = 2,
        /// <summary>
        /// 
        /// </summary>
        AdobeFlex = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AdobeProductEdition : uint
    {
        /// <summary>
        /// 
        /// </summary>
        DeveloperEdition = 0,
        /// <summary>
        /// 
        /// </summary>
        FullCommercialEdition = 1,
        /// <summary>
        /// 
        /// </summary>
        NonCommercialEdition = 2,
        /// <summary>
        /// 
        /// </summary>
        EducationalEdition = 3,
        /// <summary>
        /// 
        /// </summary>
        NotForResaleEdition = 4,
        /// <summary>
        /// 
        /// </summary>
        TrialEdition = 5,
        /// <summary>
        /// 
        /// </summary>
        None = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductID : AbstractTagHandler
    {
        private AdobeProductType _ProductType;
        /// <summary>
        /// 
        /// </summary>
        private AdobeProductEdition _ProductEdition;
        /// <summary>
        /// 
        /// </summary>
        private byte _MajorVersion;
        /// <summary>
        /// 
        /// </summary>
        private byte _MinorVersion;
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _BuildLow;
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _BuildHigh;
        /// <summary>
        /// 
        /// </summary>
        private UInt64 _CompileTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialVersion"></param>
        public ProductID(byte initialVersion) : base(initialVersion) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get
            {
                // TODO: we actually don't know
                return 9;
            }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                //
                // * ProductID (UI32)                
                // * Edition (UI32)
                // * MajorVersion (UI8)
                // * MinorVersion (UI8)
                // * BuildLow (UI32)
                // * BuildHigh (UI32)
                // * CompilationDate (UI64)      
                //

                return 4 * sizeof(UInt32) + 2 * sizeof(byte) + sizeof(UInt64);

                // return 24;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            //
            // Not much to verify
            //
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(_dataStream);

            UInt32 pt = br.ReadUInt32();
            if (!Enum.IsDefined(typeof(AdobeProductType), pt))
            {
                SwfFormatException sfe = new SwfFormatException("Invalid product type: " + pt.ToString("d"));
                Log.Error(this, sfe);
                throw sfe;
            }
            _ProductType = (AdobeProductType)pt;

            UInt32 pe = br.ReadUInt32();
            if (!Enum.IsDefined(typeof(AdobeProductEdition), pe))
            {
                SwfFormatException sfe = new SwfFormatException("Invalid product edition: " + pe.ToString("d"));
                Log.Error(this, sfe);
                throw sfe;
            }
            _ProductEdition = (AdobeProductEdition)pe;

            _MajorVersion = br.ReadByte();
            _MinorVersion = br.ReadByte();

            _BuildLow = br.ReadUInt32();
            _BuildHigh = br.ReadUInt32();
            _CompileTime = br.ReadUInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(System.IO.Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            WriteTagHeader(output);

            //            bw.Write((byte)_ProductType);
            //            bw.Write((byte)_ProductEdition);
            bw.Write((UInt32)_ProductType);
            bw.Write((UInt32)_ProductEdition);
            bw.Write(_MajorVersion);
            bw.Write(_MinorVersion);
            bw.Write(_BuildLow);
            bw.Write(_BuildHigh);
            bw.Write(_CompileTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);

            sb.AppendFormat("Adobe Product Information: {0}({1}) Ver. {2}.{3}, Build {4}, Compiled {5}",
                Enum.GetName(typeof(AdobeProductType), _ProductType),
                Enum.GetName(typeof(AdobeProductEdition), _ProductEdition),
                _MajorVersion,
                _MinorVersion,
                (UInt64)(((ulong)_BuildHigh << (int)32) | (ulong)_BuildLow),
                dt.AddSeconds(_CompileTime).ToShortDateString());

            return sb.ToString();
        }
    }
}
