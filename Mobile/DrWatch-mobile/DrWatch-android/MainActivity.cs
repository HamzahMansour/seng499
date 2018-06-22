﻿using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Content;
using BottomNavigationBar;

namespace DrWatch_android
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener
    {
        private BottomBar _bottomBar;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

            _bottomBar = BottomBar.Attach(this, savedInstanceState);
            _bottomBar.SetItems(Resource.Menu.menu_bottombar);
            _bottomBar.SetOnMenuTabClickListener(this);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

			FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            Button btn = (Button)FindViewById(Resource.Id.APIQueryTestButton);
            btn.Click += delegate
            {
                var intent = new Intent(this, typeof(QueryAPI));
                StartActivity(intent);
            };

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            // Necessary to restore the BottomBar's state, otherwise we would
            // lose the current tab on orientation change.
            _bottomBar.OnSaveInstanceState(outState);
        }

        public void OnMenuTabSelected(int menuItemId)
        {
            switch (menuItemId)
            {
                case(Resource.Id.bottomBarSchedule): //load Schedule fragment
                    break;
                case (Resource.Id.bottomBarPrescriptions): //load Prescriptions fragment
                    break;
                case (Resource.Id.bottomBarAnalytics): //load Analytics fragment
                    break;
                case (Resource.Id.bottomBarSettings): //load Settings fragment
                    break;
                default:
                    break;

            }

        }

        public void OnMenuTabReSelected(int menuItemId)
        {
            throw new NotImplementedException();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

    }
}

