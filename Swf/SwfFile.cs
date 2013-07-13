using Recurity.Swf.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Recurity.Swf
{
    /// <summary>
    /// A uncompressed Swf file implementing ISwfElement, extending BaseFile
    /// </summary>
    public class SwfFile : Interfaces.ISwfElement
    {
        internal static Config Configuration;
        internal static ulong HeaderDeclaredLength;

        /// <summary>
        /// 
        /// </summary>
        public CwsFile CwsSource { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public FwsFile FwsSource { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TagHandler.AbstractTagHandler> TagHandlers { get; private set; }

        private event SwfFileReadProgressChangedEventHandler ReadProgressChanged;
        private event SwfFileWriteProgressChangedEventHandler WriteProgressChanged;

        private event TagReadCompletedEventHandler TagReadCompleted;
        private event TagProducedEventHandler TagProduced;

        private event SwfFileCompressedEventHandler SwfFileCompressed;
        private event SwfFileProtectedEventHandler SwfFileProtected;

        private event VerificationCompletedEventHandler VerificationCompleted;

        /// <summary>
        /// A uncompressed Swf file implementing ISwfElement, extending BaseFile
        /// </summary>
        public SwfFile()
        {
            Configuration = new Config();
        }

        /// <summary>
        /// 
        /// </summary>
        public byte WriteVersion
        {
            get
            {
                byte version = 3;
                for (int i = 0; i < TagHandlers.Count; i++)
                {
                    version = TagHandlers[i].WriteVersion > version ? TagHandlers[i].WriteVersion : version;
                }
                return version;
            }
        }

        /// <summary>
        /// Writes the contents of this SwfFile instance to a stream. 
        /// Compression is used if configured.
        /// </summary>
        /// <param name="output">The destination stream to write to</param>
        public void Write(Stream output)
        {
            if (SwfFile.Configuration.WriteCompressed)
            {
                CwsFile cws = new CwsFile();
                cws.CompressionLevel = SwfFile.Configuration.WriteCompressionLevel;
                cws.WriteContent = new MemoryStream();
                WriteContent(cws.WriteContent);
                cws.Version = this.Version;
                // copy frame information from old FWS record
                cws.FrameHeader = FwsSource.FrameHeader;
                cws.Write(output);
            }
            else
            {
                FwsFile fws = new FwsFile();
                fws.WriteContent = new MemoryStream();
                WriteContent(fws.WriteContent);
                fws.Version = this.Version;
                // copy frame information from old FWS record
                fws.FrameHeader = FwsSource.FrameHeader;
                fws.Write(output);
            }
        }

        /// <summary>
        /// Writes the content of the Swf file into an output stream (no header)
        /// </summary>
        /// <param name="output">The output stream</param>
        /// <returns>size of the content</returns>
        private ulong WriteContent(Stream output)
        {
            // Before doing anything else, see if we have multiple FileAttributes and fix this
            if (SwfFile.Configuration.FixMultipleFileAttributes)
            {
                this.FixFileAttributes();
                Log.Debug(this, "Fixing File Attributes");
            }

            Log.Debug(this, "Writing stream for " + TagHandlers.Count.ToString("d") + " tags");

            // This call collects the required version information.
            // Since version also decides size, this has to be done first.

            if (this.WriteVersion > this.Version)
            {
                this.Version = this.WriteVersion;
                Log.Debug(this, "Writing in version " + this.Version.ToString("d"));
            }

            ulong lengthOverAll = 0;
            Log.Debug(this, "Calculating Length");
            for (int i = 0; i < TagHandlers.Count; i++)
            {
                // Now calculate length 
                try
                {

                    lengthOverAll += (ulong)TagHandlers[i].TagAndLengthEncoded.Length;
                    lengthOverAll += TagHandlers[i].Length;
                }
                catch (Exception e)
                {

                    throw e;
                }
            }

            for (int i = 0; i < TagHandlers.Count; i++)
            {
                long posBefore = output.Position;
                ulong tagHandlerLength = this.TagHandlers[i].Length;

                Log.Debug(this, "Writing Tag #" + i + "(" + TagHandlers[i].Tag.TagTypeName + ")" + " Total-Length : 0x" + TagHandlers[i].Tag.LengthTotal.ToString("X08") + " @ Stream-Poistion : 0x" + output.Position.ToString("X08") + 1);
                TagHandlers[i].Write(output);

                // Fire WriteProgressChanged event
                if (null != WriteProgressChanged)
                {
                    WriteProgressChanged(this, new SwfWriteProgressChangedEventArgs(TagHandlers[i].Tag.TagType, output.Length, output.Position));
                }

                if ((ulong)(output.Position - posBefore) != (tagHandlerLength + (ulong)TagHandlers[i].TagAndLengthEncoded.Length))
                {
                    Exception e = new Exception("Critical internal error: "
                        + TagHandlers[i].GetType().ToString() + " wrote "
                        + (ulong)(output.Position - posBefore) + " bytes, but declared "
                        + (ulong)(tagHandlerLength + (ulong)TagHandlers[i].TagAndLengthEncoded.Length) + " bytes length"
                        );
                    Log.Error(this, e);
                    throw e;
                }
            }

            if (lengthOverAll != (ulong)output.Length)
            {
                Exception e = new Exception("Critical internal error: calculated length " + lengthOverAll.ToString("d") + " != output stream length " + output.Length);
                Log.Error(this, e);
                throw e;
            }

            return lengthOverAll;
        }

        /// <summary>
        /// Create proper file attributes for this Swf file
        /// </summary>
        private void FixFileAttributes()
        {
            TagHandler.FileAttributes fileAttrib = null;
            bool atLeastOneSeen = false;

            uint faCount = 0;
            for (int i = 0; i < TagHandlers.Count; i++)
            {
                if (TagHandlers[i].Tag.TagType == TagTypes.FileAttributes)
                {
                    faCount++;
                    if (!atLeastOneSeen)
                    {
                        atLeastOneSeen = true;
                        fileAttrib = (TagHandler.FileAttributes)TagHandlers[i];
                    }
                }
            }

            if (1 == faCount)
                return;

            // now, remove them all
            uint faRemovalCount = faCount;
            while (faRemovalCount > 0)
            {
                for (int i = 0; i < TagHandlers.Count; i++)
                {
                    if (TagHandlers[i].Tag.TagType == TagTypes.FileAttributes)
                    {
                        TagHandlers.RemoveAt(i);
                        faRemovalCount--;
                        break;
                    }
                }
            }

            if (1 < faCount)
            {
                String s4 = String.Format("{0:d} FileAttributes removed", faCount);
                Log.Warn(this, s4);
            }

            //
            // If we didn't have a single one, create it
            //
            if (!atLeastOneSeen)
            {
                fileAttrib = new Recurity.Swf.TagHandler.FileAttributes(this.Version);
                fileAttrib._ActionScript3 = false;
                fileAttrib._HasMetadata = false;
                fileAttrib._UseDirectBlit = false;
                fileAttrib._UseGPU = false;
                fileAttrib._UseNetwork = false;
                Log.Warn(this, "FileAttribute Tag added");
            }

            TagHandlers.Insert(0, fileAttrib);
        }

        /// <summary>
        /// Parses a Swf file tag by tag
        /// </summary>
        /// <param name="input">The Swf file as stream</param>
        /// <returns>The next tag</returns>
        public Stream Read(Stream input)
        {
            CwsSource = new CwsFile();
            FwsSource = new FwsFile();
            List<Tag> tags = new List<Tag>();

            Stream next = CwsSource.Read(input);

            if (this.CwsSource.Compressed)
            {
                // Fire compressed event
                if (null != SwfFileCompressed)
                    SwfFileCompressed(this, new CompressedEventArgs(true));
            }

            next = FwsSource.Read(next);

            HeaderDeclaredLength = FwsSource.Length;

            #region Reading tags

            Tag t;
            int tagNumber = 1;
            uint tagsLenghts = (uint)next.Position;

            do
            {
                t = new Tag();

                try
                {
                    next = t.Read(next);
                    Log.Debug(this, "Reading Tag #" + tagNumber + "(" + t.TagTypeName +")" + " Offset : 0x" + t.Offset.ToString("X08") + " Total-Length : 0x" + t.LengthTotal.ToString("X08"));
                    // Fire TagReadCompleted, ReadProgressChanged, if protected SwfFileProtected  events
                    if (null != TagReadCompleted)
                        TagReadCompleted(this, new TagHandlerReadCompleteEventArgs(t.TagType));
                    if (null != this.ReadProgressChanged)
                        ReadProgressChanged(this, new SwfReadProgressChangedEventArgs(next.Length, next.Position));
                    if (null != SwfFileProtected && t.TagType == TagTypes.Protect)
                        SwfFileProtected(this, new ProtectionEventArgs(""));

                    // Knowing if the offset end is longer than header declared length is enough to verfiy
                    if (t.OffsetEnd > this.FwsSource.Length)
                    {
                        if ((SwfFile.Configuration.HandleHeadSizeIssue == HandleHeadSizeIssueBy.Fix) && t.OffsetEnd <= (ulong)input.Length)
                        {
                            this.FwsSource.Length = (uint)t.OffsetEnd;
                            Log.Warn(this, "Tag #" + tagNumber + " ends outside (0x" + t.OffsetEnd.ToString("X08") + ") the declared Swf content range 0x" + this.FwsSource.Length.ToString("X08") + " Fixing.");
                        }
                        if ((SwfFile.Configuration.HandleHeadSizeIssue == HandleHeadSizeIssueBy.Ignore) && t.OffsetEnd <= (ulong)input.Length)
                        {
                            Log.Warn(this, "Tag #" + tagNumber + " ends outside (0x" + t.OffsetEnd.ToString("X08") + ") the declared Swf content range 0x" + this.FwsSource.Length.ToString("X08") + " Ignoring.");
                        }
                        else
                        {
                            SwfFormatException e = new SwfFormatException("Tag #" + tagNumber + " ends outside (0x" + t.OffsetEnd.ToString("X08") + ") the declared Swf content range 0x" + this.FwsSource.Length.ToString("X08"));
                            Log.Error(this, e);
                            throw e;
                        }

                    }

                    tagsLenghts += t.LengthTotal;
                    tagNumber++;
                    tags.Add(t);
                }
                catch (IOException ioe)
                {
                    //This is the point where we find no end tag which basically means that a tag declared more memory than the stream actaully has
                    Log.Error(this, ioe);
                    SwfFormatException e = new SwfFormatException("Tag list is incomplete, does not end with an END tag or a tag length exceeds the file size.");
                    Log.Error(this, e);

                    throw e;
                }
            }
            while (t.TagType != TagTypes.End);

            #endregion

            #region Length checking


            // Performing length checks now
            //
            // 1. Do the length of all tags match the stream length
            if (tagsLenghts != next.Length)
            {

                    SwfFormatException e = new SwfFormatException("The length of tags (" + tagsLenghts.ToString() + ") does not match the stream size(" + next.Length.ToString() + ").");
                    Log.Error(this, e);
                    throw e;
            }

            // 2. Does the tags lengths do match the header declared length
            if (tagsLenghts != this.CwsSource.Length)
            {
                if (SwfFile.Configuration.HandleHeadSizeIssue == HandleHeadSizeIssueBy.Fix)
                {
                    this.CwsSource.Length = tagsLenghts;
                    Log.Warn(this, "The length of tags (" + tagsLenghts.ToString() + ") does not match the header declared length(" + this.CwsSource.Length.ToString() + "). Stream size will be fixed.");
                }
                else if (SwfFile.Configuration.HandleHeadSizeIssue == HandleHeadSizeIssueBy.RaiseError)
                {
                    SwfFormatException e = new SwfFormatException("The length of tags (" + tagsLenghts.ToString() + ") does not match the header declared length(" + this.CwsSource.Length.ToString() + ").");
                    Log.Error(this, e);
                    throw e;
                }
                else
                {
                    Log.Warn(this, "The length of tags (" + tagsLenghts.ToString() + ") does not match the header declared length(" + this.CwsSource.Length.ToString() + "). Stream size will be fixed.");
                }


            }

            // 3. If stream and header length match has already been checked in FWSFile class

            // 4. Has the stream been consumed completely
            if (next.Position != next.Length)
            {
                if (SwfFile.Configuration.HandleStreamOversize == HandleStreamOversizeBy.Resize)
                {
                    this.FixIncorrectStreamSize(next, next.Position);
                    Log.Warn(this, "Trailing garbage after END tag detected. Position 0x" + input.Position.ToString("X08") + ", Length " + input.Length.ToString() + ". Dumping Trailing Garbage.");
                }
                else if (SwfFile.Configuration.HandleStreamOversize == HandleStreamOversizeBy.Ignore)
                {
                    Log.Warn(this, "Trailing garbage after END tag detected. Position 0x" + input.Position.ToString("X08") + ", Length " + input.Length.ToString());
                }
                else
                {
                    SwfFormatException e = new SwfFormatException("Trailing garbage after END tag detected. Position 0x" + input.Position.ToString("X08") + ", Length " + input.Length.ToString());
                    Log.Error(this, e);
                }

            }

            #endregion

            #region Producing tag handlers

            TagHandlers = new List<Recurity.Swf.TagHandler.AbstractTagHandler>();
            for (int i = 0; i < tags.Count; i++)
            {
                //
                // Only accept tag types that are documented by Adobe
                //
                if (!tags[i].IsTagTypeKnown)
                {
                    string msg = "Tag type " + ((UInt16)tags[i].TagType).ToString("d") + " not known/documented";

                    if (SwfFile.Configuration.AllowUnknownTagTypes)
                    {
                        Log.Warn(this, msg);
                    }
                    else
                    {
                        SwfFormatException e = new SwfFormatException(msg);
                        Log.Error(this, e);
                        throw e;
                    }
                }

                //
                // The factory automatically fires .Read() on the produced class. Therefore,
                // we catch Exceptions here (Stream too short) and convert them to SwfFormatExceptions
                //
                try
                {
                    TagHandlers.Add(TagHandlerFactory.Create(tags[i], this, next));

                    // Fire TagProduced event
                    if (null != TagProduced)//If a handler is attached
                        TagProduced(this, new TagHandlerProducedEventArgs(tags[i].TagType, (Int64)tags.Count, (Int64)i));
                }
                catch (Exception e)
                {
                    SwfFormatException swfE = new SwfFormatException("Tag handler #" + i + " (" + tags[i].TagTypeName + ") failed parsing: " + e.Message);
                    throw swfE;
                }

                if (tags[i].TagType.Equals(TagTypes.ST_GLYPHNAMES) || tags[i].TagType.Equals(TagTypes.ST_REFLEX))
                {
                    //this.CWSSource._GeneratorSoftware = "SwfTools";
                }

                //
                // Verify the required Version of each Tag against the header declared
                // Version. 
                // It may be considered to make failing this test fatal.
                //
                if (TagHandlers[i].MinimumVersionRequired > this.Version)
                {
                    Log.Warn(this, "Tag " + (tags[i].IsTagTypeKnown ? tags[i].TagTypeName : tags[i].TagType.ToString()) +
                        " requires Swf version " + TagHandlers[i].MinimumVersionRequired.ToString() +
                        ", header declares " + this.Version.ToString());
                }
            }
            #endregion

            return next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            //
            // Make sure that the FileAttributes Tag is the first (if any)
            //
            uint fileAttributesCnt = 0;
            // TODO : Check if TagHandlers not null
            for (int i = 0; i < TagHandlers.Count; i++)
            {
                if (TagHandlers[i].Tag.TagType == TagTypes.FileAttributes)
                {
                    if ((0 != i) && (this.Version < 8))
                    {
                        Log.Warn(this, "TAG FileAttributes not first in file, but at Tag #" + i.ToString());
                    }
                    fileAttributesCnt++;
                }
            }
            if (fileAttributesCnt > 1)
            {
                string msg = "Multiple FileAttributes Tags: " + fileAttributesCnt.ToString();

                if (!SwfFile.Configuration.AllowMultipleFileAttributes)
                {
                    Log.Error(this, msg);
                    return false;
                }
                else
                {
                    Log.Warn(this, msg);
                }
            }
            //
            // If the Swf version is 8 or higher, one FileAttributes Tag is required
            // 
            if ((this.Version >= 8) && (fileAttributesCnt < 1))
            {
                if (SwfFile.Configuration.RequireFileAttributes)
                {
                    String s = String.Format("Swf version {0:d} requires FileAttributes Tag, but none was found", this.Version);
                    Log.Error(this, s);
                    return false;
                }
                else
                {
                    String s = String.Format("Swf version {0:d} requires FileAttributes Tag, but none was found - fixing it", this.Version);
                    Log.Warn(this, s);
                    this.FixFileAttributes();
                }
            }

            //
            // validate all Tags
            //
            bool result = true;

            for (int i = 0; i < this.TagCount; i++)
            {
                result = result && this[i].Verify();

                if (!result)
                {
                    //
                    // Terminate verification when it failed one Tag
                    //
                    String s = String.Format("Tag #{0:d} {1} failed verification", i, this[i].Tag.TagTypeName);
                    Log.Info(this, s);
                    break;
                }
            }

            // Fire VerficationCompleted event
            if (null != VerificationCompleted)
            {
                VerificationCompleted();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Version
        {
            get
            {
                return FwsSource.Version;
            }
            set
            {
                FwsSource.Version = value;

                if (null != TagHandlers)
                {
                    for (int i = 0; i < TagHandlers.Count; i++)
                    {
                        //
                        // Set the version information, otherwise the length calculation is wrong
                        //                
                        TagHandlers[i].Version = this.Version;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TagHandler.AbstractTagHandler this[int i]
        {
            get
            {
                return TagHandlers[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TagCount
        {
            get
            {
                if (null != TagHandlers)
                    return TagHandlers.Count;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charID"></param>
        /// <returns></returns>
        public TagHandler.AbstractTagHandler GetCharacterByID(UInt16 charID)
        {
            for (int i = 0; i < this.TagHandlers.Count; i++)
            {
                if (this.TagHandlers[i] is Recurity.Swf.Helper.ISwfCharacter)
                {
                    Recurity.Swf.Helper.ISwfCharacter character = (Recurity.Swf.Helper.ISwfCharacter)this.TagHandlers[i];

                    if (character.CharacterID.Equals(charID))
                    {
                        return TagHandlers[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Fixes the incorrect stream size by changing the length of a stream and dumping all data that do not fit into it.
        /// </summary>
        /// <param name="s">The stream that will be fixed</param>
        /// <param name="l">The new length of the stream</param>
        private void FixIncorrectStreamSize(Stream s, long l)
        {
            byte[] b = new byte[l];

            long position = s.Position;
            s.Seek(0, SeekOrigin.Begin);

            if (s.Length >= l)
            {
                s.Read(b, 0, (int)l);
            }
            else
            {
                s.Read(b, 0, (int)s.Length);
            }

            MemoryStream newStream = new MemoryStream(b);
            s = newStream;
            s.Seek(position, SeekOrigin.Begin);
        }
    }
}
