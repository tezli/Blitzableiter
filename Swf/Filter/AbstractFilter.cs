using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractFilter : AbstractSwfElement
    {
        /// <summary>
        /// 
        /// </summary>
        public enum FilterTypes
        {

            /// <summary>
            /// 
            /// </summary>
            DropShadowFilter = 0,

            /// <summary>
            /// 
            /// </summary>
            BlurFilter = 1,

            /// <summary>
            /// 
            /// </summary>
            GlowFilter = 2,

            /// <summary>
            /// 
            /// </summary>
            BevelFilter = 3,

            /// <summary>
            /// 
            /// </summary>
            GradientGlowFilter = 4,

            /// <summary>
            /// 
            /// </summary>
            ConvolutionFilter = 5,

            /// <summary>
            /// 
            /// </summary>
            ColorMatrixFilter = 6,

            /// <summary>
            /// 
            /// </summary>
            GradientBevelFilter = 7
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public AbstractFilter( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public abstract void Parse( Stream input );

        /// <summary>
        /// 
        /// </summary>
        public abstract uint Length { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public abstract void Write( Stream output );
    }
}
