using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Fees
{
    public class PlanBasedSupportFeeProvider : ISupportFeeProvider
    {
        private static readonly Dictionary<string, decimal> SupportFeeByPlan =
            new Dictionary<string, decimal>
            {
                { "START",      250m },
                { "PRO",        400m },
                { "ENTERPRISE", 700m },
            };
 
        public decimal GetSupportFee(string normalizedPlanCode)
        {
            return SupportFeeByPlan.TryGetValue(normalizedPlanCode, out decimal fee) ? fee : 0m;
        }
    }
}