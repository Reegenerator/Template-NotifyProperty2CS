
namespace RgenLib.TaggedSegment
{
	public enum SegmentTypes
	{
		Region,
        /// <summary>
        /// Specially constructed pair of comment only used for VB code, which does not allow region within method body
        /// </summary>
		CommentPair,
        /// <summary>
        /// Single line comment , used for InsertionPoint
        /// </summary>
        SingleComment
	}
}