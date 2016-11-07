using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace InventoryMan
{
    public class Disco : DialogFragment
    {
        private TextView txtPrompt;
        private Button btnNo;
        private Button btnYes;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // inflate view with this fragment
            var view = inflater.Inflate(Resource.Layout.Disco, container, false);

            txtPrompt = view.FindViewById<TextView>(Resource.Id.txtPrompt);
            btnNo = view.FindViewById<Button>(Resource.Id.btnNo);
            btnYes = view.FindViewById<Button>(Resource.Id.btnYes);

            string itemName = Items.ItemList[Items.SelectedItemIndex].Name;

            txtPrompt.Text = "Are you sure you want to discontinue " + itemName + " from inventory?";

            btnNo.Click += delegate
            {
                // do nothing
                this.Dismiss();
            };

            btnYes.Click += delegate
            {
                ItemLogs.DeleteLog(ItemLogs.SearchLogs());

                Items.DiscoItem();
                
                Intent home = new Intent(this.Activity, typeof(HomeActivity));
                StartActivity(home);
                
                this.Dismiss();
                
            };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // sets the title bar to invisible
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);

            // set the animation
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}