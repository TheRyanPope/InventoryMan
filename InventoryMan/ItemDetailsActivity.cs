using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class ItemDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ItemDetails);

            TextView txtHeader = FindViewById<TextView>(Resource.Id.txtItemTitle);
            TextView txtName = FindViewById<TextView>(Resource.Id.txtItemName);
            TextView txtPart = FindViewById<TextView>(Resource.Id.txtPartNumber);
            TextView txtDesc = FindViewById<TextView>(Resource.Id.txtDesc);
            TextView txtLoc = FindViewById<TextView>(Resource.Id.txtLoc);
            TextView txtLead = FindViewById<TextView>(Resource.Id.txtLead);
            TextView txtQoh = FindViewById<TextView>(Resource.Id.txtQoh);
            TextView txtComm = FindViewById<TextView>(Resource.Id.txtComments);

            Button btnHome = FindViewById<Button>(Resource.Id.btnHome);
            Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            Button btnRemove = FindViewById<Button>(Resource.Id.btnRemove);
            Button btnLog = FindViewById<Button>(Resource.Id.btnLog);
            
            txtHeader.Text = Items.ItemList[Items.SelectedItemIndex].Name + " Details";

            FillText(txtName, txtPart, txtDesc, txtLoc, txtLead, txtQoh, txtComm);
            
            btnHome.Click += delegate
            {
                Intent GoHomeIntent = new Intent(this, typeof(HomeActivity));
                StartActivity(GoHomeIntent);
            };

            btnAdd.Click += delegate
            {
                // add 1 to item QoH
                Items.ReceiveItem();

                // update QoH for item in DB
                Items.UpdateItem(Items.ItemList[Items.SelectedItemIndex]);

                // update details
                txtQoh.Text = (Int32.Parse(txtQoh.Text) + 1).ToString();
            };

            btnRemove.Click += delegate
            {
                // deduct 1 from item QoH
                Items.ConsumeItem();

                // update QoH for item in DB
                Items.UpdateItem(Items.ItemList[Items.SelectedItemIndex]);

                // update details
                txtQoh.Text = (Int32.Parse(txtQoh.Text) - 1).ToString();
            };

            btnLog.Click += delegate
            {
                Intent LogIntent = new Intent(this, typeof(ItemLogActivity));
                StartActivity(LogIntent);
            };

        }

        private static void FillText(TextView txtName, TextView txtPart, TextView txtDesc, TextView txtLoc,
            TextView txtLead, TextView txtQoh, TextView txtComm)
        {
            if (Items.SelectedItemIndex >= 0)
            {
                // determine which item will be displayed based on the current selected item index
                Item item = Items.ItemList[Items.SelectedItemIndex];
                
                // list of details for the selected item
                List<string> itemDetails = Items.AddItemDetails(item);

                // array of textviews for easy assigning of values
                TextView[] textViews = new TextView[] { txtName, txtPart, txtDesc, txtLoc, txtLead,
                    txtQoh, txtComm };

                int index = 0;
                foreach (string detail in itemDetails)
                {
                    textViews[index].Text = detail;
                    index++;
                }
            }
        }
    }
}