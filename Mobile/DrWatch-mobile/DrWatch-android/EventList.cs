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

namespace DrWatch_android
{
    public class Dosage
    {
        public string _Unit;
        public int _Amount;

        public string Unit { get { return _Unit; } }
        public int Amount { get { return _Amount; } }
    }

    public class Event
    {
        public string _EventTime;
        public string _EventPrescription;
        public Dosage _EventDosage;

        public string EventTime { get { return _EventTime; } }
        public string EventPrescription { get { return _EventPrescription; } }
        public Dosage EventDosage { get { return _EventDosage; } }
    }

    public class EventList
    {
        static Event[] _TestEvents = {
            new Event {_EventTime=DateTime.Now.AddMinutes(10).ToLongTimeString(), _EventPrescription = "Acetaminophin 5mg", _EventDosage = new Dosage { _Amount = 3, _Unit = null} },
            new Event {_EventTime=DateTime.Now.AddMinutes(20).ToLongTimeString(), _EventPrescription = "Acetaminophin 5mg", _EventDosage = new Dosage { _Amount = 1, _Unit = null} },
            new Event {_EventTime=DateTime.Now.AddMinutes(30).ToLongTimeString(), _EventPrescription = "Acetaminophin 5mg", _EventDosage = new Dosage { _Amount = 3, _Unit = null} },
        };

        private Event[] _Events;

        public EventList() { _Events = _TestEvents; }

        public int numEvents { get { return _Events.Length; } }

        public Event this[int i] { get { return _Events[i]; } }

    }
}