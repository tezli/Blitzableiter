using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1Factory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="sourceVersion"></param>
        /// <returns></returns>
        public static AVM1.AbstractAction Create(BinaryReader sourceStream, byte sourceVersion)
        {
            AbstractAction product = null;

            byte firstByte = sourceStream.ReadByte();
            sourceStream.BaseStream.Seek(-1, SeekOrigin.Current);

            //
            // Obtain the name of the action
            //
            string actionName = ActionName((AVM1Actions)firstByte);

            if (null != actionName)
            {
                product = (AbstractAction)MethodBase.GetCurrentMethod().DeclaringType.Assembly.CreateInstance("Recurity.Swf.AVM1." + actionName);
            }
            else
            {
                AVM1ExceptionByteCodeFormat e = new AVM1ExceptionByteCodeFormat("Illegal ActionCode 0x" + firstByte.ToString("X"));
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                throw e;
            }

            product.Read(sourceStream, sourceVersion);

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static AVM1.AbstractAction Create(string actionName, string statement)
        {
            AbstractAction product = null;

            if ((null == actionName) || (null == statement))
            {
                ArgumentException e = new ArgumentException("actionName or statement is null");
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e);
                throw e;
            }

            product = (AbstractAction)MethodBase.GetCurrentMethod().DeclaringType.Assembly.CreateInstance("Recurity.Swf.AVM1." + actionName);

            if (null == product)
            {
                AVM1ExceptionSourceFormat e = new AVM1ExceptionSourceFormat(actionName + " is not a valid AVM1 action");
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e);
                throw e;
            }

            if (!product.ParseFrom(statement))
            {
                AVM1ExceptionSourceFormat e = new AVM1ExceptionSourceFormat("Illegal statement: " + statement);
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e);
                throw e;
            }

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal static string ActionName(AVM1Actions number)
        {
            return Enum.GetName(typeof(AVM1Actions), number);
        }
    }
}
