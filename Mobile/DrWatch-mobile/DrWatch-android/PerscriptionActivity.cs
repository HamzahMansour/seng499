using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
            List<string> PerscriptionNames = GetPerscriptions();
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
        private List<string> GetPerscriptions()
        {
            throw new NotImplementedException();
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

            r = new Regex("^([0-1][0-9])|(2[0-3]):[0-5][0-9]");
            if (!r.IsMatch(start.EditableText.ToString()) | !r.IsMatch(end.EditableText.ToString()))
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

            HttpClient httpclient = new HttpClient();

            // exit
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }
    }
}