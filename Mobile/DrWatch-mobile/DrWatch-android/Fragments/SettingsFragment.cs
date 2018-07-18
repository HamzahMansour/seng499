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
    public class SettingsFragment : Android.Support.V4.App.Fragment, View.IOnClickListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static SettingsFragment NewInstance()
        {
            var settingsFragment = new SettingsFragment { Arguments = new Bundle() };
            return settingsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var v = inflater.Inflate(Resource.Layout.settings, container, false);

            Button logTestButton = (Button)v.FindViewById(Resource.Id.LogInTestButton);
            logTestButton.SetOnClickListener(this);

            Button queryApiTestButton = (Button)v.FindViewById(Resource.Id.APIQueryTestButton);
            queryApiTestButton.SetOnClickListener(this);

            return v;
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.LogInTestButton:
                    LogInTest(v.Context, EventArgs.Empty);
                    break;

                case Resource.Id.APIQueryTestButton:
                    TestApiQuery(v.Context, EventArgs.Empty);
                    break;
            }
        }

        public void LogInTest(object sender, EventArgs args)
        {
            var intent = new Intent(Context, typeof(LogInActivity));
            StartActivity(intent);
        }

        public void TestApiQuery(object sender, EventArgs args)
        {
            var intent = new Intent(Context, typeof(QueryAPI));
            StartActivity(intent);
        }
    }
}