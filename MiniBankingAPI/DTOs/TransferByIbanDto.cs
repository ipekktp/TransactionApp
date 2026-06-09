namespace MiniBanking.API.DTOs
{
    public class TransferByIbanDto
    {
        public int FromAccountId { get; set; }
        public string ToIban { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}