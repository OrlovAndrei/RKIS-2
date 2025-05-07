using System.Net.Http;
using System.Net.Http.Json;
using chik_chirik.Models;

namespace chik_chirik
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
        {
            var users = await _httpClient.GetFromJsonAsync<List<User>>("https://jsonplaceholder.typicode.com/users");
            var posts = await _httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");

            if (users == null || posts == null)
                return new List<Post>();

            var usersById = users.ToDictionary(u => u.Id);
            foreach (var post in posts)
            {
                if (usersById.TryGetValue(post.UserId, out var user))
                {
                    post.User = user;
                }
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts
                    .Where(p => p.User?.Username?.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }

            return posts;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{postId}/comments");
            response.EnsureSuccessStatusCode();

            var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
            return comments ?? new List<Comment>();
        }
    }
}