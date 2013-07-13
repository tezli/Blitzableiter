using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class StackPush : StackChange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtype"></param>
        public StackPush( AVM1DataTypes dtype )
        {
            this._DataType = dtype;
            this._Change = 1;
        }        
    }
}
