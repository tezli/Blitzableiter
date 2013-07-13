using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class GradientBevelFilter : GradientGlowFilter
    {
        internal new FilterTypes _FilterType = FilterTypes.GradientBevelFilter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public GradientBevelFilter( byte InitialVersion ) : base( InitialVersion ) { }
    }
}
