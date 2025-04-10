using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Bank
{
	[TestFixture]
	public class BankServerTests
	{
		private BankServer _server;

		[SetUp]
		public void Setup() => _server = new BankServer();

		[Test]
		public async Task MultipleParallelTransactions()
		{
			var account1 = await _server.CreateAccount(1000);
			var account2 = await _server.CreateAccount(1000);
			var transactions = new Task[100];

			// 50 переводов в каждом направлении
			for (int i = 0; i < 50; i++)
			{
				transactions[i*2] = _server.PerformTransactionAsync(account1, account2, 10);
				transactions[i*2 + 1] = _server.PerformTransactionAsync(account2, account1, 5);
			}

			await Task.WhenAll(transactions);

			var balance1 = await _server.GetAccountBalanceAsync(account1);
			var balance2 = await _server.GetAccountBalanceAsync(account2);

			Assert.That(1000 - 50 * 10 + 50 * 5 == balance1); // 1000 - 500 + 250 = 750
			Assert.That(1000 + 50 * 10 - 50 * 5 == balance2); // 1000 + 500 - 250 = 1250
		}

		[Test]
		public async Task AsyncTransactionsPerformance()
		{
			var account1 = await _server.CreateAccount(1000);
			var account2 = await _server.CreateAccount(1000);
			var account3 = await _server.CreateAccount(1000);
			var account4 = await _server.CreateAccount(1000);
			var sw = new Stopwatch();
			sw.Start();
			await Task.WhenAll(
				_server.PerformTransactionAsync(account1, account2, 1000),
				_server.PerformTransactionAsync(account2, account3, 1000),
				_server.PerformTransactionAsync(account3, account4, 1000),
				_server.PerformTransactionAsync(account4, account1, 1000)
				);

			sw.Stop();
			Assert.That(sw.ElapsedMilliseconds <= 150, sw.ElapsedMilliseconds.ToString());
		}

		[Test]
		public async Task HighLoadTransactions()
		{
			var accounts = Enumerable.Range(0, 100)
				.Select(_ => _server.CreateAccount(1000))
				.ToArray();

			var transactions = new Task[10000];
			var random = new Random();

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					if (i == j) {
						transactions[(i)*100+j] = Task.Run(() => { });
						continue;
					} 

					var from = await accounts[i];
					var to = await accounts[j];
					transactions[(i)*100+j] = _server.PerformTransactionAsync(from, to, 10);
				}
			}

			Assert.DoesNotThrowAsync(async () =>
			{
				await Task.WhenAll(transactions);
			});
		}

		[Test]
		public async Task TransactionWithInvalidAccount()
		{
			var validAccount = await _server.CreateAccount();
			var invalidAccount = Guid.NewGuid();

			Assert.ThrowsAsync<KeyNotFoundException>(async () =>
				await _server.PerformTransactionAsync(validAccount, invalidAccount, 100));
		}

		[Test]
		public async Task ConcurrentBalanceChecks()
		{
			var account = await _server.CreateAccount(1000);
			var transactionTask = _server.PerformTransactionAsync(account, account, 0); // Недействительная транзакция

			var balanceTasks = Enumerable.Range(0, 100)
				.Select(_ => _server.GetAccountBalanceAsync(account))
				.ToArray();

			await Task.WhenAll(balanceTasks);
			Assert.ThrowsAsync<InvalidOperationException>(async () => await transactionTask);
		}

		[Test]
		public void CreateAccount_WithNegativeBalance()
		{
			Assert.ThrowsAsync<ArgumentException>(async () => await _server.CreateAccount(-100));
		}

		[Test]
		public void GetBalance_ForInvalidAccount()
		{
			var invalidAccountId = Guid.NewGuid();
			Assert.ThrowsAsync<KeyNotFoundException>(async () => await _server.GetAccountBalanceAsync(invalidAccountId));
		}

		[Test]
		public async Task Transfer_WithNegativeAmount()
		{
			var account1 = await _server.CreateAccount(500);
			var account2 = await _server.CreateAccount();
			Assert.ThrowsAsync<ArgumentException>(async () =>
				await _server.PerformTransactionAsync(account1, account2, -100));
		}

		[Test]
		public async Task Transfer_ExceedingBalance()
		{
			var account1 = await _server.CreateAccount(100);
			var account2 = await _server.CreateAccount();
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _server.PerformTransactionAsync(account1, account2, 200));
		}
	}
}