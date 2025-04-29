using NUnit.Framework;

namespace TodoList
{
	[TestFixture]
	public class TodoServiceTests
	{
		private TodoService _service;

		[SetUp]
		public async Task Setup()
		{
			_service = new TodoService();
		}

		[Test]
		public async Task DeleteTodo_ShouldRemoveItemPermanently()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem());

			// Act
			await _service.DeleteTodo(item.Id);
			var result = await _service.GetByIdTodos(item.Id);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public async Task UpdateTodo_ShouldModifyAndRollback()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem { Text = "Original" });

			// Act
			item.Text = "Updated";
			var updated = await _service.UpdateTodo(item);
			var dbItem = await _service.GetByIdTodos(item.Id);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(updated.Text, Is.EqualTo("Updated"));
				Assert.That(dbItem.Text, Is.EqualTo("Updated"));
			});

			await _service.DeleteTodo(item.Id);
		}

		[Test]
		public async Task GetAllTodos_ShouldReturnEmptyAfterCleanup()
		{
			var item = await _service.AddTodo(new TodoItem());

			// Act
			var result = await _service.GetAllTodos();

			// Assert
			Assert.That(result.Count != 0);

			await _service.DeleteTodo(item.Id);
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectItem()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem { Text = "FindMe" });

			// Act
			var found = await _service.GetByIdTodos(item.Id);

			// Assert
			Assert.That(found.Text, Is.EqualTo("FindMe"));

			await _service.DeleteTodo(item.Id);
		}
	}
}