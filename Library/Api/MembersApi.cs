using Library.Contracts.Members;
using Library.Presentation.Settings;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Library.Api
{
	/// <summary>
	/// Calls api/members on Library.Server. Every method throws <see cref="ApiException"/> if the server
	/// answers with an error, <see cref="HttpRequestException"/> if the server can't be reached
	/// and <see cref="TaskCanceledException"/> if it doesn't answer in time.
	/// </summary>
	internal static class MembersApi
	{
		// One HttpClient for the whole app: creating one per request exhausts sockets
		private static readonly HttpClient Http = new()
		{
			BaseAddress = new Uri(UserSettings.Current.ServerUrl.TrimEnd('/') + "/api/members/"),
			// The default is 100 seconds: too long to keep the user waiting
			Timeout = TimeSpan.FromSeconds(10)
		};

		internal static async Task<List<MemberDto>> GetAllAsync()
		{
			using var response = await Http.GetAsync("");
			return await ReadAsync<List<MemberDto>>(response);
		}

		internal static async Task<MemberDto> GetAsync(long iin)
		{
			using var response = await Http.GetAsync($"{iin}");
			return await ReadAsync<MemberDto>(response);
		}

		internal static async Task<MemberDto> CreateAsync(CreateMemberRequest request)
		{
			using var response = await Http.PostAsJsonAsync("", request);
			return await ReadAsync<MemberDto>(response);
		}

		internal static async Task<MemberDto> UpdateAsync(long iin, UpdateMemberRequest request)
		{
			using var response = await Http.PutAsJsonAsync($"{iin}", request);
			return await ReadAsync<MemberDto>(response);
		}

		internal static async Task DeleteAsync(long iin)
		{
			using var response = await Http.DeleteAsync($"{iin}");
			await EnsureSuccessAsync(response);
		}

		private static async Task<T> ReadAsync<T>(HttpResponseMessage response)
		{
			await EnsureSuccessAsync(response);
			return (await response.Content.ReadFromJsonAsync<T>())!;
		}

		private static async Task EnsureSuccessAsync(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode) return;

			string body = await response.Content.ReadAsStringAsync();
			throw new ApiException(response.StatusCode, GetErrorMessage(response.StatusCode, body));
		}

		/// <summary>
		/// The server sends either plain text (e.g. 409 "already exists") or ProblemDetails JSON (400, 404, 500).
		/// </summary>
		private static string GetErrorMessage(HttpStatusCode statusCode, string body)
		{
			if (statusCode == HttpStatusCode.NotFound)
				return "Member not found. Probably it was deleted by another employee.";

			try
			{
				using var json = JsonDocument.Parse(body);
				// 400 from validation: { "errors": { "Name": ["The Name field is required."] } }
				if (json.RootElement.TryGetProperty("errors", out var errors))
				{
					return string.Join(Environment.NewLine, errors.EnumerateObject()
						.SelectMany(field => field.Value.EnumerateArray().Select(e => e.GetString())));
				}
				if (json.RootElement.TryGetProperty("title", out var title))
					return title.GetString() ?? statusCode.ToString();
			}
			catch (JsonException)
			{
				// Not JSON: plain text message
			}

			return string.IsNullOrWhiteSpace(body) ? $"Server error: {(int)statusCode} {statusCode}" : body;
		}
	}
}
