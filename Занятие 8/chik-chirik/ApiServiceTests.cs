using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net;
using chik_chirik.Models;

namespace chik_chirik
{

	[TestFixture]
	public class ApiServiceTests
	{
		private Mock<HttpMessageHandler> _handlerMock;
		private HttpClient _httpClient;
		private ApiService _apiService;

		[SetUp]
		public void Setup()
		{
			_handlerMock = new Mock<HttpMessageHandler>();
			_httpClient = new HttpClient(_handlerMock.Object);
			_apiService = new ApiService(_httpClient);
		}

		[Test]
		public async Task GetPostsAsync_ReturnsPostsWithUsers()
		{
			// Arrange
			var usersResponse = new List<User>
		{
			new User { Id = 1, Username = "User1" },
			new User { Id = 2, Username = "User2" }
		};

			var postsResponse = new List<Post>
		{
			new Post { Id = 1, UserId = 1 },
			new Post { Id = 2, UserId = 2 }
		};

			SetupMockResponse("users", usersResponse);
			SetupMockResponse("posts", postsResponse);

			// Act
			var result = await _apiService.GetPostsAsync();

			// Assert
			Assert.That(result, Has.Count.EqualTo(2));
			Assert.That(result[0].User, Is.Not.Null);
			Assert.That(result[0].User.Username, Is.EqualTo("User1"));
		}

		[Test]
		public async Task GetPostsAsync_FiltersBySearchTermCorrectly()
		{
			// Arrange
			var usersResponse = new List<User>
		{
			new User { Id = 1, Username = "User1" },
			new User { Id = 2, Username = "User2" }
		};

			var postsResponse = new List<Post>
		{
			new Post { Id = 1, UserId = 1 },
			new Post { Id = 2, UserId = 2 }
		};

			SetupMockResponse("users", usersResponse);
			SetupMockResponse("posts", postsResponse);

			// Act
			var result = await _apiService.GetPostsAsync("User1");

			// Assert
			Assert.That(result, Has.Count.EqualTo(1));
			Assert.That(result[0].User.Username, Is.EqualTo("User1"));
		}

		[Test]
		public async Task GetPostsAsync_ReturnsEmptyForInvalidSearchTerm()
		{
			// Arrange
			SetupMockResponse("users", new List<User>());
			SetupMockResponse("posts", new List<Post>());

			// Act
			var result = await _apiService.GetPostsAsync("InvalidUser");

			// Assert
			Assert.That(result, Is.Empty);
		}

		[Test]
		public async Task GetCommentsAsync_ReturnsCommentsForValidPost()
		{
			// Arrange
			var expectedComments = new List<Comment>
		{
			new Comment { Id = 1, PostId = 1 },
			new Comment { Id = 2, PostId = 1 }
		};

			SetupMockResponse("posts/1/comments", expectedComments);

			// Act
			var result = await _apiService.GetCommentsAsync(1);

			// Assert
			Assert.That(result, Has.Count.EqualTo(2));
		}

		[Test]
		public async Task GetCommentsAsync_ReturnsEmptyForInvalidPost()
		{
			// Arrange
			SetupMockResponse("posts/999/comments", new List<Comment>());

			// Act
			var result = await _apiService.GetCommentsAsync(999);

			// Assert
			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GetCommentsAsync_ThrowsForInvalidResponse()
		{
			// Arrange
			_handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError
				});

			// Act & Assert
			Assert.ThrowsAsync<HttpRequestException>(async () =>
				await _apiService.GetCommentsAsync(1));
		}

		// Общие тесты данных
		[Test]
		public async Task GetPostsAsync_ReturnsCompletePostData()
		{
			// Arrange
			var testPost = new Post
			{
				Id = 1,
				UserId = 1,
				Title = "Test Title",
				Body = "Test Body"
			};

			SetupMockResponse("posts", new List<Post> { testPost });
			SetupMockResponse("users", new List<User> { new User { Id = 1 } });

			// Act
			var result = await _apiService.GetPostsAsync();

			// Assert
			var post = result[0];
			Assert.Multiple(() =>
			{
				Assert.That(post.Title, Is.EqualTo("Test Title"));
				Assert.That(post.Body, Is.EqualTo("Test Body"));
				Assert.That(post.User, Is.Not.Null);
			});
		}

		[Test]
		public async Task GetCommentsAsync_ReturnsValidCommentStructure()
		{
			// Arrange
			var testComment = new Comment
			{
				Id = 1,
				Name = "Test",
				Email = "test@example.com",
				Body = "Test comment"
			};

			SetupMockResponse("posts/1/comments", new List<Comment> { testComment });

			// Act
			var result = await _apiService.GetCommentsAsync(1);

			// Assert
			var comment = result[0];
			Assert.Multiple(() =>
			{
				Assert.That(comment.Name, Is.EqualTo("Test"));
				Assert.That(comment.Email, Is.EqualTo("test@example.com"));
				Assert.That(comment.Body, Is.EqualTo("Test comment"));
			});
		}

		[Test]
		public async Task GetPostsAsync_HandlesLargeDataSet()
		{
			// Arrange
			var largePosts = new List<Post>();
			var largeUsers = new List<User>();

			for (int i = 1; i <= 100; i++)
			{
				largePosts.Add(new Post { Id = i, UserId = i });
				largeUsers.Add(new User { Id = i, Username = $"User{i}" });
			}

			SetupMockResponse("posts", largePosts);
			SetupMockResponse("users", largeUsers);

			// Act
			var result = await _apiService.GetPostsAsync();

			// Assert
			Assert.That(result, Has.Count.EqualTo(100));
			Assert.That(result[99].User.Username, Is.EqualTo("User100"));
		}

		private void SetupMockResponse<T>(string urlPart, T response)
		{
			_handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(r =>
						r.RequestUri.ToString().Contains(urlPart)),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(
						System.Text.Json.JsonSerializer.Serialize(response))
				});
		}
	}
}

