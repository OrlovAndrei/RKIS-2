using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;

namespace TodoList.UI
{
	public class TodoItemWithEditing
	{
		public TodoItem Todo { get; set; }
		public bool IsEditing { get; set; }
	}

	public partial class MainWindow : Window
	{
		private readonly TodoService _todoService = new();
		private ObservableCollection<TodoItemWithEditing> _todos = new();

		public MainWindow()
		{
			InitializeComponent();
			TodosItemsControl.ItemsSource = _todos;
			LoadTodos();
		}

		private async void LoadTodos()
		{
			var todos = await _todoService.GetAllTodos();
			var todoItemWithEditing = todos.Select(t =>
				new TodoItemWithEditing() { Todo = t, IsEditing = false });
			_todos = new ObservableCollection<TodoItemWithEditing>(todoItemWithEditing);
			TodosItemsControl.ItemsSource = _todos;
		}

		private async void AddTodo_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(NewTodoTextBox.Text)) return;

			var newItem = new TodoItem { Text = NewTodoTextBox.Text };
			await _todoService.AddTodo(newItem);
			_todos.Add(new TodoItemWithEditing() { Todo = newItem});
			NewTodoTextBox.Text = "";
		}

		private async void DeleteTodo_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn && btn.DataContext is TodoItemWithEditing item)
			{
				await _todoService.DeleteTodo(item.Todo.Id);
				_todos.Remove(item);
			}
		}

		private async void EditSave_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn && btn.DataContext is TodoItemWithEditing item)
			{
				if (item.IsEditing)
					await _todoService.UpdateTodo(item.Todo);

				item.IsEditing = !item.IsEditing;
				RefreshItem(item);
			}
		}

		private async void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox checkBox && checkBox.DataContext is TodoItemWithEditing item)
			{
				item.Todo.IsCompleted = checkBox.IsChecked ?? false;
				item.Todo.EndTime = item.Todo.IsCompleted ? DateTime.Now : null;
				await _todoService.UpdateTodo(item.Todo);
				RefreshItem(item);
			}
		}

		private void RefreshItem(TodoItemWithEditing item)
		{
			// Принудительное обновление элемента
			var index = _todos.IndexOf(item);
			_todos[index] = item;
		}
	}
}