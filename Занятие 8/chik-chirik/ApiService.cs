using System.Net.Http.Json;
using chik_chirik.Models;

namespace chik_chirik
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
        {
            var posts = await _httpClient.GetFromJsonAsync<List<Post>>("/posts");
            var users = await _httpClient.GetFromJsonAsync<List<User>>("/users");

            if (posts == null || users == null)
                return new List<Post>();

            var result = posts.Select(post =>
            {
                post.User = users.FirstOrDefault(u => u.Id == post.UserId);
                return post;
            }).ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = result.Where(p =>
                    p.User != null &&
                    p.User.Username != null &&
                    p.User.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return result;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            var comments = await _httpClient.GetFromJsonAsync<List<Comment>>($"/posts/{postId}/comments");
            return comments ?? new List<Comment>();
        }
    }
}