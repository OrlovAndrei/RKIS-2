using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private readonly ConcurrentDictionary<Guid, BankAccount> _bankAccounts = new();

        public async Task<Guid> CreateAccountAsync(decimal initialDepositAmount = 0)
        {
            if (initialDepositAmount < 0)
            {
                throw new ArgumentException("Начальный баланс не может быть отрицательным", nameof(initialDepositAmount));
            }

            var newAccount = new BankAccount();

            if (initialDepositAmount > 0)
            {
                await newAccount.DepositAsync(initialDepositAmount);
            }

            _bankAccounts.TryAdd(newAccount.AccountId, newAccount);
            return newAccount.AccountId;
        }

        public async Task PerformTransactionAsync(Guid senderAccountId, Guid receiverAccountId, decimal transferAmount)
        {
            if (transferAmount <= 0)
            {
                throw new ArgumentException("Сумма перевода должна быть положительной", nameof(transferAmount));
            }

            if (senderAccountId == receiverAccountId)
            {
                throw new InvalidOperationException("Нельзя переводить средства на тот же самый счёт");
            }

            if (!_bankAccounts.TryGetValue(senderAccountId, out var senderAccount))
            {
                throw new KeyNotFoundException($"Исходный счёт не найден: {senderAccountId}");
            }

            if (!_bankAccounts.TryGetValue(receiverAccountId, out var receiverAccount))
            {
                throw new KeyNotFoundException($"Целевой счёт не найден: {receiverAccountId}");
            }

            var firstLockAccount = senderAccountId.CompareTo(receiverAccountId) < 0 ? senderAccount : receiverAccount;
            var secondLockAccount = senderAccountId.CompareTo(receiverAccountId) < 0 ? receiverAccount : senderAccount;

            lock (firstLockAccount)
            {
                lock (secondLockAccount)
                {
                    if (senderAccount.GetCurrentBalance() < transferAmount)
                    {
                        throw new InvalidOperationException("Недостаточно средств на счёте отправителя");
                    }

                    senderAccount.WithdrawAsync(transferAmount).Wait();
                    receiverAccount.DepositAsync(transferAmount).Wait();
                }
            }
        }

        public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
        {
            if (!_bankAccounts.TryGetValue(accountId, out var account))
            {
                throw new KeyNotFoundException($"Счёт не найден: {accountId}");
            }

            return account.GetCurrentBalance();
        }
    }
}