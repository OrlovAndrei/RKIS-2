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
                throw new ArgumentException("Initial balance cannot be negative", nameof(initialBalance));

            var account = new BankAccount();
            if (initialBalance > 0)
            {
                await account.DepositAsync(initialBalance);
            }
            
            _accounts.TryAdd(account.Id, account);
            return account.Id;
        }

        public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Transaction amount must be positive", nameof(amount));

            if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
                throw new KeyNotFoundException($"Source account {fromAccountId} not found");

            if (!_accounts.TryGetValue(toAccountId, out var toAccount))
                throw new KeyNotFoundException($"Destination account {toAccountId} not found");

            // Выносим операцию в фоновый поток с Task.Run
            await Task.Run(async () =>
            {
                // Упорядочиваем блокировку по Id счетов
                var firstAccountId = fromAccountId.CompareTo(toAccountId) < 0 ? fromAccountId : toAccountId;
                var secondAccountId = firstAccountId == fromAccountId ? toAccountId : fromAccountId;

                var firstAccount = firstAccountId == fromAccountId ? fromAccount : toAccount;
                var secondAccount = firstAccountId == fromAccountId ? toAccount : fromAccount;

                lock (firstAccount)
                {
                    lock (secondAccount)
                    {
                        if (fromAccount.GetBalance() < amount)
                            throw new InvalidOperationException("Insufficient funds for transaction");
                    }
                }

                // Выполняем асинхронные операции с await
                await fromAccount.WithdrawAsync(amount);
                await toAccount.DepositAsync(amount);
            });
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
                throw new KeyNotFoundException($"Account {accountId} not found");

            // Используем Task.Run для асинхронного выполнения синхронной операции
            return await Task.Run(() => account.GetBalance());
        }
    }
}