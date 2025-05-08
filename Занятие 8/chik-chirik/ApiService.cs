using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using chik_chirik.Models;

namespace chik_chirik
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        }

        public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
        {
            var posts = await _httpClient.GetFromJsonAsync<List<Post>>("posts");
            var users = await _httpClient.GetFromJsonAsync<List<User>>("users");

            foreach (var post in posts)
            {
                post.User = users.FirstOrDefault(u => u.Id == post.UserId);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = posts
                    .Where(p => p.User != null &&
                                p.User.Username.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return posts;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            return await _httpClient.GetFromJsonAsync<List<Comment>>($"posts/{postId}/comments");
        }
    }
}