using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using OxyPlot;
using OxyPlot.Xamarin.Android;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace DrWatch_android
{
    public class AnalyticsFragment : Android.Support.V4.App.Fragment
    {
        public PerscriptionListing prescriptions;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static AnalyticsFragment NewInstance()
        {
            var analyticsFragment = new AnalyticsFragment { Arguments = new Bundle() };
            return analyticsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var v = inflater.Inflate(Resource.Layout.analytics, container, false);           

            PlotView view = v.FindViewById<PlotView>(Resource.Id.plot_view);
            prescriptions = new PerscriptionListing(((MainActivity)Activity).getPerscript());
            DateTime[] dateTimesTaken = new DateTime[]
            {
                new DateTime(2018, 6, 30, 9, 0, 0),
                new DateTime(2018, 7, 30, 11, 0, 0)
            };

            //view.Model = CreatePlotModel(prescriptions, dateTimesTaken);
            view.Model = CreatePlotModel();

            return v;
        }

        private PlotModel CreatePlotModel()
        {
            //Create hard coded plot model
            var model = new PlotModel
            {
                Title = "Medication Analytics",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
            };

            var s1 = new BarSeries { Title = "Taken on time", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            s1.Items.Add(new BarItem { Value = 25 });
            s1.Items.Add(new BarItem { Value = 15 });
            s1.Items.Add(new BarItem { Value = 18 });
            s1.Items.Add(new BarItem { Value = 30 });

            var s2 = new BarSeries { Title = "Taken late", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            s2.Items.Add(new BarItem { Value = 1 });
            s2.Items.Add(new BarItem { Value = 5 });
            s2.Items.Add(new BarItem { Value = 9 });
            s2.Items.Add(new BarItem { Value = 6 });

            var s3 = new BarSeries { Title = "Not taken", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            s3.Items.Add(new BarItem { Value = 3 });
            s3.Items.Add(new BarItem { Value = 4 });
            s3.Items.Add(new BarItem { Value = 9 });
            s3.Items.Add(new BarItem { Value = 4 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Medicine A");
            categoryAxis.Labels.Add("Medicine B");
            categoryAxis.Labels.Add("Medicine C");
            categoryAxis.Labels.Add("Medicine D");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);


            return model;
        }

        private PlotModel CreatePlotModel(PerscriptionListing prescriptions, DateTime[] DateTimesTaken)
        {
            //Create dynamic plot model from passed prescriptions
            var model = new PlotModel
            {
                Title = "Medication Analytics",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
            };

            var s1 = new BarSeries { Title = "Taken on time", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            for(int i = 0; i < prescriptions.numPerscriptions; i++)
            {
                List<string> IntervalsInTimePeriod = prescriptions[i].schedule;
                var StartDate = prescriptions[i].start;
                string IntervalType = prescriptions[i].interval;
                int TakenCount = CalculateTaken(IntervalsInTimePeriod, StartDate, DateTimesTaken, IntervalType);
                s1.Items.Add(new BarItem { Value = TakenCount });
            }

            var s2 = new BarSeries { Title = "Taken late", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            s2.Items.Add(new BarItem { Value = 1 });
            s2.Items.Add(new BarItem { Value = 5 });

            var s3 = new BarSeries { Title = "Not taken", StrokeColor = OxyColors.Black, StrokeThickness = 1, BarWidth = 10, LabelPlacement = LabelPlacement.Outside, LabelMargin = 3, LabelFormatString = "{0:}" };
            s3.Items.Add(new BarItem { Value = 3 });
            s3.Items.Add(new BarItem { Value = 4 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Medicine A");
            categoryAxis.Labels.Add("Medicine B");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);


            return model;
        }

        private int CalculateTaken(List<string> IntervalsInTimePeriod, string StartDate, DateTime[] DateTimesTaken, string IntervalType)
        {
            List<DateTime> AcceptableIntervals = PopulateOnTimeIntervals(IntervalsInTimePeriod, StartDate, IntervalType);

            return 2;
        }

        private List<DateTime> PopulateOnTimeIntervals(List<string> IntervalsInTimePeriod, string StartDate, string IntervalType)
        {
            if (IntervalType.Equals("Daily"))
            {
                DateTime StartDateAsDateTime = Convert.ToDateTime(StartDate);
                int DaysElapsed = (DateTime.Now - StartDateAsDateTime).Days;
                List<string> AcceptableIntervals = new List<string>();

                for (int i = 0; i < DaysElapsed; i++)
                {

                }
                return new List<DateTime>();
            }
            else
            {
                //Hardcoded value, not implemented yet
                return new List<DateTime>();
            }
        }
    }
}