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

namespace DrWatch_wearos
{
    [BroadcastReceiver]
    class AlarmReceiver : BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent intent)
        {
            // set next alarm
            var repetitionIntent = new Intent(context, typeof(AlarmReceiver));
            var source = PendingIntent.GetBroadcast(context, 0, intent, 0);
            var am = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            var calendar = DateTime.Parse(intent.GetStringExtra(Intent.ExtraText));

            DateTime dtBasis = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)calendar.ToUniversalTime().Subtract(dtBasis).TotalMilliseconds, source);

        }
    }
}