using System;
using System.Threading.Tasks;

namespace Bank
{
	public class BankAccount
	{
		public Guid Id { get; } = Guid.NewGuid();
		private decimal balance;
		private readonly object balanceLock = new object(); // Блокировка для синхронизации

		public decimal GetBalance()
		{
			lock (balanceLock) // Безопасное чтение баланса
            {
                return balance;
            }
		}

		public async Task DepositAsync(decimal amount)
		{
			if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            
            await Task.Delay(100); // Имитация долгой операции
            
            lock (balanceLock) // Безопасное изменение баланса
            {
                balance += amount;
            }
		}

		public async Task WithdrawAsync(decimal amount)
		{
			if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            
            await Task.Delay(100); // Имитация долгой операции
            
            lock (balanceLock) // Безопасное изменение баланса
            {
                if (amount > balance)
                    throw new InvalidOperationException("Insufficient funds");
                
                balance -= amount;
            }
		}
	}
}