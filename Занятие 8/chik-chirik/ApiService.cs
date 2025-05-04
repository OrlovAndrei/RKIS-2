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
        }
		public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
		{
			// 1. Загружаем все посты
            var posts = await _httpClient.GetFromJsonAsync<List<Post>>($"{BaseUrl}/posts");
            
            // 2. Загружаем всех пользователей
            var users = await _httpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}/users");
            
            // 3. Связываем посты с авторами
            foreach (var post in posts)
            {
                post.User = users.FirstOrDefault(u => u.Id == post.UserId);
            }

            // 4. Фильтруем по поисковому запросу (если он есть)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                posts = posts
                    .Where(p => p.User != null && 
                               p.User.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return posts;
		}

		// Получить комментарии к посту  
		public async Task<List<Comment>> GetCommentsAsync(int postId)
		{
			return await _httpClient.GetFromJsonAsync<List<Comment>>($"{BaseUrl}/posts/{postId}/comments");
		}
	}
}