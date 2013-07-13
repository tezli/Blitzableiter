using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Recurity.Swf.AVM2.Static;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class Ns_set_info
    {
        /// <summary>
        /// 
        /// </summary>
        private List<UInt32> _Ns;

        /// <summary>
        /// 
        /// </summary>
        public Ns_set_info()
        {
            _Ns = new List<uint>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse( Stream source )
        {
            _Ns = new List<uint>();

            UInt32 setMemberCount = VariableLengthInteger.ReadU30( source );

            for ( uint i = 0; i < setMemberCount; i++ )
            {
                UInt32 entry = VariableLengthInteger.ReadU30( source );

                if ( 0 == entry )
                {
                    AbcFormatException fe = new AbcFormatException( "Namespace set entry is 0" );
                   Log.Error(this,  fe );
                    throw fe;
                }

                _Ns.Add( entry );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify( AbcFile abc )
        {
            for ( int i = 0; i < _Ns.Count; i++ )
            {
                if ( 0 == _Ns[ i ] )
                {
                    AbcVerifierException ave = new AbcVerifierException( "Ns_set entry " + i.ToString( "d" ) + " is 0, which is not allowed" );
                   Log.Error(this,  ave );
                    throw ave;
                }
                if ( _Ns[ i ] >= abc.ConstantPool.Namespaces.Count )
                {
                    AbcVerifierException ave = new AbcVerifierException( "Ns_set entry " + i.ToString( "d" ) + " index outside Namespace array" );
                   Log.Error(this,  ave );
                    throw ave;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write( Stream destination )
        {
            VariableLengthInteger.WriteU30( destination,  ( uint )_Ns.Count );
            for ( int i = 0; i < _Ns.Count; i++ )
            {
                VariableLengthInteger.WriteU30( destination, _Ns[ i ] );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = 0;
                accumulator = VariableLengthInteger.EncodedLengthU30( (uint) _Ns.Count );
                for ( int i = 0; i < _Ns.Count; i++ )
                {
                    accumulator += VariableLengthInteger.EncodedLengthU30( _Ns[i] );
                }
                return accumulator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<UInt32> Entries
        {
            get
            {
                return _Ns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.GetType().Name );
            for ( int i = 0; i < _Ns.Count; i++ )
            {
                sb.AppendFormat( " {0:d}", i );
            }
            return sb.ToString();
        }
    }
}
