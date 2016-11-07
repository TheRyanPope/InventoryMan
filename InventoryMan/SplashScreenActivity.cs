using Android.App;
using Android.OS;
using Android.Views;
using System.Threading;

namespace InventoryMan
{
    [Activity(Label = "Inventory Man", MainLauncher = true, Theme = "@style/Theme.Splash", 
        NoHistory = true, Icon = "@drawable/mLogo")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // create item database
            Items.CreateItemDatabase();

            // load items from DB
            Items.LoadItemDB();

            // create user database
            Users.CreateUserDatabase();

            // load users from DB
            Users.LoadUserDB();

            // create log database
            ItemLogs.CreateLogsDatabase();

            // load users from DB
            ItemLogs.LoadLogDB();

            // keep the slapsh screen up for at least 2 seconds + app load time
            Thread.Sleep(2000);

            StartActivity(typeof(MainActivity));
        }
    }
}