using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class MultinameQname : AbstractMultinameEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindId"></param>
        public MultinameQname( byte kindId ) : base( kindId ) { }

        /// <summary>
        /// 
        /// </summary>
        public override uint Namespace
        {
            get
            {
                return _Namespace;
            }
            set
            {
                _Namespace = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint NameIndex
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNamespace { get { return true; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool HasNameIndex { get { return true; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool HasNsSet { get { return false; } }
    }
}
