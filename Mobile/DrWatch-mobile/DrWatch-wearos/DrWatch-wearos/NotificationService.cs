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
}