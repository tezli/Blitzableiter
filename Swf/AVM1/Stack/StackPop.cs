using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class StackPop : StackChange
    {
        /// <summary>
        /// 
        /// </summary>
        public StackPop()
        {
            this._DataType = AVM1DataTypes.AVM_UNKNOWN;
            this._Change = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtype"></param>
        public StackPop( AVM1DataTypes dtype )
        {
            this._DataType = dtype;
            this._Change = -1;
        }
    }
}
