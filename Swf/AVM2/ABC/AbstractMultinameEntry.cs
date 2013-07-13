using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.ABC
{

    /// <summary>
    /// 
    /// </summary>
    public enum MultinameType : byte
    {

        /// <summary>
        /// 
        /// </summary>
        QName = 0x07,

        /// <summary>
        /// 
        /// </summary>
        QNameA = 0x0D,

        /// <summary>
        /// 
        /// </summary>
        RTQName = 0x0F,

        /// <summary>
        /// 
        /// </summary>
        RTQNameA = 0x10,

        /// <summary>
        /// 
        /// </summary>
        RTQNameL = 0x11,

        /// <summary>
        /// 
        /// </summary>
        RTQNameLA = 0x12,

        ///// <summary>
        ///// 
        ///// </summary>
        //Name = 0x13,
        ///// <summary>
        ///// 
        ///// </summary>
        //NameL = 0x14,

        /// <summary>
        /// 
        /// </summary>
        Multiname = 0x09,

        /// <summary>
        /// 
        /// </summary>
        MultinameA = 0x0E,

        /// <summary>
        /// 
        /// </summary>
        MultinameL = 0x1B,

        /// <summary>
        /// 
        /// </summary>
        MultinameLA = 0x1C,

        /// <summary>
        /// An (as of yet) undocumented multiname used in Flash Player 10
        /// See http://opensource.adobe.com/svn/opensource/flex/sdk/trunk/modules/swfutils/src/java/flash/swf/tools/AbcPrinter.java
        /// </summary>
        Multiname0x1D = 0x1D
    }  

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractMultinameEntry
    {

        /// <summary>
        /// 
        /// </summary>
        protected MultinameType _Type;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Namespace;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _Name;

        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _NsSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindId"></param>
        public AbstractMultinameEntry( byte kindId )
        {            
            if ( !Enum.IsDefined( typeof( MultinameType ), kindId ) )
            {
                AbcFormatException fe = new AbcFormatException( "Invalid Multiname entry kind " + kindId.ToString( "X2" ) );
                Log.Error(this,  fe );
                throw ( fe );
            }

            _Type = ( MultinameType )kindId;
        }

        /// <summary>
        /// 
        /// </summary>
        public MultinameType Type
        {
            get
            {
                return _Type;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual UInt32 Namespace
        {
            get
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof(MultinameType), _Type) + " has no Namespace");                
            }

            set
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof( MultinameType ), _Type ) + " has no Namespace" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual UInt32 NameIndex
        {
            get
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof( MultinameType ), _Type ) + " has no NameIndex" );
            }
            set
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof( MultinameType ), _Type ) + " has no NameIndex" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual UInt32 NsSet
        {
            get
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof( MultinameType ), _Type ) + " has no NsSet" );
            }
            set
            {
                throw new InvalidCastException( "Multiname type " + Enum.GetName( typeof( MultinameType ), _Type ) + " has no NsSet" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        abstract public bool HasNamespace { get; }

        /// <summary>
        /// 
        /// </summary>
        abstract public bool HasNameIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        abstract public bool HasNsSet { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual uint Length
        {
            get
            {
                uint accumulator = 1; // kindID

                /*
                 * This should be equal to the one below:
                switch ( ( MultinameType )_Type )
                {
                    case MultinameType.QName:
                    case MultinameType.QNameA:
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.Namespace );
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NameIndex );
                        break;

                    case MultinameType.RTQName:
                    case MultinameType.RTQNameA:
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NameIndex );
                        break;

                    case MultinameType.RTQNameL:
                    case MultinameType.RTQNameLA:
                        break;

                    case MultinameType.Multiname:
                    case MultinameType.MultinameA:
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NameIndex );
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NsSet );
                        break;

                    case MultinameType.MultinameL:
                    case MultinameType.MultinameLA:
                        accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NsSet );
                        break;
                }
                 */

                if ( this.HasNameIndex )
                {
                    accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NameIndex );
                }
                if ( this.HasNamespace )
                {
                    accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.Namespace );
                }
                if ( this.HasNsSet )
                {
                    accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this.NsSet );
                }
                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public virtual void Write( Stream destination )
        {
            destination.WriteByte( ( byte )_Type );

            switch ( ( MultinameType )_Type )
            {
                case MultinameType.QName:   // 0x07
                case MultinameType.QNameA:  // 0x0D
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.Namespace );
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.NameIndex );                    
                    break;

                case MultinameType.RTQName:  // 0x0F
                case MultinameType.RTQNameA: // 0x10
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.NameIndex );                    
                    break;

                case MultinameType.RTQNameL:    // 0x11
                case MultinameType.RTQNameLA:   // 0x12
                    break;

                case MultinameType.Multiname: // 0x09
                case MultinameType.MultinameA: // 0x0E
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.NameIndex );
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.NsSet );
                    break;

                case MultinameType.MultinameL: // 0x1B
                case MultinameType.MultinameLA: // 0x1C
                    AVM2.Static.VariableLengthInteger.WriteU30( destination, this.NsSet );
                    break;
                //case MultinameType.Multiname0x1D: // 0x1D
                //    AVM2.Static.VariableLengthInteger.WriteU30(destination, this.NameIndex);
                //    AVM2.Static.VariableLengthInteger.WriteU30(destination, this.);
                //    break;  
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify( AbcFile abc )
        {
            try
            {
                if ( this.HasNsSet )
                {
                    for ( int i = 0; i < abc.ConstantPool.NsSets[ ( int )this.NsSet ].Entries.Count; i++ )
                    {
                        int ldummy = abc.ConstantPool.Strings[ ( int )abc.ConstantPool.NsSets[ ( int )this.NsSet ].Entries[ i ] ].Length;
                        ldummy++;
                    }
                }
                if ( this.HasNamespace )
                {
                    int ldummy = abc.ConstantPool.Strings[ ( int )abc.ConstantPool.Namespaces[ ( int )this.Namespace ].NameIndex ].Length;
                    ldummy++;
                }
                if ( this.HasNameIndex )
                {
                    int ldummy = abc.ConstantPool.Strings[ ( int )this.NameIndex ].Length;
                    ldummy++;
                }
            }
            catch ( Exception e )
            {
                AbcVerifierException ave = new AbcVerifierException( "Multiname entry has invalid indices", e );
                Log.Error(this,  ave );
                throw ave;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        /// <returns></returns>
        public string ToString( AbcFile abc )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "[{0}]", Enum.GetName( typeof( MultinameType ), _Type ) );
            
            if ( this.HasNsSet )
            {
                sb.Append( "{" );
                for ( int i = 0; i < abc.ConstantPool.NsSets[ (int)this.NsSet ].Entries.Count; i++ )
                {
                    string ns = abc.ConstantPool.Strings[ ( int )abc.ConstantPool.NsSets[ ( int )this.NsSet  ].Entries[ i ] ];
                    sb.Append( ns );
                    if ( ( i + 1 ) != abc.ConstantPool.NsSets[ ( int )this.NsSet ].Entries.Count )
                    {
                        sb.Append( "|" );
                    }
                }
                sb.Append( "}." );
            }

            if ( this.HasNamespace )
            {
                if ( abc.ConstantPool.EmptyString == abc.ConstantPool.Namespaces[ ( int )this.Namespace ].NameIndex )
                {
                    sb.Append( "*:" );
                }
                else
                {
                    sb.Append( abc.ConstantPool.Strings[ ( int )abc.ConstantPool.Namespaces[ ( int )this.Namespace ].NameIndex ] );
                    sb.Append( ":" );
                }
            }

            if ( this.HasNameIndex )
            {
                if ( abc.ConstantPool.EmptyString == this.NameIndex )
                {
                    sb.Append( "(unnamed)" );
                }
                else
                {
                    sb.Append( abc.ConstantPool.Strings[ ( int )this.NameIndex ] );
                }
            }

            return sb.ToString();
        }
    }
}
