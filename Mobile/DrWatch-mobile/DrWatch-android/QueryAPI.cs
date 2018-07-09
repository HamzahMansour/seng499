using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DrWatch_android
{
    [Activity(Label = "QueryAPI")]
    public class QueryAPI : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.TestAPIQuery);

            //Get text content from brand name textbox
            EditText BrandText = FindViewById<EditText>(Resource.Id.medicationBrandText);
            Button TestQueryButton = FindViewById<Button>(Resource.Id.queryButton);

            //When query button is clicked
            TestQueryButton.Click += async (sender, e) =>
            {
                string url = "https://health-products.canada.ca/api/drug/drugproduct/?lang=en&type=json&brandname=" + 
                    BrandText.Text;

                JsonValue json = await FetchMedicationAsync(url);
                // ParseAndDisplay(json);

            };

        }

        private async Task<JsonValue> FetchMedicationAsync(string url)
        {
            //Create HTTP request using the URL
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            //Send the request ot the server and wait for the response
            //TODO: Add exception handling to web queries and Json parsing
            using (System.Net.WebResponse response = await request.GetResponseAsync())
            {
                //Get a stream representation of the HTTP web response
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    //Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    //Return the JSON document
                    return jsonDoc;
                }
                    
            }

        }


    }
}