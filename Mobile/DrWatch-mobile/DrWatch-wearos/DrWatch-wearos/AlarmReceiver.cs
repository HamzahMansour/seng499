using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content.Res;
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
            //var repetitionIntent = new Intent(context, typeof(AlarmReceiver));
            //var source = PendingIntent.GetBroadcast(context, 0, intent, 0);
            //var am = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            //var calendar = DateTime.Parse(intent.GetStringExtra(Intent.ExtraText)).AddMinutes(1);

            //DateTime dtBasis = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)calendar.ToUniversalTime().Subtract(dtBasis).TotalMilliseconds, source);


            int notificationId = 001;
            // The channel ID of the notification.
            String id = "my_channel_01";

            // sound the alarm
            var alert = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            var _mediap = MediaPlayer.Create(context, alert);

            AudioManager audioM = (AudioManager)context.GetSystemService(Context.AudioService);
            if (audioM.GetStreamVolume(Stream.Alarm) != 0) {
                _mediap.Start();

                // wait 5 sec... then stop the player.
                new Handler().PostDelayed(() =>
                {
                    _mediap.Stop();
                }, 5000);//millisec.
        }

            NotificationManager notificationManager = (NotificationManager) context.GetSystemService(Context.NotificationService);
            
            Intent respondIntent = new Intent(context, typeof(NotificationService));
            PendingIntent respontPendingIntent = PendingIntent.GetService(context, 0, respondIntent, 0);

            Notification.Action action = new Notification.Action(Resource.Drawable.generic_confirmation,"confirm", respontPendingIntent);

            Intent snoozeIntent = new Intent(context, typeof(SnoozeService));
            PendingIntent snoozePendingIntent = PendingIntent.GetService(context, 0, snoozeIntent, 0);

            Notification.Action snooze = new Notification.Action(Resource.Drawable.generic_confirmation, "snooze", snoozePendingIntent);
            var noti = new Notification.Builder(context)
                .SetContentTitle("Title").SetContentText("content text")
                .SetSmallIcon(Resource.Drawable.pills)
                .AddAction(action)
                .AddAction(snooze);
            

            notificationManager.Notify(notificationId, noti.Build());
        }

        //public PendingIntent GetPendingIntent() {
        //    Intent urlIntent = new Intent(Intent.ActionSend);
        //    Uri uri = new Uri("http://localhost/");
        //
        //}
    }
}