using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Bank
{
	[TestFixture]
	public class BankAccountTests
	{
		[Test]
		public async Task Deposit_WithPositiveAmount_UpdatesBalance()
		{
			var account = new BankAccount();
			await account.DepositAsync(500);
			Assert.That(500 == account.GetBalance());
		}

		[Test]
		public void Deposit_WithNegativeAmount_ThrowsException()
		{
			var account = new BankAccount();
			Assert.ThrowsAsync<ArgumentException>(async () => await account.DepositAsync(-100));
		}

		[Test]
		public async Task Withdraw_WithSufficientFunds_UpdatesBalance()
		{
			var account = new BankAccount();
			await account.DepositAsync(1000);
			await account.WithdrawAsync(300);
			Assert.That(700 == account.GetBalance());
		}

		[Test]
		public void Withdraw_WithInsufficientFunds_ThrowsException()
		{
			var account = new BankAccount();
			Assert.ThrowsAsync<InvalidOperationException>(async () => await account.WithdrawAsync(100));
		}

		[Test]
		public async Task ConcurrentDeposits_ShouldUpdateBalanceCorrectly()
		{
			var account = new BankAccount();
			var tasks = new Task[100];
			for (int i = 0; i < 100; i++)
			{
				tasks[i] = account.DepositAsync(1);
			}
			await Task.WhenAll(tasks);
			Assert.That(100 == account.GetBalance());
		}
	}
}