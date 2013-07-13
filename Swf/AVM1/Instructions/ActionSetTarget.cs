using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionSetTarget represents the Adobe AVM1 ActionSetTarget
    /// </summary>
    public class ActionSetTarget : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected string _target;
        #endregion

        #region constructors:

        /// <summary>
        /// Instructs Flash Player to change the context of subsequent 
        /// actions, so they apply to a named object (TargetName) rather 
        /// than the current file
        /// </summary>
        public ActionSetTarget()
        {
            this._target = "";
            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Instructs Flash Player to change the context of subsequent 
        /// actions, so they apply to a named object (TargetName) rather 
        /// than the current file
        /// </summary>
        /// <param name="target">The target</param>
        public ActionSetTarget( string target ) : this()
        {
            this._target = target;
        }

        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public string Target 
        { 
            get 
            { 
                return this._target; 
            } 
            set 
            { 
                this._target = value; 
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
        /// Parses the target from a source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _target = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
        }

        /// <summary>
        /// Renders the target back to the output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>Helper.SwfStrings.SwfWriteString(version, outputStream, _target)</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            return Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _target );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " Target:{0}", _target );
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
            _target = token[ 0 ];
            return true;
        }

        #endregion
    }
}
