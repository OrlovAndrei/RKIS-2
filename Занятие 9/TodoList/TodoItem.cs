using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList
{
    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; } = "";

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}

