using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
