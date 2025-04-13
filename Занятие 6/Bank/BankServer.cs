using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Начальный баланс не может быть отрицательным", nameof(initialBalance));
            }

            var account = new BankAccount();
            if (initialBalance > 0)
            {
                await account.DepositAsync(initialBalance);
            }

            _accounts.TryAdd(account.Id, account);
            return account.Id;
        }

        public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма перевода должна быть положительной", nameof(amount));
            }

            if (fromAccountId == toAccountId)
            {
                throw new InvalidOperationException("Нельзя переводить средства на тот же самый счёт");
            }

            if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
            {
                throw new KeyNotFoundException($"Исходный счёт не найден: {fromAccountId}");
            }

            if (!_accounts.TryGetValue(toAccountId, out var toAccount))
            {
                throw new KeyNotFoundException($"Целевой счёт не найден: {toAccountId}");
            }

            // Определяем порядок блокировки для предотвращения deadlock
            var firstLock = fromAccountId.CompareTo(toAccountId) < 0 ? fromAccount : toAccount;
            var secondLock = fromAccountId.CompareTo(toAccountId) < 0 ? toAccount : fromAccount;

            // Блокируем счета в определённом порядке
            lock (firstLock)
            {
                lock (secondLock)
                {
                    // Проверяем баланс и выполняем перевод
                    if (fromAccount.GetBalance() < amount)
                    {
                        throw new InvalidOperationException("Недостаточно средств на счёте");
                    }

                    fromAccount.WithdrawAsync(amount).Wait();
                    toAccount.DepositAsync(amount).Wait();
                }
            }
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
            {
                throw new KeyNotFoundException($"Счёт не найден: {accountId}");
            }

            return account.GetBalance();
        }
    }
}