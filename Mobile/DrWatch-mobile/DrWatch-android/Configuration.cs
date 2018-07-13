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

namespace DrWatch_android
{
    public static class Configuration
    {
        public const string ClientId = "397648763809-044990k2ah07p4t9h449nstrvstsdlq3.apps.googleusercontent.com";
        public const string Scope = "openid email";
        public const string RedirectUrl = "com.googleusercontent.apps.397648763809-044990k2ah07p4t9h449nstrvstsdlq3:/oauth2redirect";
    }
}