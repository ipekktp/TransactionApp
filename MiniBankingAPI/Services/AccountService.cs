using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Data;
using MiniBanking.API.DTOs;
using MiniBanking.API.Models;
using MiniBanking.API.Services.Interfaces;
using MiniBanking.API.Helpers;

namespace MiniBanking.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetMyAccounts(int userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserID == userId)
                .Select(a => new
                {
                    a.AccountID,
                    a.Balance,
                    a.AccountType,
                    a.IBAN
                })
                .ToListAsync();

            return accounts;
        }

        public async Task<object> CreateMyAccount(int userId)
        {
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserID == userId);

            if (existingAccount != null)
                throw new Exception("Bu kullanıcının zaten hesabı var.");

            var account = new Account
            {
                UserID = userId,
                Balance = 1000,
                AccountType = "Checking",
                IBAN = IbanGenerator.Generate()
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return new
            {
                account.AccountID,
                account.Balance,
                account.AccountType
            };
        }

        public async Task<object> Deposit(int userId, AccountOperationDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Tutar 0'dan büyük olmalıdır.");

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a =>
                    a.AccountID == dto.AccountId &&
                    a.UserID == userId);

            if (account == null)
                throw new Exception("Hesap bulunamadı.");

            account.Balance += dto.Amount;

            var transaction = new Transaction
            {
                FromAccountID = account.AccountID,
                ToAccountID = account.AccountID,
                Amount = dto.Amount,
                TransactionDate = DateTime.Now,
                TransactionType = "Deposit"
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return new
            {
                message = "Para yatırma işlemi başarılı.",
                balance = account.Balance
            };
        }

        public async Task<object> Withdraw(int userId, AccountOperationDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Tutar 0'dan büyük olmalıdır.");

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a =>
                    a.AccountID == dto.AccountId &&
                    a.UserID == userId);

            if (account == null)
                throw new Exception("Hesap bulunamadı.");

            if (account.Balance < dto.Amount)
                throw new Exception("Yetersiz bakiye.");

            account.Balance -= dto.Amount;

            var transaction = new Transaction
            {
                FromAccountID = account.AccountID,
                ToAccountID = account.AccountID,
                Amount = dto.Amount,
                TransactionDate = DateTime.Now,
                TransactionType = "Withdraw"
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return new
            {
                message = "Para çekme işlemi başarılı.",
                balance = account.Balance
            };
        }
    }
}