namespace Bank
{
        public class BankAccount
        {
                public Guid Id { get; } = Guid.NewGuid();
                private decimal balance;

                public decimal GetBalance()
                {
                        return balance;
                }

                public async Task DepositAsync(decimal amount)
                {
                        await Task.Delay(100);
                        balance += amount;
                }

                public async Task WithdrawAsync(decimal amount)
                {
                        await Task.Delay(100);
                        balance -= amount;
                }
        }
}
