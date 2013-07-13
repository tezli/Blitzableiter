using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipActions : AbstractSwfElement
    {
        internal ClipEventFlags _ClipEventFlags;
        internal List<ClipActionRecord> _ClipActionRecords;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The version of the Swf file using this object.</param>
        public ClipActions( byte InitialVersion ) : base( InitialVersion ) { }

        /// <summary>
        /// 
        /// </summary>
        public override byte Version
        {
            get
            {
                return base.Version;
            }
            set
            {
                base.Version = value;
                
                if ( null != _ClipEventFlags )
                    _ClipEventFlags.Version = base.Version;

                if ( null != _ClipActionRecords )
                {
                    for ( int i = 0; i < _ClipActionRecords.Count; i++ )
                        _ClipActionRecords[ i ].Version = base.Version;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Parse( Stream input )
        {
            BinaryReader2 br = new BinaryReader2( input );
            bool parsingSuccess = true;

            UInt16 uselessButFiddelingWithBitsThanksAdobe = br.ReadUInt16();
            if ( 0 != uselessButFiddelingWithBitsThanksAdobe )
            {
                SwfFormatException sfe = new SwfFormatException( "Reserved 16Bit Field in CLIPACTION used" );
               Log.Error(this,  sfe );
                throw sfe;
            }

            _ClipEventFlags = new ClipEventFlags( this.Version );
            _ClipEventFlags.Parse( input );

            _ClipActionRecords = new List<ClipActionRecord>();
            // 
            // The ClipActionEndFlag is Version dependent!
            //
            while ( 0 != ( this.Version <= 5 ? br.PeekUInt16() : br.PeekUInt32() ) )
            {
                ClipActionRecord record = new ClipActionRecord( this.Version );
                bool recordParsingResult = record.Parse( input );
                _ClipActionRecords.Add( record );
                parsingSuccess = recordParsingResult & parsingSuccess;
            }
            // 
            // Get the EndRecord (Version dependent) and ignore
            //
            UInt32 endRecord = this.Version <= 5 ? br.ReadUInt16() : br.ReadUInt32();

            if ( 0 != endRecord )
            {
                SwfFormatException sfe = new SwfFormatException( "endRecord is not 0x00/0x0000" );
               Log.Error(this,  sfe );
                throw sfe;
            }

           //Log.Debug(this,  _ClipActionRecords.Count + " ClipActionRecords added" );
            
            return parsingSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            bool actionRecordsValid = true;

            for ( int i = 0; i < _ClipActionRecords.Count; i++ )
            {
                actionRecordsValid = actionRecordsValid & _ClipActionRecords[ i ].Verify();
            }

            // not clear why this was commented out ?!?
            if ( ! actionRecordsValid )
                return false;

            return true;
        }        

        /// <summary>
        /// 
        /// </summary>
        public uint Length
        {
            get
            {
                uint res = 0;

                res += 2; // reserved
                res += _ClipEventFlags.Length;
                for ( int i = 0; i < _ClipActionRecords.Count; i++ )
                {
                    res += _ClipActionRecords[ i ].Length;
                }
                res += ( this.Version <= 5 ? ( uint )2 : ( uint )4 ); // EndRecord

                return res;
            }
        }

        //public void Write( byte version, Stream output )
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public void Write( Stream output )
        {
            UInt16 thanksAdobe = 0;
            BinaryWriter bw = new BinaryWriter( output );
            bw.Write( thanksAdobe );            
            _ClipEventFlags.Write( output );
            
            for ( int i = 0; i < _ClipActionRecords.Count; i++ )
            {                
                _ClipActionRecords[ i ].Write( output );
            }

            if ( this.Version <= 5 )
            {
                UInt16 term = 0x00;
                bw.Write( term );
            }
            else
            {
                UInt32 term = 0x00;
                bw.Write( term );
            }
        }
    }
}
