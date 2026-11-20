using System.Net;

namespace Library.Api
{
	/// <summary>
	/// The server answered with an error (4xx/5xx). Message is ready to be shown to the user.
	/// </summary>
	internal class ApiException(HttpStatusCode statusCode, string message) : Exception(message)
	{
		public HttpStatusCode StatusCode { get; } = statusCode;
	}
}
