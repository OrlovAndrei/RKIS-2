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
                throw new ArgumentException("Amount must be positive", nameof(amount));

            if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
                throw new KeyNotFoundException("Source account not found");

            if (!_accounts.TryGetValue(toAccountId, out var toAccount))
                throw new KeyNotFoundException("Destination account not found");

            if (fromAccountId == toAccountId)
                throw new InvalidOperationException("Cannot transfer to the same account");

            // Упорядочиваем блокировки по ID счетов для предотвращения deadlock
            var firstLockId = fromAccountId.CompareTo(toAccountId) < 0 ? fromAccountId : toAccountId;
            var secondLockId = fromAccountId.CompareTo(toAccountId) < 0 ? toAccountId : fromAccountId;

            var firstAccount = _accounts[firstLockId];
            var secondAccount = _accounts[secondLockId];

            lock (firstAccount)
            {
                lock (secondAccount)
                {
                    // Синхронные вызовы внутри lock
                    fromAccount.WithdrawAsync(amount).GetAwaiter().GetResult();
                    toAccount.DepositAsync(amount).GetAwaiter().GetResult();
                }
            }
		}

		public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
		{
			if (!_accounts.TryGetValue(accountId, out var account))
                throw new KeyNotFoundException("Account not found");

            return await Task.FromResult(account.GetBalance());
		}
	}
}