using Android.App;
using Android.Content;
using Android.OS;
using System;

namespace DrWatch_android
{
    [Activity(Label = "GoogleAuthInterceptor")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]{
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                //First part of the redirect url
                "com.googleusercontent.apps.397648763809-044990k2ah07p4t9h449nstrvstsdlq3"
            },
            DataPaths  = new[]
            {
                //Second part of the redirect url (Path)
                "/oauth2redirect"
            }
        )
    ]
    public class GoogleAuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            //Convert Android.Net.Url to Uri
            Uri uri_netfx = new Uri(uri_android.ToString());

            //Send the URI to the Authenticator for continuation
            LogInActivity.Auth?.OnPageLoading(uri_netfx);

            //Automatically switch back to app
            var intent = new Intent(this, typeof(LogInActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            Finish();
        }
    }
}