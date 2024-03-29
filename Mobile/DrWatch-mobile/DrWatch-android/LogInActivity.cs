﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace DrWatch_android
{
    [Activity(Label = "LogInActivity")]
    public class LogInActivity : Activity, IGoogleAuthenticationDelegate
    {

        public static GoogleAuthenticator Auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LogIn);

            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);

            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            //Display the activity handling the authentication
            var authenticator = Auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token, Account account)
        {
            //Retrieve the user's email address
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(token.TokenType, token.AccessToken);

            //Add email address to account
            account.Properties.Add("Email", email);

            //Retrieve IdToken
            //var idToken = await googleService.GetOpenIdAsync(token.TokenType, token.AccessToken, account);            

            //Display email on the UI
            //var googleButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            //googleButton.Text = $"Connected with {email}";

            //Save logged in user
            CredentialsService credentialsService = new CredentialsService();
            credentialsService.SaveCredentials(account);
            
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        public void OnAuthenticationCancelled()
        {
            new AlertDialog.Builder(this)
                            .SetTitle("Authentication Cancelled")
                            .SetMessage("The authentication process was cancelled.")
                            .Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                            .SetTitle(message)
                            .SetMessage(exception?.ToString())
                            .Show();
        }
    }
}