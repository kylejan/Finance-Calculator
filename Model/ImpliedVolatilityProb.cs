using Financer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financer.Model
{
    public class ImpliedVolatilityProb
    {
        public double SpotPrice;
        public double InterestRate;
        public double Maturity;
        public double StrikePrice;
        public double OptionPremium;
        public OptionTypeEnum OptionType;

        public ImpliedVolatilityProb(double? spotPrice, double? interestRate, 
            double? maturity, double? strikePrice, double? optionPremium, OptionTypeEnum optionType)
        {
            SpotPrice = spotPrice ?? default(double);
            InterestRate = interestRate ?? default(double);
            Maturity = maturity ?? default(double);
            StrikePrice = strikePrice ?? default(double);
            OptionPremium = optionPremium ?? default(double);
            OptionType = optionType;
        }

        public ErrorCode validate()
        {
            if (InterestRate > 5.0)
                return ErrorCode.LargeInterestRate;
            if (Maturity > 100)
                return ErrorCode.LargeMaturity;

            return ErrorCode.OK;
        }

        public double calculate()
        {
            return FinanceFormula.ImpliedVolatilityCalculate(this);
        }
    }
}
