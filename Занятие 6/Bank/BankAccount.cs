using System;
using System.Threading.Tasks;
namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();
        private decimal _balance;
        private readonly object _balanceLock = new object();
        public decimal GetBalance()
        {
            return _balance;
        }
        public async Task DepositAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма пополнения должна быть положительной", nameof(amount));
            }
            await Task.Delay(100); 
            lock (_balanceLock)
            {
                _balance += amount;
            }
        }
        public async Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма снятия должна быть положительной", nameof(amount));
            }
            await Task.Delay(100); 
            lock (_balanceLock)
            {
                if (_balance < amount)
                {
                    throw new InvalidOperationException("Недостаточно средств на счете");
                }
                _balance -= amount;
            }
        }
    }
}
