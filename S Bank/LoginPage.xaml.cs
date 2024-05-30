using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace S_Bank
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private static AppDatabase _database;

        public LoginPage()
        {
            InitializeComponent();
            _database = new AppDatabase("dbBank.db3");
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Email and password are required", "OK");
                return;
            }

            // Check if the user exists
            var user = await _database.GetUserByEmailAsync(EmailEntry.Text);
            if (user == null || user.Password != PasswordEntry.Text)
            {
                await DisplayAlert("Error", "Invalid email or password", "OK");
                return;
            }
            App.SetCurrentUser(user);

            // Navigate to the main page
            await Navigation.PushAsync(new MainPage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Registration_Page());

        }
        
    }
}