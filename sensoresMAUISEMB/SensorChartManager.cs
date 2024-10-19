using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace sensoresMAUISEMB
{
    public class SensorChartManager
    {
        public ObservableCollection<ChartDataPoint> XAxisData { get; set; }
        public ObservableCollection<ChartDataPoint> YAxisData { get; set; }
        public ObservableCollection<ChartDataPoint> ZAxisData { get; set; }
        private int maxPoints = 70;
        private int timeCounter = 0;

        public SensorChartManager(SfCartesianChart sensorChart)
        {
            XAxisData = new ObservableCollection<ChartDataPoint>();
            YAxisData = new ObservableCollection<ChartDataPoint>();
            ZAxisData = new ObservableCollection<ChartDataPoint>();

            // Configure the chart series
            sensorChart.Series[0].ItemsSource = XAxisData;
            sensorChart.Series[1].ItemsSource = YAxisData;
            sensorChart.Series[2].ItemsSource = ZAxisData;
        }

        public void AddAccelerometerReading(double x, double y, double z)
        {
            timeCounter++;

            XAxisData.Add(new ChartDataPoint(timeCounter, x));
            YAxisData.Add(new ChartDataPoint(timeCounter, y));
            ZAxisData.Add(new ChartDataPoint(timeCounter, z));

            if (XAxisData.Count > maxPoints) XAxisData.RemoveAt(0);
            if (YAxisData.Count > maxPoints) YAxisData.RemoveAt(0);
            if (ZAxisData.Count > maxPoints) ZAxisData.RemoveAt(0);
        }
    }
    public class ChartDataPoint
    {
        public int Time { get; set; }
        public double Value { get; set; }

        public ChartDataPoint(int time, double value)
        {
            Time = time;
            Value = value;
        }
    }
}
