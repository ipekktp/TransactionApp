using MiniBanking.API.DTOs;

namespace MiniBanking.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<object> GetMyAccounts(int userId);
        Task<object> CreateMyAccount(int userId);
        Task<object> Deposit(int userId, AccountOperationDto dto);
        Task<object> Withdraw(int userId, AccountOperationDto dto);
    }
}