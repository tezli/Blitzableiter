using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class StackChange
    {
        /// <summary>
        /// 
        /// </summary>
        protected AVM1DataTypes _DataType;

        /// <summary>
        /// 
        /// </summary>
        protected int _Change;

        /// <summary>
        /// 
        /// </summary>
        protected StackChange()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public int Change
        {
            get
            {
                return _Change;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AVM1DataTypes DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }
    }
}
