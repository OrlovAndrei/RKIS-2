using System.Collections.Concurrent;

namespace Bank
{
        public class BankServer
        {
            private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

            public async Task<Guid> CreateAccount(decimal initialBalance = 0)
            {
                if (initialBalance < 0)
                {
                    throw new ArgumentException("Начальный баланс не может быть отрицательным.");
                }

                var account = new BankAccount();
                await account.DepositAsync(initialBalance);
                _accounts.TryAdd(account.Id, account);
                return account.Id;
            }

            public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
            {
                if (amount <= 0)
                {
                    throw new ArgumentException("Сумма перевода должна быть положительной.");
                }

                if (fromAccountId == toAccountId)
                {
                    return;
                }

                if (!_accounts.TryGetValue(fromAccountId, out _))
                {
                    throw new KeyNotFoundException($"Аккаунт с ID {fromAccountId} не найден.");
                }

                if (!_accounts.TryGetValue(toAccountId, out _))
                {
                    throw new KeyNotFoundException($"Аккаунт с ID {toAccountId} не найден.");
                }

                var firstLockId = fromAccountId.CompareTo(toAccountId) < 0 ? fromAccountId : toAccountId;
                var secondLockId = fromAccountId.CompareTo(toAccountId) < 0 ? toAccountId : fromAccountId;

                var firstAccount = _accounts[firstLockId];
                var secondAccount = _accounts[secondLockId];

                try
                {
                    await firstAccount.WithdrawAsync(amount);
                    await secondAccount.DepositAsync(amount);
                }

                catch (Exception ex)
                {
                    await Console.Error.WriteLineAsync($"Ошибка транзакции: {ex.Message}");
                    throw;
                }
            }

            public Task<decimal> GetAccountBalanceAsync(Guid accountId)
            {
                if (_accounts.TryGetValue(accountId, out var account))
                {
                    return Task.FromResult(account.GetBalance());
                }

                throw new KeyNotFoundException($"Аккаунт с ID {accountId} не найден.");
            }
        }
}
