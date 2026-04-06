using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Tax
{
    public class CountryTaxRateProvider : ITaxRateProvider
    {
        private const decimal DefaultTaxRate = 0.20m;
 
        private static readonly Dictionary<string, decimal> TaxRateByCountry =
            new Dictionary<string, decimal>
            {
                { "Poland",         0.23m },
                { "Germany",        0.19m },
                { "Czech Republic", 0.21m },
                { "Norway",         0.25m },
            };
 
        public decimal GetTaxRate(string country)
        {
            return TaxRateByCountry.TryGetValue(country, out decimal rate) ? rate : DefaultTaxRate;
        }
    }
}