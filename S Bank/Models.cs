using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace S_Bank
{
    internal class Models
    {
    }
    // User model
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Gender { get; set; }
    }

    // Account model
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } // "Balance" or "Savings"
        public decimal Balance { get; set; }
    }

}
