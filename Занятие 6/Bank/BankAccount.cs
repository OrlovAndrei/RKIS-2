namespace Bank
{
        public class BankAccount
        {
                public Guid Id { get; } = Guid.NewGuid();
                private decimal balance;
                private readonly SemaphoreSlim _balanceLock = new SemaphoreSlim(1, 1);

                public decimal GetBalance()
                {
                        return balance;
                }

                public async Task DepositAsync(decimal amount)
                {
                        if (amount <= 0)
                        {
                                throw new ArgumentException("Сумма пополнения должна быть положительной.");
                        }

                        await _balanceLock.WaitAsync();

                        try
                        {
                                await Task.Delay(100);
                                balance += amount;
                        }

                        finally
                        {
                                _balanceLock.Release();
                        }
                }

                public async Task WithdrawAsync(decimal amount)
                {
                        if (amount <= 0)
                        {
                                throw new ArgumentException("Сумма снятия должна быть положительной.");
                        }

                        await _balanceLock.WaitAsync();

                        try
                        {
                                if (balance < amount)
                                {
                                        throw new InvalidOperationException("Недостаточно средств на счете.");
                                }

                                await Task.Delay(100);
                                balance -= amount;
                        }

                        finally
                        {
                                _balanceLock.Release();
                        }
                }
        }
}
