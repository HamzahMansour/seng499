using System;
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
        private Android.Support.V4.App.Fragment scheduleFragment = ScheduleFragment.NewInstance();
        private Android.Support.V4.App.Fragment prescriptionsFragment = PrescriptionsFragment.NewInstance();
        private Android.Support.V4.App.Fragment analyticsFragment = AnalyticsFragment.NewInstance();
        private Android.Support.V4.App.Fragment settingsFragment = SettingsFragment.NewInstance();

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.activity_main);

            _bottomBar = BottomBar.Attach(this, savedInstanceState);
            _bottomBar.SetItems(Resource.Menu.menu_bottombar);
            _bottomBar.SetOnMenuTabClickListener(this);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

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
            LoadFragment(menuItemId);
        }

        public void OnMenuTabReSelected(int menuItemId)
        {
            // not implemented yet :/
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


        private void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case (Resource.Id.bottomBarSchedule): //load Schedule fragment
                    fragment = scheduleFragment;
                    break;
                case (Resource.Id.bottomBarPrescriptions): //load Prescriptions fragment
                    fragment = prescriptionsFragment;
                    break;
                case (Resource.Id.bottomBarAnalytics): //load Analytics fragment
                    fragment = analyticsFragment;
                    break;
                case (Resource.Id.bottomBarSettings): //load Settings fragment
                    fragment = settingsFragment;
                    break;
            }

            if (fragment == null)
                return;

            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.content_main, fragment)
                .Commit();
        }
    }

}

