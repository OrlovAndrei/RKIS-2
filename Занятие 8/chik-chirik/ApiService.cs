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
            var usersResponse = await _httpClient.GetFromJsonAsync<List<User>>("https://example.com/users");
            var postsResponse = await _httpClient.GetFromJsonAsync<List<Post>>("https://example.com/posts");

            foreach (var post in postsResponse)
            {
                post.User = usersResponse.Find(u => u.Id == post.UserId);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                postsResponse = postsResponse.FindAll(p => p.User?.Username == searchTerm);
            }

            return postsResponse;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            var comments = await _httpClient.GetFromJsonAsync<List<Comment>>($"https://example.com/posts/{postId}/comments");
            return comments ?? new List<Comment>();
        }
    }
}