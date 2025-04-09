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
			throw new NotImplementedException();
		}

		public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
		{
			throw new NotImplementedException();
		}

		public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
		{
			throw new NotImplementedException();
		}
	}
}