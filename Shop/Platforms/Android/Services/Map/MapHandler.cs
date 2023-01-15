

using Shop.Enums;
using IMap = Shop.Controls.Map.IMap;

using Android.Gms.Maps;
using PlatformView = Android.Gms.Maps.MapView;
using Android.Gms.Maps.Model;

using AndroidX.Core.Content;
using Android.Content.PM;
using Android;

using System.Diagnostics;
using Microsoft.Maui.Handlers;


namespace Shop.Platforms.Android.Services.Map
{
    public class MapHandler : ViewHandler<IMap, MapView>, IMapHandler
    {
        MapView _mapView;
        MapCallbackHandler _mapReady;


        //IMap interface
        public GoogleMap Map { get; set; }

        public static IPropertyMapper<IMap, IMapHandler> Mapper =
            new PropertyMapper<IMap, IMapHandler>(ViewHandler.ViewMapper)
            {
            [nameof(Shop.Controls.Map.IMap.MapType)] = MapMapType,
            [nameof(Shop.Controls.Map.IMap.IsShowingUser)] = MapIsShowingUser,
            [nameof(Shop.Controls.Map.IMap.Address)] = SetAddress,
            };


        IMap IMapHandler.VirtualView => VirtualView;
        PlatformView IMapHandler.PlatformView => PlatformView;

        public MapHandler() : base(Mapper)
        {      
        }
        ///////////////IMAP

        protected override MapView CreatePlatformView()
        {
            _mapView = new MapView(Context);
            _mapView.OnCreate(null);
            _mapView.OnResume();
            return _mapView;
        }

        protected override void ConnectHandler(MapView platformView)
        {
            base.ConnectHandler(platformView);

            _mapReady = new MapCallbackHandler(this);
            platformView.GetMapAsync(_mapReady);
        }

        protected override void DisconnectHandler(MapView platformView)
        {
            _mapReady = null;
            _mapView?.Dispose();

            base.DisconnectHandler(platformView);
        }

        public static void MapMapType(IMapHandler handler, IMap map)
        {
            GoogleMap googleMap = handler?.Map;
            if (googleMap == null)
                return;

            googleMap.MapType = map.MapType switch
            {
                MapType.Street => GoogleMap.MapTypeNormal,
                MapType.Satellite => GoogleMap.MapTypeSatellite,
                MapType.Hybrid => GoogleMap.MapTypeHybrid,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void MapIsShowingUser(IMapHandler handler, IMap map)
        {
            GoogleMap googleMap = handler?.Map;
          
            if (googleMap == null)
                return;

            if (handler?.MauiContext?.Context == null)
                return;


            if (map.IsShowingUser)
            {
                var coarseLocationPermission = ContextCompat.CheckSelfPermission(handler.MauiContext.Context,
                                                   Manifest.Permission.AccessCoarseLocation);
                var fineLocationPermission = ContextCompat.CheckSelfPermission(handler.MauiContext.Context,
                                                   Manifest.Permission.AccessFineLocation);

                if (coarseLocationPermission == Permission.Granted || fineLocationPermission == Permission.Granted)
                    googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = true;
                else
                {
                    Debug.WriteLine("Missing location permissions for IsShowingUser.");
                    googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
            else
            {
                googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = false;
            }
        }

        internal async void OnMapReady(GoogleMap map)
        {
            if (map == null)
                return;

            Map = map;

            ////////////////////it`s my/////////////////////
            map.MyLocationEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.ZoomGesturesEnabled = true;
            map.UiSettings.ScrollGesturesEnabled = true;

            map.MapClick += Map_MapClick;
            map.MyLocationButtonClick += Map_MyLocationButtonClickAsync;

            var location = await MyLocationAsync();
            MoveToAsync(location);    
        }


        ////////////////////it`s my/////////////////////


        private static IMap _handler;

        private static void SetAddress(IMapHandler handler, IMap mapProject)
        {
            _handler = mapProject;
        }

        private void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            Add_Marker(new LatLng(e.Point.Latitude, e.Point.Longitude));
            MoveToAsync(new LatLng(e.Point.Latitude, e.Point.Longitude));
        }

        private void Add_Marker(LatLng location)
        {
            Map.Clear();
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(location);
            markerOpt1.SetTitle("My Address");

            Map.AddMarker(markerOpt1);
        }

        private async void Map_MyLocationButtonClickAsync(object sender,
                                            GoogleMap.MyLocationButtonClickEventArgs e)
        {
            MoveToAsync(await MyLocationAsync());
        }

        private async Task GetAddress(LatLng location)
        {
            var temp = (await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude)).ToList();

            var res = temp[0].CountryName + ";" + temp[0].AdminArea + ";" + temp[0].Locality +
                   ";" + temp[0].SubLocality + ";" + temp[0].Thoroughfare + ";" + temp[0].SubThoroughfare +
                   ";" + temp[0].PostalCode + ";";

            if (res != null && _handler != null)
            {
                _handler.Address = res;
            }
        }

        private async Task<LatLng> MyLocationAsync()
        {
            var loc = await Geolocation.Default.GetLocationAsync();

            return new LatLng(loc.Latitude, loc.Longitude);
        }

        private async void MoveToAsync(LatLng location)
        {
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(16);
            builder.Bearing(0);
            builder.Tilt(0);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            await GetAddress(location);

            Add_Marker(location);

            Map.MoveCamera(cameraUpdate);
        }

    }

    class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        MapHandler _handler;
        GoogleMap _googleMap;

        public MapCallbackHandler(MapHandler mapHandler)
        {
            _handler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;
            _handler.OnMapReady(googleMap);
        }
    }
}
