using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Auth;
using System;

namespace DrWatch_android
{
    class GoogleService
    {
        public async Task<string> GetEmailAsync(string tokenType, string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            var json = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/email?alt=json");
            var email = JsonConvert.DeserializeObject<GoogleEmail>(json);
            return email.Data.Email;
        }

        public async Task<string> GetOpenIdAsync(string tokenType, string accessToken, Account account)
        {
            /*var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            var json = await httpClient.PostAsync("https://www.googleapis.com/oauth2/v4/token", accessToken);
            return json.ToString();*/

            var request = new OAuth2Request("POST", new Uri("https://www.googleapis.com/oauth2/v4/token"), null, account);
            var response = await request.GetResponseAsync();
            if (response != null)
            {
                string Id = response.GetResponseText();
                return Id;
            }
            else return null;
        }
    }
}