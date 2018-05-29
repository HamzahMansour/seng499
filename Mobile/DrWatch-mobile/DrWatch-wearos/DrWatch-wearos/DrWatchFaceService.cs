using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Service.Wallpaper;
using Android.Support.Wearable.Watchface;
using Android.Views;
using Android.Widget;

namespace DrWatch_wearos
{
    [Service(Name ="com.xamarin.watch_wear.DrWatchFaceService")]
    class DrWatchFaceService : CanvasWatchFaceService
    {
        public override WallpaperService.Engine OnCreateEngine()
        {
            return new DrWatchEngine(this);
        }

        public class DrWatchEngine : CanvasWatchFaceService.Engine
        {
            Paint hoursPaint;
            CanvasWatchFaceService owner;
            public DrWatchEngine(CanvasWatchFaceService owner) : base(owner)
            {
                this.owner = owner;
            }

            public override void OnCreate(ISurfaceHolder surfaceHolder)
            {
                base.OnCreate(surfaceHolder);
                
                //can add watch face settings here
                SetWatchFaceStyle(new WatchFaceStyle.Builder(owner)
                    .Build());

                hoursPaint = new Paint();
                hoursPaint.Color = Color.Black;
                hoursPaint.TextSize = 48f;
            }

            // draws watch face elements
            public override void OnDraw(Canvas canvas, Rect bounds)
            {
                var str = DateTime.Now.ToString("h:mm tt");
                canvas.DrawText(str, (float)(bounds.Left + 70), (float)(bounds.Top + 80), hoursPaint);
            }

            //schedules ondraw
            public override void OnTimeTick()
            {
                Invalidate();
            }



        }
    }
}