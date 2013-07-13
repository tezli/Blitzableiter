using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionGetURL represents the Adobe AVM1 ActionGetURL
    /// </summary>
    public class ActionGetURL : AbstractAction
    {
        #region fields:

        /// <summary>
        /// The URL
        /// </summary>
        protected string _URL;
        /// <summary>
        /// The target
        /// </summary>
        protected string _target;

        #endregion

        #region constructors:
        /// <summary>
        /// Instructs Flash Player to get the URL that 
        /// UrlString specifies. The URL can be of any type, 
        /// including an HTML file, an image or another 
        /// Swf file. If the file is playing in a browser, 
        /// the URL is displayed in the frame that TargetString 
        /// specifies. The "_level0" and "_level1" special 
        /// target names are used to load another Swf file 
        /// into levels 0 and 1 respectively
        /// </summary>
        public ActionGetURL()
        {
            this._URL = "";
            this._target = "";

            _StackOps = new StackChange[ 0 ];
        }
        /// <summary>
        /// instructs Flash Player to get the URL that 
        /// UrlString specifies. The URL can be of any type, 
        /// including an HTML file, an image or another 
        /// Swf file. If the file is playing in a browser, 
        /// the URL is displayed in the frame that TargetString 
        /// specifies. The "_level0" and "_level1" special 
        /// target names are used to load another Swf file 
        /// into levels 0 and 1 respectively
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="target">The target (_self,_blank,_parent,_top)</param>
        public ActionGetURL( string URL, string target ) : this()
        {
            this._URL = URL;
            this._target = target;
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public string URL 
        { 
            get 
            { 
                return this._URL; 
            } 
            set 
            { 
                this._URL = value; 
            } 
        }

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
        /// Parses URL and target from the source stream
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _URL = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
            _target = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
        }

        /// <summary>
        /// Renders URL and target back to the output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>URL and target as string</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            return (
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _URL ) +
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _target )
            );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            sb.AppendFormat( " URL:'{0}', Target:'{1}'", _URL, _target );
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            if ( token.Length < 2 )
                return false;
            _URL = token[ 0 ];
            _target = token[ 1 ];
            return true;
        }

        #endregion
    }
}
