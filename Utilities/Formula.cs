using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Financer.Model;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Financer.Utilities
{
    public static class FinanceFormula
    {
        // Black-Scholes Formula - European Options
        private static double[] GetDValues(double s, double k, double t, double r, double sigma, OptionTypeEnum optionType) {
            double[] dValueArray = new double[2];

            double dHead = (Math.Log(s/ k) + r * t) / (sigma * Math.Sqrt(t));
            double dTail = sigma * Math.Sqrt(t) / 2;

            double d1 = dHead + dTail;
            double d2 = dHead - dTail;

            dValueArray[0] = d1;
            dValueArray[1] = d2;

            return dValueArray;
        }

        public static double BSPricing(double s, double k, double t, double r, double sigma, OptionTypeEnum optionType) 
        {
            double[] dValueArray = GetDValues(s, k, t, r, sigma, optionType);
            double d1 = dValueArray[0];
            double d2 = dValueArray[1];

            Normal normal = new Normal();
            
            if (optionType == OptionTypeEnum.Call)
            {
                return s * normal.CumulativeDistribution(d1) - k * Math.Exp(-r * t) * normal.CumulativeDistribution(d2);
            }
            else
            {
                return k * Math.Exp(-r * t) * normal.CumulativeDistribution(-d2) - s * normal.CumulativeDistribution(-d1);
            }
        }

        public static double BSPricing(EuropeanOptionProb prob)
        {
            double s = prob.SpotPrice;
            double k = prob.StrikePrice;
            double t = prob.Maturity;
            double r = prob.InterestRate;
            double sigma = prob.Volatility;
            OptionTypeEnum optionType = prob.OptionType;

            return BSPricing(s, k, t, r, sigma, optionType);
        }

        // Implied Volatility
        private static double GetVega(double s, double k, double t, double r, double sigma)
        {
            Normal normal = new Normal();
            double d1 = (Math.Log(s / k) + r * t) / (sigma * t) + 0.5 * sigma * t;
            double vega = s * Math.Sqrt(t) * normal.CumulativeDistribution(d1);
            return vega;
        }

        private static double GetIncrement(double s, double k, double t, double r, double sigma, double premium, OptionTypeEnum optionType)
        {
            double vega = GetVega(s, k, t, r, sigma);
            double newOptionPremium = newOptionPremium = BSPricing(s, k, t, r, sigma, optionType);
            return (newOptionPremium - premium) / vega;
        }

        public static double ImpliedVolatilityCalculate(ImpliedVolatilityProb prob)
        {
            double s = prob.SpotPrice;
            double k = prob.StrikePrice;
            double t = prob.Maturity;
            double r = prob.InterestRate;
            double premium = prob.OptionPremium;
            OptionTypeEnum optionType = prob.OptionType;

            double sigmaHat = Math.Sqrt(2 * Math.Abs((Math.Log(s / k) + r * t) / t));
            double tol = Double.MinValue;
            double sigma = sigmaHat;
            double sigmaDiff = 1;
            int n = 1, nmax = 100;
            while (sigmaDiff >= tol && n < nmax)
            {
                double increment = GetIncrement(s, k, t, r, sigma, premium, optionType);
                sigma = sigma - increment;
                sigmaDiff = Math.Abs(increment);
                n++;
            }
            return sigma;
        }

        // Binomial Tree - American Options
        private static double BinomialTreePayoff(double s, double k, OptionTypeEnum optionType)
        {
            if (OptionTypeEnum.Call == optionType)
                return Math.Max(s - k, 0.0);
            else
                return Math.Max(k - s, 0.0);
        }

        public static double BinomialTreePricing(AmericanOptionProb prob)
        {
            double s = prob.SpotPrice;
            double k = prob.StrikePrice;
            double t = prob.Maturity;
            double r = prob.InterestRate;
            double sigma = prob.Volatility;
            OptionTypeEnum optionType = prob.OptionType;
            int n = prob.StepsNum;

            double dt = t / n;
            double u = Math.Exp(sigma * Math.Sqrt(dt));
            double d = Math.Exp(-sigma * Math.Sqrt(dt));
            double p = (Math.Exp(r * dt) - d) / (u - d);

            double[,] stockTree = new double[n + 1, n + 1];

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    stockTree[i, j] = s * Math.Pow(u, j) * Math.Pow(d, i - j);
                }
            }

            double[,] valueTree = new double[n + 1, n + 1];
            for (int j = 0; j <= n; j++)
            {
                valueTree[n, j] = BinomialTreePayoff(stockTree[n, j], k, optionType);
            }

            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = 0; j <= i; j++)
                {
                    valueTree[i, j] = Math.Exp(-r * dt) 
                        * (p * valueTree[i + 1, j + 1] + (1 - p) * valueTree[i + 1, j]);
                    
                    if (optionType == OptionTypeEnum.Call)
                        valueTree[i, j] = Math.Max(stockTree[i, j] - k, valueTree[i, j]);
                    else
                        valueTree[i, j] = Math.Max(k - stockTree[i, j], valueTree[i, j]);
                }
            }

            return valueTree[0, 0];
        }

        // Geometric Asian Options
        private static double getGeoAsianOptionPrice(double s, double k, double r, double t, double sigma, int n, OptionTypeEnum optionType)
        {
            double sigsqT = Math.Pow(sigma, 2) * t * (n + 1) * (2 * n + 1) / (6 * n * n);
            double muT = 0.5 * sigsqT + (r - 0.5 * sigma * sigma) * t * (n + 1) / (2 * n);

            double d1 = (Math.Log(s / k) + (muT + 0.5 * sigsqT)) / (Math.Sqrt(sigsqT));
            double d2 = d1 - Math.Sqrt(sigsqT);

            Normal normal = new Normal();

            if (optionType == OptionTypeEnum.Call)
            {
                return Math.Exp(-r * t) * (s * Math.Exp(muT) * normal.CumulativeDistribution(d1) - k * normal.CumulativeDistribution(d2));
            }
            else
            {
                return Math.Exp(-r * t) * (k * normal.CumulativeDistribution(-d2) - s * Math.Exp(muT) * normal.CumulativeDistribution(-d1));
            }
        }

        public static double GeometricAsianOptionPricing(GeometricAsianOptionProb prob)
        {
            double s = prob.SpotPrice;
            double k = prob.StrikePrice;
            double r = prob.InterestRate;
            double t = prob.Maturity;
            double sigma = prob.Volatility;
            int n = prob.ObservationTime;
            OptionTypeEnum optionType = prob.OptionType;

            return getGeoAsianOptionPrice(s, k, r, t, sigma, n, optionType);
        }

        // Arithmetic Asian Option
        private static double[] GetMeanVarStd(List<double> valueList) {
            double mean = 0.0;
            double var  = 0.0;
            double std  = 0.0;

            int length = valueList.Count;
            
            // Compute mean
            for (int i = 0; i < length; i++) {
                mean += valueList[i];
            }
            mean = mean / length;

            // Compute var
            for (int i = 0; i < length; i++) {
                var += Math.Pow((valueList[i] - mean), 2);
            }

            if (length > 1) {
                var = var / (length - 1);
            }

            // Compute std
            std = Math.Sqrt(var);

            double[] results = {mean, var, std};
            return results;
        }

        private static double[] GetAAOPrice(double s, double k, double t, double r, double sigma, OptionTypeEnum optionType, ControlVariateMethod method, int pathNum, int n)
        {
            const int seed = 11151;

            double Dt = t / n;
            double drift = Math.Exp((r - 0.5 * Math.Pow(sigma, 2)) * Dt);

            Normal normal = new Normal();
            normal.RandomSource = new Random(seed);

            List<Double> arithPayoff = new List<Double>();
            List<Double> geoPayoff = new List<Double>();

            for (int i = 1; i <= pathNum; i++)
            {
                double growthFactor = drift * Math.Exp(sigma * Math.Sqrt(Dt) * normal.Sample());
                List<Double> spath = new List<double>();
                spath.Add(s * growthFactor);

                double logSpathSum = Math.Log(spath.ElementAt(0));

                for (int j = 2; j <= n; j++)
                {
                    growthFactor = drift * Math.Exp(sigma * Math.Sqrt(Dt) * normal.Sample());
                    double tmp = spath.ElementAt(spath.Count - 1) * growthFactor;
                    spath.Add(tmp);
                    logSpathSum += Math.Log(tmp);
                }

                double arithMean = GetMeanVarStd(spath)[0];
                double geoMean = Math.Exp((1.0 / n) * logSpathSum);

                if (optionType == OptionTypeEnum.Call)
                {
                    arithPayoff.Add(Math.Exp(-r * t) * Math.Max(arithMean - k, 0));
                    geoPayoff.Add(Math.Exp(-r * t) * Math.Max(geoMean - k, 0));
                }
                else
                {
                    arithPayoff.Add( Math.Exp(-r * t) * Math.Max(k - arithMean, 0));
                    geoPayoff.Add(Math.Exp(-r * t) * Math.Max(k - geoMean, 0));
                }
                spath.Clear();
            }

            double[] arithPayoffMvs = GetMeanVarStd(arithPayoff);
            double arithPayoffMean  = arithPayoffMvs[0];
            double arithPayoffVar   = arithPayoffMvs[1];
            double arithPayoffStd   = arithPayoffMvs[2];

            double[] geoPayoffMvs = GetMeanVarStd(geoPayoff);
            double geoPayoffMean  = geoPayoffMvs[0];
            double geoPayoffVar   = geoPayoffMvs[1];
            double geoPayoffStd   = geoPayoffMvs[2];

            List<double> pointMultiplyList = new List<double>();
            for (int iter = 0; iter < arithPayoff.Count; iter++)
            {
                pointMultiplyList.Add(arithPayoff[iter] * geoPayoff[iter]);
            }

            double covXY = GetMeanVarStd(pointMultiplyList)[0] - arithPayoffMean * geoPayoffMean;
            double theta = covXY / geoPayoffVar;

            double geoPrice = getGeoAsianOptionPrice(s, k, r, t, sigma, n, optionType);

            List<double> controlVar = new List<double>();
            for (int iter = 0; iter < arithPayoff.Count; iter++)
            {
                double tempVar = arithPayoff[iter] + theta * (geoPrice - geoPayoff[iter]);
                controlVar.Add(tempVar);
            }

            double[] controlVarMvs = GetMeanVarStd(controlVar);
            double controlVarMaen  = controlVarMvs[0];
            double controlVarVar   = controlVarMvs[1];
            double controlVarStd   = controlVarMvs[2];

            double lbound = 0;
            double ubound = 0;
            double[] results = new double[3];
            if (method == ControlVariateMethod.None) {
                lbound = arithPayoffMean - (1.96 * arithPayoffStd) / Math.Sqrt(pathNum);
                ubound = arithPayoffMean + (1.96 * arithPayoffStd) / Math.Sqrt(pathNum);

                results[0] = lbound;
                results[1] = arithPayoffMean;
                results[2] = ubound;
            } else {
                lbound = controlVarMaen - (1.96 * controlVarStd) / Math.Sqrt(pathNum);
                ubound = controlVarMaen + (1.96 * controlVarStd) / Math.Sqrt(pathNum);

                results[0] = lbound;
                results[1] = controlVarMaen;
                results[2] = ubound;
            }

            return results;
        }

        public static double[] ArithmeticAsianOptionPricing(ArithmeticAsianOptionProb prob)
        {
            double s = prob.SpotPrice;
            double k = prob.StrikePrice;
            double r = prob.InterestRate;
            double t = prob.Maturity;
            double sigma = prob.Volatility;
            int n = prob.ObservationTime;
            OptionTypeEnum optionType = prob.OptionType;
            int pathNum = prob.PathNumber;
            ControlVariateMethod method = prob.VariateMethod;

            return GetAAOPrice(s, k, t, r, sigma, optionType, method, pathNum, n);
        }
    }
}
