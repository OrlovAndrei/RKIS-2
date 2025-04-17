using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();

        private decimal _balance;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public SemaphoreSlim Lock => _semaphore;

        public decimal GetBalance()
        {
            return _balance;
        }

        public async Task<decimal> GetBalanceAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return _balance;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            await Task.Delay(100);

            await _semaphore.WaitAsync();
            try
            {
                _balance += amount;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            await Task.Delay(100);

            await _semaphore.WaitAsync();
            try
            {
                if (amount > _balance)
                    throw new InvalidOperationException("Insufficient funds");

                _balance -= amount;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void DepositInternal(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            _balance += amount;
        }

        public void WithdrawInternal(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            if (amount > _balance)
                throw new InvalidOperationException("Insufficient funds");

            _balance -= amount;
        }
    }

    public static class BankOperations
    {
        public static async Task PerformTransactionAsync(BankAccount fromAccount, BankAccount toAccount, decimal amount)
        {
            if (fromAccount == null) throw new ArgumentNullException(nameof(fromAccount));
            if (toAccount == null) throw new ArgumentNullException(nameof(toAccount));
            if (amount < 0) throw new ArgumentException("Amount must be positive", nameof(amount));

            var firstLock = fromAccount.Id.CompareTo(toAccount.Id) < 0 ? fromAccount.Lock : toAccount.Lock;
            var secondLock = fromAccount.Id.CompareTo(toAccount.Id) < 0 ? toAccount.Lock : fromAccount.Lock;

            await firstLock.WaitAsync();
            try
            {
                await secondLock.WaitAsync();
                try
                {
                    await Task.Delay(100);

                    var balance = fromAccount.GetBalance();

                    if (balance < amount)
                        throw new InvalidOperationException("Insufficient funds");

                    fromAccount.WithdrawInternal(amount);
                    toAccount.DepositInternal(amount);
                }
                finally
                {
                    secondLock.Release();
                }
            }
            finally
            {
                firstLock.Release();
            }
        }
    }
}