using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();
        private readonly Dictionary<Guid, object> _locks = new();

        public async Task<Guid> CreateAccount(decimal initialBalance = 0)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Начальный баланс не может быть отрицательным.");
            }
            var account = new BankAccount();
            if (initialBalance > 0)
            {
                await account.DepositAsync(initialBalance);
            }
            _accounts[account.Id] = account;
            _locks[account.Id] = new object();
            return account.Id;
        }

        public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (fromAccountId == toAccountId)
            {
                throw new InvalidOperationException("Нельзя перевести деньги на тот же счет.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Сумма перевода должна быть положительной.");
            }

            if (!_accounts.ContainsKey(fromAccountId) || !_accounts.ContainsKey(toAccountId))
            {
                throw new KeyNotFoundException("Один или оба счета не найдены.");
            }

            if (!_locks.TryGetValue(fromAccountId, out var fromLock) || !_locks.TryGetValue(toAccountId, out var toLock))
            {
                throw new KeyNotFoundException("Один или оба счета не найдены.");
            }


            var firstLock = fromAccountId.CompareTo(toAccountId) < 0 ? fromLock : toLock;
            var secondLock = fromAccountId.CompareTo(toAccountId) < 0 ? toLock : fromLock;


            try
            {
                lock (firstLock)
                lock (secondLock)
                {
                    // Получаем account внутри lock, чтобы избежать race condition при удалении аккаунта
                    if (!_accounts.TryGetValue(fromAccountId, out var fromAccount) || !_accounts.TryGetValue(toAccountId, out var toAccount))
                    {
                        throw new KeyNotFoundException("Один или оба счета не найдены.");
                    }

                    if (fromAccount.GetBalance() < amount)
                    {
                        throw new InvalidOperationException("Недостаточно средств на счете.");
                    }

                    await fromAccount.WithdrawAsync(amount);
                    await toAccount.DepositAsync(amount);

                }
            }
            catch (InvalidOperationException)
            {
                throw; 
            }
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_accounts.TryGetValue(accountId, out var account))
            {
                throw new KeyNotFoundException("Счет не найден.");
            }
            return account.GetBalance();
        }
    }
}
