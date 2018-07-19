using DrWatch_android;
using Xamarin.Auth;

namespace StoreCredentials
{
    public interface ICredentialsService
    {

        void SaveCredentials(Account account);

        void DeleteCredentials();

        bool DoCredentialsExist();

        Account LoadStoredCredentials();
    }
}