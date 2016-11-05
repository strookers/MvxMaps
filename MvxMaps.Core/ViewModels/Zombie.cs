﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvxMaps.Core.ViewModels
{
    public class Zombie
        : MvxNotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private Location _location;
        public Location Location
        {
            get { return _location; }
            set { _location = value; RaisePropertyChanged(() => Location); }
        }

        private bool _isMale;
        public bool IsMale
        {
            get { return _isMale; }
            set { _isMale = value; RaisePropertyChanged(() => IsMale); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Location);
        }
    }
}
