using System;
using System.Collections.Generic;
using Recurity.Swf.Interfaces;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// <para>The SHAPEWITHSTYLE structure extends the SHAPE structure by including fill style and</para>
    /// <para>line style information. SHAPEWITHSTYLE is used by the DefineShape tag.</para>
    /// <para>The story of byte aligness is the following. Structures are byte aligned if they only read byte aligned</para>
    /// <para>structures or if the last read opreration has been an integer type or an other byte aligned structure. Sounds </para>
    /// <para>complicated but it isn't.</para>
    /// <para>&#160;</para>
    /// <para>Lets assume Stream.Postion is 0 and BitStream.Position is 0.</para>
    /// <para>&#160;</para>
    /// <para>we read 3 bits:      bits.GetBits( 3 ); Stream.Postion is 1 and BitStream.Position is 3</para>
    /// <para>we read a byte:      br.ReadByte();     Stream.Postion is 2 and BitStream.Position is 3</para>
    /// <para>we read 3 bits:      bits.GetBits( 3 ); Stream.Postion is 3 and BitStream.Position is 6 </para>
    /// <para>&#160;</para>
    /// <para>but after reading a byte aligned structure the first 3 bits should be {0,1,2}.</para>
    /// <para>That is the reason why we have to reset the BitStream.Position before or after we red a byte aligned</para>
    /// <para>structure.</para>
    /// </summary>
    public class ShapeWithStyle : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numFillBits;
        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numLineBits;
        /// <summary>
        /// 
        /// </summary>
        protected FillStyleArray _fillStyles;
        /// <summary>
        /// 
        /// </summary>
        protected LineStyleArray _lineStyles;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] _shapeRecordBuffer;
        /// <summary>
        /// 
        /// </summary>
        protected List<ShapeRecord> _shapeRecords;
        /// <summary>
        /// 
        /// </summary>
        protected MemoryStream _shapeRecordStream;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public ShapeWithStyle(byte InitialVersion): base(InitialVersion)
        {
            this._shapeRecordBuffer = new byte[0];
            this._shapeRecords = new List<ShapeRecord>();
            this._fillStyles = new FillStyleArray(this._SwfVersion);
            this._lineStyles = new LineStyleArray(this._SwfVersion);
        }

        /// <summary>
        /// The length of this object.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the tag is documentation compliant.</returns>
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="caller"></param>
        public virtual void TryParseShapeRecords(MemoryStream input, TagTypes caller)
        {
            BitStream bits = new BitStream(input);

            this._shapeRecords = new List<ShapeRecord>();

            StyleChangeRecord tempSCR = new StyleChangeRecord(this._SwfVersion);
            StraightEdgeRecord tempSER = new StraightEdgeRecord(this._SwfVersion);
            CurvedEdgeRecord tempCER = new CurvedEdgeRecord(this._SwfVersion);

            byte fiveBits = 0;
            bool endShapeRecordSeen = false;
            int numberOfStyleChanges = 0;

            UInt16 localNumFillBits = this._numFillBits;
            UInt16 localNumLineBits = this._numLineBits;

            do
            {
                bool typeFlag = (0 != bits.GetBits(1)) ? true : false;

                if (typeFlag)
                {
                    bool straightFlag = Convert.ToBoolean(bits.GetBits(1));

                    if (straightFlag)
                    {
                        tempSER = new StraightEdgeRecord(this._SwfVersion);
                        tempSER.Parse(input, bits);
                        this._shapeRecords.Add(tempSER);
                    }
                    else
                    {
                        tempCER = new CurvedEdgeRecord(this._SwfVersion);
                        tempCER.Parse(input, bits);
                        this._shapeRecords.Add(tempCER);
                    }
                }
                else
                {
                    fiveBits = (byte)bits.GetBits(5);

                    if (0 == fiveBits)
                    {
                        endShapeRecordSeen = true;
                    }
                    else
                    {
                        tempSCR = new StyleChangeRecord(this._SwfVersion);
                        tempSCR.Parse(input, bits, fiveBits, caller, ref localNumFillBits, ref localNumLineBits, 0 == numberOfStyleChanges ? true : false);
                        numberOfStyleChanges += 1;
                        this._shapeRecords.Add(tempSCR);
                    }
                }
            }
            while (!endShapeRecordSeen);
            bits.Reset();
        }

        /// <summary>
        /// Parses this object out of a stream.
        /// </summary>
        /// <param name="input">The input Stream</param>
        /// <param name="length">The length of the ShapeRecords</param>
        /// <param name="caller">The tag that calls this method</param>
        public void Parse(Stream input, long length, TagTypes caller)
        {

            BitStream bits = new BitStream(input);

            this._fillStyles.Parse(input, caller);
            this._lineStyles.Parse(input, caller);

            this._numFillBits = (UInt16)bits.GetBits(4);
            this._numLineBits = (UInt16)bits.GetBits(4);

            this._shapeRecordBuffer = new Byte[length - 1];
            input.Read(this._shapeRecordBuffer, 0, (Int32)length - 1);

            // (Un)comment these lines to disbale/enable ShapeRecord pasing
            this._shapeRecordStream = new MemoryStream();
            this._shapeRecordStream.Write(this._shapeRecordBuffer, 0, this._shapeRecordBuffer.Length); // To protect the input stream
            this._shapeRecordStream.Seek(0, SeekOrigin.Begin);
            this.TryParseShapeRecords(this._shapeRecordStream, caller);

        }

        /// <summary>
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public void Write(Stream output)
        {
            this._fillStyles.Write(output);
            this._lineStyles.Write(output);

            BitStream bits = new BitStream(output);

            bits.WriteBits(4, (Int32)this._numFillBits);
            bits.WriteBits(4, (Int32)this._numLineBits);

            long startPosition = output.Position;

            output.Write(this._shapeRecordBuffer, 0, this._shapeRecordBuffer.Length);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            return sb.ToString();
        }

    }
}
