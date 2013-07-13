using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Helper;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class PlaceObject2 : PlaceObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasClipActions;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasClipDepth;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasName;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasRatio;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasColorTransform;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasMatrix;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagHasCharacter;
        /// <summary>
        /// 
        /// </summary>
        protected bool _PlaceFlagMove;
        /// <summary>
        /// 
        /// </summary>
        protected CxFormWithAlpha _CxFormWithAlpha;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _Ratio;
        /// <summary>
        /// 
        /// </summary>
        protected string _Name;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _ClipDepth;
        /// <summary>
        /// 
        /// </summary>
        protected ClipActions _ClipActions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public PlaceObject2(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// 
        /// </summary>
        public override byte Version
        {
            get
            {
                return base.Version;
            }
            set
            {
                base.Version = value;
                if (null != _CxFormWithAlpha)
                    _CxFormWithAlpha.Version = base.Version;
                if (null != _ClipActions)
                    _ClipActions.Version = base.Version;
            }
        }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 3; }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                ulong ret = 0;

                ret += 1;   // flags
                ret += 2;   // Depth
                if (_PlaceFlagHasCharacter) ret += 2; // character ID
                if (_PlaceFlagHasMatrix) ret += base._TransformMatrix.Length;
                if (_PlaceFlagHasColorTransform) ret += _CxFormWithAlpha.Length;
                if (_PlaceFlagHasRatio) ret += 2; // _Ratio                
                if (_PlaceFlagHasName) ret += (ulong)Helper.SwfStrings.SwfStringLength(this.Version, _Name);
                if (_PlaceFlagHasClipDepth) ret += sizeof(UInt16);
                if (_PlaceFlagHasClipActions) ret += _ClipActions.Length;

                return ret;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public override bool Verify()
        {
            // CharacterID is only valid if flag is set
            if ((!_PlaceFlagHasCharacter) && (_CharacterID != 0))
            {
                return false;
            }

            // TODO: Make more verifications on PlaceObject2            

            // now, verify the code itself
            return VerifyAllCode();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            try
            {
                ParsePlaceObject2Flags();
                ParsePlaceObject2Data();
            }
            catch (SwfFormatException swfe)
            {
                throw swfe;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ParsePlaceObject2Flags()
        {
            BitStream bits = new BitStream(_dataStream);

            _PlaceFlagHasClipActions = (0 != bits.GetBits(1));
            _PlaceFlagHasClipDepth = (0 != bits.GetBits(1));
            _PlaceFlagHasName = (0 != bits.GetBits(1));
            _PlaceFlagHasRatio = (0 != bits.GetBits(1));
            _PlaceFlagHasColorTransform = (0 != bits.GetBits(1));
            _PlaceFlagHasMatrix = (0 != bits.GetBits(1));
            _PlaceFlagHasCharacter = (0 != bits.GetBits(1));
            _PlaceFlagMove = (0 != bits.GetBits(1));

            if (!(_PlaceFlagMove || _PlaceFlagHasCharacter))
            {
                SwfFormatException e = new SwfFormatException("Object is neither a creation (PlaceFlagHasCharacter) nor an update (PlaceFlagMove).");
                Log.Error(this, e.Message);
                throw e;
            }

            // Flag only exists for Swf5 and later
            if (_PlaceFlagHasClipActions && (this.Version < 5))
            {
                SwfFormatException e = new SwfFormatException("PlaceFlagHasClipActions in Swf Version " + this.Version.ToString());
                Log.Error(this, e);
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ParsePlaceObject2Data()
        {
            BinaryReader br = new BinaryReader(_dataStream);

            _Depth = br.ReadUInt16();

            if (_PlaceFlagHasCharacter)
            {
                _CharacterID = br.ReadUInt16();
            }

            if (_PlaceFlagHasMatrix)
            {
                base._TransformMatrix = new Matrix(this.Version);
                base._TransformMatrix.Parse(_dataStream);
            }

            if (_PlaceFlagHasColorTransform)
            {
                _CxFormWithAlpha = new CxFormWithAlpha(this.Version);
                _CxFormWithAlpha.Parse(_dataStream);
            }

            if (_PlaceFlagHasRatio)
            {
                _Ratio = br.ReadUInt16();
            }

            if (_PlaceFlagHasName)
            {
                _Name = Helper.SwfStrings.SwfString(this.Version, br);
            }

            if (_PlaceFlagHasClipDepth)
            {
                _ClipDepth = br.ReadUInt16();
            }

            if (_PlaceFlagHasClipActions)
            {
                if (this.Version < 5)
                {
                    throw new SwfFormatException("ClipActions in PlaceObject2 for Swf Version < 5");
                }
                _ClipActions = new ClipActions(this.Version);
                _ClipActions.Parse(_dataStream);
            }
            Log.Debug(this,  this.ToString() );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            this.WriteTagHeader(output);

            long pos = output.Position;

            BitStream bits = new BitStream(output);
            bits.WriteBits(1, (_PlaceFlagHasClipActions ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasClipDepth ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasName ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasRatio ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasColorTransform ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasMatrix ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagHasCharacter ? 1 : 0));
            bits.WriteBits(1, (_PlaceFlagMove ? 1 : 0));
            bits.WriteFlush();

            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(_Depth);

            if (_PlaceFlagHasCharacter)
                bw.Write(_CharacterID);

            if (_PlaceFlagHasMatrix)
                base._TransformMatrix.Write(output);

            if (_PlaceFlagHasColorTransform)
                _CxFormWithAlpha.Write(output);

            if (_PlaceFlagHasRatio)
                bw.Write(_Ratio);

            if (_PlaceFlagHasName)
                Helper.SwfStrings.SwfWriteString(this.Version, bw, _Name);

            if (_PlaceFlagHasClipDepth)
                bw.Write(_ClipDepth);

            if (this.Version >= 5)
            {
                if (_PlaceFlagHasClipActions)
                    _ClipActions.Write(output);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Recurity.Swf.AVM1.AVM1Code[] Code
        {
            get
            {
                List<AVM1.AVM1Code> l = new List<AVM1.AVM1Code>();

                if (_PlaceFlagHasClipActions)
                {
                    for (int i = 0; i < _ClipActions._ClipActionRecords.Count; i++)
                    {
                        l.Add(_ClipActions._ClipActionRecords[i].Code);
                    }
                }
                return l.ToArray();
            }
            set
            {
                throw new NotImplementedException("because I suck");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._CharacterID);
            sb.Append( " Flags: ClipActions=" ); sb.Append( _PlaceFlagHasClipActions );
            sb.Append( ", ClipDepth=" ); sb.Append( _PlaceFlagHasClipDepth );
            sb.Append( ", Name=" ); sb.Append( _PlaceFlagHasName );
            sb.Append( ", Ratio=" ); sb.Append( _PlaceFlagHasRatio );
            sb.Append( ", ColorTransform=" ); sb.Append( _PlaceFlagHasColorTransform );
            sb.Append( ", Matrix=" ); sb.Append( _PlaceFlagHasMatrix );
            sb.Append( ", CharacterID=" ); sb.Append( _PlaceFlagHasCharacter );
            sb.Append( ", Move=" ); sb.Append( _PlaceFlagMove );

            sb.AppendFormat( ", Depth={0:d}", _Depth );

            if (_PlaceFlagHasCharacter)
                sb.AppendFormat(", CharacterID={0:d}", _CharacterID);

            if (_PlaceFlagHasMatrix)
                sb.AppendFormat(", Matrix={0}", base._TransformMatrix.ToString());

            if (_PlaceFlagHasColorTransform)
                sb.AppendFormat(", CXFormWithAlpha={0}", _CxFormWithAlpha.ToString());

            if (_PlaceFlagHasRatio)
                sb.AppendFormat(", Ratio={0:d}", _Ratio);

            if (_PlaceFlagHasName)
                sb.AppendFormat(", Name={0}", _Name);

            if (_PlaceFlagHasClipDepth)
                sb.AppendFormat(", ClipDepth={0:d}", _ClipDepth);

            if (_PlaceFlagHasClipActions)
                sb.AppendFormat(", Actions={0:d}", _ClipActions._ClipActionRecords.Count);                

            return sb.ToString();
        }
    }
}
