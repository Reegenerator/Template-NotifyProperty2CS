namespace RgenLib.TaggedSegment {
    public class TaggedRange: TextRange {
        public TagFormat TagFormat { get; set; }

        public SegmentTypes SegmentType { get; set; }

        public TaggedRange Clone()
        {
            return (TaggedRange)MemberwiseClone();
        }
    }
}
