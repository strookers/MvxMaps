using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvxMaps.Core.ViewModels;

namespace MvxMaps.Core.Services
{
    public class GoogleMapService : IGoogleMapService
    {
        public List<Location> FnDecodePolylinePoints(string encodedPoints)
        {
            throw new NotImplementedException();
        }

        public void FnDrawPath(string strSource, string strDestination)
        {
            throw new NotImplementedException();
        }

        public Task<string> FnHttpRequest(string strUri)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FnLocationToLatLng()
        {
            throw new NotImplementedException();
        }
    }
}
