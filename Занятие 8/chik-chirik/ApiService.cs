using System.Net.Http.Json;
using chik_chirik.Models;

namespace chik_chirik
{
	public class ApiService
	{
		public async Task<List<Post>> GetPostsAsync(string searchTerm = null)
		{
			throw new NotImplementedException();
		}

		// Получить комментарии к посту  
		public async Task<List<Comment>> GetCommentsAsync(int postId)
		{
			throw new NotImplementedException();
		}
	}
}