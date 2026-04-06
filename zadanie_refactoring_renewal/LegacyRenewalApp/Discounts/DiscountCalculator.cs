using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Discounts
{
    public class DiscountCalculator
    {
        private const decimal MinimumDiscountedSubtotal = 300m;
 
        private readonly IEnumerable<IDiscountRule> _rules;
 
        public DiscountCalculator(IEnumerable<IDiscountRule> rules)
        {
            _rules = rules;
        }
 
        /// <summary>
        /// Applies all discount rules and returns the subtotal after discounts,
        /// respecting the configured minimum floor.
        /// </summary>
        public decimal ApplyDiscounts(
            Customer customer,
            SubscriptionPlan plan,
            int seatCount,
            decimal baseAmount,
            ref decimal discountAmount,
            ref string notes)
        {
            foreach (var rule in _rules)
            {
                discountAmount += rule.Calculate(customer, plan, seatCount, baseAmount, ref notes);
            }
 
            decimal subtotal = baseAmount - discountAmount;
 
            if (subtotal < MinimumDiscountedSubtotal)
            {
                subtotal = MinimumDiscountedSubtotal;
                notes += "minimum discounted subtotal applied; ";
            }
 
            return subtotal;
        }
    }
}