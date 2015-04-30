using GalaSoft.MvvmLight;
using Financer.Model;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Financer.Views;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Financer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public const string ContentPropertyName = "ContentControlView";

        public string PageName { get; private set; }

        private FrameworkElement _contentControlView;
        public FrameworkElement ContentControlView
        {
            get
            {
                return _contentControlView;
            }

            set
            {
                _contentControlView = value;
                RaisePropertyChanged(ContentPropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Messenger.Default.Register<ChangeViewMessage>(this, (changeViewMessage) =>
            {
                ChangeView(changeViewMessage.ViewName);
            });
        }

        public ICommand ChangeToEuropeanOptionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeView("EuropeanOptionControl");
                });
            }
        }

        public ICommand ChangeToVolatilityCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeView("ImpliedVolatilityControl");
                });
            }
        }

        public ICommand ChangeToAmericanOptionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeView("AmericanOptionControl");
                });
            }
        }

        public ICommand ChangeToGeomaticAsianOptionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeView("GeometricAsianOptionControl");
                });
            }
        }

        public ICommand ChangeToArithmeticAsianOptionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ChangeView("ArithmeticAsianOptionControl");
                });
            }
        }

        public void ChangeView(string viewName)
        {
            switch (viewName)
            {
                case "EuropeanOptionControl":
                    ContentControlView = new EuropeanOptionControl();
                    ContentControlView.DataContext = new EuropeanOptionViewModel();
                    PageName = "European Option";
                    break;
                case "ImpliedVolatilityControl":
                    ContentControlView = new ImpliedVolatilityControl();
                    ContentControlView.DataContext = new ImpliedVolatilityViewModel();
                    PageName = "Implied Volatility";
                    break;
                case "AmericanOptionControl":
                    ContentControlView = new AmericanOptionControl();
                    ContentControlView.DataContext = new AmericanOptionViewModel();
                    PageName = "American Option";
                    break;
                case "GeometricAsianOptionControl":
                    ContentControlView = new GeometricAsianOptionControl();
                    ContentControlView.DataContext = new GeometricAsianOptionViewModel();
                    PageName = "Geometric Asian Option";
                    break;
                case "ArithmeticAsianOptionControl":
                    ContentControlView = new ArithmeticAsianOptionControl();
                    ContentControlView.DataContext = new ArithmeticAsianOptionViewModel();
                    PageName = "Arithmetic Asian Option";
                    break;
                default:
                    break;
            }
            RaisePropertyChanged("PageName");
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}