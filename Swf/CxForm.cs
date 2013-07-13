using System;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// The CXFORM record defines a simple transform that can be applied to the color space of a graphic object
    /// </summary>
    public class CxForm : AbstractSwfElement
    {
        private Boolean _HasAddTerms;
        private Boolean _HasMultTerms;
        private Byte _Nbits;

        private Int16 _RedMultTerm;
        private Int16 _GreenMultTerm;
        private Int16 _BlueMultTerm;

        private Int16 _RedAddTerm;
        private Int16 _GreenAddTerm;
        private Int16 _BlueAddTerm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public CxForm(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The input stream.</param>
        internal void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            BitStream bs = new BitStream(input);

            this._HasAddTerms = 0 != bs.GetBits(1) ? true : false;
            this._HasMultTerms = 0 != bs.GetBits(1) ? true : false;

            this._Nbits = (Byte)bs.GetBits(4);

            this._RedMultTerm = (Byte)bs.GetBits(this._Nbits);
            this._GreenMultTerm = (Byte)bs.GetBits(this._Nbits);
            this._BlueMultTerm = (Byte)bs.GetBits(this._Nbits);

            this._RedAddTerm = (Byte)bs.GetBits(this._Nbits);
            this._GreenAddTerm = (Byte)bs.GetBits(this._Nbits);
            this._BlueAddTerm = (Byte)bs.GetBits(this._Nbits);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        internal void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            BitStream bs = new BitStream(output);

            bs.WriteBits(1, false == this._HasAddTerms ? 0 : 1);
            bs.WriteBits(1, false == this._HasMultTerms ? 0 : 1);

            bs.WriteBits(4, this._Nbits );

            bs.WriteBits(this._Nbits, this._RedMultTerm);
            bs.WriteBits(this._Nbits, this._GreenMultTerm);
            bs.WriteBits(this._Nbits, this._BlueMultTerm);

            bs.WriteBits(this._Nbits, this._RedAddTerm);
            bs.WriteBits(this._Nbits, this._GreenAddTerm);
            bs.WriteBits(this._Nbits, this._BlueAddTerm);
            bs.WriteFlush();
        }
    }
}
