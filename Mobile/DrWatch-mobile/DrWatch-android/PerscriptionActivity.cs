using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DrWatch_android
{
    [Activity(Label = "PerscriptionActivity")]
    public class PerscriptionActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NewEditPerscription);


            //intent intent = getIntent();
            //String value = intent.getStringExtra("key") to receive something from activity start


            Button cancel = FindViewById<Button>(Resource.Id.btnPerscriptionCancel);
            cancel.Click += cancelOnClick;

            Button save = FindViewById<Button>(Resource.Id.btnPerscriptionSave);
            save.Click += cancelOnClick;
            // Create your application here
        }

        private void cancelOnClick(object sender, EventArgs eventArgs)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }
    }
}