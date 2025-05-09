using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();
        private decimal _balance;
        
        public decimal GetBalance() => _balance;

        public async Task DepositAsync(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            
            await Task.Delay(100);
            _balance += amount;
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            
            await Task.Delay(100);
            
            if (_balance < amount)
                throw new InvalidOperationException("Insufficient funds");
            
            _balance -= amount;
        }
    }
}