using System;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using MvvmCross.Droid.Views;
using MvxMaps.Core.ViewModels;

namespace MvxMaps.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        private Marker _helen;
        private Marker _keith;
        private GoogleMap map;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

            var viewModel = (FirstViewModel) ViewModel;

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            map = mapFragment.Map;
            
            var options = new MarkerOptions();
            options.SetPosition(new LatLng(viewModel.Keith.Location.Lat, viewModel.Keith.Location.Lng));
            options.SetTitle("keith");
            map.AddMarker(options);

            var options2 = new MarkerOptions();
            options2.SetPosition(new LatLng(viewModel.Helen.Location.Lat, viewModel.Helen.Location.Lng));
            options2.SetTitle("helen");
            map.AddMarker(options2);

            UpdateCameraPosition(new LatLng(viewModel.Helen.Location.Lat, viewModel.Helen.Location.Lng));


        }

        void UpdateCameraPosition(LatLng pos)
        {
            try
            {
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(pos);
                builder.Zoom(12);
                builder.Bearing(45);
                builder.Tilt(10);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                map.AnimateCamera(cameraUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
    }
}
