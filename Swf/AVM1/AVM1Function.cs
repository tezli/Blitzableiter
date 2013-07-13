using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// 
    /// </summary>
    public class AVM1Function
    {

        /// <summary>
        /// 
        /// </summary>
        private string _Name;

        /// <summary>
        /// 
        /// </summary>
        private List<UInt32> _Instructions;


        /// <summary>
        /// 
        /// </summary>
        public AVM1Function()
        {
            _Instructions = new List<uint>();
        }        

        /// <summary>
        /// 
        /// </summary>
        public IList<UInt32> Instructions
        {
            get
            {
                return _Instructions;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ContainsInstruction( UInt32 index )
        {
            for ( int i = 0; i < _Instructions.Count; i++ )
            {
                if ( _Instructions[ i ] == index )
                {
                    return true;
                }
            }
            return false;
        }        

        /// <summary>
        /// 
        /// </summary>
        public string Name
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
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( _Name ?? "[ERROR: _Name is null]" );
            sb.AppendFormat( " ({0:d} instructions)", _Instructions.Count );
            return sb.ToString();
        }
    }
}
