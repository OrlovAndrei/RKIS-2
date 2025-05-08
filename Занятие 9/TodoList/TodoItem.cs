using System.ComponentModel.DataAnnotations;

namespace TodoList
{
	public class TodoItem
	{
		public Guid Id { get; } = Guid.NewGuid();
		public string Text { get; set; } = "Default Todo";
		public DateTime StartTime { get; } = DateTime.Now;
		public DateTime? EndTime { get; set;} 
		public bool IsCompleted { get; set; }
	}
}
