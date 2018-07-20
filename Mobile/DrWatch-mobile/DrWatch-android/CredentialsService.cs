using System.Linq;
using Xamarin.Auth;
using DrWatch_android;

namespace DrWatch_android
{
    public class CredentialsService : ICredentialsService
    {
        public void SaveCredentials(Account account)
        {
            if(account != null)
            {
                AccountStore.Create().Save(account, "DrWatch_android");
            } 
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService("DrWatch_android").FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, "DrWatch_android");
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService("DrWatch_android").Any() ? true : false;
        }

        public Account LoadStoredCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService("DrWatch_android").FirstOrDefault();
            return account;
        }
    }
}