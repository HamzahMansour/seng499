using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Content;
using BottomNavigationBar;
using Xamarin.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using Android.Runtime;

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

        private CredentialsService _credentialsService = new CredentialsService();

        public Perscription[] Perscript { get; private set; }
        // defaulting to test
        private int endposPerscript;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.activity_main);

            _bottomBar = BottomBar.Attach(this, savedInstanceState);
            _bottomBar.SetItems(Resource.Menu.menu_bottombar);
            _bottomBar.SetOnMenuTabClickListener(this);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            if (_credentialsService.DoCredentialsExist())
            {
                //Load stored credentials from AccountManager
                Account account = _credentialsService.LoadStoredCredentials();
            }
            else
            {
                //No credentials stored, user must log in
                var intent = new Intent(this, typeof(LogInActivity));
                StartActivity(intent);
                Finish();
            }
            
            if (Perscript == null)
            {
                Perscript = PerscriptionListing._TestPerscriptions;
                endposPerscript = Perscript.Length;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok) {
                if (data.Extras != null && data.Extras.ContainsKey("Perscription"))
                {
                    if (!data.Extras.Get("Perscription").ToString().Equals("cancel"))
                        addPerscription(data.Extras.Get("Perscription").ToString());

                    ((PrescriptionsFragment)prescriptionsFragment).activityResult(requestCode, resultCode, data);
                    //LoadFragment(Resource.Id.bottomBarPrescriptions);
                }
            }
        }

        public Perscription[] getPerscript() {
            return Perscript;
        }
        public int GetPerscriptSize() {
            return endposPerscript;
        }

        private void addPerscription(string v)
        {
            PerscriptionDeseriaized deserialized = JsonConvert.DeserializeObject<PerscriptionDeseriaized>(v);

            int idForm = 0;
            int idTake = 0;

            switch (deserialized.form) {
                case "Pill":
                    idForm = Resource.Drawable.pills;
                    break;
                case "Capsule":
                    idForm = Resource.Drawable.capsule;
                    break;
                case "Patch":
                    idForm = Resource.Drawable.patch;
                    break;
                case "Liquid":
                    idForm = Resource.Drawable.liquid;
                    break;
                case "Injection":
                    idForm = Resource.Drawable.needle;
                    break;
            }

            switch (deserialized.takeWith)
            {
                case "None":
                    idTake = Resource.Drawable.none;
                    break;
                case "Food":
                    idTake = Resource.Drawable.food;
                    break;
                case "Drink":
                    idTake = Resource.Drawable.water;
                    break;
                case "Food and Drink":
                    idTake = Resource.Drawable.foodwater;
                    break;
            }

            Perscription newP = new Perscription
            {
                _formID = idForm,
                _perscription = deserialized.medication,
                _dosage = deserialized.dosage,
                _takeID = idTake,
                _interval = deserialized.interval,
                _start = deserialized.startDate,
                _end = deserialized.endDate,
                _schedule = new List<string>(deserialized.intervals)
            };
            if (Perscript.Length == endposPerscript) {
                var arr = Perscript;
                Array.Resize(ref arr, arr.Length + 20);
                Perscript = arr;
            }
            Perscript[endposPerscript] = newP;
            ++endposPerscript;
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

