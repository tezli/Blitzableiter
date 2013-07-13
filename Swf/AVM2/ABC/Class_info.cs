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
    public class Class_info
    {

        /// <summary>
        /// 
        /// </summary>
        public UInt32 Cinit { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Traits_info> Traits { get; internal set; }

        /// <summary>
        /// The Length of this object in bytes.
        /// </summary>
        public uint Length
        {
            get
            {
                uint accumulator = VariableLengthInteger.EncodedLengthU30( Cinit );
                accumulator += VariableLengthInteger.EncodedLengthU30( ( uint )Traits.Count );
                for ( int i = 0; i < Traits.Count; i++ )
                {
                    accumulator += Traits[ i ].Length;
                }
                return accumulator;
            }
        }

        /// <summary>
        /// Parses this object out of a stream
        /// </summary>
        /// <param name="source">The stream to read from</param>
        public void Parse( Stream source )
        {
            Cinit = VariableLengthInteger.ReadU30( source );

            UInt32 traits_count = VariableLengthInteger.ReadU30( source );
            Traits = new List<Traits_info>( ( int )traits_count );
            for ( uint i = 0; i < traits_count; i++ )
            {
                Traits_info tr = new Traits_info();
                tr.Parse( source );
                Traits.Add( tr );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify( AbcFile abc )
        {
            if ( Cinit >= abc.Methods.Count )
            {
                AbcVerifierException ave = new AbcVerifierException( "Invalid Cinit: " + Cinit.ToString( "d" ) );
                Log.Error(this,  ave );
                throw ave;
            }

            foreach ( Traits_info t in Traits )
            {
                t.Verify( abc );
            }
        }

        /// <summary>
        /// Writes 
        /// </summary>
        /// <param name="destination"></param>
        public void Write( Stream destination )
        {
            VariableLengthInteger.WriteU30( destination, Cinit );
            VariableLengthInteger.WriteU30( destination, ( uint ) Traits.Count );
            for ( int i = 0; i < Traits.Count; i++ )
            {
                Traits[ i ].Write( destination );
            }
        }
    }
}
