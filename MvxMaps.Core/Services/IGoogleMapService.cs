using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvxMaps.Core.Models;
using MvxMaps.Core.ViewModels;

namespace MvxMaps.Core.Services
{
    public interface IGoogleMapService
    {
        //burde returnere en string bestående af PolyLinePoints fra google til at tegne ruten
        Task<GoogleDirectionClass> FnDrawPath(string strSource, string strDestination);

        //Location er en viewModel fra Core. Er ikke sikker på at om den skal være der!
        List<Models.Location> FnDecodePolylinePoints(string encodedPoints);

        //Vil mene denne her skal returnere en string eller en Location/LatLng
        Task<bool> FnLocationToLatLng();

    }
}
