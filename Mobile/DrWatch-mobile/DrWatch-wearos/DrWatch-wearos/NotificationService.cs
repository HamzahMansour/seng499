using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DrWatch_wearos
{
    [Service]
    class NotificationService : IntentService
    {

        public NotificationService() : base("NotificationService") { }
        
        protected override void OnHandleIntent(Intent intent)
        {
            Console.WriteLine("Got here!");
        }
    }
    [Service]
    class SnoozeService : IntentService
    {

        public SnoozeService() : base("SnoozeService") { }

        protected override void OnHandleIntent(Intent intent)
        {
            // set next alarm
            var repetitionIntent = new Intent(this.BaseContext, typeof(AlarmReceiver));
            var source = PendingIntent.GetBroadcast(this.BaseContext, 0, intent, 0);
            var am = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            var calendar = DateTime.Now.AddMinutes(5);

            DateTime dtBasis = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)calendar.ToUniversalTime().Subtract(dtBasis).TotalMilliseconds, source);

        }
    }
}