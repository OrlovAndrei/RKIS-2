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
            {
                throw new ArgumentException("Initial balance cannot be negative", nameof(initialBalance));
            }

            var account = new BankAccount();
            await account.DepositAsync(initialBalance); // Начальный баланс
            _accounts.TryAdd(account.Id, account);
            return account.Id;
        }

        public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Transaction amount cannot be negative", nameof(amount));
            }

            if (!_accounts.ContainsKey(fromAccountId) || !_accounts.ContainsKey(toAccountId))
            {
                throw new KeyNotFoundException("One or both accounts do not exist.");
            }

            var fromAccount = _accounts[fromAccountId];
            var toAccount = _accounts[toAccountId];

            await fromAccount.WithdrawAsync(amount);
            await toAccount.DepositAsync(amount);
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.ContainsKey(accountId))
            {
                throw new KeyNotFoundException("Account does not exist.");
            }

            return await Task.FromResult(_accounts[accountId].GetBalance());
        }
    }
} 