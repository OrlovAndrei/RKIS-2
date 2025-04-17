using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank;

public class BankServer
{
    private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();

    public async Task<Guid> CreateAccount(decimal initialBalance = 0)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative", nameof(initialBalance));

        var account = new BankAccount();
        await account.DepositAsync(initialBalance);
        _accounts.TryAdd(account.Id, account);
        return account.Id;
    }

    public async Task PerformTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Transaction amount cannot be negative", nameof(amount));

        if (!_accounts.TryGetValue(fromAccountId, out var fromAccount) ||
            !_accounts.TryGetValue(toAccountId, out var toAccount))
            throw new KeyNotFoundException("One or both accounts do not exist.");

        if (fromAccountId == toAccountId && amount != 0)
            throw new InvalidOperationException("Cannot transfer money to the same account.");

        if (amount == 0 && fromAccountId == toAccountId)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");

        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero amount transfer to the same account.");


        if (fromAccountId == toAccountId && amount == 0)
            throw new InvalidOperationException("Invalid transaction: zero-amount self-transfer is not allowed.");



        var firstLock = fromAccount.Id.CompareTo(toAccount.Id) < 0 ? fromAccount.Lock : toAccount.Lock;
        var secondLock = fromAccount.Id.CompareTo(toAccount.Id) < 0 ? toAccount.Lock : fromAccount.Lock;

        await firstLock.WaitAsync();
        try
        {
            await secondLock.WaitAsync();
            try
            {
                if (fromAccount.GetBalance() < amount)
                    throw new InvalidOperationException("Insufficient funds");

                fromAccount.WithdrawInternal(amount);
                toAccount.DepositInternal(amount);
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
            throw new KeyNotFoundException("Account does not exist.");

        return await account.GetBalanceAsync();
    }
}