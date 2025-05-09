using System.Net.Http.Json;
using chik_chirik.Models;

namespace chik_chirik
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
		private const string BaseUrl = "https://jsonplaceholder.typicode.com";

		public ApiService()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(BaseUrl)
			};
		}

		public ApiService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
		{
			var posts = await _httpClient.GetFromJsonAsync<List<Post>>("/posts");
			var users = await _httpClient.GetFromJsonAsync<List<User>>("/users");

			if (posts == null || users == null)
				return new List<Post>();

			foreach (var post in posts)
			{
				post.User = users.FirstOrDefault(u => u.Id == post.UserId);
			}

			if (!string.IsNullOrEmpty(searchTerm))
			{
				posts = posts.Where(p => p.User?.Username?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true).ToList();
			}

			return posts;
		}

		// Получить комментарии к посту  
		public async Task<List<Comment>> GetCommentsAsync(int postId)
		{
			var response = await _httpClient.GetAsync($"/posts/{postId}/comments");
			response.EnsureSuccessStatusCode();
			
			var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
			return comments ?? new List<Comment>();
		}
	}
}