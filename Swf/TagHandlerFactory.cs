using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Recurity.Swf.Configuration;

namespace Recurity.Swf
{
    /// <summary>
    /// 
    /// </summary>
    public class TagHandlerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="sourceFile"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static TagHandler.AbstractTagHandler Create(Tag tag, SwfFile sourceFile, Stream inputStream)
        {
            TagHandler.AbstractTagHandler product = null;

            //
            // Make parsing non-linear by explicitly placing the inputStream at the 
            // OffsetData of the Tag.
            //
            inputStream.Seek((long)tag.OffsetData, SeekOrigin.Begin);
            long before = inputStream.Position;

            //
            // Produce TagHandler
            //      
            bool enabled = false;

            try
            {
                enabled = SwfFile.Configuration.TagHandlers(tag.TagTypeName);
            }
            catch(Exception e)
            {
                Log.Error("TagHandlerFactory", "The tag" + tag.TagTypeName + "could not be found in the Dictionary");
                throw e;
            }

            if (!enabled)
            {
                product = new TagHandler.GenericTag(sourceFile.Version);
                Log.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TagHandler for Tag type " + tag.TagTypeName + " disabled in configuration, using generic");
            }


            if (null == product)
            {

                switch (tag.TagType)
                {
                    // Actions section
                    //
                    case TagTypes.DoAction:
                        product = new TagHandler.DoAction(sourceFile.Version);
                        break;

                    case TagTypes.DoInitAction:
                        product = new TagHandler.DoInitAction(sourceFile.Version);
                        break;

                    case TagTypes.FileAttributes:
                        product = new TagHandler.FileAttributes(sourceFile.Version);
                        break;

                    case TagTypes.PlaceObject:
                        product = new TagHandler.PlaceObject(sourceFile.Version);
                        break;

                    case TagTypes.PlaceObject2:
                        product = new TagHandler.PlaceObject2(sourceFile.Version);
                        break;

                    case TagTypes.PlaceObject3:
                        product = new TagHandler.PlaceObject3(sourceFile.Version);
                        break;

                    // Shapes section
                    //
                    case TagTypes.DefineShape:
                        product = new TagHandler.DefineShape(sourceFile.Version);
                        break;

                    case TagTypes.DefineShape2:
                        product = new TagHandler.DefineShape(sourceFile.Version);
                        break;

                    case TagTypes.DefineShape3:
                        product = new TagHandler.DefineShape(sourceFile.Version);
                        break;

                    case TagTypes.DefineShape4:
                        product = new TagHandler.DefineShape4(sourceFile.Version);
                        break;

                    // Bitmap section
                    //
                    case TagTypes.DefineBits:
                        product = new TagHandler.DefineBits(sourceFile.Version);
                        break;

                    case TagTypes.JPEGTables:
                        product = new TagHandler.JPEGTables(sourceFile.Version);
                        break;

                    case TagTypes.DefineBitsJPEG2:
                        product = new TagHandler.DefineBitsJPEG2(sourceFile.Version);
                        break;

                    case TagTypes.DefineBitsJPEG3:
                        product = new TagHandler.DefineBitsJPEG3(sourceFile.Version);
                        break;

                    case TagTypes.DefineBitsLossless:
                        product = new TagHandler.DefineBitsLossless(sourceFile.Version);
                        break;

                    case TagTypes.DefineBitsLossless2:
                        product = new TagHandler.DefineBitsLossless2(sourceFile.Version);
                        break;

                    // Shape Morphing Section
                    //
                    case TagTypes.DefineMorphShape:
                        product = new TagHandler.DefineMorphShape(sourceFile.Version);
                        break;
                    case TagTypes.DefineMorphShape2:
                        product = new TagHandler.DefineMorphShape2(sourceFile.Version);
                        break;

                    // Fonts and Text section
                    //
                    case TagTypes.DefineFont:
                        product = new TagHandler.DefineFont(sourceFile.Version);
                        break;

                    case TagTypes.DefineFont2:
                        product = new TagHandler.DefineFont2(sourceFile.Version);
                        break;

                    case TagTypes.DefineFont3:
                        product = new TagHandler.DefineFont3(sourceFile.Version);
                        break;

                    case TagTypes.DefineFont4:
                        product = new TagHandler.DefineFont4(sourceFile.Version);
                        break;

                    case TagTypes.DefineFontInfo:
                        product = new TagHandler.DefineFontInfo(sourceFile.Version);
                        break;

                    case TagTypes.DefineFontInfo2:
                        product = new TagHandler.DefineFontInfo(sourceFile.Version);
                        break;

                    case TagTypes.DefineFontAlignZones:
                        product = new TagHandler.DefineFontAlignZones(sourceFile.Version);
                        break;

                    case TagTypes.DefineFontName:
                        product = new TagHandler.DefineFontName(sourceFile.Version);
                        break;

                    case TagTypes.DefineText:
                        product = new TagHandler.DefineText(sourceFile.Version);
                        break;

                    case TagTypes.DefineText2:
                        product = new TagHandler.DefineText(sourceFile.Version);
                        break;

                    case TagTypes.DefineEditText:
                        product = new TagHandler.DefineEditText(sourceFile.Version);
                        break;

                    case TagTypes.CSMTextSettings:
                        product = new TagHandler.CsmTextSettings(sourceFile.Version);
                        break;

                    // Sounds Section
                    //
                    case TagTypes.DefineSound:
                        product = new TagHandler.DefineSound(sourceFile.Version);
                        break;

                    case TagTypes.StartSound:
                        product = new TagHandler.StartSound(sourceFile.Version);
                        break;

                    case TagTypes.StartSound2:
                        product = new TagHandler.StartSound2(sourceFile.Version);
                        break;

                    case TagTypes.SoundStreamHead:
                        product = new TagHandler.SoundStreamHead(sourceFile.Version);
                        break;

                    case TagTypes.SoundStreamHead2:
                        product = new TagHandler.SoundStreamHead2(sourceFile.Version);
                        break;

                    case TagTypes.SoundStreamBlock:
                        product = new TagHandler.SoundStreamBlock(sourceFile.Version);
                        break;


                    // Control Section
                    //
                    case TagTypes.DefineSceneAndFrameLabelData:
                        product = new TagHandler.DefineSceneAndFrameLabelData(sourceFile.Version);
                        break;

                    case TagTypes.DefineScalingGrid:
                        product = new TagHandler.DefineScalingGrid(sourceFile.Version);
                        break;

                    case TagTypes.SetTabIndex:
                        product = new TagHandler.SetTabIndex(sourceFile.Version);
                        break;

                    case TagTypes.SetBackgroundColor:
                        product = new TagHandler.SetBackgroundColor(sourceFile.Version);
                        break;

                    case TagTypes.Protect:
                        product = new TagHandler.Protect(sourceFile.Version);
                        break;

                    case TagTypes.Metadata:
                        product = new TagHandler.Metadata(sourceFile.Version);
                        break;

                    case TagTypes.ImportAssets:
                        product = new TagHandler.ImportAssets(sourceFile.Version);
                        break;

                    case TagTypes.ImportAssets2:
                        product = new TagHandler.ImportAssets2(sourceFile.Version);
                        break;

                    case TagTypes.FrameLabel:
                        product = new TagHandler.FrameLabel(sourceFile.Version);
                        break;


                    case TagTypes.ExportAssets:
                        product = new TagHandler.ExportAssets(sourceFile.Version);
                        break;

                    case TagTypes.EnableDebugger:
                        product = new TagHandler.EnableDebugger(sourceFile.Version);
                        break;

                    case TagTypes.EnableDebugger2:
                        product = new TagHandler.EnableDebugger2(sourceFile.Version);
                        break;

                    // Buttons Section
                    //
                    case TagTypes.DefineButton:
                        product = new TagHandler.DefineButton(sourceFile.Version);
                        break;

                    case TagTypes.DefineButton2:
                        product = new TagHandler.DefineButton2(sourceFile.Version);
                        break;

                    // Sprites and Movie Clips section
                    //
                    case TagTypes.DefineSprite:
                        product = new TagHandler.DefineSprite(sourceFile.Version);
                        break;

                    // Video section
                    //

                    // Binary Data section
                    //

                    // Blitzabeleiter internals

                    case TagTypes.ProductID:
                        product = new TagHandler.ProductID(sourceFile.Version);
                        break;

                    case TagTypes.DebugID:
                        product = new TagHandler.DebugID(sourceFile.Version);
                        break;

                    case TagTypes.DefineBinaryData:
                        product = new TagHandler.DefineBinaryData(sourceFile.Version);
                        break;

                    // ABC / AVM2 Code
                    case TagTypes.DoABC:
                        product = new TagHandler.DoABC(sourceFile.Version);
                        break;

                    //case TagTypes.ScriptLimits:
                    //    product = new TagHandler.ScriptLimits(sourceFile.Version);
                    //    break;

                    default:
                        product = new TagHandler.GenericTag(sourceFile.Version);
                        break;
                }
            }

            // Exceptions during this operation propagate all 
            // the way up to SwfFile
            product.Read(tag, sourceFile, inputStream);


            // Note: we don't have to check that the stream did only consume as much 
            // data as the Tag declared, since the Tag.ReadContent() method reads only
            // the declared amount of data from the Tag and produces a MemoryStream with
            // that. Tag.ReadContent() is used by AbstractTagHandler.Read().


            return product;
        }
    }
}
