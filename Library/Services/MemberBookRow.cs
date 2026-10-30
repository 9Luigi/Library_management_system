namespace Library.Services
{
	/// <summary>
	/// One borrowed book of a member (column order matters: the return form reads BookID by index).
	/// </summary>
	internal record MemberBookRow(long IIN, int BookID, string? BookTitle);
}
