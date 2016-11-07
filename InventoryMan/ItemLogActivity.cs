using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class ItemLogActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ItemLog);

            Button btnHomeLog = FindViewById<Button>(Resource.Id.btnHomeLog);
            Button btnDisco = FindViewById<Button>(Resource.Id.btnDisco);
            TextView txtItemName = FindViewById<TextView>(Resource.Id.txtItemName);

            txtItemName.Text = Items.ItemList[Items.SelectedItemIndex].Name + " Log";




            List<Log> searchLog = ItemLogs.SearchLogs();

            string[] itemLog = new string[searchLog.Count];

            int index = 0;
            foreach (Log log in searchLog)
            {
                itemLog[index] = log.Entry;
                index++;
            }




            ListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, itemLog);

            btnHomeLog.Click += delegate
            {
                Intent home = new Intent(this, typeof(HomeActivity));
                StartActivity(home);
            };

            btnDisco.Click += (object sender, EventArgs args) =>
            {
                // pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                Disco discoDialog = new Disco();
                discoDialog.Show(transaction, "Discontinue Dialog");
                
            };
        }
    }
}