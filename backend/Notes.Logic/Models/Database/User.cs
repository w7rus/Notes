namespace Notes.Logic.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSystem { get; set; }
    }
}