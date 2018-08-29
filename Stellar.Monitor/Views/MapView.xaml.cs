using Mapsui.UI;
using Mapsui.UI.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Stellar.Monitor.Maps;

namespace Stellar.Monitor.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();

            //MapControl.FeatureInfo += MapControlFeatureInfo;
            //MapControl.MouseMove += MapControlOnMouseMove;
            MapControl.RenderMode = RenderMode.Skia;
            MapControl.RotationLock = true;
            MapControl.UnSnapRotationDegrees = 30;
            MapControl.ReSnapRotationDegrees = 5;
            MapControl.Renderer.WidgetRenders[typeof(CustomWidget)] = new CustomWidgetSkiaRenderer();

            var osmMap = new OsmMap();
            MapControl.Map = osmMap;

            osmMap.AddOffer("Trivadis GmbH - Stuttgart", 48.724974, 9.113017);

            //MapControl.Refresh();

        }

        private void MapControlFeatureInfo(object sender, FeatureInfoEventArgs e)
        {
            //MessageBox.Show(e.FeatureInfo.ToDisplayText());
        }

        private void MapControlOnMouseMove(object sender, MouseEventArgs e)
        {
            //var screenPosition = e.GetPosition(MapControl);
            //var worldPosition = MapControl.Viewport.ScreenToWorld(screenPosition.X, screenPosition.Y);
            //MouseCoordinates.Text = $"{worldPosition.X:F0}, {worldPosition.Y:F0}";
        }

        private void RotationSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //var percent = RotationSlider.Value / (RotationSlider.Maximum - RotationSlider.Minimum);
            //MapControl.Navigator.RotateTo(percent * 360);
            MapControl.Refresh();
        }
    }
}
