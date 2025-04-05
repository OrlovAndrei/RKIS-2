using System;
using System.Threading.Tasks;

namespace Bank
{
	public class BankAccount
	{
		public Guid Id { get; } = Guid.NewGuid();

		public decimal GetBalance()
		{
			throw new NotImplementedException();
		}

		public async Task DepositAsync(decimal amount)
		{
			throw new NotImplementedException();
		}

		public async Task WithdrawAsync(decimal amount)
		{
			throw new NotImplementedException();
		}
	}
}