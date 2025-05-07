using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            if (amount < 0)
                throw new ArgumentException("Transaction amount cannot be negative");

            if (fromId == toId)
                throw new InvalidOperationException("Cannot transfer to the same account");

            if (!_accounts.TryGetValue(fromId, out var fromAccount) ||
                !_accounts.TryGetValue(toId, out var toAccount))
                throw new KeyNotFoundException("One or both accounts not found");

            var ordered = new[] { fromAccount, toAccount };
            Array.Sort(ordered, (a, b) => a.Id.CompareTo(b.Id));

            lock (ordered[0])
            {
                lock (ordered[1])
                {
                    if (fromAccount.GetBalance() < amount)
                        throw new InvalidOperationException("Insufficient funds");
                }
            }
            var withdrawTask = fromAccount.WithdrawAsync(amount);
            var depositTask = toAccount.DepositAsync(amount);
            await Task.WhenAll(withdrawTask, depositTask);
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
                throw new KeyNotFoundException("Account not found");

            await Task.Yield();
            return account.GetBalance();
        }
    }
}