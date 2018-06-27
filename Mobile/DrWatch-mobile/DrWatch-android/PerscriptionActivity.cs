using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        ArrayAdapter<string> adapter;
        ListView ListIntervals;
        Spinner intervalSelection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NewEditPerscription);


            //intent intent = getIntent();
            //String value = intent.getStringExtra("key") to receive something from activity start


            Button cancel = FindViewById<Button>(Resource.Id.btnPerscriptionCancel);
            cancel.Click += cancelOnClick;

            Button save = FindViewById<Button>(Resource.Id.btnPerscriptionSave);
            save.Click += cancelOnClick;
            // Create your application here

            // needed for interval addition
            intervalSelection = FindViewById<Spinner>(Resource.Id.intervalselect);
            Button addIntervals = FindViewById<Button>(Resource.Id.btnPerscriptionTime);
            ListIntervals = FindViewById<ListView>(Resource.Id.add_intervals);
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lisIntervals);
            ListIntervals.Adapter = adapter;
            Button deleteIntervals = FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime);
            
            intervalSelection.ItemSelected += (sender, args) =>
            {
                if ((string)((Spinner)sender).SelectedItem == "Daily")
                    FindViewById<EditText>(Resource.Id.txtIntervalDate).Visibility = ViewStates.Gone;
                else
                    FindViewById<EditText>(Resource.Id.txtIntervalDate).Visibility = ViewStates.Visible;
                adapter.Clear();
                adapter.NotifyDataSetChanged();
            };
            addIntervals.Click += addIntervalClick;
            deleteIntervals.Click += deleteIntervalsClick;
        }

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
            tmp.AppendFormat("Start {0} End {0}", start.EditableText.ToString(), end.EditableText.ToString());

            adapter.Add(tmp.ToString());
            adapter.NotifyDataSetChanged();
        }

        private void deleteIntervalsClick(object sender, EventArgs e)
        {
            if (ListIntervals.Selected == false) return;
            var lst = ListIntervals.GetCheckedItemIds();


        }

        private void addDateStartClick(object sender, EventArgs e)
        {
            DatePickerDialog dial = new DatePickerDialog(this);
            dial.Show();
        }

        private void cancelOnClick(object sender, EventArgs eventArgs)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }
    }
}