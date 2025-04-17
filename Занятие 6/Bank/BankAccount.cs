using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        private decimal balance;

        public Guid Id { get; } = Guid.NewGuid();

        // Метод для получения текущего баланса
        public decimal GetBalance()
        {
            return balance;
        }

        // Метод для пополнения счета
        public async Task DepositAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма должна быть положительной", nameof(amount));
            }

            // Имитация долгих вычислений
            await Task.Delay(100);

            balance += amount;
        }

        // Метод для снятия средств
        public async Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма должна быть положительной", nameof(amount));
            }

            if (balance < amount)
            {
                throw new InvalidOperationException("Недостаточно средств");
            }

            // Имитация долгих вычислений
            await Task.Delay(100);

            balance -= amount;
        }
    }
}
