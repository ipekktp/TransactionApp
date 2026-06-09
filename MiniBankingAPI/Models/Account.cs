namespace MiniBanking.API.Models
{
    public class Account
    {
        public int AccountID { get; set; } // PK [cite: 103]
        public int UserID { get; set; } // FK [cite: 103]
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public User User { get; set; }

        public string IBAN { get; set; } = string.Empty;
    }
}