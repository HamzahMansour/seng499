using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DrWatch_android
{
    public class PrescriptionsFragment : Android.Support.V4.App.Fragment, View.IOnClickListener
    {
        private RecyclerView _recycler;
        private RecyclerView.Adapter mAdapter;
        private RecyclerView.LayoutManager mLayoutManager;
        public PerscriptionListing perscription;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static PrescriptionsFragment NewInstance()
        {
            var prescriptionsFragment = new PrescriptionsFragment { Arguments = new Bundle() };
            return prescriptionsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var v = inflater.Inflate(Resource.Layout.prescriptions, container, false);

            Button addPerscriptionButton = (Button)v.FindViewById(Resource.Id.plusbtn);

            addPerscriptionButton.SetOnClickListener(this);

            _recycler = (RecyclerView)v.FindViewById(Resource.Id.recyclerViewPerscription);

            perscription = new PerscriptionListing(((MainActivity) Activity).getPerscript());

            // use a linear layout manager
            mLayoutManager = new LinearLayoutManager(this.Context);
            _recycler.SetLayoutManager(mLayoutManager);

            mAdapter = new PerscriptionsAdapter(perscription);
            _recycler.SetAdapter(mAdapter);

            return v;
        }

        private static int req = 0;
        private void Perscription(object sender, EventArgs eventArgs)
        {
            Intent myIntent = new Intent(Context, typeof(PerscriptionActivity));
            // myIntent.PutExtra("key", value)
            this.StartActivityForResult(myIntent, req);
        }

        public void activityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            if (resultCode == Result.Ok)
            {
                var tmp = new Perscription[((MainActivity)Activity).GetPerscriptSize()];
                Array.Copy(((MainActivity)Activity).getPerscript(), tmp, tmp.Length);
                perscription = new PerscriptionListing(tmp);
                mAdapter = new PerscriptionsAdapter(perscription);
                _recycler.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            }
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.plusbtn:
                    Perscription(v.Context, EventArgs.Empty);
                    break;
            }
        }
    }
    //----------------------------------------------------------------------
    // VIEW HOLDER

    // Implement the ViewHolder pattern: each ViewHolder holds references
    // to the UI components (ImageView and TextView) within the CardView 
    // that is displayed in a row of the RecyclerView:
    public class PerscriptionViewHolder : RecyclerView.ViewHolder
    {
        public ImageView form { get; private set; }
        public TextView title { get; private set; }
        public TextView start { get; private set; }
        public TextView end { get; private set; }
        public Spinner schedule { get; private set; }
        public ImageView take { get; private set; }

        public PerscriptionViewHolder(View itemView, Action<int> lisener) : base(itemView) {
            form = itemView.FindViewById<ImageView>(Resource.Id.formView);
            take = itemView.FindViewById<ImageView>(Resource.Id.takeView);
            title = itemView.FindViewById<TextView>(Resource.Id.textViewTitle);
            start = itemView.FindViewById<TextView>(Resource.Id.textViewStart);
            end = itemView.FindViewById<TextView>(Resource.Id.textViewEnd);
            schedule = itemView.FindViewById<Spinner>(Resource.Id.scheduleSpinner);

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
        // Event handler for Item clicks:
        public event EventHandler<int> ItemClick;

        //Underlying data set (a photo album):
        public PerscriptionListing _PListing;

        // Load the adapter with the data set (perscriptions) at construction time:
        public PerscriptionsAdapter(PerscriptionListing p)
        {
            _PListing = p;
        }

        Context context;

        // Create a new layout
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            context = parent.Context;
            // inflate the view for the perscription
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.PerscriptionListingLayout, parent, false);

            // create a fiew holder to find and hold the views references and
            //Registe onclick with the holder
            PerscriptionViewHolder vh = new PerscriptionViewHolder(itemView, OnClick);
            return vh;
        }

        // Fill in the contents of the listing
        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PerscriptionViewHolder vh = holder as PerscriptionViewHolder;

            vh.form.SetImageResource(_PListing[position].formID);
            vh.take.SetImageResource(_PListing[position].takeID);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(context, Resource.Layout.support_simple_spinner_dropdown_item, _PListing[position].schedule);
            vh.schedule.Adapter = adapter;
            vh.title.Text = _PListing[position].perscription;
            vh.start.Text = _PListing[position].start;
            vh.end.Text = _PListing[position].end;

        }

        // return the number of perscriptions
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