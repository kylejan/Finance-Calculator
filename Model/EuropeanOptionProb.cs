using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Financer.Utilities;

namespace Financer.Model
{
    public class EuropeanOptionProb
    {
        public double SpotPrice;
        public double Volatility;
        public double InterestRate;
        public double Maturity;
        public double StrikePrice;
        public OptionTypeEnum OptionType;

        public EuropeanOptionProb(double? spotPrice, double? volatility, 
            double? interestRate, double? maturity, double? strikePrice, OptionTypeEnum optionType)
        {
            SpotPrice = spotPrice ?? default(double);
            Volatility = volatility ?? default(double);
            InterestRate = interestRate ?? default(double);
            Maturity = maturity ?? default(double);
            StrikePrice = strikePrice ?? default(double);
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

            return ErrorCode.OK;
        }

        public double calculate()
        {
            return FinanceFormula.BSPricing(this);
        }
    }
}
