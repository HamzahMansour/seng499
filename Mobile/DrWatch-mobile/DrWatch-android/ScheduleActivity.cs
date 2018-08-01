using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace DrWatch_android
{
    [Activity(Label = "ScheduleActivity")]
    public class ScheduleActivity : Activity
    {
        private RecyclerView _RecyclerView; // RecyclerView instance that displays event list
        private RecyclerView.Adapter _Adapter; // Adapter that accesses the data set (event list)
        private RecyclerView.LayoutManager _LayoutManager; // Layout manager that lays out each card in the RecyclerView
        EventList _EventList; // Event list that is managed by the adapter

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _EventList = new EventList(); // Instantiate the event list 

            SetContentView(Resource.Layout.schedule); // Set the view from the "schedule" layout resource
            _RecyclerView = FindViewById<RecyclerView>(Resource.Id.scheduleRecyclerView); // Get the RecyclerView layout

            _LayoutManager = new LinearLayoutManager(this); // Use the built-in linear layout manager
            _RecyclerView.SetLayoutManager(_LayoutManager); // Plug the layout manager into the RecyclerView

            //Create an adapter for the RecyclerView, and pass it the data set (the photo album) to manage
            _Adapter = new EventListAdapter(_EventList);
            // _Adapter.ItemClick += OnItemClick; // Register the item click handler with the adapter
            _RecyclerView.SetAdapter(_Adapter); // Plug the adapter into the RecyclerView

            // Handler for the item click event
            void OnItemClick(object sender, int position)
            {
                // Display a toast that briefly shows the enumeration of the selected photo:
                int eventNum = position + 1;
                Toast.MakeText(this, "This is Event #" + eventNum, ToastLength.Short).Show();
            }
        }

        // View Holder
        public class ScheduleViewHolder : RecyclerView.ViewHolder
        {
            public TextView EventTime { get; private set; }
            public TextView EventPrescription { get; private set; }
            //public TextView EventDosage { get; private set; }

            // Get references to the views defined in the CardView layout.
            public ScheduleViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                // Locate and cache view references:
                EventTime = itemView.FindViewById<TextView>(Resource.Id.textViewStart);
                EventPrescription = itemView.FindViewById<TextView>(Resource.Id.textView1);
                
                // Detect user clicks on the item view and report which item was clicked (by layout position) to the listener
                // itemView.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }

        // Adapter
        // ADAPTER

        // Adapter to connect the data set (event list) to the RecyclerView: 
        public class EventListAdapter : RecyclerView.Adapter
        {
            // Event handler for item clicks:
            public event EventHandler<int> ItemClick;

            // Underlying data set (an event list):
            public EventList _EventList;

            // Load the adapter with the data set at construction time:
            public EventListAdapter(EventList eventList)
            {
                _EventList = eventList;
            }

            // Create a new photo CardView (invoked by the layout manager): 
            public override RecyclerView.ViewHolder
                OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                // Inflate the CardView for the event:
                View itemView = LayoutInflater.From(parent.Context).
                            Inflate(Resource.Layout.EventListView, parent, false);

                // Create a ViewHolder to find and hold these view references, and 
                // register OnClick with the view holder:
                ScheduleViewHolder vh = new ScheduleViewHolder(itemView, OnClick);
                return vh;
            }

            // Fill in the contents of the event card (invoked by the layout manager):
            public override void
                OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                ScheduleViewHolder vh = holder as ScheduleViewHolder;

                // Set the TextView in this ViewHolder's CardView 
                vh.EventTime.Text = _EventList[position].EventTime;
            }

            // Return the number of events available in the list:
            public override int ItemCount
            {
                get { return _EventList.numEvents; }
            }

            // Raise an event when the item-click takes place:
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }
        }

    }
}