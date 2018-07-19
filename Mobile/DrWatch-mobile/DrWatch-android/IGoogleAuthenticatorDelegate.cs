using System;
using Xamarin.Auth;

namespace DrWatch_android
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token, Account account);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCancelled();
    }
}