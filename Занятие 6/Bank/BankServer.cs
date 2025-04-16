using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank
{
	public class BankServer
	{
		private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new ConcurrentDictionary<Guid, BankAccount>();

        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative");

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
                throw new ArgumentException("Amount must be positive");

            if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
                throw new System.Collections.Generic.KeyNotFoundException("Source account not found");

            if (!_accounts.TryGetValue(toAccountId, out var toAccount))
                throw new System.Collections.Generic.KeyNotFoundException("Destination account not found");

            // Упорядочиваем блокировки по Id для предотвращения deadlock
            var lock1 = fromAccountId.CompareTo(toAccountId) < 0 ? fromAccount : toAccount;
            var lock2 = fromAccountId.CompareTo(toAccountId) < 0 ? toAccount : fromAccount;

            lock (lock1._balanceLock)
            {
                lock (lock2._balanceLock)
                {
                    if (fromAccount.GetBalance() < amount)
                        throw new InvalidOperationException("Insufficient funds");

                    fromAccount.Withdraw(amount);
                    toAccount.Deposit(amount);
                }
            }
        }

        public Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
                throw new System.Collections.Generic.KeyNotFoundException("Account not found");

            return Task.FromResult(account.GetBalance());
        }
    }

    public class BankAccount
    {
        private decimal _balance;
        internal readonly object _balanceLock = new object();

        public Guid Id { get; } = Guid.NewGuid();

        public decimal GetBalance()
        {
            lock (_balanceLock)
            {
                return _balance;
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");

            lock (_balanceLock)
            {
                _balance += amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            lock (_balanceLock)
            {
                if (_balance < amount)
                    throw new InvalidOperationException("Insufficient funds");
                
                _balance -= amount;
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            await Task.Delay(10);
            Deposit(amount);
        }

        public async Task WithdrawAsync(decimal amount)
        {
            await Task.Delay(10);
            Withdraw(amount);
        }
    }
}

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
