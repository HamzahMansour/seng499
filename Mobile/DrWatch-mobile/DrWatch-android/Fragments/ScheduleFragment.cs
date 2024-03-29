﻿using System;
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
    public class ScheduleFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static ScheduleFragment NewInstance()
        {
            var scheduleFragment = new ScheduleFragment { Arguments = new Bundle() };
            return scheduleFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.schedule, container, false);
        }

        private void Schedule(object sender, EventArgs eventArgs)
        {
            Intent myIntent = new Intent(Context, typeof(ScheduleActivity));
            //myIntent.PutExtra("key", value)
            this.StartActivity(myIntent);
        }
    }
}