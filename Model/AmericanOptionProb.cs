using Financer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financer.Model
{
    public class AmericanOptionProb
    {
        public double SpotPrice;
        public double Volatility;
        public double InterestRate;
        public double Maturity;
        public double StrikePrice;
        public int StepsNum;
        public OptionTypeEnum OptionType;

        public AmericanOptionProb(double? spotPrice, double? volatility, double? interestRate, 
            double? maturity, double? strikePrice, int? stepsNum, OptionTypeEnum optionType)
        {
            SpotPrice = spotPrice ?? default(double);
            Volatility = volatility ?? default(double);
            InterestRate = interestRate ?? default(double);
            Maturity = maturity ?? default(double);
            StrikePrice = strikePrice ?? default(double);
            StepsNum = stepsNum ?? default(int);
            OptionType = optionType;
        }

        public ErrorCode validate()
        {
            if (Volatility > 5.0)
                return ErrorCode.LargeVolatility;
            if (InterestRate > 5.0)
                return ErrorCode.LargeInterestRate;
            if (Maturity > 100)
                return ErrorCode.LargeMaturity;
            if (StepsNum > 8000)
                return ErrorCode.LargeStepsNum;

            return ErrorCode.OK;
        }

        public double calculate()
        {
            return FinanceFormula.BinomialTreePricing(this);
        }
    }
}
