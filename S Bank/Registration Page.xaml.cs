using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace S_Bank
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration_Page : ContentPage
    {
        private AppDatabase _database;
        public Registration_Page()
        {
            InitializeComponent();
            _database = new AppDatabase("dbBank.db3");
        }
        private bool IsEmailValid(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }

        private bool IsPasswordLengthValid(string password)
        {
            // Check if password is at least 6 characters long
            return password.Length >= 6;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            //Checks entries for errors
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text)
            || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text)
            || string.IsNullOrWhiteSpace(MobileEntry.Text) || GenderPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            var existingUser = await _database.GetUserByEmailAsync(EmailEntry.Text);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "Email already exists", "OK");
                return;
            }
            var newUser = new User
            {
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text,
                MobileNumber = MobileEntry.Text,
                Gender = GenderPicker.SelectedItem.ToString()
            };

            if (!IsEmailValid(EmailEntry.Text))
            {
                await DisplayAlert("Error", "Email pattern invalid", "OK");
                return;
            }

            if (!IsPasswordLengthValid(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Password not longer than 5 characters", "OK");
                return;
            }

            await _database.SaveUserAsync(newUser);

            // Create Balance and Savings accounts for the user
            var balanceAccount = new Account
            {
                UserId = newUser.Id,
                Type = "Balance",
                Balance = 1000
            };

            var savingsAccount = new Account
            {
                UserId = newUser.Id,
                Type = "Savings",
                Balance = 1000
            };

            await _database.SaveAccountAsync(balanceAccount);
            await _database.SaveAccountAsync(savingsAccount);

            // Navigates to the main page
            await DisplayAlert("Account Created", "Account Successfuly Created", "OK");
            await Navigation.PushAsync(new LoginPage());

        }

    }
}