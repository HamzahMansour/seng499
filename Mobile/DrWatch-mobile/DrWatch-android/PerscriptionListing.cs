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
using Newtonsoft.Json;

namespace DrWatch_android
{
    public class PerscriptionDeseriaized
    {
        /* writer.WritePropertyName("perscription");
                 writer.WriteValue(perscription);
                 writer.WritePropertyName("start_date");
                 writer.WriteValue(startDate);
                 writer.WritePropertyName("end_date");
                 writer.WriteValue(endDate);
                 writer.WritePropertyName("dosage");
                 writer.WriteValue(dosage);
                 writer.WritePropertyName("form");
                 writer.WriteValue(form);
                 writer.WritePropertyName("take_with");
                 writer.WriteValue(takewith);
                 writer.WritePropertyName("Interval");
                 writer.WriteValue(Interval);
                 writer.WritePropertyName("Intervals");
                 writer.WriteStartArray();*/

        [JsonProperty("perscription")]
        public string medication { get; set; }

        [JsonProperty("start_date")]
        public string startDate { get; set; }

        [JsonProperty("end_date")]
        public string endDate { get; set; }

        [JsonProperty("dosage")]
        public string dosage { get; set; }

        [JsonProperty("take_with")]
        public string takeWith { get; set; }

        [JsonProperty("Interval")]
        public string interval { get; set; }

        [JsonProperty("Intervals")]
        public string[] intervals { get; set; }

        [JsonProperty("form")]
        public string form { get; set; }
    }
    public class Perscription
    {

        // schedule listing
        public List<string> _schedule;

        // perscription name
        public string _perscription;

        // dosage
        public string _dosage;

        // take with
        public int _takeID;

        // form
        public int _formID;

        //start and end of perscription
        public string _start;

        public string _end = "";

        public string _interval;

        public List<string> schedule { get { return _schedule; } }

        public string perscription { get { return _perscription; } }

        public int takeID { get { return _takeID; } }

        public int formID { get { return _formID; } }

        public string start { get { return _start; } }

        public string end { get { return _end; } }

        public string interval { get { return _interval; } }

    }
    public class PerscriptionListing
    {
        private Perscription[] _perscriptions;

        public static Perscription[] _TestPerscriptions =  {
            new Perscription { _formID = Resource.Drawable.pills,
                                _perscription = "acetaminofin", _dosage = "2.0",
                                _takeID = Resource.Drawable.none, _interval = "Daily",
                                _start = "18/07/13", _schedule = new List<string>(new string [] {"06:00 09:00","13:00 18:00" })
            },
            new Perscription { _formID = Resource.Drawable.capsule,
                                _perscription = "tylenol", _dosage = "100.0",
                                _takeID = Resource.Drawable.water, _interval = "Monthly",
                                _start = "18/07/13", _schedule = new List<string>(new string [] {"05/19 06:00 18:00"})
            }
        };

        // testing purposes only
        public PerscriptionListing() {
            _perscriptions = _TestPerscriptions;
        }

        public int numPerscriptions { get { return _perscriptions.Length; } }

        // initialize list of perscriptions
        public PerscriptionListing(Perscription[] pers) {
            _perscriptions = pers;
        }

        public Perscription this[int i] { get { return _perscriptions[i]; } }
    }
}