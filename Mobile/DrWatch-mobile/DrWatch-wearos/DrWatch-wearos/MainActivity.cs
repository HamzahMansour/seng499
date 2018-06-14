using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
using Java.Interop;
using System.Globalization;
using Android.Graphics;
using Android.Support.V7.Widget;
using Java.Util;

namespace DrWatch_wearos
{

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        private TextView _textView;
        private RecyclerView _recycler;
        private RecyclerView.Adapter mAdapter;
        private RecyclerView.LayoutManager mLayoutManager;
        PerscriptionListing perscription;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            _textView = FindViewById<TextView>(Resource.Id.text);
            _recycler = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            perscription = new PerscriptionListing();


            DateTime nowtime = DateTime.Now.AddMinutes(2.0);

            // use a linear layout manager
            mLayoutManager = new LinearLayoutManager(this);
            _recycler.SetLayoutManager(mLayoutManager);

            mAdapter = new PerscriptionsAdapter(perscription);
            _recycler.SetAdapter(mAdapter);

            SetAmbientEnabled();

            setAlarm(nowtime);

            // Register the item click handler(below) with the adapter:
            //mAdapter.ItemClick += OnItemClick;
        }

        //set our alarm with a datetime object
        private void setAlarm(DateTime calendar) {
            Color c = Color.Red;
            _textView.SetTextColor(c);
            
            // set my alarm (repeating function taken out)
            Intent intent = new Intent(this, typeof(AlarmReceiver));
           
            // add information to pending intent !!!
            DateTime tmp = calendar;
            tmp.AddMinutes(2.0);
            intent.PutExtra(Intent.ExtraText, tmp.ToLongTimeString());
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 14532, intent, 0);
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            
            DateTime dtBasis = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)(calendar.ToUniversalTime().Subtract(dtBasis).TotalMilliseconds), pendingIntent);


        }

    }

    //----------------------------------------------------------------------
    // VIEW HOLDER

    // Implement the ViewHolder pattern: each ViewHolder holds references
    // to the UI components (ImageView and TextView) within the CardView 
    // that is displayed in a row of the RecyclerView:
    public class PerscriptionViewHolder : RecyclerView.ViewHolder
    {
        public ImageView label { get; private set; }
        public TextView datetime { get; private set; }
        public TextView perscription { get; private set; }

        // Get references to the views defined in the CardView layout.
        public PerscriptionViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            // Locate and cache view references:
            label = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            datetime = itemView.FindViewById<TextView>(Resource.Id.textViewDate);
            perscription = itemView.FindViewById<TextView>(Resource.Id.textViewMed);

            // Detect user clicks on the item view and report which item
            // was clicked (by layout position) to the listener:
            //itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }

    //----------------------------------------------------------------------
    // ADAPTER

    // Adapter to connect the data set (photo album) to the RecyclerView: 
    public class PerscriptionsAdapter : RecyclerView.Adapter
    {
        // Event handler for item clicks:
        public event EventHandler<int> ItemClick;

        // Underlying data set (a photo album):
        public PerscriptionListing _PListing;

        // Load the adapter with the data set (photo album) at construction time:
        public PerscriptionsAdapter(PerscriptionListing p)
        {
            _PListing = p;
        }

        // Create a new photo CardView (invoked by the layout manager): 
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.PerscriptionScheduleView, parent, false);

            // Create a ViewHolder to find and hold these view references, and 
            // register OnClick with the view holder:
            PerscriptionViewHolder vh = new PerscriptionViewHolder(itemView, OnClick);
            return vh;
        }

        // Fill in the contents of the photo card (invoked by the layout manager):
        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PerscriptionViewHolder vh = holder as PerscriptionViewHolder;

            // Set the ImageView and TextView in this ViewHolder's CardView 
            // from this position in the photo album:
            vh.label.SetImageResource(_PListing[position].labelID);
            vh.datetime.Text = _PListing[position].datetime;
            vh.perscription.Text = _PListing[position].perscription;
        }

        // Return the number of photos available in the photo album:
        public override int ItemCount
        {
            get { return _PListing.numPerscriptions; }
        }

        // Raise an event when the item-click takes place:
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}


