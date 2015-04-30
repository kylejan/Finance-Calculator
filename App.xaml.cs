using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Financer
{
    public enum OptionTypeEnum { Call, Put };
    public enum ControlVariateMethod { None, Geometric };
    public enum ErrorCode
    {
        OK,

        IllegalSpotPrice,
        IllegalVolatility,
        IllegalInterestRate,
        IllegalMaturity,
        IllegalStrikePrice,
        IllegalOptionPremium,
        IllegalStepsNum,
        IllegalObservationTime,
        IllegalOptionType,
        IllegalMonteCarloPath,
        IllegalControlVariateMethod,

        NegativeSpotPrice,
        NegativeVolatility,
        NegativeInterestRate,
        NegativeMaturity,
        NegativeStrikePrice,
        NegativeOptionPremium,
        NegativeStepsNum,
        NegativeObservationTime,
        NegativeMonteCarloPath,

        LargeSpotPrice,
        LargeVolatility,
        LargeInterestRate,
        LargeMaturity,
        LargeStrikePrice,
        LargeOptionPremium,
        LargeStepsNum,
        LargeObservationTime,
        LargeMonteCarloPath
    };

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
