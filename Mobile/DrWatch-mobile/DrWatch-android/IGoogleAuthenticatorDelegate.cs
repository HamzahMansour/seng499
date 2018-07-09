using System;

namespace DrWatch_android
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCancelled();
    }
}