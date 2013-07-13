using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class MultinameRTQnameL : AbstractMultinameEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindId"></param>
        public MultinameRTQnameL( byte kindId ) : base( kindId ) { }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNamespace { get { return false; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool HasNameIndex { get { return false; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool HasNsSet { get { return false; } }
    }
}
