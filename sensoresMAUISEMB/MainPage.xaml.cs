using Microsoft.Maui.Controls;

namespace sensoresMAUISEMB
{
    public partial class MainPage : ContentPage
    {
        SensorManager sensorManager;

        public MainPage()
        {
            InitializeComponent();
            sensorManager = new SensorManager(AccelLabel, BarometerLabel, CompassLabel, GyroscopeLabel, MagnetometerLabel, OrientationLabel);
        }

        private void OnToggleAccelerometerClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleAccelerometer();
        }

        private void OnToggleBarometerClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleBarometer();
        }

        private void OnToggleCompassClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleCompass();
        }

        private void OnToggleGyroscopeClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleGyroscope();
        }

        private void OnToggleMagnetometerClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleMagnetometer();
        }

        private void OnToggleOrientationClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleOrientation();
        }

        private void OnToggleShakeClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleShake();
        }
    }
}
