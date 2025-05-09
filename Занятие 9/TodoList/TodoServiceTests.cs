using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoList
{
	[TestFixture]
	public class TodoServiceTests
	{
		private TodoService _service;
		private AppDbContext _context;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
				.Options;

			_context = new AppDbContext(options);
			_service = new TodoService(_context);
		}
		
		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public async Task DeleteTodo_ShouldRemoveItemPermanently()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem { Text = "Test Delete" });

			// Act
			await _service.DeleteTodo(item.Id);
			var result = await _service.GetByIdTodos(item.Id);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public async Task UpdateTodo_ShouldModifyText()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem { Text = "Original" });

			// Act
			item.Text = "Updated";
			var updatedItem = await _service.UpdateTodo(item);
			
			// Assert
			Assert.That(updatedItem, Is.Not.Null);
			Assert.That(updatedItem.Text, Is.EqualTo("Updated"));
			
			var dbItem = await _service.GetByIdTodos(item.Id);
			Assert.That(dbItem, Is.Not.Null);
			Assert.That(dbItem.Text, Is.EqualTo("Updated"));
		}
		
		[Test]
		public async Task GetAllTodos_ShouldReturnAddedItems()
		{
			// Arrange
			var item1 = await _service.AddTodo(new TodoItem { Text = "Item 1" });
			var item2 = await _service.AddTodo(new TodoItem { Text = "Item 2" });

			// Act
			var result = await _service.GetAllTodos();

			// Assert
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result.Any(i => i.Id == item1.Id));
			Assert.That(result.Any(i => i.Id == item2.Id));
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectItem()
		{
			// Arrange
			var item = await _service.AddTodo(new TodoItem { Text = "FindMe" });

			// Act
			var found = await _service.GetByIdTodos(item.Id);

			// Assert
			Assert.That(found, Is.Not.Null);
			Assert.That(found.Text, Is.EqualTo("FindMe"));
		}
	}
}