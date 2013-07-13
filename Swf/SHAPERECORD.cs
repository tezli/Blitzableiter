using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf.Interfaces;

namespace Recurity.Swf
{
    /// <summary>
    /// Abstract class from which all SHAPERECORDs derive
    /// </summary>
    public abstract class ShapeRecord : AbstractSwfElement
    {
        /// <summary>
        /// Abstract class from which all SHAPERECORDs derive
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ShapeRecord(byte InitialVersion) : base(InitialVersion)
        {

        }

        /// <summary>
        /// Verifies a SHAPE.
        /// </summary>
        /// <returns>True if the SHAPE has been verified.</returns>
        public virtual bool Verify()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets the length of this object in bytes
        /// </summary>
        public virtual uint Length
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        public virtual void Parse(Stream input, BitStream bits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="bits"></param>
        public virtual void Write(Stream output, BitStream bits)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
