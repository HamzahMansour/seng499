using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
using Java.Interop;
using System.Globalization;
using Android.Graphics;

namespace DrWatch_wearos
{

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        private TextView _textView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            _textView = FindViewById<TextView>(Resource.Id.text);
            SetAmbientEnabled();

            DateTime nowtime = DateTime.Now.AddMinutes(2.0);

            setAlarm(nowtime);
        }

        //set our alarm with a datetime object
        private void setAlarm(DateTime calendar) {
            Color c = Color.Red;
            _textView.SetTextColor(c);
            
            // set my alarm (repeating function taken out)
            Intent intent = new Intent(this, typeof(AlarmReceiver));
           
            // add information to pending intent !!!
            DateTime tmp = calendar;
            tmp.AddMinutes(2.0);
            intent.PutExtra(Intent.ExtraText, tmp.ToLongTimeString());
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 14532, intent, 0);
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            
            DateTime dtBasis = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)(calendar.ToUniversalTime().Subtract(dtBasis).TotalMilliseconds), pendingIntent);


        }

    }
}


