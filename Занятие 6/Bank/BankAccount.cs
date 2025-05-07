using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        private decimal _balance;
        private readonly object _lock = new object();

        public Guid Id { get; } = Guid.NewGuid();

        public decimal GetBalance()
        {
            // Возвращаем текущий баланс
            lock (_lock)
            {
                return _balance;
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            await Task.Delay(100); // Имитация долгих вычислений

            lock (_lock)
            {
                _balance += amount;
            }
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            await Task.Delay(100); // Имитация долгих вычислений

            lock (_lock)
            {
                if (_balance < amount)
                {
                    throw new InvalidOperationException("Insufficient funds");
                }

                _balance -= amount;
            }
        }
    }
}