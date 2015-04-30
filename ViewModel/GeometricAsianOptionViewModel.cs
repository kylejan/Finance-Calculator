using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

using Financer.Model;
using Financer.Utilities;

namespace Financer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class GeometricAsianOptionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the GeometricAsianOptionViewModel class.
        /// </summary>
        public GeometricAsianOptionViewModel()
        {
            _spotPrice = 100;
            _interestRate = 0.05;
            _volatility = 0.3;
            _maturity = 3;
            _strikePrice = 100;
            _optionType = OptionTypeEnum.Call;
            _observationTime = 50;

            IsCalculateEnabled = true;
        }

        private double? _spotPrice;
        public string SpotPrice
        {
            get { return _spotPrice.ToString(); }
            set
            {
                try
                {
                    _spotPrice = double.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _spotPrice = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private double? _volatility;
        public string Volatility
        {
            get { return _volatility.ToString(); }
            set
            {
                try
                {
                    _volatility = double.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _volatility = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private double? _interestRate;
        public string InterestRate
        {
            get { return _interestRate.ToString(); }
            set
            {
                try
                {
                    _interestRate = double.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _interestRate = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private double? _maturity;
        public string Maturity
        {
            get { return _maturity.ToString(); }
            set
            {
                try
                {
                    _maturity = double.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _maturity = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private double? _strikePrice;
        public string StrikePrice
        {
            get { return _strikePrice.ToString(); }
            set
            {
                try
                {
                    _strikePrice = double.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _strikePrice = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private int? _observationTime;
        public string ObservationTime
        {
            get { return _observationTime.ToString(); }
            set
            {
                try
                {
                    _observationTime = int.Parse(value.Trim());
                }
                catch (Exception)
                {
                    _observationTime = null;
                    throw;
                }
                finally
                {
                    RaisePropertyChanged();
                }
            }
        }

        private OptionTypeEnum _optionType;
        public OptionTypeEnum OptionType
        {
            get { return _optionType; }
            set
            {
                _optionType = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CalculateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DoCalculation();
                });
            }
        }

        public string Result { get; private set; }
       
        private ErrorCode FirstValidation()
        {
            if (_spotPrice == null)
                return ErrorCode.IllegalSpotPrice;
            if (_spotPrice < 0)
                return ErrorCode.NegativeSpotPrice;
            if (_volatility == null)
                return ErrorCode.IllegalVolatility;
            if (_volatility < 0)
                return ErrorCode.NegativeVolatility;
            if (_interestRate == null)
                return ErrorCode.IllegalInterestRate;
            if (_interestRate < 0)
                return ErrorCode.NegativeInterestRate;
            if (_maturity == null)
                return ErrorCode.IllegalMaturity;
            if (_maturity < 0)
                return ErrorCode.NegativeMaturity;
            if (_strikePrice == null)
                return ErrorCode.IllegalStrikePrice;
            if (_strikePrice < 0)
                return ErrorCode.NegativeStrikePrice;
            if (_observationTime == null)
                return ErrorCode.IllegalObservationTime;
            if (_observationTime < 0)
                return ErrorCode.NegativeObservationTime;

            return ErrorCode.OK;
        }

        public bool IsCalculateEnabled { get; private set; }

        private void DoCalculation()
        {
            IsCalculateEnabled = false;
            RaisePropertyChanged("IsCalculateEnabled");

            ErrorCode errcode = FirstValidation();
            if (errcode != ErrorCode.OK)
            {
                Result = Utility.getErrorMsg(errcode);
            }
            else
            {
                var prob = new GeometricAsianOptionProb(
                        _spotPrice,
                        _volatility,
                        _interestRate,
                        _maturity,
                        _strikePrice,
                        _observationTime,
                        _optionType
                    );

                errcode = prob.validate();
                if (errcode != ErrorCode.OK)
                {
                    Result = Utility.getErrorMsg(errcode);
                }
                else
                {
                    Result = Math.Round(prob.calculate(), 4).ToString();
                }
            }

            RaisePropertyChanged("Result");
            IsCalculateEnabled = true;
            RaisePropertyChanged("IsCalculateEnabled");
        }
    }
}