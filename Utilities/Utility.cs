using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Financer.Utilities
{
    public static class Utility
    {
        public static bool IsTextAllowed(string text, string input)
        {
            double tmp;
            if (input != "." && !double.TryParse(input, out tmp))
                return false;
            Regex numRegex = new Regex("^(-?[0-9]*[.]*[0-9]*)$");
            if (text.IndexOf('.') > -1)
            {
                if (input == ".")
                    return false;
            }
            return numRegex.IsMatch(text);
        }

        public static string getErrorMsg(ErrorCode errorcode)
        {
            StringBuilder builder = new StringBuilder();

            if (errorcode == ErrorCode.IllegalSpotPrice)
                builder.Append("illegal spot price input!");
            else if (errorcode == ErrorCode.IllegalVolatility)
                builder.Append("illegal volatility input!");
            else if (errorcode == ErrorCode.IllegalInterestRate)
                builder.Append("illegal interest rate input!");
            else if (errorcode == ErrorCode.IllegalMaturity)
                builder.Append("illegal maturity input!");
            else if (errorcode == ErrorCode.IllegalStrikePrice)
                builder.Append("illegal strike price input!");
            else if (errorcode == ErrorCode.IllegalOptionPremium)
                builder.Append("illegal option premium input!");
            else if (errorcode == ErrorCode.IllegalStepsNum)
                builder.Append("illegal steps num input!");
            else if (errorcode == ErrorCode.IllegalObservationTime)
                builder.Append("illegal observation input!");
            else if (errorcode == ErrorCode.IllegalOptionType)
                builder.Append("illegal option type input!");
            else if (errorcode == ErrorCode.IllegalMonteCarloPath)
                builder.Append("illegal monteCarlo path num input!");
            else if (errorcode == ErrorCode.IllegalControlVariateMethod)
                builder.Append("illegal control variate method input!");

            else if (errorcode == ErrorCode.NegativeSpotPrice)
                builder.Append("negative spot price!");
            else if (errorcode == ErrorCode.NegativeVolatility)
                builder.Append("negative volatility!");
            else if (errorcode == ErrorCode.NegativeInterestRate)
                builder.Append("negative interest rate!");
            else if (errorcode == ErrorCode.NegativeMaturity)
                builder.Append("negative maturity!");
            else if (errorcode == ErrorCode.NegativeStrikePrice)
                builder.Append("negative strike price!");
            else if (errorcode == ErrorCode.NegativeOptionPremium)
                builder.Append("negative option premium!");
            else if (errorcode == ErrorCode.NegativeStepsNum)
                builder.Append("negative steps num!");
            else if (errorcode == ErrorCode.NegativeObservationTime)
                builder.Append("negative observation time!");
            else if (errorcode == ErrorCode.NegativeMonteCarloPath)
                builder.Append("negative monteCarlo path num!");

            else if (errorcode == ErrorCode.LargeVolatility)
                builder.Append("too large volatility!");
            else if (errorcode == ErrorCode.LargeInterestRate)
                builder.Append("too large interest rate!");
            else if (errorcode == ErrorCode.LargeMaturity)
                builder.Append("too large maturity!");
            else if (errorcode == ErrorCode.LargeStepsNum)
                builder.Append("too large steps number!");

            return builder.ToString();
        }
    }
}
