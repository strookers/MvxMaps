using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvxMaps.Core.ViewModels
{
    public class FourthViewModel
        : MvxViewModel
    {
        private Location _location;
        public Location Location
        {
            get { return _location; }
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        private double _lat;
        public double Lat
        {
            get { return _lat; }
            set { _lat = value; RaisePropertyChanged(() => Lat); }
        }

        private double _lng;
        public double Lng
        {
            get { return _lng; }
            set { _lng = value; RaisePropertyChanged(() => Lng); }
        }

        public FourthViewModel()
        {
            Location = new Location()
            {
                Lat = 51.4,
                Lng = 0.4
            };
        }

        public IMvxCommand UpdateCenterCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Location = new Location()
                    {
                        Lat = Lat,
                        Lng = Lng
                    };
                });
            }
        }
    }
}
