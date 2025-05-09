using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _accountLocks = new();

        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Начальный баланс не может быть отрицательным", nameof(initialBalance));
            }

            var account = new BankAccount();
            _accountLocks.TryAdd(account.Id, new SemaphoreSlim(1, 1));

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

            if (!_accounts.TryGetValue(fromAccountId, out var fromAccount) ||
                !_accountLocks.TryGetValue(fromAccountId, out var fromLock))
            {
                throw new KeyNotFoundException($"Исходный счёт не найден: {fromAccountId}");
            }

            if (!_accounts.TryGetValue(toAccountId, out var toAccount) ||
                !_accountLocks.TryGetValue(toAccountId, out var toLock))
            {
                throw new KeyNotFoundException($"Целевой счёт не найден: {toAccountId}");
            }

            // Определяем порядок блокировки для предотвращения deadlock
            var firstLock = fromAccountId.CompareTo(toAccountId) < 0 ? fromLock : toLock;
            var secondLock = fromAccountId.CompareTo(toAccountId) < 0 ? toLock : fromLock;

            // Блокируем счета в определённом порядке
            await firstLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await secondLock.WaitAsync().ConfigureAwait(false);
                try
                {
                    // Проверяем баланс и выполняем перевод
                    if (fromAccount.GetBalance() < amount)
                    {
                        throw new InvalidOperationException("Недостаточно средств на счёте");
                    }

                    await fromAccount.WithdrawAsync(amount).ConfigureAwait(false);
                    await toAccount.DepositAsync(amount).ConfigureAwait(false);
                }
                finally
                {
                    secondLock.Release();
                }
            }
            finally
            {
                firstLock.Release();
            }
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
            {
                throw new KeyNotFoundException($"Счёт не найден: {accountId}");
            }

            return await Task.Run(() => account.GetBalance()).ConfigureAwait(false);
        }
    }
}