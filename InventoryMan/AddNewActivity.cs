using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Text;

namespace InventoryMan
{
    [Activity(Label = "InventoryMan")]
    public class AddNewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddNew);

            EditText edtName = FindViewById<EditText>(Resource.Id.edtItemName);
            EditText edtPartNum = FindViewById<EditText>(Resource.Id.edtPartNum);
            EditText edtDesc = FindViewById<EditText>(Resource.Id.edtDesc);
            EditText edtLoc = FindViewById<EditText>(Resource.Id.edtLoc);
            EditText edtLead = FindViewById<EditText>(Resource.Id.edtLead);
            EditText edtQoh = FindViewById<EditText>(Resource.Id.edtQoh);
            EditText edtComments = FindViewById<EditText>(Resource.Id.edtComments);
            Button btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            // set max character limit on description and comments
            edtDesc.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(250) });
            edtComments.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(2000) });

            btnSubmit.Click += delegate
            {
                int value;
                if (FieldsPopulated(edtName, edtPartNum, edtDesc, edtLoc, edtLead, edtQoh, edtComments) &&
                        int.TryParse(edtQoh.Text, out value))
                {
                    var item = new Item(edtName.Text, edtPartNum.Text, edtDesc.Text, edtLoc.Text,
                      edtLead.Text, Int32.Parse(edtQoh.Text), edtComments.Text);

                    if (!Items.VerifyItem(item))
                    {
                        // add new item to the item list
                        Items.AddItem(item);

                        // insert the item into the item db
                        Items.InsertItem(item);

                        // identify the index for the item whose details will be displaying
                        Items.SelectedItemIndex = Items.ItemList.Count - 1;

                        DateTime localTime = DateTime.Now;

                        // add the first entry to the new items logs
                        ItemLogs.Logs.Add(new Log(item.Name + " added to inventory" + "\n" + 
                            localTime + "\n", Items.SelectedItemIndex));

                        Toast.MakeText(this, item.Name + " Added", ToastLength.Long).Show();
                        
                        Intent DetailsToHomeIntent = new Intent(this, typeof(HomeActivity));
                        StartActivity(DetailsToHomeIntent);

                        edtName.Text = "";
                        edtPartNum.Text = "";
                        edtDesc.Text = "";
                        edtLoc.Text = "";
                        edtLead.Text = "";
                        edtQoh.Text = "";
                        edtComments.Text = "";
                    }
                    else
                    {
                        Toast.MakeText(this, item.Name + " Already Exists", 
                            ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Fill All Fields Correctly", ToastLength.Long).Show();
                }
            };
        }

        private bool FieldsPopulated(EditText edtName, EditText edtPartNum, EditText edtDesc, EditText edtLoc,
                    EditText edtLead, EditText edtQoh, EditText edtComments)
        {
           if(edtName.Text != "" && edtPartNum.Text != "" && edtDesc.Text != "" && edtLoc.Text != "" && 
                edtLead.Text != "" && edtQoh.Text != "" && edtComments.Text != "")
           {
                return true;
           }

           // some/all fields are empty
           return false;
        }
    }
}