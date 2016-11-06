using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvxMaps.Core.Models;
using MvxMaps.Core.Services;

namespace MvxMaps.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {

        private Zombie _helen;
        public Zombie Helen
        {
            get { return _helen; }
            set { _helen = value; RaisePropertyChanged(() => Helen); }
        }

        private Zombie _keith;
        public Zombie Keith
        {
            get { return _keith; }
            set { _keith = value; RaisePropertyChanged(() => Keith); }
        }

        private readonly IGoogleMapService _googleMapService;
        public FirstViewModel(IGoogleMapService googleMapService)
        {
            _googleMapService = googleMapService;
            var g = getIt();
            Helen = new Zombie()
            {
                Name = "Helen",
                Location = new Location()
                {
                    Lat = 51.4,
                    Lng = 0.4
                },
            };
            Keith = new Zombie()
            {
                IsMale = true,
                Name = "Keith",
                Location = new Location()
                {
                    Lat = 51.5,
                    Lng = 0.3
                }
            };
        }

        public async Task<GoogleDirectionClass> getIt()
        {
            GoogleDirectionClass g = await _googleMapService.FnDrawPath("aalborg", "herning");
            System.Diagnostics.Debug.WriteLine(g.routes.Count);
            return g;
        }

        public IMvxCommand MoveCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Keith.Location = new Location()
                    {
                        Lat = Keith.Location.Lat + 0.1,
                        Lng = Keith.Location.Lng
                    };

                    Helen.Location = new Location()
                    {
                        Lat = Helen.Location.Lat - 0.1,
                        Lng = Helen.Location.Lng
                    };

                });
            }
        }
    }
}
