using System;

namespace RgenLib.TaggedSegment {
    /// <summary>
    /// This class is only used to wrap Tag so the json produced is wrapped in {Reegenerator:{...Tag...}}
    /// </summary>
    public partial class Manager<TRenderer, TOptionAttr>
        where TRenderer : TaggedCodeRenderer, new()
        where TOptionAttr : Attribute, new()
    {
        private class TagWrapper<T> where T : TaggedCodeRenderer, new()
        {
            public Tag Reegenerator { get; set; }

            public TagWrapper(Tag tag)
            {
                Reegenerator = tag;
            }

            public const string MainPropertyName = "Reegenerator";

            public static TagWrapper<T> Wrap(Tag tag)
            {
                return new TagWrapper<T>(tag);
            }
        }
    }
}
