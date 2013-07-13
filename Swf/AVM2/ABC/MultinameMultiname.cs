using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class MultinameMultiname : AbstractMultinameEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindId"></param>
        public MultinameMultiname( byte kindId ) : base( kindId ) { }

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
                    AbcFormatException fe = new AbcFormatException( "NsSet cannot be 0 for Multiname multinames" );
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
        public override bool HasNameIndex { get { return true; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool HasNsSet { get { return true; } }
    }
}
