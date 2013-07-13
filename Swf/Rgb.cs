using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// The RGB record represents a color as a 24-bit red, green, and blue value.
    /// </summary>
    public class Rgb : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        internal byte _Red;
        /// <summary>
        /// 
        /// </summary>
        internal byte _Blue;
        /// <summary>
        /// 
        /// </summary>
        internal byte _Green;
        /// <summary>
        /// 
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// The RGB record represents a color as a 24-bit red, green, and blue value.
        /// </summary>
        /// <param name="InitialVersion">The version of the swf file using this object</param>
        public Rgb(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// The length of this object in bytes.
        /// </summary>
        public virtual uint Length
        {
            get
            {
                return (sizeof(byte) * 3);
            }
        }

        /// <summary>
        /// Verifies this object and its components for documentation compliance.
        /// </summary>
        /// <returns>True if the object is documentation compliant.</returns>
        public virtual bool Verify()
        {
            return true; // nothing to verify here
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="input">The input stream.</param>
        public virtual void Parse(Stream input)
        {
            
            BinaryReader br = new BinaryReader(input);
            _Red = br.ReadByte();
            _Green = br.ReadByte();
            _Blue = br.ReadByte();
            //String s = string.Format("#{0:X2}{1:X2}{2:X2}",_Red, _Green, _Blue);
            //Log.Debug(this, "[Begin Structure] RGB : color=" + s);
            //Log.Debug(this, "[End Structure] RGB" );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public virtual void Write(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);
            bw.Write(_Red);
            bw.Write(_Green);
            bw.Write(_Blue);
        }

        /// <summary>
        /// Converts the value of this instance to a System.String.
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat(" R:{0:d} G:{1:d} B:{2:d}", _Red, _Green, _Blue);
            return sb.ToString();
        }
    }
}
