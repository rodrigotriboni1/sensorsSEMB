using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace sensoresMAUISEMB
{
    public class SensorChartManager
    {
        public ObservableCollection<ChartDataPoint> XAxisData { get; set; }
        public ObservableCollection<ChartDataPoint> YAxisData { get; set; }
        public ObservableCollection<ChartDataPoint> ZAxisData { get; set; }
        readonly private int maxPoints = 70;
        private int timeCounter = 0;

        public SensorChartManager(SfCartesianChart sensorChart)
        {
            XAxisData = [];
            YAxisData = [];
            ZAxisData = [];

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
    public class ChartDataPoint(int time, double value)
    {
        public int Time { get; set; } = time;
        public double Value { get; set; } = value;
    }
}
