using System;
using System.Collections.Generic;
using System.Text;
using Recurity.Swf.TagHandler;

namespace Recurity.Swf.Helper
{
    /// <summary>
    /// This class implements the base of all definition Tags that define an Swf character.
    /// </summary>
    public interface ISwfCharacter
    {

        /// <summary>
        /// Id of the Character that is defined by this tag
        /// </summary>
        UInt16 CharacterID { get; }



    }
}
