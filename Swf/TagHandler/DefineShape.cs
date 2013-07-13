using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// <para>The DefineShape tag defines a shape for later use by control tags such as PlaceObject. The</para>
    /// <para>ShapeId uniquely identifies this shape as ‘character’ in the Dictionary. The ShapeBounds field</para>
    /// <para>is the rectangle that completely encloses the shape. The SHAPEWITHSTYLE structure</para>
    /// <para>includes all the paths, fill styles and line styles that make up the shape.</para>
    /// </summary>
    public class DefineShape : AbstractTagHandler, Helper.ISwfCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt16 _shapeID;

        /// <summary>
        /// 
        /// </summary>
        public Rect _shapeBounds;

        /// <summary>
        /// 
        /// </summary>
        public ShapeWithStyle _shapes;

        /// <summary>
        /// The DefineShape tag defines a shape for later use by control tags such as PlaceObject.
        /// </summary>
        /// <param name="InitialVersion">The minimum version of the Swf file using this tag.</param>
        public DefineShape(byte InitialVersion) : base(InitialVersion)
        {
            this._shapeID = 0;
            this._shapeBounds = new Rect(this._SwfVersion);
            this._shapes = new ShapeWithStyle(this._SwfVersion);
        }

        /// <summary>
        /// Character ID of the defined character
        /// </summary>
        public UInt16 CharacterID
        {
            get
            {
                return this._shapeID;
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
                uint ret = 0;
                using (MemoryStream temp = new MemoryStream())
                {
                    BinaryWriter bw = new BinaryWriter(temp);
                    bw.Write(this._shapeID);
                    this._shapeBounds.Write(temp);
                    this._shapes.Write(temp);

                    ret = (uint)temp.Position;
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
            // TODO: Do proper check here
            //if (this._shapeBounds.Verify() && this._shapes.Verify())
            //{
                return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        protected override void Parse()
        {
            BinaryReader br = new BinaryReader(this._dataStream);
            this._shapeID = br.ReadUInt16();
            this._shapeBounds.Parse(this._dataStream);

            Int64 shapesLength = this._dataStream.Length - this._dataStream.Position;
            this._shapes.Parse(this._dataStream, shapesLength, this._tag.TagType);  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            this.WriteTagHeader(output);
            bw.Write(this._shapeID);
            this._shapeBounds.Write(output);
            this._shapes.Write(output);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this._tag.TagType.ToString());
            sb.AppendFormat(" Character ID : {0:d}", this._shapeID);
            return sb.ToString();
        }

    }
}
