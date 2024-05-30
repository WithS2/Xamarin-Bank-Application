using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace S_Bank
{
    public partial class MainPage : ContentPage
    {
        private User currentUser;
        public MainPage()
        {
            InitializeComponent();
            //sets current user to whomever logged in
            currentUser = App.GetCurrentUser();
            userNameLabel.Text = $"Welcome Name: {currentUser.FirstName}";
        }

        private async void ViewBalanceButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to the AccountsPage
            await Navigation.PushAsync(new AccountsPage(currentUser));

        }

        private async void TransferFundsButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to the TransferPage
            await Navigation.PushAsync(new TransferPage(currentUser));
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            // Navigate back to the LoginPage
            await Navigation.PopToRootAsync();
        }
    }
}
