

using Shop.Services.Interfaces;
using Shop.ViewModels;

using a = Microsoft.Maui.ApplicationModel;

using Android.App;
using Android.Views;
using Android.Widget;
using android = Android;
using Android.Graphics;


namespace Shop.Platforms.Android.Services.Permissions
{
    internal class Android_Services : Activity, ICheck_AndroidServives
    {

        private ChangeNethwork _callback;
        private bool _state;

        public Android_Services() :base()
        {
            _state = true;     
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }


        ~Android_Services()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }


        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            
            if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                _state = false;
                Print(Shop.Resources.Strings.Resource.ConLimited, "#DC143C");
                _callback(_state);
            }

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                _state = false;
                Print(Shop.Resources.Strings.Resource.Offline, "#DC143C");
                _callback(_state);
            }

            if (!_state &&
                e.NetworkAccess == NetworkAccess.Internet &&
                e.NetworkAccess != NetworkAccess.ConstrainedInternet)
            {
                _state = true;
                Print(Shop.Resources.Strings.Resource.ConRestored, "#228B22");
                
                _callback(_state);
            }
        }

        public async Task PermissionsStatus()
        {

            PermissionStatus status =
                      await a.Permissions.CheckStatusAsync<a.Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Denied)
            {
                await a.Permissions.RequestAsync<a.Permissions.LocationWhenInUse>();
                Print(Shop.Resources.Strings.Resource.NeedLocation, "#DC143C");
                _state = false;
                _callback(_state);
            }
            else
            {
                Print(Shop.Resources.Strings.Resource.ConRestored, "#228B22");
                _state = true;
                _callback(_state);
            }
        }

        public async Task CheckServices(ChangeNethwork callback = null)
        {

            if (callback != null)
            {
                _callback = callback;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.None
                || Connectivity.NetworkAccess == NetworkAccess.Unknown
                || Connectivity.NetworkAccess == NetworkAccess.Local)
            {
                _state = false;
                Print(Shop.Resources.Strings.Resource.Offline, "#DC143C");
                _callback(_state);
            }

        }

        private void Print(string str, string color)
        {

            if (color == "#228B22")
            {
                MainActivity.Inst.Window.SetStatusBarColor(android.Graphics.Color.ParseColor("#F8F9FA"));
            }
            else
            {
                MainActivity.Inst.Window.SetStatusBarColor(android.Graphics.Color.ParseColor(color));
            }

            Toast toast = Toast.MakeText(MainActivity.Inst, str, ToastLength.Long);
            toast.View.Background.SetColorFilter(android.Graphics.Color.ParseColor(color), PorterDuff.Mode.SrcIn);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            toast.Show();
        }

    }
}
