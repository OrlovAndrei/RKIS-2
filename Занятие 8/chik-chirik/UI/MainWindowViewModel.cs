using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using chik_chirik.Models;

namespace chik_chirik.UI
{
	public class MainWindowViewModel : ReactiveObject
	{
		private string _searchTerm;
		private ObservableCollection<PostViewModel> _posts;
		private string _statusMessage;

		public string StatusMessage
		{
			get => _statusMessage;
			set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
		}

		public ReactiveCommand<Unit, Unit> SearchCommand { get; }
		public Interaction<Exception, Unit> ShowError { get; } = new();

		public string SearchTerm
		{
			get => _searchTerm;
			set => this.RaiseAndSetIfChanged(ref _searchTerm, value);
		}

		public ObservableCollection<PostViewModel> Posts
		{
			get => _posts;
			set => this.RaiseAndSetIfChanged(ref _posts, value);
		}

		public MainWindowViewModel(ApiService apiService)
		{
			SearchCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				try
				{
					var posts = await apiService.GetPostsAsync(SearchTerm);
					Posts = new ObservableCollection<PostViewModel>(
						posts.Select(p => new PostViewModel(p, apiService)));
					StatusMessage = null;
				}
				catch
				{
					StatusMessage = "Ошибка";
				}
			});
		}
	}

	public class PostViewModel : ReactiveObject
	{
		private readonly ApiService _apiService;
		private bool _areCommentsVisible;
		private string _statusMessage;

		public Post Post { get; }
		public ReactiveCommand<Unit, Unit> LoadCommentsCommand { get; }
		public ObservableCollection<Comment> Comments { get; } = new();

		public bool AreCommentsVisible
		{
			get => _areCommentsVisible;
			set => this.RaiseAndSetIfChanged(ref _areCommentsVisible, value);
		}

		public string StatusMessage
		{
			get => _statusMessage;
			set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
		}

		public PostViewModel(Post post, ApiService apiService)
		{
			Post = post;
			_apiService = apiService;

			LoadCommentsCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				AreCommentsVisible = true;
				StatusMessage = "Комментарии загружаются...";
				try
				{
					var comments = await _apiService.GetCommentsAsync(Post.Id);
					Comments.Clear();
					foreach (var comment in comments)
						Comments.Add(comment);
					StatusMessage = null;
				}
				catch
				{
					StatusMessage = "Ошибка";
				}
			});
		}
	}
}