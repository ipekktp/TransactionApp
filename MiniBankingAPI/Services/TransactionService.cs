using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Data;
using MiniBanking.API.DTOs;
using MiniBanking.API.Models;
using MiniBanking.API.Services.Interfaces;

namespace MiniBanking.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> TransferByIban(TransferByIbanDto transferDto, int userId)
        {
            if (transferDto.Amount <= 0)
                throw new Exception("Transfer tutarı 0'dan büyük olmalıdır.");

            if (string.IsNullOrWhiteSpace(transferDto.ToIban))
                throw new Exception("Alıcı IBAN boş olamaz.");

            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountID == transferDto.FromAccountId);

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.IBAN == transferDto.ToIban);

            if (fromAccount == null || toAccount == null)
                throw new Exception("Gönderen hesap veya alıcı IBAN bulunamadı.");

            if (fromAccount.UserID != userId)
                throw new UnauthorizedAccessException("Bu hesaptan para gönderme yetkiniz yok.");

            if (fromAccount.AccountID == toAccount.AccountID)
                throw new Exception("Kendi hesabınıza transfer yapamazsınız.");

            if (fromAccount.Balance < transferDto.Amount)
                throw new Exception("Yetersiz bakiye.");

            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                fromAccount.Balance -= transferDto.Amount;
                toAccount.Balance += transferDto.Amount;

                var transaction = new Transaction
                {
                    FromAccountID = fromAccount.AccountID,
                    ToAccountID = toAccount.AccountID,
                    Amount = transferDto.Amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Transfer"
                };

                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return new
                {
                    message = "IBAN ile transfer başarılı.",
                    fromAccountBalance = fromAccount.Balance
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<object> GetAccountTransactions(int accountId, int userId)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountID == accountId);

            if (account == null)
                throw new Exception("Hesap bulunamadı.");

            if (account.UserID != userId)
                throw new UnauthorizedAccessException("Bu hesabın işlem geçmişini görme yetkiniz yok.");

            var transactions = await _context.Transactions
                .Where(t => t.FromAccountID == accountId || t.ToAccountID == accountId)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new
                {
                    t.TransactionID,
                    t.FromAccountID,
                    t.ToAccountID,
                    t.Amount,
                    t.TransactionDate,
                    t.TransactionType,
                    Direction = t.FromAccountID == accountId ? "Outgoing" : "Incoming"
                })
                .ToListAsync();

            return transactions;
        }
    }
}