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
				throw new ArgumentException("Начальный баланс не может быть отрицательным", nameof(initialBalance));
				
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
			if (amount < 0)
				throw new ArgumentException("Сумма перевода не может быть отрицательной", nameof(amount));
				
			if (fromAccountId == toAccountId)
				throw new InvalidOperationException("Нельзя перевести деньги на тот же счет");
				
			if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
				throw new KeyNotFoundException($"Счет с ID {fromAccountId} не найден");
				
			if (!_accounts.TryGetValue(toAccountId, out var toAccount))
				throw new KeyNotFoundException($"Счет с ID {toAccountId} не найден");
				
			await fromAccount.WithdrawAsync(amount);
			await toAccount.DepositAsync(amount);
		}

		public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
		{
			if (!_accounts.TryGetValue(accountId, out var account))
				throw new KeyNotFoundException($"Счет с ID {accountId} не найден");
				
			return account.GetBalance();
		}
	}
}