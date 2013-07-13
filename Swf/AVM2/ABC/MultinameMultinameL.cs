using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class MultinameMultinameL : AbstractMultinameEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindId"></param>
        public MultinameMultinameL( byte kindId ) : base( kindId ) { }

        /// <summary>
        /// 
        /// </summary>
        public override uint NsSet
        {
            get
            {
                return _NsSet;
            }
            set
            {
                if ( 0 == value )
                {
                    AbcFormatException fe = new AbcFormatException( "NsSet cannot be 0 for MultinameL multinames" );
                    throw fe;
                }
                else
                {
                    _NsSet = value;
                }
            }
        }

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
        public override bool HasNsSet { get { return true; } }
    }
}
