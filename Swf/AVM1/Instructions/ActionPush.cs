using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM1
{    
    /// <summary>
    /// Blitzableiter ActionPush represents the Adobe AVM1 ActionPush
    /// </summary>
    public class ActionPush : AbstractAction
    {
        #region fields:

        /// <summary>
        /// 
        /// </summary>
        private List<AVM1DataElement> _arguments;

        #endregion

        #region constructors:
        /// <summary>
        /// Pushes one or more values to the stack
        /// </summary>
        public ActionPush()
        {
            this._arguments = new List<AVM1DataElement>();
            _StackOps = null;
        }
        /// <summary>
        /// Pushes one or more values to the stack
        /// </summary>
        /// <param name="args">The list of elements that will be pushed on the stack</param>
        public ActionPush( List<AVM1DataElement> args ) : this()
        {
            this._arguments = args;
        }
        #endregion

        #region accessors:

        /// <summary>
        /// 
        /// </summary>
        public List<AVM1DataElement> Arguments 
        { 
            get 
            { 
                return this._arguments; 
            } 
            set 
            { 
                this._arguments = value; 
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
                return 4; 
            }
        }
        /// <summary>
        /// Parses the arguments from a source stream (The documentation is misleading. 
        /// The _length member does define the overall length of the action's arguments. 
        /// Now, we have to track how much we used.)
        /// </summary>
        /// <param name="sourceStream">The source stream</param>
        /// <param name="sourceVersion">The version</param>
        protected override void Parse( System.IO.BinaryReader sourceStream, byte sourceVersion )
        {
            _arguments = new List<AVM1DataElement>();

            // 
            // The documentation is misleading. The _length member does define the overall 
            // length of the action's arguments. Now, we have to track how much we used.
            //
            long before = sourceStream.BaseStream.Position;
            while ( sourceStream.BaseStream.Position < ( before + _length ) )
            {
                AVM1DataElement element = new AVM1DataElement();
                element.DataType = ( AVM1DataTypes )sourceStream.ReadByte();

                if ( ( element.DataType >= AVM1DataTypes.AVM_null ) && ( sourceVersion < 5 ) )
                {
                    throw new AVM1ExceptionVersion( "ActionPush with data type " + element.DataType.ToString() + " in Swf Version " + sourceVersion.ToString() );
                }

                switch ( element.DataType )
                {
                    case AVM1DataTypes.AVM_String:
                        element.Value = ( object )Helper.SwfStrings.SwfString( sourceVersion, sourceStream );
                        break;

                    case AVM1DataTypes.AVM_float:
                        element.Value = ( object )sourceStream.ReadSingle();
                        break;

                    case AVM1DataTypes.AVM_null:
                        element.Value = null;
                        break;

                    case AVM1DataTypes.AVM_undefined:
                        element.Value = null;
                        break;

                    case AVM1DataTypes.AVM_register:
                        element.Value = ( object )sourceStream.ReadByte();
                        break;

                    case AVM1DataTypes.AVM_boolean:
                        element.Value = ( object )sourceStream.ReadBoolean();
                        break;

                    case AVM1DataTypes.AVM_double:
                        element.Value = ( object )sourceStream.ReadDouble();
                        break;

                    case AVM1DataTypes.AVM_integer:
                        element.Value = ( object )sourceStream.ReadUInt32();
                        break;

                    case AVM1DataTypes.AVM_constUInt8:
                        element.Value = ( object )sourceStream.ReadByte();
                        break;

                    case AVM1DataTypes.AVM_constUInt16:
                        element.Value = ( object )sourceStream.ReadUInt16();
                        break;

                    default:
                        throw new AVM1ExceptionByteCodeFormat( "ActionPush with invalid data type 0x" + element.DataType.ToString( "X" ) );
                }

                _arguments.Add( element );
            }

            if ( sourceStream.BaseStream.Position != ( before + _length ) )
            {
                throw new AVM1ExceptionByteCodeFormat( "ActionPush arguments consumed more than the Action's length (" +
                    ( ( long )( sourceStream.BaseStream.Position - before ) ).ToString() + " consumed, " +
                    _length.ToString() + " declared as length)" );
            }
        }
        /// <summary>
        /// Renders the arguments to a output stream
        /// </summary>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The stream position</returns>
        protected override ulong Render( System.IO.BinaryWriter outputStream )
        {
            long pos = outputStream.BaseStream.Position;

            for ( int i = 0; i < _arguments.Count; i++ )
            {
                byte dataType = ( byte )_arguments[ i ].DataType;
                outputStream.Write( dataType );

                switch ( _arguments[ i ].DataType )
                {
                    case AVM1DataTypes.AVM_boolean:
                        bool b = ( bool )_arguments[ i ].Value;
                        outputStream.Write( b );
                        break;

                    case AVM1DataTypes.AVM_constUInt16:
                        UInt16 v = ( UInt16 )_arguments[ i ].Value;
                        outputStream.Write( v );
                        break;

                    case AVM1DataTypes.AVM_constUInt8:
                        byte v2 = ( byte )_arguments[ i ].Value;
                        outputStream.Write( v2 );
                        break;

                    case AVM1DataTypes.AVM_double:
                        double v3 = ( double )_arguments[ i ].Value;
                        outputStream.Write( v3 );
                        break;

                    case AVM1DataTypes.AVM_float:
                        Single v4 = ( Single )_arguments[ i ].Value;
                        outputStream.Write( v4 );
                        break;

                    case AVM1DataTypes.AVM_integer:
                        UInt32 v5 = ( UInt32 )_arguments[ i ].Value;
                        outputStream.Write( v5 );
                        break;

                    case AVM1DataTypes.AVM_null:
                        break;

                    case AVM1DataTypes.AVM_register:
                        byte v6 = ( byte )_arguments[ i ].Value;
                        outputStream.Write( v6 );
                        break;

                    case AVM1DataTypes.AVM_String:
                        string s = ( string )_arguments[ i ].Value;
                        Helper.SwfStrings.SwfWriteString( this.Version, outputStream, s );
                        break;

                    case AVM1DataTypes.AVM_undefined:
                        break;

                    default:
                        throw new AVM1ExceptionByteCodeFormat( "Rendering ActionPush with invalid data type 0x" + dataType.ToString( "X02" ) );
                }
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

            for ( int i = 0; i < _arguments.Count; i++ )
            {
                sb.AppendFormat( " [{0:d}]", i );
                switch ( _arguments[ i ].DataType )
                {
                    case AVM1DataTypes.AVM_boolean:
                        bool b = ( bool )_arguments[ i ].Value;
                        sb.AppendFormat( "Bool:{0}", b );
                        break;

                    case AVM1DataTypes.AVM_constUInt16:
                        UInt16 v = ( UInt16 )_arguments[ i ].Value;
                        sb.AppendFormat( "Const16:{0:X04}", v );
                        break;

                    case AVM1DataTypes.AVM_constUInt8:
                        byte v2 = ( byte )_arguments[ i ].Value;
                        sb.AppendFormat( "Const8:{0:X02}", v2 );
                        break;

                    case AVM1DataTypes.AVM_double:
                        double v3 = ( double )_arguments[ i ].Value;
                        sb.AppendFormat( "Double:{0:G}", v3 );
                        break;

                    case AVM1DataTypes.AVM_float:
                        Single v4 = ( Single )_arguments[ i ].Value;
                        sb.AppendFormat( "Single:{0:G}", v4 );
                        break;

                    case AVM1DataTypes.AVM_integer:
                        UInt32 v5 = ( UInt32 )_arguments[ i ].Value;
                        sb.AppendFormat( "UInt32:{0:X08}", v5 );
                        break;

                    case AVM1DataTypes.AVM_null:
                        sb.Append( "NULL" );
                        break;

                    case AVM1DataTypes.AVM_register:
                        byte v6 = ( byte )_arguments[ i ].Value;
                        sb.AppendFormat( "Reg:{0:d}", v6 );
                        break;

                    case AVM1DataTypes.AVM_String:
                        string s = ( string )_arguments[ i ].Value;
                        sb.AppendFormat( "String:'{0}'", s );
                        break;

                    case AVM1DataTypes.AVM_undefined:
                        sb.Append( "UNDEFINED" );
                        break;

                    default:
                        throw new AVM1ExceptionByteCodeFormat( "ActionPush with invalid data type 0x" + _arguments[ i ].DataType.ToString( "X02" ) );
                }
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
            _arguments = new List<AVM1DataElement>();
            for ( int i = 0; i < token.Length; i++ )
            {
                string arg = null;
                if ( token[ i ].Contains( ":" ) )
                {
                    arg = token[ i ].Substring( token[ i ].IndexOf( ":" ) + 1 );
                }
                AVM1DataElement e = new AVM1DataElement();

                if ( token[ i ].StartsWith( "Bool:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_boolean;
                    e.Value = Boolean.Parse( arg );
                }
                else if ( token[ i ].StartsWith( "Const16:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_constUInt16;
                    e.Value = UInt16.Parse( arg, System.Globalization.NumberStyles.AllowHexSpecifier );
                }
                else if ( token[ i ].StartsWith( "Const8:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_constUInt8;
                    e.Value = Byte.Parse( arg, System.Globalization.NumberStyles.AllowHexSpecifier );
                }
                else if ( token[ i ].StartsWith( "Double:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_double;
                    e.Value = Double.Parse( arg );
                }
                else if ( token[ i ].StartsWith( "Single:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_float;
                    e.Value = float.Parse( arg );
                }
                else if ( token[ i ].StartsWith( "UInt32:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_integer;
                    e.Value = UInt32.Parse( arg, System.Globalization.NumberStyles.AllowHexSpecifier );
                }
                else if ( token[ i ].StartsWith( "NULL", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_null;
                    e.Value = null;
                }
                else if ( token[ i ].StartsWith( "Reg:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_register;
                    e.Value = Byte.Parse( arg, System.Globalization.NumberStyles.AllowHexSpecifier );
                }
                else if ( token[ i ].StartsWith( "String:", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_String;
                    e.Value = arg;
                }
                else if ( token[ i ].StartsWith( "UNDEFINED", StringComparison.InvariantCulture ) )
                {
                    e.DataType = AVM1DataTypes.AVM_undefined;
                    e.Value = null;
                }
                else
                {
                    return false;
                }

                _arguments.Add( e );
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public override StackChange[] StackOperations
        {
            get
            {
                _StackOps = new StackChange[ _arguments.Count ];
                for ( int i = 0; i < _arguments.Count; i++ )
                {
                    _StackOps[ i ] = new StackPush( _arguments[ i ].DataType );
                }
                return _StackOps;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStack"></param>
        /// <returns></returns>
        public override Stack.AVM1Stack PerformStackOperations( Stack.AVM1Stack sourceStack )
        {
            for ( int i = 0; i < _arguments.Count; i++ )
            {
                sourceStack.Push( _arguments[ i ] );
            }

            return sourceStack;
        }
    }
}
