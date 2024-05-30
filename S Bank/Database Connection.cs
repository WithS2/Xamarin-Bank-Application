using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
//
namespace S_Bank
{
    internal class Database_Connection
    {
    }
    public class AppDatabase
    {
        readonly SQLiteAsyncConnection _database;

  //---------------------------------------------------------------------------------
  //Creating of Database
        public AppDatabase(string dbFileName)
        {
            string folderPath = GetDatabaseFolderPath();

            string databasePath = Path.Combine(folderPath, dbFileName);

            _database = new SQLiteAsyncConnection(databasePath);

            // Create tables if they don't exist
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Account>().Wait();
        }


//-------------------------------------------------------------------------------------
//Connecting to Database

        private string GetDatabaseFolderPath()
        {
            // Gets the platform-specific folder path
            string folderPath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");
                    break;
                case Device.Android:
                    folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported platform");
            }

            string appFolderPath = Path.Combine(folderPath, "YourXamarinFormsAppFolder");

            // Creates the folder if it doesn't exist
            if (!Directory.Exists(appFolderPath))
            {
                Directory.CreateDirectory(appFolderPath);
            }

            // Combines with a subfolder for the accounts table
            string accountsFolderPath = Path.Combine(appFolderPath, "AccountsFolder");

            // Creates the subfolder if it doesn't exist
            if (!Directory.Exists(accountsFolderPath))
            {
                Directory.CreateDirectory(accountsFolderPath);
            }

            return accountsFolderPath;
        }

//---------------------------------------------------------------------------------------------------------
//From here on, the methods are explained by their names
        public Task<List<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return _database.Table<Account>().Where(a => a.UserId == userId).ToListAsync();
        }
//------------------------------------------------------------------------------------------------------
        public Task<int> SaveAccountAsync(Account account)
        {
            if (account.Id != 0)
            {
                return _database.UpdateAsync(account);
            }
            else
            {
                return _database.InsertAsync(account);
            }
        }
//------------------------------------------------------------------------------------------------

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }
//----------------------------------------------------------------------------------------------------------

        public async Task<decimal> GetUserAccountBalanceAsync(int userId, string accountType)
        {
            try
            {
                var account = await _database.Table<Account>()
                    .Where(a => a.UserId == userId && a.Type == accountType)
                    .FirstOrDefaultAsync();

                return account?.Balance ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserAccountBalanceAsync: {ex.Message}");
                return 0;
            }
        }
        //--------------------------------------------------------------------------------------------------------

        public async Task<bool> TransferMoneyAsync(int userId, decimal amount, string sourceAccountType, string destinationAccountType)
        {
            try
            {
                var sourceAccount = await _database.Table<Account>()
                    .Where(a => a.UserId == userId && a.Type == sourceAccountType)
                    .FirstOrDefaultAsync();

                var destinationAccount = await _database.Table<Account>()
                    .Where(a => a.UserId == userId && a.Type == destinationAccountType)
                    .FirstOrDefaultAsync();

                if (sourceAccount == null || destinationAccount == null)
                {
                    return false;
                }

                if (sourceAccount.Balance >= amount)
                {
                    sourceAccount.Balance -= amount;
                    destinationAccount.Balance += amount;

                    await _database.UpdateAsync(sourceAccount);
                    await _database.UpdateAsync(destinationAccount);

                    return true; 
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TransferMoneyAsync: {ex.Message}");
                return false;
            }
        }
    }
}
