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
    public class Metadata_item_info
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt32 _Key;

        /// <summary>
        /// 
        /// </summary>
        private UInt32 _Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Parse( Stream source )
        {
            //Log.Debug(this, "Offset : " + source.Position); 
            _Key = VariableLengthInteger.ReadU30( source );
            _Value = VariableLengthInteger.ReadU30( source );

            if ( ( 0 == _Key ) && ( 0 == _Value ) )
            {
                Log.Warn(this,  "Metadata_item_info entry with key/value 0/0" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                return VariableLengthInteger.EncodedLengthU30( _Key ) +
                    VariableLengthInteger.EncodedLengthU30( _Value );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void Write( Stream destination )
        {
            VariableLengthInteger.WriteU30( destination, _Key );
            VariableLengthInteger.WriteU30( destination, _Value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abc"></param>
        public void Verify( AbcFile abc )
        {
            if ( ( !abc.VerifyNameIndex( _Key ) ) || ( !abc.VerifyNameIndex( _Value ) ) )
            {
                AbcVerifierException ave = new AbcVerifierException( "Invalid Key/Value pair: " + _Key.ToString( "d" ) + "/" + _Value.ToString( "d" ) );
               Log.Error(this,  ave );
                throw ave;
            }
        }
    }
}
