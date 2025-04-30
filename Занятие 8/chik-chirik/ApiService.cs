using System.Net.Http.Json;
using chik_chirik.Models;
using System.Linq;

namespace chik_chirik
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        }

        public async Task<List<Post>> GetPostsAsync(string? searchTerm = null)
        {
            var postsTask = _httpClient.GetFromJsonAsync<List<Post>>("posts") ?? Task.FromResult(new List<Post>());
            var usersTask = _httpClient.GetFromJsonAsync<List<User>>("users") ?? Task.FromResult(new List<User>());

            await Task.WhenAll(postsTask, usersTask);

            var posts = await postsTask;
            var users = await usersTask;

            var result = posts.Select(post =>
            {
                post.User = users.FirstOrDefault(u => u.Id == post.UserId);
                return post;
            }).ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = result.Where(p =>
                    p.User != null &&
                    !string.IsNullOrEmpty(p.User.Username) &&
                    p.User.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return result;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            var response = await _httpClient.GetAsync($"posts/{postId}/comments");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Comment>>() ?? new List<Comment>();
        }
    }
}