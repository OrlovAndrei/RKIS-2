using System;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var account = new BankAccount();
            
            Console.WriteLine("Bank Account Demo");
            Console.WriteLine($"Initial balance: {account.GetBalance()}");

            // Демонстрация работы
            try
            {
                await account.DepositAsync(500);
                Console.WriteLine($"After deposit: {account.GetBalance()}");

                await account.WithdrawAsync(200);
                Console.WriteLine($"After withdrawal: {account.GetBalance()}");

                // Попытка снять больше, чем есть
                await account.WithdrawAsync(400);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
