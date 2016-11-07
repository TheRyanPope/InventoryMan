using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            Window.SetStatusBarColor(Color.Transparent);

            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);   

            // Create variables for onscreen widgets
            EditText edtUsername = FindViewById<EditText>(Resource.Id.editUsername);
            EditText edtPassword = FindViewById<EditText>(Resource.Id.editPassword);
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            TextView btnCreate = FindViewById<TextView>(Resource.Id.btnCreate);

            // disable the login button until the user enters credentials
            disableLogin(edtUsername, edtPassword, btnLogin);

            // toggle login button on/off if fields are full/empty
            toggleLogin(edtUsername, edtPassword, btnLogin);

            btnLogin.Click += delegate
            {
                if (Users.VerifyLogin(new User(edtUsername.Text, edtPassword.Text)))
                {
                    Users.CurrentUser = edtUsername.Text;

                    // creds verified go to Home screen
                    Intent homeInent = new Intent(this, typeof(HomeActivity));
                    StartActivity(homeInent);

                    edtUsername.Text = "";
                    edtPassword.Text = "";
                }
                else
                {
                    // toast credentials fail
                    Toast.MakeText(this, "Invalid Credentials", ToastLength.Short).Show();
                    edtUsername.Text = "";
                    edtPassword.Text = "";
                }

            };

            btnCreate.Click += delegate
            {
                Intent createIntent = new Intent(this, typeof(CreateActivity));
                StartActivity(createIntent);
            };
        }

        // enable login button
        public static void enableLogin(EditText editText, Button login)
        {
            if(editText.Text != "")
            {
                login.Enabled = true;
            }
        }

        // disable login button
        public static void disableLogin(EditText user, EditText pass, Button login)
        {
            if (user.Text == "" || pass.Text == "")
            {
                login.Enabled = false;
            }
        }

        // toggle login button based on user/pass fields being filled
        public static void toggleLogin(EditText user, EditText pass, Button login)
        {
            user.AfterTextChanged += delegate (object sender, Android.Text.AfterTextChangedEventArgs e)
            {
                enableLogin(pass, login);
                disableLogin(user, pass, login);
            };

            pass.AfterTextChanged += delegate (object sender2, Android.Text.AfterTextChangedEventArgs e2)
            {
                enableLogin(pass, login);
                disableLogin(user, pass, login);
            };
        }

    }
}