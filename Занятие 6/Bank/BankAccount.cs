﻿using System;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public Guid Id { get; } = Guid.NewGuid();
        private decimal _balance;
        private readonly object _balanceLock = new object();

        public decimal GetBalance()
        {
            lock (_balanceLock)
            {
                return _balance;
            }
        }

        public async Task DepositAsync(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive", nameof(amount));

            await Task.Delay(100);
            
            lock (_balanceLock)
            {
                _balance += amount;
            }
        }

        public async Task WithdrawAsync(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));

            await Task.Delay(100);
            
            lock (_balanceLock)
            {
                if (_balance < amount)
                    throw new InvalidOperationException("Insufficient funds");
                
                _balance -= amount;
            }
        }
    }
}