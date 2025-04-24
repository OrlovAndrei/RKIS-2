using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid AccountId { get; } = Guid.NewGuid();
        private decimal _currentBalance;
        private readonly object _balanceMutex = new object();

        public decimal GetCurrentBalance()
        {
            return _currentBalance;
        }

        public async Task DepositAsync(decimal depositAmount)
        {
            if (depositAmount <= 0)
            {
                throw new ArgumentException("Сумма пополнения должна быть положительной", nameof(depositAmount));
            }

            await Task.Delay(100); // Имитация долгой операции

            lock (_balanceMutex)
            {
                _currentBalance += depositAmount;
            }
        }

        public async Task WithdrawAsync(decimal withdrawalAmount)
        {
            if (withdrawalAmount <= 0)
            {
                throw new ArgumentException("Сумма снятия должна быть положительной", nameof(withdrawalAmount));
            }

            await Task.Delay(100); // Имитация долгой операции

            lock (_balanceMutex)
            {
                if (_currentBalance < withdrawalAmount)
                {
                    throw new InvalidOperationException("Недостаточно средств на счете");
                }
                _currentBalance -= withdrawalAmount;
            }
        }
    }
}