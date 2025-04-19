using System.Collections.Concurrent;

namespace Bank
{
        public class BankServer
        {
                private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

                public async Task<Guid> CreateAccount(decimal initialBalance = 0)
                {
                        var account = new BankAccount();

                        await account.DepositAsync(initialBalance);
                        _accounts.TryAdd(account.Id, account);
                        return account.Id;
                }

                public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
                {
                    if (fromAccountId == toAccountId)
                    {
                            return;
                    }

                    if (!_accounts.TryGetValue(fromAccountId, out var fromAccount))
                    {
                            throw new ArgumentException($"Аккаунт с ID  {fromAccountId} не найден.");
                    }

                    if (!_accounts.TryGetValue(toAccountId, out var toAccount))
                    {
                            throw new ArgumentException($"Аккаунт с ID  {toAccountId} не найден.");
                    }

                    await fromAccount.WithdrawAsync(amount);
                    await toAccount.DepositAsync(amount);
                }

                public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
                {
                    if (_accounts.TryGetValue(accountId, out var account))
                    {
                            return account.GetBalance();
                    }

                    throw new ArgumentException($"Аккаунт с ID {accountId} не найден.");
                }
        }
}
