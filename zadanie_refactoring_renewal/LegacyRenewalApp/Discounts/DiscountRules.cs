using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Discounts
{
    /// <summary>
    /// Percentage discount based on the customer's loyalty segment (Silver/Gold/Platinum/Education).
    /// </summary>
    public class SegmentDiscountRule : IDiscountRule
    {
        public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
        {
            switch (customer.Segment)
            {
                case "Silver":
                    notes += "silver discount; ";
                    return baseAmount * 0.05m;
 
                case "Gold":
                    notes += "gold discount; ";
                    return baseAmount * 0.10m;
 
                case "Platinum":
                    notes += "platinum discount; ";
                    return baseAmount * 0.15m;
 
                case "Education" when plan.IsEducationEligible:
                    notes += "education discount; ";
                    return baseAmount * 0.20m;
 
                default:
                    return 0m;
            }
        }
    }
 
    /// <summary>
    /// Percentage discount based on how long the customer has been with the company.
    /// </summary>
    public class LoyaltyYearsDiscountRule : IDiscountRule
    {
        public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
        {
            if (customer.YearsWithCompany >= 5)
            {
                notes += "long-term loyalty discount; ";
                return baseAmount * 0.07m;
            }
 
            if (customer.YearsWithCompany >= 2)
            {
                notes += "basic loyalty discount; ";
                return baseAmount * 0.03m;
            }
 
            return 0m;
        }
    }
 
    /// <summary>
    /// Volume discount based on the number of seats purchased.
    /// </summary>
    public class TeamSizeDiscountRule : IDiscountRule
    {
        public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
        {
            if (seatCount >= 50)
            {
                notes += "large team discount; ";
                return baseAmount * 0.12m;
            }
 
            if (seatCount >= 20)
            {
                notes += "medium team discount; ";
                return baseAmount * 0.08m;
            }
 
            if (seatCount >= 10)
            {
                notes += "small team discount; ";
                return baseAmount * 0.04m;
            }
 
            return 0m;
        }
    }
 
    /// <summary>
    /// Fixed monetary discount redeemed from the customer's accumulated loyalty points (up to 200 points).
    /// </summary>
    public class LoyaltyPointsDiscountRule : IDiscountRule
    {
        private readonly bool _useLoyaltyPoints;
 
        public LoyaltyPointsDiscountRule(bool useLoyaltyPoints)
        {
            _useLoyaltyPoints = useLoyaltyPoints;
        }
 
        public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes)
        {
            if (!_useLoyaltyPoints || customer.LoyaltyPoints <= 0)
            {
                return 0m;
            }
 
            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            notes += $"loyalty points used: {pointsToUse}; ";
            return pointsToUse;
        }
    }
}