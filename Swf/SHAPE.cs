using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Interfaces;

namespace Recurity.Swf
{
    /// <summary>
    /// The SHAPE structure defines a shape without fill style or line style information.
    /// </summary>
    public class Shape : AbstractSwfElement
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
        /// The SHAPE structure defines a shape without fill style or line style information.
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public Shape(byte InitialVersion)
            : base(InitialVersion)
        {
            this._shapeRecordBuffer = new byte[0];
            this._shapeRecords = new List<ShapeRecord>();
            this._fillStyles = new FillStyleArray(this._SwfVersion);
            this._lineStyles = new LineStyleArray(this._SwfVersion);

        }

        /// <summary>
        /// Verifies a SHAPE.
        /// </summary>
        /// <returns>True if the SHAPE has been verified.</returns>
        public virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Gets the length of this object in bytes
        /// </summary>
        public virtual UInt64 Length
        {
            get
            {
                using (MemoryStream temp = new MemoryStream())
                {

                }
                return 0;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<ShapeRecord> ShapeRecords
        {
            get
            {
                return this._shapeRecords;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="caller"></param>
        public virtual void Parse(Stream input, Int64 length, TagTypes caller)
        {

            BitStream bits = new BitStream(input);

            this._numFillBits = (UInt16)bits.GetBits(4);
            this._numLineBits = (UInt16)bits.GetBits(4);

            if (length >= 1)
            {

                this._shapeRecordBuffer = new Byte[length - 1];
                input.Read(this._shapeRecordBuffer, 0, (Int32)length - 1);
                this._shapeRecordStream = new MemoryStream();

                this._shapeRecordStream.Write(this._shapeRecordBuffer, 0, this._shapeRecordBuffer.Length); // To protect the input stream
                this._shapeRecordStream.Seek(0, SeekOrigin.Begin);
                this.TryParseShapeRecords(this._shapeRecordStream, caller);

            }
            else
            {
                Log.Warn(this, "Attempt to parse shapes out of a too small field by: " + caller + ". Length was: " + length);
            }

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
        /// Writes this object back to a stream.
        /// </summary>
        /// <param name="output">The stream to write to.</param>
        public virtual void Write(Stream output)
        {
            BitStream bits = new BitStream(output);

            bits.WriteBits(4, (Int32)this._numFillBits);
            bits.WriteBits(4, (Int32)this._numLineBits);
            bits.WriteFlush();

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
