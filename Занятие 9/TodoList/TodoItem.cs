namespace TodoList
{
	public class TodoItem
	{
		public Guid Id { get; } = Guid.NewGuid();
		public string Text { get; set; } = "";
		public DateTime StartTime { get; } = DateTime.Now;
		public DateTime? EndTime { get; set; 
		public bool IsCompleted { get; set; }
	}
}
