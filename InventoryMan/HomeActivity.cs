using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);

            List<Item> searchItems = null;;

            EditText searchBar = FindViewById<EditText>(Resource.Id.searchBar);
            Button btnLogout = FindViewById<Button>(Resource.Id.btnLogout);
            Button btnAddNew = FindViewById<Button>(Resource.Id.btnAddNew);
            ListView items = FindViewById<ListView>(Resource.Id.list);
            TextView txtWelcome = FindViewById<TextView>(Resource.Id.txtWelcome);

            txtWelcome.Text = "Welcome " + Users.CurrentUser + "!";

            // search bar listens for when text is changed
            searchBar.TextChanged += (Object sender, Android.Text.TextChangedEventArgs args) =>
            {
                if(searchBar.Text == "")
                {
                    items.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1,
                        Items.ItemListToArray());
                }
                else
                {
                    searchItems = (from item in Items.ItemList
                                   where item.Name.Contains(searchBar.Text)
                                   || item.PartNumber.Contains(searchBar.Text)
                                   select item).ToList<Item>();

                    items.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1,
                    Items.SearchItemsToArray(searchItems));
                }
            };

            // create an adapter for displaying list of items in the list view
            items.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1,
                Items.ItemListToArray());

            // item is selected from list
            items.ItemClick += (Object sender, AdapterView.ItemClickEventArgs clickedItem) =>
            {
                if (Items.ItemList.Count > 0)
                {
                    
                    var test = clickedItem.Position;

                    try
                    {
                        if(searchBar.Text == "")
                        {
                            // nothing has been searched
                            Items.SelectedItemIndex = clickedItem.Position;

                        }
                        else
                        {
                            // something is being searched
                            Items.ItemList.IndexOf(searchItems[test]);
                        }
                    }
                    catch (NullReferenceException)
                    {

                    }
                    
                    Intent itemDetails = new Intent(this, typeof(ItemDetailsActivity));
                    StartActivity(itemDetails);

                    searchBar.Text = "";
                }
                else
                {
                    Toast.MakeText(this, "No Items Available", ToastLength.Short).Show();
                }
            };
           
            // logout button clicked
            btnLogout.Click += delegate
            {
                Intent logoutIntent = new Intent(this, typeof(MainActivity));
                StartActivity(logoutIntent);

                searchBar.Text = "";
            };

            // add a new item button clicked
            btnAddNew.Click += delegate
            {
                Intent addNewIntent = new Intent(this, typeof(AddNewActivity));
                StartActivity(addNewIntent);

                searchBar.Text = "";
            };
            
        }

    }
}