using Caliburn.Micro;
using Mapsui;
using Stellar.Monitor.Maps;
using System.ComponentModel.Composition;

namespace Stellar.Monitor.ViewModels
{
    [Export(typeof(MapViewModel))]
    public class MapViewModel : Screen
    {
        private Map map;
        public Map Map { get => map; set => map = value; }

        public MapViewModel()
        {         
        }
    }
}
