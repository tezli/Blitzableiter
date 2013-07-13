using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf
{
    /// <summary>
    /// Class that represents an element that can appear in Swf files or the Swf files themselves implementing the ISwfElement
    /// </summary>
    public class AbstractSwfElement : Recurity.Swf.Interfaces.ISwfElement
    {

        /// <summary>
        /// 
        /// </summary>
        protected byte _SwfVersion;

        /// <summary>
        /// 
        /// </summary>
        protected const byte _MaximumSwfVersion = 11;

        /// <summary>
        /// Class that represents a Swf file implementing the ISwfElement
        /// </summary>
        /// <param name="InitialVersion">The version of the element</param>
        public AbstractSwfElement( byte InitialVersion )
        {
            if ( InitialVersion > _MaximumSwfVersion )
            {
                throw new ArgumentOutOfRangeException( "Swf version > 10 invalid" );
            }

            _SwfVersion = InitialVersion;
        }

        #region ISwfElement Members


        /// <summary>
        /// 
        /// </summary>
        public virtual byte Version
        {
            get
            {
                return _SwfVersion;
            }
            set
            {
                _SwfVersion = value;
            }
        }

        #endregion
    }
}
