using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative");

            var account = new BankAccount();
            if (initialBalance > 0) 
                await account.DepositAsync(initialBalance);
                
            _accounts[account.Id] = account;
            return account.Id;
        }

        public async Task PerformTransactionAsync(Guid fromId, Guid toId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            if (!_accounts.TryGetValue(fromId, out var from) || !_accounts.TryGetValue(toId, out var to))
                throw new KeyNotFoundException("Account not found");

            if (from.GetBalance() < amount)
                throw new InvalidOperationException("Insufficient funds");

            await Task.WhenAll(
                from.WithdrawAsync(amount),
                to.DepositAsync(amount)
            );
        }

        public Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
                throw new KeyNotFoundException("Account not found");

            return Task.FromResult(account.GetBalance());
        }
    }
}