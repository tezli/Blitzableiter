using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Recurity.Swf.AVM2.Static;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// The instance_info entry is used to define the characteristics of a run-time object (a class instance) within the
    /// AVM2. The corresponding class_info entry is used in order to fully define an ActionScript 3.0 Class.
    /// </summary>
    public class Instance_info
    {
        /// <summary>
        /// The index into the multiname array of the constant pool. It provides a name for the 
        /// class. The entry specified must be a QName.
        /// </summary>
        private UInt32 _Name;
        /// <summary>
        /// The index into the multiname array of the constant pool. It provides the name of
        /// the base class of this class, if any. A value of zero indicates that this class has no base class.
        /// </summary>
        private UInt32 _Supername;
        /// <summary>
        /// This flags field is used to identify if a class is a sealed class.
        /// </summary>
        private bool _FlagClassSealed;
        /// <summary>
        /// This flags field is used to identify if a class is a final class.
        /// </summary>
        private bool _FlagClassFinal;
        /// <summary>
        /// This flags field is used to identify if a class is an interface.
        /// </summary>
        private bool _FlagClassInterface;
        /// <summary>
        /// This flags field is used to identify if a class is a protected class.
        /// </summary>
        private bool _FlagClassProtectedNs;
        /// <summary>
        /// This field is present only if the CONSTANT_ProtectedNs bit of flags is set. It is an index into the
        /// namespace array of the constant pool and identifies the namespace that serves as the protected namespace
        /// for this class.
        /// </summary>
        private UInt32 _ProtectedNs;
        /// <summary>
        /// The value of the intrf_count field is the number of entries in the interface array. The interface array contains 
        /// indices into the multiname array of the constant pool; the referenced names specify the interfaces implemented by 
        /// this class. None of the indices may be zero.
        /// </summary>
        private List<UInt32> _Interface;
        /// <summary>
        /// This is an index into the method array of the abcFile; it references the method that is invoked whenever an object 
        /// of this class is constructed. This method is sometimes referred to as an instance initializer.
        /// </summary>
        private UInt32 _Iinit;
        /// <summary>
        /// The value of trait_count is the number of elements in the trait array. The trait array defines the set
        /// of traits of a class instance. The next section defines the meaning of the traits_info structure.
        /// </summary>
        private List<Traits_info> _Trait;

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30( _Name );
                accumulator += VariableLengthInteger.EncodedLengthU30( _Supername );
                accumulator += 1; // flags
                if ( _FlagClassProtectedNs )
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30( _ProtectedNs );
                }
                accumulator += VariableLengthInteger.EncodedLengthU30( ( uint ) _Interface.Count );
                for ( int i = 0; i < _Interface.Count; i++ )
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30( _Interface[ i ] );
                }
                accumulator += VariableLengthInteger.EncodedLengthU30( _Iinit );
                accumulator += VariableLengthInteger.EncodedLengthU30( ( uint )_Trait.Count );
                for ( int i = 0; i < _Trait.Count; i++ )
                {
                    accumulator += _Trait[ i ].Length;
                }
                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse( Stream source )
        {

            //Log.Debug(this, "Offset : " + source.Position); 
            _Name = VariableLengthInteger.ReadU30( source );
            _Supername = VariableLengthInteger.ReadU30( source );

            byte flags = VariableLengthInteger.ReadU8( source );
            _FlagClassSealed = ( ( flags & 0x01 ) != 0 );
            _FlagClassFinal = ( ( flags & 0x02 ) != 0 );
            _FlagClassInterface = ( ( flags & 0x04 ) != 0 );
            _FlagClassProtectedNs = ( ( flags & 0x08 ) != 0 );

            if ( ( flags & 0xF0 ) != 0 )
            {
                AbcFormatException fe = new AbcFormatException( "Reserved flags used in Instance_Info" );
               Log.Error(this,  fe );
                throw fe;
            }

            if ( _FlagClassProtectedNs )
            {
                _ProtectedNs = VariableLengthInteger.ReadU30( source );
            }

            UInt32 interfaceCount = VariableLengthInteger.ReadU30( source );
            _Interface = new List<UInt32>( (int)interfaceCount );
            for ( uint i = 0; i < interfaceCount; i++ )
            {
                UInt32 interf = VariableLengthInteger.ReadU30( source );

                if ( 0 == interf )
                {
                    AbcFormatException fe = new AbcFormatException( "Instance_info interface index is 0" );
                   Log.Error(this,  fe );
                    throw fe;
                }

                _Interface.Add( interf );
            }

            _Iinit = VariableLengthInteger.ReadU30( source );

            UInt32 traitCount = VariableLengthInteger.ReadU30( source );
            _Trait = new List<Traits_info>( ( int )traitCount );
            for ( uint i = 0; i < traitCount; i++ )
            {
                Traits_info ti = new Traits_info();
                ti.Parse( source );
                _Trait.Add( ti );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify( AbcFile abc )
        {
            //            
            // _Name must be a multiname of type QName
            // _Supername must be a multiname
            // _ProtectedNs if _FlagClassProtectedNs must point to namespace
            // _Interface is multiname index
            // _Iinit is index into method array
            //

            if ( !abc.VerifyMultinameIndex( _Name ) )
            {
                AbcVerifierException ave = new AbcVerifierException( "Invalid Name: " + _Name.ToString( "d" ) );
               Log.Error(this,  ave );
                throw ave;
            }

            if ( abc.ConstantPool.Multinames[ (int)_Name ].Type != MultinameType.QName )
            {
                AbcVerifierException ave = new AbcVerifierException( "Name: " + _Name.ToString( "d" ) + " is not of type QName" );
               Log.Error(this,  ave );
                throw ave;
            }

            if ( _FlagClassProtectedNs )
            {
                if ( !abc.VerifyMultinameIndex( _ProtectedNs ) )
                {
                    AbcVerifierException ave = new AbcVerifierException( "Invalid ProtectedNs: " + _ProtectedNs.ToString( "d" ) );
                   Log.Error(this,  ave );
                    throw ave;
                }
            }

            if ( !abc.VerifyMultinameIndex( _Supername ) )
            {
                AbcVerifierException ave = new AbcVerifierException( "Invalid SuperName: " + _Supername.ToString( "d" ) );
               Log.Error(this,  ave );
                throw ave;
            }

            for ( int i = 0; i < _Interface.Count; i++ )
            {
                if ( !abc.VerifyMultinameIndex( _Interface[i] ) )
                {
                    AbcVerifierException ave = new AbcVerifierException( "Invalid interface: " + _Interface[i].ToString( "d" ) );
                   Log.Error(this,  ave );
                    throw ave;
                }
                if ( 0 == _Interface[ i ] )
                {
                    AbcVerifierException ave = new AbcVerifierException( "Invalid interface, being 0" );
                   Log.Error(this,  ave );
                    throw ave;
                }
            }

            if ( _Iinit >= abc.Methods.Count )
            {
                AbcVerifierException ave = new AbcVerifierException( "Iinit does not point into method array: " + _Iinit.ToString( "d" ) );
               Log.Error(this,  ave );
                throw ave;
            }

            foreach ( Traits_info ti in _Trait )
            {
                ti.Verify( abc );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write( Stream destination )
        {
            VariableLengthInteger.WriteU30( destination, _Name );
            VariableLengthInteger.WriteU30( destination, _Supername );

            byte flags = 0;
            flags = (byte)( 
                ( _FlagClassSealed ? 0x01 : 0 ) |
                ( _FlagClassFinal ? 0x02 : 0 ) |
                ( _FlagClassInterface ? 0x04 : 0 ) |
                ( _FlagClassProtectedNs ? 0x08 : 0 )
                );
            VariableLengthInteger.WriteU8( destination, flags );

            if ( _FlagClassProtectedNs )
            {
                VariableLengthInteger.WriteU30( destination, _ProtectedNs );
            }
            VariableLengthInteger.WriteU30( destination, ( uint )_Interface.Count );
            for ( int i = 0; i < _Interface.Count; i++ )
            {
                VariableLengthInteger.WriteU30( destination, _Interface[ i ] );
            } 
            VariableLengthInteger.WriteU30( destination, _Iinit );
            VariableLengthInteger.WriteU30( destination, ( uint )_Trait.Count );
            for ( int i = 0; i < _Trait.Count; i++ )
            {
                _Trait[ i ].Write( destination );
            }
        }
    }
}
