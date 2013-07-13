using System;

namespace Recurity.Swf
{
    /// <summary>
    /// The level of a message that will be logged
    /// </summary>
    public enum LogLevel : byte
    {
        /// <summary>
        /// The log message is of kind "Info"
        /// </summary>
        Info = 0,
        
        /// <summary>
        /// The log message is of kind "Debug"
        /// </summary>
        Debug = 1,
        
        /// <summary>
        /// The log message is of kind "Warn"
        /// </summary>
        Warn = 2,
        
        /// <summary>
        /// The log message is of kind "Error"
        /// </summary>
        Error = 3
    }

    /// <summary>
    /// Represents the method that will handle the FileReadProgressChanged event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void SwfFileReadProgressChangedEventHandler(object sender, SwfReadProgressChangedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the FileWriteProgressChanged event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void SwfFileWriteProgressChangedEventHandler(object sender, SwfWriteProgressChangedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the FileProtected event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void SwfFileProtectedEventHandler(object sender, ProtectionEventArgs e);

    /// <summary>
    /// Represents the method that will handle the FileCompressed event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void SwfFileCompressedEventHandler(object sender, CompressedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the UncompressingFile event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void FileDecompressEventHandler(object sender, SwfDecompressSateChangedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the CompressingFile event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void FileCompressEventHandler(object sender, SwfCompressStateChangedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the TagProduced event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void TagProducedEventHandler(object sender, TagHandlerProducedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the TagReadCompleted event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event args</param>
    public delegate void TagReadCompletedEventHandler(object sender, TagHandlerReadCompleteEventArgs e);

    /// <summary>
    /// Represents the method that will handle the VerificationCompleted event.
    /// </summary>
    public delegate void VerificationCompletedEventHandler();

    /// <summary>
    /// Represents the method that will handle the Log events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LogEventHandler(object sender, LogEventArgs e);

    /// <summary>
    /// Provides data for SwfReadProgressChanged events
    /// </summary>
    public class SwfReadProgressChangedEventArgs : EventArgs
    {
        private Int64 _FileSize;
        private Int64 _ReadPosition;

        /// <summary>
        /// Provides data for the ReadProgressChanged events.
        /// </summary>
        /// <param name="size">The size of the stream.</param>
        /// <param name="position">The stream position</param>
        public SwfReadProgressChangedEventArgs(Int64 size, Int64 position)
        {
            this._FileSize = size;
            this._ReadPosition = position;
        }

        /// <summary>
        /// The size of the data stream 
        /// </summary>
        public Int64 Size
        {
            get
            {
                return this._FileSize;
            }
        }

        /// <summary>
        /// The position in the data stream
        /// </summary>
        public Int64 Position
        {
            get
            {
                return this._ReadPosition;
            }
        }

        /// <summary>
        /// The progress in percent
        /// </summary>
        public Double Percent
        {
            get
            {
                if (0 != this._FileSize)
                {
                    return 100 * ((Double)this._ReadPosition / (Double)this._FileSize);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// Provides data for SwfWriteProgressChanged events
    /// </summary>
    public class SwfWriteProgressChangedEventArgs : EventArgs
    {
        private Int64 _FileSize;
        private Int64 _WritePosition;
        private TagTypes _Type;
        /// <summary>
        /// Provides data for the WriteProgressChanged events.
        /// </summary>
        /// <param name="type">The type of the tag that has been written</param>
        /// <param name="size">The size of the stream.</param>
        /// <param name="position">The stream position.</param>
        public SwfWriteProgressChangedEventArgs(TagTypes type,  Int64 size, Int64 position)
        {
            this._FileSize = size;
            this._WritePosition = position;
            this._Type = type;
        }

        /// <summary>
        /// The type of the tag
        /// </summary>
        public TagTypes Type
        {
            get
            {
                return this._Type;
            }
        }

        /// <summary>
        /// The size of the stream
        /// </summary>
        public Int64 Size
        {
            get
            {
                return this._FileSize;
            }
        }

        /// <summary>
        /// The stream position
        /// </summary>
        public Int64 Position
        {
            get
            {
                return this._WritePosition;
            }
        }

        /// <summary>
        /// Progress in percent
        /// </summary>
        public Double Percent
        {
            get
            {
                if (0 != this._FileSize)
                {
                    return 100 * ((Double)this._WritePosition / (Double)this._FileSize);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// Provides data for Protection events.
    /// </summary>
    public class ProtectionEventArgs : EventArgs
    {
        private Boolean _Protected;
        private String _Password;

        /// <summary>
        /// Will be fired if a Swf file is protected with an password
        /// </summary>
        public ProtectionEventArgs()
        {
            this._Protected = false;
        }

        /// <summary>
        /// Will be fired if a Swf file is protected with an password
        /// </summary>
        /// <param name="passwordHash">The password hash</param>
        public ProtectionEventArgs(String passwordHash)
        {
            this._Protected = true;
            this._Password = passwordHash;
        }

        /// <summary>
        /// The password hash as string
        /// </summary>
        public String Password
        {
            get
            {
                return this._Password;
            }

        }

        /// <summary>
        /// If the Swf file is protected
        /// </summary>
        public Boolean Protected
        {
            get
            {
                return this._Protected;
            }

        }
    }

    /// <summary>
    /// Provides data for the Compressed events.
    /// </summary>
    public class CompressedEventArgs : EventArgs
    {
        private Boolean _Compressed;

        /// <summary>
        /// Determines whether a file is compressed or not
        /// </summary>
        /// <param name="compressed">True if a Swf file is compressed</param>
        public CompressedEventArgs(Boolean compressed)
        {
            this._Compressed = compressed;
        }

        /// <summary>
        /// Returns true if a Swf file is compressed
        /// </summary>
        public Boolean Comrpessed
        {
            get
            {
                return this._Compressed;
            }
        }
    }

    /// <summary>
    /// Provides data for DecompressSateChanged events.
    /// </summary>
    public class SwfDecompressSateChangedEventArgs : EventArgs
    {
        private UInt64 _OverAll;
        private UInt64 _Recent;

        /// <summary>
        /// Provides data for DecompressSateChanged events.
        /// </summary>
        /// <param name="overAll">The stream length</param>
        /// <param name="currentState">The stream position</param>
        public SwfDecompressSateChangedEventArgs(UInt64 overAll, UInt64 currentState)
        {
            this._OverAll = overAll;
            this._Recent = currentState;
        }

        /// <summary>
        /// The length of the data over all
        /// </summary>
        public UInt64 OverAll
        {
            get
            {
                return this._OverAll;
            }
        }

        /// <summary>
        /// The current position in the data stream
        /// </summary>
        public UInt64 Current
        {
            get
            {
                return this._Recent;
            }
        }

        /// <summary>
        /// The progress in percent
        /// </summary>
        public Double Percent
        {
            get
            {
                if (0 != this._OverAll)
                {
                    return 100 * ((Double)this._Recent / (Double)this._OverAll);
                }
                else
                {
                    return 0;
                }
            }
        }

    }

    /// <summary>
    /// Provides data for CompressSateChanged events.
    /// </summary>
    public class SwfCompressStateChangedEventArgs : EventArgs
    {
        private UInt64 _OverAll;
        private UInt64 _Recent;

        /// <summary>
        /// Provides data for CompressSateChanged events.
        /// </summary>
        /// <param name="overAll">The stream length</param>
        /// <param name="currentState">The stream position</param>
        public SwfCompressStateChangedEventArgs(UInt64 overAll, UInt64 currentState)
        {
            this._OverAll = overAll;
            this._Recent = currentState;
        }

        /// <summary>
        /// The length of the data over all
        /// </summary>
        public UInt64 OverAll
        {
            get
            {
                return this._OverAll;
            }
        }

        /// <summary>
        /// The current position in the data stream
        /// </summary>
        public UInt64 Current
        {
            get
            {
                return this._Recent;
            }
        }

        /// <summary>
        /// The progress in percent
        /// </summary>
        public Double Percent
        {
            get
            {
                if (0 != this._OverAll)
                {
                    return 100 * ((Double)this._Recent / (Double)this._OverAll);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// Provides data for TagHandlerProduced events.
    /// </summary>
    public class TagHandlerProducedEventArgs : EventArgs
    {
        private TagTypes _TagType;
        private Int64 _NumberHandlers;
        private Int64 _NumberHandler;

        /// <summary>
        /// Provides data for TagHandlerProduced events.
        /// </summary>
        /// <param name="type">The tag type</param>
        /// <param name="handlers">Number of handlers overall</param>
        /// <param name="handler">Index of this handler</param>
        public TagHandlerProducedEventArgs(TagTypes type, Int64 handlers, Int64 handler)
        {
            this._TagType = type;
            this._NumberHandlers = handlers;
            this._NumberHandler = handler;
        }

        /// <summary>
        /// Number of handlers overall 
        /// </summary>
        public Int64 Handlers
        {
            get
            {
                return this._NumberHandlers;
            }
        }

        /// <summary>
        /// Index of this handler
        /// </summary>
        public Int64 Handler
        {
            get
            {
                return this._NumberHandler;
            }
        }

        /// <summary>
        /// The tag type
        /// </summary>
        public TagTypes Type
        {
            get
            {
                return this._TagType;
            }
        }
    }

    /// <summary>
    /// Provides data for TagHandlerReadComplete events. 
    /// </summary>
    public class TagHandlerReadCompleteEventArgs : EventArgs
    {
        private TagTypes _TagType;

        /// <summary>
        /// Provides data for TagHandlerReadComplete events. 
        /// </summary>
        /// <param name="type">The type of the tag just been red</param>
        public TagHandlerReadCompleteEventArgs(TagTypes type)
        {
            this._TagType = type;
        }

        /// <summary>
        /// The type of the tag
        /// </summary>
        public TagTypes Type
        {
            get
            {
                return this._TagType;
            }
        }

    }

    /// <summary>
    /// Provides data Dump events. 
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// The kind of the log message
        /// </summary>
        public LogLevel Level { get; private set; }
        
        /// <summary>
        /// The date and time teh message has been logged
        /// </summary>
        public DateTime Datetime { get; private set; }
        
        /// <summary>
        /// The actual log message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Provides data for Log events. 
        /// </summary>
        /// <param name="l">The kind of the log message</param>
        /// <param name="d">The date and time teh message has been logged</param>
        /// <param name="message">The actual log message</param>
        public LogEventArgs(LogLevel l, DateTime d, string message)
        {
            this.Datetime = d;
            this.Level = l;
            this.Message = message;
        }

    }
}
