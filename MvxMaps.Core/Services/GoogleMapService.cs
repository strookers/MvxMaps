using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MvxMaps.Core.Models;
using MvxMaps.Core.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace MvxMaps.Core.Services
{
    public class GoogleMapService : IGoogleMapService
    {
        public List<Models.Location> FnDecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<Models.Location>();
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
                    Models.Location p = new Models.Location();
                    p.lat = Convert.ToDouble(currentLat) / 100000.0;
                    p.lng = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }

            return poly;
        }

        //Returnere selve ruten
        public async Task<GoogleDirectionClass> FnDrawPath(string strSource, string strDestination)
        {
            GoogleDirectionClass directions;
            string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + strSource + "&destination=" + strDestination + "&key=AIzaSyCqZ0l_RSmTJ4wvq37SovhNjZqwjhUJPdM";

            var client = new HttpClient();

            string json = await client.GetStringAsync(url);

                directions = JsonConvert.DeserializeObject<GoogleDirectionClass>(json);

            return directions;
        }

        public Task<bool> FnLocationToLatLng()
        {
            throw new NotImplementedException();
        }
    }
}
