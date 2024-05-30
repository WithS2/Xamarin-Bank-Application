using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

//Ignore comment
//< x:String > Transfer to Another User's Balance</x:String>
namespace S_Bank
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage : ContentPage
    {
        private User currentUser;
        public TransferPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            DisplayBalances();
        }

        private async void DisplayBalances()
        {
            //Explained in Accounts Page
            accountBalanceLabel.Text = $"Balance: {await App.Database.GetUserAccountBalanceAsync(currentUser.Id, "Balance"):C}";
            savingsBalanceLabel.Text = $"Savings: {await App.Database.GetUserAccountBalanceAsync(currentUser.Id, "Savings"):C}";
        }

//Method used to transfer money
        private async void TransferButton_Clicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(amountEntry.Text, out decimal amount) && !string.IsNullOrEmpty(sourceAccountPicker.SelectedItem?.ToString()) && 
                !string.IsNullOrEmpty(destinationAccountPicker.SelectedItem?.ToString()))
            {
                bool transferResult = await App.Database.TransferMoneyAsync
                    (currentUser.Id, amount, sourceAccountPicker.SelectedItem.ToString(), destinationAccountPicker.SelectedItem.ToString());

                if (transferResult)
                {
                    await DisplayAlert("Success", "Transfer successful", "OK");
                    DisplayBalances();
                }
                else
                {
                    await DisplayAlert("Error", "Transfer failed. Please check your account balances.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid input. Please enter a valid amount and select source/destination accounts.", "OK");
            }
        }

//Refreshes page to see current amounts, however amounts auto refresh when transfering 
        private void refreshPage_Clicked(object sender, EventArgs e)
        {
            DisplayBalances();
        }
    }
}