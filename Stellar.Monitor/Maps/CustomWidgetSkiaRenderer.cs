using Mapsui;
using Mapsui.Geometries;
using Mapsui.Rendering.Skia;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Widgets;
using SkiaSharp;

using Mapsui.Styles;
using Mapsui.Widgets;

namespace Stellar.Monitor.Maps
{
    public class CustomWidget : IWidget
    {
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public float MarginX { get; set; } = 20;
        public float MarginY { get; set; } = 20;
        public BoundingBox Envelope { get; set; }
        public void HandleWidgetTouched(INavigator navigator, Point position)
        {
            navigator.NavigateTo(0, 0);
        }

        public Color Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class CustomWidgetSkiaRenderer : ISkiaWidgetRenderer
    {
        public void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IWidget widget, float layerOpacity)
        {
            // Cast to custom widget to be able to access the specific CustomWidget fields
            var customWidget = (CustomWidget)widget;

            // Update the envelope so the MapControl can do hit detection
            widget.Envelope = ToEnvelope(customWidget);

            // Use the envelope to draw
            canvas.DrawRect(widget.Envelope.ToSkia(), new SKPaint { Color = customWidget.Color.ToSkia(0.5f) });
        }

        private static BoundingBox ToEnvelope(CustomWidget customWidget)
        {
            // A better implementation would take into account widget alignment
            return new BoundingBox(customWidget.MarginX, customWidget.MarginY,
                customWidget.MarginX + customWidget.Width,
                customWidget.MarginY + customWidget.Height);
        }
    }
}
