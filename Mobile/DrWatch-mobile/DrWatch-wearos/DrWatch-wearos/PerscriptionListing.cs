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

namespace DrWatch_wearos
{
    public class Perscription
    {
        // image for perscription
        public int _labelID;

        // datetime for perscription
        public string _datetime;

        // perscription info
        public string _perscription;

        public int labelID { get { return _labelID;  } }

        public string datetime { get { return _datetime; } }

        public string perscription { get { return _perscription;  } }
    }
    public class PerscriptionListing
    {
        private Perscription[] _perscriptions;

        static Perscription[] _TestPerscriptions = {
                new Perscription { _labelID = Resource.Drawable.pills, _datetime = DateTime.Now.ToLongTimeString(), _perscription = "acetaminophin 5mg" }
        };

        // for testing purposes only
        public PerscriptionListing() {
            _perscriptions = _TestPerscriptions;
        }

        public int numPerscriptions { get { return _perscriptions.Length;  } }

        //initialize list of perscriptions
        public PerscriptionListing(Perscription[] pers) {
            _perscriptions = pers;
        }

        public Perscription this[int i] { get { return _perscriptions[i]; } }
    }
}