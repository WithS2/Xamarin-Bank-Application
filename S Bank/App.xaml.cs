using System;
using Xamarin.Forms;
using SQLite;
using Xamarin.Forms.Xaml;

namespace S_Bank
{
    
    public partial class App : Application
    {
        public static AppDatabase Database { get; private set; } //initialize database
        public static User CurrentUser { get; private set; } //initialize user
        public App()
        {
            InitializeComponent();
            Database = new AppDatabase("dbBank.db3");
            //sets MainPage to LoginPage
            MainPage = new NavigationPage(new LoginPage());
            
        }

        // Method to set the current user
        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        // Method to get the current user
        public static User GetCurrentUser()
        {
            return CurrentUser;
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
