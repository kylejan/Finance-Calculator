using Financer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financer.Model
{
    public class ArithmeticAsianOptionProb
    {
        public double SpotPrice;
        public double Volatility;
        public double InterestRate;
        public double Maturity;
        public double StrikePrice;
        public int ObservationTime;
        public OptionTypeEnum OptionType;
        public int PathNumber;
        public ControlVariateMethod VariateMethod;

        public ArithmeticAsianOptionProb(double? spotPrice, double? volatility, double? interestRate, 
            double? maturity, double? strikePrice, int? observationTime, OptionTypeEnum optionType,
            int? pathNumber, ControlVariateMethod variateMethod)
        {
            SpotPrice = spotPrice ?? default(double);
            Volatility = volatility ?? default(double);
            InterestRate = interestRate ?? default(double);
            Maturity = maturity ?? default(double);
            StrikePrice = strikePrice ?? default(double);
            ObservationTime = observationTime ?? default(int);
            OptionType = optionType;
            PathNumber = pathNumber ?? default(int);
            VariateMethod = variateMethod;
        }

        public ErrorCode validate()
        {
            if (Volatility > 5.0)
                return ErrorCode.LargeVolatility;
            if (InterestRate > 5.0)
                return ErrorCode.LargeInterestRate;
            if (Maturity > 100)
                return ErrorCode.LargeMaturity;

            return ErrorCode.OK;
        }

        public double[] calculate()
        {
            return FinanceFormula.ArithmeticAsianOptionPricing(this);
        }
    }
}
