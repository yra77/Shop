

using Android.Gms.Maps;
using PlatformView = Android.Gms.Maps.MapView;


namespace Shop.Platforms.Android.Services.Map
{
    public interface IMapHandler : IViewHandler
    {
        new Shop.Controls.Map.IMap VirtualView { get; }
        new PlatformView PlatformView { get; }
        GoogleMap Map { get; set; }
    }
}
