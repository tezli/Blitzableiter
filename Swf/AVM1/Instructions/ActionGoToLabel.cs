using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGoToLabel represents the Adobe AVM1 ActionGoToLabel
    /// </summary>
    public class ActionGoToLabel : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected string _label;
        #endregion

        #region constructors:

        /// <summary>
        /// Instructs Flash Player to go to the frame associated with the 
        /// specified label. You can attach a label to a frame with the 
        /// FrameLabel tag
        /// </summary>
        public ActionGoToLabel()
        {
            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Instructs Flash Player to go to the frame associated with the 
        /// specified label. You can attach a label to a frame with the 
        /// FrameLabel tag
        /// </summary>
        /// <param name="label">The label name</param>
        public ActionGoToLabel( string label ) : this()
        {
            this._label = label;
        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public string Label 
        { 
            get 
            { 
                return this._label; 
            } 
            set 
            { 
                this._label = value; 
            } 
        }

        #endregion

        #region code:

        /// <summary>
        /// The minimum version that is required for the action
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get 
            { 
                return 3; 
            }
        }

        /// <summary>
        /// Reads the label to jump to from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _label = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
        }

        /// <summary>
        /// Renders the label to jump to back to an output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>Helper.SwfStrings.SwfWriteString(version, outputStream, _label)</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            return Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _label );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " {0}", _label );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            if ( token.Length != 1 )
                return false;
            _label = token[ 0 ];
            return true;
        }

        #endregion
    }
}
