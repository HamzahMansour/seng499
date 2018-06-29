using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace DrWatch_android
{
    [Activity(Label = "PerscriptionActivity")]
    public class PerscriptionActivity : Activity
    {
        List<string> lisIntervals = new List<string>();
        ArrayAdapter<string> intervalAdapter;
        ArrayAdapter<string> perscriptionAdapter;
        ListView ListIntervals;
        List<int> checkedIntervals = new List<int>();
        Spinner intervalSelection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NewEditPerscription);


            //intent intent = getIntent();
            //String value = intent.getStringExtra("key") to receive something from activity start
            List<string> PerscriptionNames = GetPerscriptions().Result;
            AutoCompleteTextView perscription = (AutoCompleteTextView)FindViewById(Resource.Id.autoCompletePerscription);
            perscriptionAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, PerscriptionNames);
            perscription.Adapter = perscriptionAdapter;
            perscriptionAdapter.NotifyDataSetChanged();

            // handle cancel and save events they should both return to the perscriptions view
            Button cancel = FindViewById<Button>(Resource.Id.btnPerscriptionCancel);
            cancel.Click += cancelOnClick;

            Button save = FindViewById<Button>(Resource.Id.btnPerscriptionSave);
            save.Click += saveOnClick;

            // needed for interval addition
            // handling all interval logic
            intervalSelection = FindViewById<Spinner>(Resource.Id.intervalselect);
            Button addIntervals = FindViewById<Button>(Resource.Id.btnPerscriptionTime);
            ListIntervals = FindViewById<ListView>(Resource.Id.add_intervals);
            intervalAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lisIntervals);
            ListIntervals.Adapter = intervalAdapter;
            Button deleteIntervals = FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime);
            
            intervalSelection.ItemSelected += (sender, args) =>
            {
                if ((string)((Spinner)sender).SelectedItem == "Daily")
                    FindViewById<EditText>(Resource.Id.txtIntervalDate).Visibility = ViewStates.Gone;
                else
                    FindViewById<EditText>(Resource.Id.txtIntervalDate).Visibility = ViewStates.Visible;
                intervalAdapter.Clear();
                intervalAdapter.NotifyDataSetChanged();
                FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime).Enabled = false;
            };

            ListIntervals.ChoiceMode = ChoiceMode.Multiple;
            ListIntervals.ItemClick += itemSelectedInterval;
            addIntervals.Click += addIntervalClick;
            deleteIntervals.Click += deleteIntervalsClick;
        }

        // get our data from the api, or from saved (depending on how we want to do it
        private async Task<List<string>> GetPerscriptions()
        {
            string url = "https://health-products.canada.ca/api/drug/drugproduct/?lang=en&type=json";
            string jsonString = await FetchMedicationAsync(url);
            List<string> allBrands = new List<string>{"Brand1", "Brand2", "Brand3"};
            return allBrands;
        }

        private async Task<string> FetchMedicationAsync(string url)
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
                    return jsonDoc.ToString();
                }

            }

        }

        // select list item from interval list
        private void itemSelectedInterval(object sender, AdapterView.ItemClickEventArgs e)
        {
            checkedIntervals.Add(e.Position);
        }

        // validate and add intervals from date and time edittext boxes
        private void addIntervalClick(object sender, EventArgs e)
        {
            if (intervalSelection.SelectedItem == null) return;
            EditText start = FindViewById<EditText>(Resource.Id.txtIntervalStart);
            EditText end = FindViewById<EditText>(Resource.Id.txtIntervalEnd);
            var tmp = new StringBuilder();
            Regex r;
            Toast error;

            if ((string)intervalSelection.SelectedItem != "Daily") {
                EditText date = FindViewById<EditText>(Resource.Id.txtIntervalDate);
                r = new Regex("^(0[0-9])|(1[0-2])/([0-2][0-9])|(3[0-1])");

                if (!r.IsMatch(date.EditableText.ToString())) {
                    error = Toast.MakeText(this, "incorrect format for month should be mm/dd", ToastLength.Long);
                    error.Show();
                    return;
                }
                if ((string)intervalSelection.SelectedItem == "Yearly")
                    tmp.AppendFormat("Date {0} ", date.EditableText.ToString());
                else
                    tmp.AppendFormat("Date {0} ", date.EditableText.ToString().Split('/')[1]);
            }

            r = new Regex("^(([0-1][0-9])|(2[0-3])):[0-5][0-9]");
            if (!r.IsMatch(start.EditableText.ToString()) || !r.IsMatch(end.EditableText.ToString()))
            {
                error = Toast.MakeText(this, "incorrect format for time should be hh:mm", ToastLength.Long);
                error.Show();
                return;
            }
            tmp.AppendFormat("Start {0} End {1}", start.EditableText.ToString(), end.EditableText.ToString());

            intervalAdapter.Add(tmp.ToString());
            intervalAdapter.NotifyDataSetChanged();
            FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime).Enabled = true;
        }

        // remove selected intervals on delete buttin click
        private void deleteIntervalsClick(object sender, EventArgs e)
        {
            List<string> strlst = new List<string>();
            foreach (var item in checkedIntervals)
                strlst.Add(intervalAdapter.GetItem(item));

            foreach(var item in strlst)
                intervalAdapter.Remove(item);

            intervalAdapter.NotifyDataSetChanged();

            checkedIntervals.Clear();
            if (intervalAdapter.IsEmpty)
                FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime).Enabled = false;
        }

        // don't save anything return to perscriptions view
        private void cancelOnClick(object sender, EventArgs eventArgs)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

        // submit our data to the database, sinc to the watch and exit
        private void saveOnClick(object sender, EventArgs e)
        {
            // gather all data (excluding list data)
            string perscription = ((AutoCompleteTextView)FindViewById(Resource.Id.autoCompletePerscription)).Text;
            string startDate = ((EditText)FindViewById(Resource.Id.perscriptionStartTime)).Text;
            string endDate = ((EditText)FindViewById(Resource.Id.perscriptionEndTime)).Text;
            string dosage = ((EditText)FindViewById(Resource.Id.txtDosage)).Text;
            string form = (string)((Spinner)FindViewById(Resource.Id.formselect)).SelectedItem;
            string takewith = (string)((Spinner)FindViewById(Resource.Id.instructionselect)).SelectedItem;
            string Interval = (string)((Spinner)FindViewById(Resource.Id.intervalselect)).SelectedItem;

            // validate data (excluding list data) already validated
            Regex r = new Regex("^[0-9][0-9]/(0[0-9])|(1[0-2])/([0-2][0-9])|(3[0-1])");
            bool error_occured = false;
            StringBuilder error = new StringBuilder("Incorrect/missing inputs for ");
            if (!r.IsMatch(startDate)) {
                error_occured = true;
                error.AppendFormat("{0} ", Resource.String.perscription_start);
            }
            if (endDate.Length > 0 && !r.IsMatch(endDate))
            {
                error_occured = true;
                error.AppendFormat("{0} ", Resource.String.perscription_end);
            }
            if (perscription.Length == 0)
            {
                error_occured = true;
                error.Append("Perscription ");
            }
            if (dosage.Length == 0 || decimal.Parse(dosage) == 0)
            {
                error_occured = true;
                error.Append("Dosage ");
            }
            if (intervalAdapter.Count == 0) {
                error_occured = true;
                error.Append("Intervals ");
            }

            if (error_occured) {
                Toast tost = Toast.MakeText(this, error.ToString(), ToastLength.Long);
                tost.Show();
                return;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw)) {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("user");
                writer.WriteValue("temp@gmail.com");// temporary until google authentication complete
                writer.WritePropertyName("perscription");
                writer.WriteValue(perscription);
                writer.WritePropertyName("start_date");
                writer.WriteValue(startDate);
                writer.WritePropertyName("end_date");
                writer.WriteValue(endDate);
                writer.WritePropertyName("dosage");
                writer.WriteValue(dosage);
                writer.WritePropertyName("form");
                writer.WriteValue(form);
                writer.WritePropertyName("take_with");
                writer.WriteValue(takewith);
                writer.WritePropertyName("Interval");
                writer.WriteValue(Interval);
                writer.WritePropertyName("Intervals");
                writer.WriteStartArray();
                for (int x = 0; x < intervalAdapter.Count; x++) {
                    writer.WriteValue(intervalAdapter.GetItem(x));
                }
                writer.WriteEnd();
                writer.WriteEndObject();
            }

            // nowhere to send to yet
            //sendRequest(sb.ToString());

            // exit
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

        private async void sendRequest(string str)
        {
            HttpClient httpclient = new HttpClient();

            httpclient.BaseAddress = new Uri("http://web.uvic.ca/~rsaujla:8080"); // wherever the server will 

            // get content and post request
            var content = new StringContent(str, System.Text.Encoding.UTF8, "application/json");
            var result = await httpclient.PostAsync("/test", content);
            string resultContent = await result.Content.ReadAsStringAsync();
        }
    }
}