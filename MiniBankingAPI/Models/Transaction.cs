namespace MiniBanking.API.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; } // PK [cite: 97]
        public int FromAccountID { get; set; } // FK [cite: 97]
        public int ToAccountID { get; set; } // FK [cite: 98]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }    }
}