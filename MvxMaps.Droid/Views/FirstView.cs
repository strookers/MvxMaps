using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Widget;
using MvvmCross.Droid.Views;
using MvxMaps.Core.Models;
using MvxMaps.Core.ViewModels;
using Newtonsoft.Json;
using Location = MvxMaps.Core.ViewModels.Location;

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
            

            //MarkOnMap("keith", new LatLng(viewModel.Keith.Location.Lat, viewModel.Keith.Location.Lng));
            //MarkOnMap("Helen", new LatLng(viewModel.Helen.Location.Lat, viewModel.Helen.Location.Lng));

            //var options = new MarkerOptions();
            //options.SetPosition(new LatLng(viewModel.Keith.Location.Lat, viewModel.Keith.Location.Lng));
            //options.SetTitle("keith");
            //map.AddMarker(options);

            //var options2 = new MarkerOptions();
            //options2.SetPosition(new LatLng(viewModel.Helen.Location.Lat, viewModel.Helen.Location.Lng));
            //options2.SetTitle("helen");
            //map.AddMarker(options2);

            //UpdateCameraPosition(new LatLng(viewModel.Helen.Location.Lat, viewModel.Helen.Location.Lng));

            FnDrawPath("Aalborg","Herning");

        }

        void FnSetDirectionQuery(GoogleDirectionClass objRoutes)
        {

            //objRoutes.routes.Count  --may be more then one 
            if (objRoutes.routes.Count > 0)
            {
                string encodedPoints = objRoutes.routes[0].overview_polyline.points;

                var lstDecodedPoints = FnDecodePolylinePoints(encodedPoints);
                //convert list of location point to array of latlng type
                var latLngPoints = new LatLng[lstDecodedPoints.Count];
                int index = 0;
                foreach (var loc in lstDecodedPoints)
                {
                    latLngPoints[index++] = new LatLng(loc.lat, loc.lng);
                }

                var polylineoption = new PolylineOptions();
                polylineoption.InvokeColor(Android.Graphics.Color.Red);
                polylineoption.Geodesic(true);
                polylineoption.Add(latLngPoints);
                RunOnUiThread(() =>
              map.AddPolyline(polylineoption));
            }
        }

        async void FnDrawPath(string strSource, string strDestination)
        {
            //string strFullDirectionURL = string.Format (Constants.strGoogleDirectionUrl,strSource,strDestination); // &waypoints=Viborg,Denmark|Herning,Denmark
            GoogleDirectionClass objRoutes;
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + strSource + "&destination=" + strDestination + "&key=AIzaSyCqZ0l_RSmTJ4wvq37SovhNjZqwjhUJPdM";

            var client = new WebClient();
            string json = await client.DownloadStringTaskAsync(new Uri(url));

            objRoutes = JsonConvert.DeserializeObject<GoogleDirectionClass>(json);
                RunOnUiThread(() =>
                {
                    if (map != null)
                    {
                        var sourceLatLng = new LatLng(objRoutes.routes[0].legs[0].start_location.lat, objRoutes.routes[0].legs[0].start_location.lng);
                        var destinationLatLng = new LatLng(objRoutes.routes[0].legs[0].end_location.lat, objRoutes.routes[0].legs[0].end_location.lng);
                        map.Clear();
                        MarkOnMap(objRoutes.routes[0].legs[0].start_address, sourceLatLng);
                        MarkOnMap(objRoutes.routes[0].legs[0].end_address, destinationLatLng);

                        UpdateCameraPosition(sourceLatLng);
                    }
                });
                FnSetDirectionQuery(objRoutes);


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

        void MarkOnMap(string title, LatLng pos)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    var marker = new MarkerOptions();
                    marker.SetTitle(title);
                    marker.SetPosition(pos); //Resource.Drawable.BlueDot
                    map.AddMarker(marker);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        public List<Core.Models.Location> FnDecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<Core.Models.Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                Core.Models.Location p = new Core.Models.Location();
                p.lat = Convert.ToDouble(currentLat) / 100000.0;
                p.lng = Convert.ToDouble(currentLng) / 100000.0;
                poly.Add(p);
            }

            return poly;
        }
    }
}
