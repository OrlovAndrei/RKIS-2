namespace chik_chirik.Models
{
	public class Comment
	{
		public int Id { get; set; }          // ID комментария
        public int PostId { get; set; }      // ID поста, к которому относится
        public string Name { get; set; }     // Имя комментатора (в API нет отдельного UserId)
        public string Email { get; set; }    // Email комментатора
        public string Body { get; set; }     // Текст комментария
	}
}
