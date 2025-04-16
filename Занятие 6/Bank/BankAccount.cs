using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();
        private decimal _balance;
        private readonly object _lock = new();

        public decimal GetBalance()
        {
            lock (_lock)
            {
                return _balance;
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Cannot deposit a negative amount.");

            await Task.Delay(100); // имитация долгой операции

            lock (_lock)
            {
                _balance += amount;
            }
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Cannot withdraw a negative amount.");

            await Task.Delay(100); // имитация долгой операции

            lock (_lock)
            {
                if (_balance < amount)
                    throw new InvalidOperationException("Insufficient funds.");

                _balance -= amount;
            }
        }
    }
}
