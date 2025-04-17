using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

        // Создание нового счета с начальным балансом
        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Начальный баланс не может быть отрицательным", nameof(initialBalance));
            }

            var account = new BankAccount();
            await account.DepositAsync(initialBalance);
            _accounts[account.Id] = account;
            return account.Id;
        }

        // Выполнение транзакции (перевод средств между счетами)
        public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма должна быть положительной", nameof(amount));
            }

            if (!_accounts.ContainsKey(fromAccountId) || !_accounts.ContainsKey(toAccountId))
            {
                throw new KeyNotFoundException("Один или оба идентификатора учетной записи недействительны");
            }

            var fromAccount = _accounts[fromAccountId];
            var toAccount = _accounts[toAccountId];

            // Имитация перевода средств с задержкой для асинхронности
            await Task.WhenAll(
                fromAccount.WithdrawAsync(amount),
                toAccount.DepositAsync(amount)
            );
        }

        // Получение баланса счета
        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.ContainsKey(accountId))
            {
                throw new KeyNotFoundException("Account not found");
            }

            var account = _accounts[accountId];
            return account.GetBalance();
        }
    }
}
