using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();
        private decimal _balance;
        private readonly object _balanceLock = new object(); // Объект для блокировки

        public decimal GetBalance()
        {
            lock (_balanceLock) // Блокируем перед чтением баланса
            {
                return _balance;
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Сумма пополнения не может быть отрицательной.");
            }
            await Task.Delay(100);
            lock (_balanceLock) // Блокируем перед изменением баланса
            {
                _balance += amount;
            }
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Сумма снятия не может быть отрицательной.");
            }
            await Task.Delay(100);
            lock (_balanceLock) // Блокируем перед изменением баланса
            {
                if (_balance < amount)
                {
                    throw new InvalidOperationException("Недостаточно средств на счете.");
                }
                _balance -= amount;
            }
        }
    }
}
