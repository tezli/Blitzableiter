using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A morph gradient
    /// </summary>
    class MorphGradient : AbstractSwfElement
    {
        private byte _numGradients;
        private MorphGradRecord[] _gradientRecords;

        /// <summary>
        /// <para>Swf 8 and later supports up to 15 gradient control points, spread modes and a new</para>
        /// <para>interpolation type.</para>
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public MorphGradient(byte InitialVersion) : base(InitialVersion) 
        {

        }

        /// <summary>
        /// The length of this object.
        /// </summary>
        public ulong Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public bool Verify()
        {
            return true;
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The stream to read from</param>
        public void Parse(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this._numGradients = br.ReadByte();

            if (1 > this._numGradients || 8 < this._numGradients)
            {
                SwfFormatException e = new SwfFormatException("Illegal number of gradients! The number of grad records must be between 1 and 8. This Gradient has " + this._numGradients + " grad records. Skipping.");
                Log.Warn(this, e.Message);
                throw e;
            }

            this._gradientRecords = new MorphGradRecord[this._numGradients];

            for (byte i = 0; i < this._numGradients; i++)
            {
                MorphGradRecord mg = new MorphGradRecord(this._SwfVersion);
                mg.Parse(input);
                this._gradientRecords[i] = mg;
            }
        }

        /// <summary>
        /// Writes this object back to a stream
        /// </summary>
        /// <param name="output">The stream to write to</param>
        public void Write(Stream output)
        {
            if (0 == this._numGradients)
            {
                Exception e = new Exception("There must be a least one MorphGradient!");
                Log.Warn(this, e.Message);
            }

            output.WriteByte(this._numGradients);

            for (int i = 0; i < this._numGradients; i++)
            {
                this._gradientRecords[i].Write(output);
            }
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
