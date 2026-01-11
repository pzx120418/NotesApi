namespace NotesApi.Models
{
    public class Note
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
