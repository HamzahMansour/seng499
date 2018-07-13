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

namespace DrWatch_android
{
    public class PrescriptionsFragment : Android.Support.V4.App.Fragment, View.IOnClickListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static PrescriptionsFragment NewInstance()
        {
            var prescriptionsFragment = new PrescriptionsFragment { Arguments = new Bundle() };
            return prescriptionsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var v = inflater.Inflate(Resource.Layout.prescriptions, container, false);

            Button addPerscriptionButton = (Button)v.FindViewById(Resource.Id.plusbtn);

            addPerscriptionButton.SetOnClickListener(this);
            return v;
        }


        private void Perscription(object sender, EventArgs eventArgs)
        {
            Intent myIntent = new Intent(Context, typeof(PerscriptionActivity));
            // myIntent.PutExtra("key", value)
            this.StartActivity(myIntent);
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.plusbtn:
                    Perscription(v.Context, EventArgs.Empty);
                    break;
            }
        }
    }
}