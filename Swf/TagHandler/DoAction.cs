using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Recurity.Swf;
using Recurity.Swf.AVM1;

namespace Recurity.Swf.TagHandler
{
    /// <summary>
    /// 
    /// </summary>
    public class DoAction : AbstractTagCodeHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialVersion">The initial version of the Swf file</param>
        public DoAction(byte InitialVersion) : base(InitialVersion) { }

        /// <summary>
        /// The minimum swf version for using this tag.
        /// </summary>
        public override byte MinimumVersionRequired
        {
            get { return 3; }
        }

        /// <summary>
        /// The length of this tag including the header.
        /// </summary>
        public override ulong Length
        {
            get
            {
                return (_code.Length + (ulong)this.CodeOffset);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Verify()
        {
            return VerifyAllCode();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Parse()
        {
            // Call the code reader
            ParseCode((uint)(_tag.Length - this.CodeOffset));

            uint finalCodeLength = _code.Length;
            if (finalCodeLength > (_tag.Length - this.CodeOffset))
            {
                SwfFormatException e = new SwfFormatException("code length exceeds Tag length");
                Log.Error(this, e);
                throw e;
            }
            //
            // Sanity check: the total length of the code should 
            // exactly consume the Tag's payload length.
            //
            else if (finalCodeLength < (_tag.Length - this.CodeOffset))
            {
                Log.Warn(this, "code length " + finalCodeLength.ToString("d") + " less than Tag length " + _tag.Length.ToString("d"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        public override void Write(Stream output)
        {
            WriteTagHeader(output);
            WriteCodeAndCheck(output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        protected void WriteCodeAndCheck(Stream output)
        {
            long pos = output.Position;

            using (MemoryStream codeContent = RenderCode())
            {
                if (((ulong)codeContent.Length + (ulong)this.CodeOffset) != this.Length)
                {
                    Exception e = new Exception("CRITICAL internal error: generated code length differs from expected length!");
                    Log.Error(this, e);
                    throw e;
                }

                codeContent.WriteTo(output);
            }

            //Log.Debug(this,  ( ( long )( output.Position - pos ) ).ToString( "d" ) + " bytes written" );
        }

        /// <summary>
        /// CodeOffset is used to abstract the additional overhead (SpriteID) away
        /// and hereby allow most of the code in DoAction to be inherited by DoInitAction.
        /// </summary>
        protected virtual long CodeOffset
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ReParseCode()
        {
            // TODO: This code only follows code flow rudimentarily. It does not create
            // a valid _Code member. To do this, there needs to be logic that determines 
            // the relative position of the new jump target.
            //

            throw new NotImplementedException("sucks");

            // remove this comment if parsing is enables for this tag
            //_dataStream.Seek( CodeOffset, SeekOrigin.Begin );
            //this._code = null;
            //AVM1InstructionSequence seq = new AVM1InstructionSequence();
            //BinaryReader br = new BinaryReader( _dataStream );

            //long startPosition = br.BaseStream.Position;

            //bool done = false;

            //do
            //{

            //    AbstractAction action = null;
            //    bool wasValid = false;

            //    //
            //    // remember position before and read the opcode byte
            //    //
            //    long currentPosition = br.BaseStream.Position;

            //    try
            //    {
            //        try
            //        {
            //            action = AVM1Factory.Create( br, this.Version );
            //            //seq.Add( action );
            //           Log.DebugFormat( "{0:d} 0x{1:X08} {2}", seq.Count - 1, currentPosition, action.ToString() );
            //            wasValid = true;
            //        }
            //        catch ( AVM1Exception )
            //        {                        
            //            br.BaseStream.Seek( currentPosition, SeekOrigin.Begin );
            //            byte opcode = br.ReadByte();
            //           Log.DebugFormat( "Invalid opcode 0x{0:X02} at stream position 0x{1:X08}", opcode, currentPosition );
            //            wasValid = false;
            //        }
            //    }
            //    catch ( IOException e )
            //    {
            //       Log.Error(this,  e );
            //        SwfFormatException swfe = new SwfFormatException( "Code flow leaves Tag data block through missing ActionEnd" );
            //        throw swfe;
            //    }

            //    if ( wasValid )
            //    {
            //        if ( action.IsBranch )
            //        {
            //            try
            //            {
            //                br.BaseStream.Seek( currentPosition + action.BranchTargetAdjusted, SeekOrigin.Begin );
            //            }
            //            catch ( IOException e )
            //            {
            //               Log.Error(this,  e );
            //                SwfFormatException swfe = new SwfFormatException( "Code flow leaves Tag data block through branch" );
            //                throw swfe;
            //            }
            //           Log.DebugFormat( "Next instruction 0x{0:X08} -> {1:X08}", currentPosition, br.BaseStream.Position );
            //        }
            //    }
            //    else
            //    {
            //        br.BaseStream.Seek( currentPosition, SeekOrigin.Begin );
            //        byte opcode = br.ReadByte();

            //        if ( 0 != ( 0x80 & opcode ) )
            //        {
            //            //
            //            //  Read length
            //            //                                                
            //            try
            //            {
            //                UInt16 length = br.ReadUInt16();
            //                br.BaseStream.Seek( currentPosition + length, SeekOrigin.Begin );
            //            }
            //            catch ( IOException e )
            //            {
            //               Log.Error(this,  e );
            //                SwfFormatException swfe = new SwfFormatException( "Code flow leaves Tag data block through undocumented opcode" );
            //                throw swfe;
            //            }
            //        }
            //       Log.DebugFormat( "Next instruction 0x{0:X08} -> {1:X08}", currentPosition, br.BaseStream.Position );
            //    }

            //    if ( wasValid && ( action.ActionType == AVM1Actions.ActionEnd ) )
            //        done = true;

            //} while ( !done );
        }
    }
}
