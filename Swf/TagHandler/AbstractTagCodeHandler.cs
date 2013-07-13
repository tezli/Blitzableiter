using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// A swf tag with code
    /// </summary>
    public abstract class AbstractTagCodeHandler : AbstractTagHandler
    {

        /// <summary>
        /// The code of the TagCodeHandlerObject
        /// </summary>
        protected AVM1.AVM1Code _code;

        /// <summary>
        /// A swf tag with code
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public AbstractTagCodeHandler( byte InitialVersion ) : base( InitialVersion ) { }
        
        /// <summary>
        /// 
        /// </summary>
        public override byte WriteVersion
        {
            get
            {
                byte version = this.MinimumVersionRequired;

                if ( null != this._code )
                {
                    for ( int i = 0; i < this._code.Count; i++ )
                    {
                        version = this._code[ i ].MinimumVersionRequired > version ? this._code[ i ].MinimumVersionRequired : version;
                    }
                }
                return version;
            }
        }         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedLength"></param>
        protected void ParseCode( uint expectedLength )
        {
            BinaryReader2 br = new BinaryReader2( this._dataStream );            
            AVM1.AVM1InstructionSequence bytecode = null;

            Log.Debug(this,  "Parsing AVM1 code" );
            
            try
            {
                bytecode = Helper.SwfCodeReader.GetCode( expectedLength, br, this.Version );
            }
            catch ( AVM1.AVM1Exception be )
            {
                Log.Error(this,  be );

                if ( SwfFile.Configuration.AVM1DeleteInvalidBytecode)
                {
                    bytecode = new AVM1.AVM1InstructionSequence();
                }
                else
                {
                    SwfFormatException swfe = new SwfFormatException( "Tag with invalid byte code", be );
                    Log.Error(this, swfe);
                    throw swfe;
                }
            }

            this._code = new Recurity.Swf.AVM1.AVM1Code( bytecode );
           //Log.Debug(this,  this._code.Count.ToString() + " actions added" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected MemoryStream RenderCode()
        {
            MemoryStream ms = new MemoryStream();

           //Log.Debug(this,  "Rendering AVM1 code" );

            for ( int i = 0; i < this._code.Count; i++ )
            {
                this._code[ i ].Write( ms );
            }            

            return ms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool VerifyAllCode()
        {
            bool result = true;

            AVM1.AVM1Code[] code = this.Code;
            for ( int i = 0; i < code.Length; i++ )
            {                
                result = result && code[ i ].Verify();
            }            

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override Recurity.Swf.AVM1.AVM1Code[] Code
        {
            get
            {
                AVM1.AVM1Code[] code = new Recurity.Swf.AVM1.AVM1Code[ 1 ];
                code[ 0 ] = this._code;
                return code;
            }
            set
            {
                AVM1.AVM1Code[] v = value;
                if ( v.Length != 1 )
                {
                    throw new ArgumentException( this.GetType().ToString() + " can only carry one AVM1Code instance" );
                }
                else
                {
                    this._code = v[ 0 ];
                }
            }
        }        
    }
}
