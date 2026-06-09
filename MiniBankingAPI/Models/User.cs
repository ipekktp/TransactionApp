namespace MiniBanking.API.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<Account> Accounts { get; set; }
            = new List<Account>();
    }
}