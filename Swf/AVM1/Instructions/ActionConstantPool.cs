using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM1
{
    /// <summary>
    /// Blitzableiter ActionConstantPool represents the Adobe AVM1 ActionConstantPool
    /// </summary>
    public class ActionConstantPool : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        protected UInt16 _numConstants;

        /// <summary>
        /// 
        /// </summary>
        protected List<string> _constants;

        #endregion

        #region constructors:

        /// <summary>
        /// Creates a new constant pool, and replaces the old constant pool if one already exists
        /// </summary>
        public ActionConstantPool()
        {
            this._numConstants = 0;
            this._constants = new List<string>();

            _StackOps = new StackChange[ 0 ];
        }

        /// <summary>
        /// Creates a new constant pool, and replaces the old constant pool if one already exists
        /// </summary>
        /// <param name="list">The list of constants</param>
        public ActionConstantPool( List<string> list ) : this()
        {            
            this._constants = list;
            this._numConstants = Convert.ToUInt16( list.Count );
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public UInt16 ConstantCount 
        { 
            get 
            { 
                return this._numConstants; 
            } 
            set 
            { 
                this._numConstants = value; 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Constants 
        { 
            get 
            { 
                return this._constants; 
            } 
            set 
            { 
                this._constants = value; 
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
                return 5; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="sourceVersion"></param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            if ( _length < 2 )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionConstantPool length < 2" );
            }

            _numConstants = sourceStream.ReadUInt16();
            _constants = new List<string>();
            for ( int i = 0; i < _numConstants; i++ )
            {
                string newConst = Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
                _constants.Add( newConst );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            UInt16 numberOfConstants = ( UInt16 )_constants.Count;

            outputStream.Write( numberOfConstants );

            for ( int i = 0; i < _constants.Count; i++ )
            {
                Helper.SwfStrings.SwfWriteString( this.Version, outputStream, _constants[ i ] );
            }

            return ( ulong )( outputStream.BaseStream.Position - pos );
        }

        /// <summary>
        /// Converts the action to a string 
        /// </summary>
        /// <returns>The action as string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            for ( int i = 0; i < _constants.Count; i++ )
            {                
                sb.AppendFormat( " {0:d}:'{1}'", i, _constants[ i ] );
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parses the action from a string array
        /// </summary>
        /// <param name="token">The action as string arry</param>
        /// <returns>True - If parsing was successful. False - If it was not</returns>
        protected override bool ParseFrom( params string[] token )
        {
            _constants = new List<string>();
            for ( int i = 0; i < token.Length; i++ )
            {
                _constants.Add( token[ i ] );
            }
            return true;
        }

        #endregion
    }
}
