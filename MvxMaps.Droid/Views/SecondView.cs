using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using MvxMaps.Core.ViewModels;

namespace MvxMaps.Droid.Views
{
    [Activity(Label = "View for SecondViewModel")]
    public class SecondView : MvxActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SecondView);

            var viewModel = (SecondViewModel)ViewModel;

        }
    }
}