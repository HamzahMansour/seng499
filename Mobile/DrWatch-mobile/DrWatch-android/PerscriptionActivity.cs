using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
            adapter = new ArrayAdapter<string>(this,Resource.Id.add_intervals,lisIntervals);
            ListIntervals.SetAdapter(adapter);
            Button deleteIntervals = FindViewById<Button>(Resource.Id.btnDeleteSelectedPerscriptionTime);
            
            addIntervals.Click += addIntervalClick;
            deleteIntervals.Click += deleteIntervalsClick;
        }

        private void addIntervalClick(object sender, EventArgs e)
        {
            if (intervalSelection.SelectedItem == null) return;

            if ((string)intervalSelection.SelectedItem == "Daily") {
        }
            else {

            }
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