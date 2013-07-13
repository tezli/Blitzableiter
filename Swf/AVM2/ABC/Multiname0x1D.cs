using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.AVM2.ABC
{
    /// <summary>
    /// 
    /// </summary>
    public class Multiname0x1D : AbstractMultinameEntry
    {
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 _NameIndex;

        /// <summary>
        /// 
        /// </summary>
        protected List<UInt32> _TypeIndices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kindID"></param>
        public Multiname0x1D( byte kindID ) : base( kindID ) { }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNamespace
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNameIndex
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasNsSet
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override uint Length
        {
            get
            {
                uint accumulator = 1;

                accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30( this._NameIndex );
                accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30((uint)this._TypeIndices.Count);
               
                foreach(var item in this._TypeIndices)
                {
                    accumulator += AVM2.Static.VariableLengthInteger.EncodedLengthU30(item);
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
            BinaryReader br = new BinaryReader( source );

            Log.Warn(this,  "Parsing UNDOCUMENTED Multiname Entry Type 0x1D" );

            _NameIndex = AVM2.Static.VariableLengthInteger.ReadU30( br );
            UInt32 count = AVM2.Static.VariableLengthInteger.ReadU30( br );

            _TypeIndices = new List<uint>();
            for ( uint i = 0; i < count; i++ )
            {
                UInt32 entry = AVM2.Static.VariableLengthInteger.ReadU30( br );
                _TypeIndices.Add( entry );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public override void Write( Stream destination )
        {
            destination.WriteByte( ( byte )_Type );
            BinaryWriter bw = new BinaryWriter( destination );            
            AVM2.Static.VariableLengthInteger.WriteU30( bw, _NameIndex );
            AVM2.Static.VariableLengthInteger.WriteU30( bw, (UInt32)_TypeIndices.Count );
            for ( int i = 0; i < _TypeIndices.Count; i++ )
            {
                AVM2.Static.VariableLengthInteger.WriteU30( bw, _TypeIndices[ i ] );
            }
        }
    }
}
