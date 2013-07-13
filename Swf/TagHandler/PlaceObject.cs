using System;
using System.Text;
using Recurity.Swf.Helper;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class PlaceObject : AbstractTagCodeHandler, ISwfCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _CharacterID;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _Depth;
        /// <summary>
        /// 
        /// </summary>
        protected Matrix _TransformMatrix;
        /// <summary>
        /// 
        /// </summary>
        protected CxForm _ColorTransform;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public PlaceObject(byte InitialVersion) : base(InitialVersion)
        {
            this._TransformMatrix = new Matrix(this._SwfVersion);
            this._ColorTransform = new CxForm(this._SwfVersion);
        }

        /// <summary>
        /// Character ID of the definition
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return _CharacterID;
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
        /// TODO : Calulcate length
        /// </summary>
        public override ulong Length
        {
            get 
            {
                ulong ret = 0;

                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(ms);

                    bw.Write(this._CharacterID);
                    bw.Write(this._Depth);

                    this._TransformMatrix.Write(ms);
                    this._ColorTransform.Write(ms);

                    ret = (ulong)ms.Position;
                }

                return ret;
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
        /// 
        /// </summary>
        protected override void Parse()
        {
            try
            {
                BinaryReader br = new BinaryReader(this._dataStream);

                this._CharacterID = br.ReadUInt16();
                this._Depth = br.ReadUInt16();

                this._TransformMatrix.Parse(this._dataStream);
                this._ColorTransform.Parse(this._dataStream);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Writes this object to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            this.WriteTagHeader(output);
            bw.Write(this._CharacterID);
            bw.Write(this._Depth);

            this._TransformMatrix.Write(output);
            this._ColorTransform.Write(output);

        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat("Character ID: {0}, Depth: {1}", this._CharacterID, this._Depth);
            return sb.ToString();

        }
    }
}
