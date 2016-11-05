using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvxMaps.Core.ViewModels
{
    public class HomeViewModel
        : MvxViewModel
    {
        public IMvxCommand First { get { return ShowCommand<FirstViewModel>(); } }
        public IMvxCommand Second { get { return ShowCommand<SecondViewModel>(); } }
        public IMvxCommand Third { get { return ShowCommand<ThirdViewModel>(); } }
        public IMvxCommand Fourth { get { return ShowCommand<FourthViewModel>(); } }
        public IMvxCommand Fifth { get { return ShowCommand<FifthViewModel>(); } }

        private MvxCommand ShowCommand<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            return new MvxCommand(() => ShowViewModel<TViewModel>());
        }

        
    }
}
