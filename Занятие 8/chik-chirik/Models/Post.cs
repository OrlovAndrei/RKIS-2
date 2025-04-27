namespace chik_chirik.Models
{
	public class Post
	{
	public int Id { get; set; }          // ID поста
        public int UserId { get; set; }      // ID автора
        public string Title { get; set; }    // Заголовок поста
        public string Body { get; set; }     // Текст поста
        public User User { get; set; }       // Ссылка на автора (будет заполняться отдельно)
    }
}
