using System;
using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Fees
{
    public class PaymentFeeResolver
    {
        private readonly IEnumerable<IPaymentFeeCalculator> _calculators;
 
        public PaymentFeeResolver(IEnumerable<IPaymentFeeCalculator> calculators)
        {
            _calculators = calculators;
        }
 
        public decimal Resolve(string normalizedPaymentMethod, decimal subtotal, ref string notes)
        {
            foreach (var calculator in _calculators)
            {
                if (calculator.Supports(normalizedPaymentMethod))
                {
                    return calculator.Calculate(subtotal, ref notes);
                }
            }
 
            throw new ArgumentException("Unsupported payment method");
        }
    }
}