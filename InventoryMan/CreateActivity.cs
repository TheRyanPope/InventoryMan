using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class CreateActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Create);

            EditText edtUser = FindViewById<EditText>(Resource.Id.edtUsername);
            EditText edtPass = FindViewById<EditText>(Resource.Id.edtPassword);
            Button btnCreate = FindViewById<Button>(Resource.Id.btnCreateAcc);

            // disable the login button until the user enters credentials
            MainActivity.disableLogin(edtUser, edtPass, btnCreate);

            // toggle login button on/off if fields are full/empty
            MainActivity.toggleLogin(edtUser, edtPass, btnCreate);

            btnCreate.Click += delegate
            {
                User person = new User(edtUser.Text, edtPass.Text);

                if (!Users.VerifyLogin(person))
                {
                    Users.CurrentUser = edtUser.Text;

                    // add person to the current list
                    Users.AddUsers(person);

                    // add person to the db
                    Users.InsertUpdateUser(person);

                    Toast.MakeText(this, "Account Created", ToastLength.Short).Show();

                    Intent homeNewIntent = new Intent(this, typeof(HomeActivity));
                    StartActivity(homeNewIntent);

                    edtUser.Text = "";
                    edtPass.Text = "";
                }
                else
                {
                    Toast.MakeText(this, "Account Already Exists", ToastLength.Short).Show();

                    edtUser.Text = "";
                    edtPass.Text = "";
                }
                
            };
        }
    }
}