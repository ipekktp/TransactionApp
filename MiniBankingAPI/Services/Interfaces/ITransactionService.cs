using MiniBanking.API.DTOs;

namespace MiniBanking.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<object> GetAccountTransactions(int accountId, int userId);
        Task<object> TransferByIban(TransferByIbanDto transferDto, int userId);
    }
}