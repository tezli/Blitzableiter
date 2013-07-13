using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    abstract public class AbstractInstruction
    {

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _MethodID;

        /// <summary>
        /// 
        /// </summary>
        protected AVM2OpCodes _OpCode;        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="opCode"></param>
        /// <param name="method"></param>
        public void Read( BinaryReader sourceStream, byte opCode, UInt32 method )
        {            
            _MethodID = method;
            Parse( sourceStream );
        }

        // this gets overwritten by instructions with additional arguments
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        protected virtual void Parse( BinaryReader sourceStream )
        {
            return;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual uint Length
        {
            get
            {
                return sizeof( byte );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write( Stream destination )
        {
            byte opCode = (byte)Enum.Parse( typeof( AVM2OpCodes ), this.GetType().Name, false );

            destination.WriteByte( opCode );

            WriteArgs( destination );
        }

        // overwritten by instructions with additional arguments
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public virtual void Verify( ABC.AbcFile abc )
        {
            return;
        }

        // overwritten by instructions with additional arguments
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        protected virtual void WriteArgs( Stream destination )
        {
            return;
        }

        // 
        // Gives the instruction's name, without the OP_ prefix
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetType().Name.Remove(0,3);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsBranch
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Int32[] BranchTarget
        {
            get
            {
                Int32[] nothing = new Int32[ 0 ];
                return nothing;
            }
        }
    }
}
