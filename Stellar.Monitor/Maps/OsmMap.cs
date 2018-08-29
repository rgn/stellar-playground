using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Stellar.Monitor.Maps
{
    public class OsmMap : Map
    {
        private Dictionary<string, Feature> featureMap = new Dictionary<string, Feature>();        
        private MemoryLayer offerLayer;
        private MemoryLayer locationLayer;
        private MemoryProvider offerMemoryProvider;
        private IStyle trivadisStyle;
        private IStyle offerBitmapStyle;

        public OsmMap()
            : base()
        {
            CRS = "EPSG:3857";
            Transformation = new MinimalTransformation();

            Layers.Add(OpenStreetMap.CreateTileLayer());
            Layers.Add(CreateLocationLayer(this.Envelope));
            Layers.Add(CreateOfferLayer(this.Envelope));            
            Home = n => n.NavigateTo(this.Layers[1].Envelope.Centroid, this.Resolutions[15]);            
        }

        private ILayer CreateLocationLayer(BoundingBox envelope)
        {
            trivadisStyle = CreateTrivadisStyle();

            var locationMemoryProvider = new MemoryProvider(DefaultLocationFeatures());
            locationLayer = new MemoryLayer
            {
                Name = "Locations",
                DataSource = locationMemoryProvider
            };

            return locationLayer;
        }

        private ILayer CreateOfferLayer(BoundingBox envelope)
        {            
            offerBitmapStyle = CreateOfferStyle();

            offerMemoryProvider = new MemoryProvider(DefaultLocationFeatures());
            offerLayer = new MemoryLayer
            {
                Name = "Offers",
                DataSource = offerMemoryProvider,
                Style = offerBitmapStyle 
            };

            return offerLayer;
        }

        private IEnumerable<IFeature> DefaultLocationFeatures()
        {
            var defaultLocations = new[]
            {
                new { name = "Trivadis GmbH - Stuttgart", longitude = 9.113017, latitude = 48.724974, offers = 0, style = trivadisStyle }
            };

            return defaultLocations.Select(x =>
            {
                var f = new Feature();

                f.Geometry = SphericalMercator.FromLonLat(x.longitude, x.latitude);
                f.Styles.Add(x.style);
                f["name"] = x.name;
                f["offers"] = 0;

                return f;
            });
        }

        private IEnumerable<IFeature> DefaultOfferFeatures()
        {
            var defaultLocations = new[]
            {
                new { name = "IceCream", longitude = 9.034618, latitude = 48.741921, offers = 0, style = trivadisStyle }
            };

            return defaultLocations.Select(x =>
            {
                var f = new Feature();

                f.Geometry = SphericalMercator.FromLonLat(x.longitude, x.latitude);
                f["name"] = x.name;
                f["offers"] = 0;

                return f;
            });
        }

        private IStyle CreateTrivadisStyle()
        {
            var path = "Stellar.Monitor.Resources.trivadis.png";
            var bitmapId = GetBitmapIdForEmbeddedResource(path);
            var bitmapHeight = 176;

            return CreateBitmapStyle(bitmapId, bitmapHeight, 0.2);
        }

        private IStyle CreateOfferStyle()
        {
            var path = "Stellar.Monitor.Resources.offer.png";
            var bitmapId = GetBitmapIdForEmbeddedResource(path);
            var bitmapHeight = 462;

            return CreateBitmapStyle(bitmapId, bitmapHeight, 0.2);
        }

        private IStyle CreateOfferLabelStyle(string text)
        {
            return new LabelStyle
            {
                Text = text,
                BackColor = new Brush(Color.Transparent),
                ForeColor = Color.Black
            };
        }

        private SymbolStyle CreateBitmapStyle(int bitmapId, int bitmapHeight, double symbolScale = 0.2)
        {
            return new SymbolStyle
            {                
                BitmapId = bitmapId,
                SymbolScale = symbolScale,
                SymbolOffset = new Offset(0, bitmapHeight * 0.5)
            };
        }

        private int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var assembly = typeof(OsmMap).GetTypeInfo().Assembly;
            var image = assembly.GetManifestResourceStream(imagePath);
            return BitmapRegistry.Instance.Register(image);
        }

        public void AddOffer(string name, double longitude, double latitude)
        {
            if (!featureMap.ContainsKey(name))
            {
                var polygon = new Polygon(new LinearRing());
                var point = SphericalMercator.FromLonLat(longitude, latitude);

                var feature = new Feature();

                feature.Geometry = point;
                feature["name"] = name;
                feature["offers"] = 0;
                feature.Styles.Add(CreateOfferLabelStyle(name));
                
                var features = new List<IFeature>(offerMemoryProvider.Features);
                features.Add(feature);
                offerMemoryProvider.ReplaceFeatures(features);                
                featureMap.Add(name, feature);
            }

            offerLayer.RefreshData(offerLayer.Envelope, 1, true);
        }

        public void RemoveOffer(string name)
        {
            if (featureMap.ContainsKey(name))
            {
                featureMap.Remove(name);

                offerLayer.RefreshData(offerLayer.Envelope, 1, true);
            }
        }

        //public CreateMap()
        //{
        //    var map = new Map
        //    {
        //        CRS = "EPSG:3857",
        //        Transformation = new MinimalTransformation()
        //    };
        //    map.Layers.Add(OpenStreetMap.CreateTileLayer());
        //    map.Widgets.Add(new ScaleBarWidget(map) { TextAlignment = Mapsui.Widgets.Alignment.Center, HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center, VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top });
        //    map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget { MarginX = 20, MarginY = 40 });
        //    return map;
        //}
    }
}
