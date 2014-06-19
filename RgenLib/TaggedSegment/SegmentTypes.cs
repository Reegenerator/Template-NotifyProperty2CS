
namespace RgenLib.TaggedSegment
{
	public enum SegmentTypes
	{
		Region,
        /// <summary>
        /// Specially constructed comment only used for VB code, which does not allow region within method body
        /// </summary>
		Statements
	}
}