using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace S_Bank
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountsPage : ContentPage
    {
        private User currentUser;
        public AccountsPage(User user)
        {
            InitializeComponent();

            // Set the current user (passed from the MainPage or login logic)
            currentUser = user;

            // Display user information
            userNameLabel.Text = $"Name: {currentUser.FirstName}";
            userSurnameLabel.Text = $"Surname: {currentUser.LastName}";
            DisplayBalances();
        }
        private async void DisplayBalances()
        {
            //Method to Retrieve Balance and Savings
            accountBalanceLabel.Text = $"Balance: {await App.Database.GetUserAccountBalanceAsync(currentUser.Id, "Balance"):C}";
            savingsBalanceLabel.Text = $"Savings: {await App.Database.GetUserAccountBalanceAsync(currentUser.Id, "Savings"):C}";
        }
    }
}