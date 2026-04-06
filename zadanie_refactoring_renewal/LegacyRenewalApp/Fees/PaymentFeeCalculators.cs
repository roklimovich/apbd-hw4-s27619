using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Fees
{
    public class CardPaymentFeeCalculator : IPaymentFeeCalculator
    {
        public bool Supports(string normalizedPaymentMethod) => normalizedPaymentMethod == "CARD";
 
        public decimal Calculate(decimal subtotal, ref string notes)
        {
            notes += "card payment fee; ";
            return subtotal * 0.02m;
        }
    }
 
    public class BankTransferFeeCalculator : IPaymentFeeCalculator
    {
        public bool Supports(string normalizedPaymentMethod) => normalizedPaymentMethod == "BANK_TRANSFER";
 
        public decimal Calculate(decimal subtotal, ref string notes)
        {
            notes += "bank transfer fee; ";
            return subtotal * 0.01m;
        }
    }
 
    public class PayPalFeeCalculator : IPaymentFeeCalculator
    {
        public bool Supports(string normalizedPaymentMethod) => normalizedPaymentMethod == "PAYPAL";
 
        public decimal Calculate(decimal subtotal, ref string notes)
        {
            notes += "paypal fee; ";
            return subtotal * 0.035m;
        }
    }
 
    public class InvoicePaymentFeeCalculator : IPaymentFeeCalculator
    {
        public bool Supports(string normalizedPaymentMethod) => normalizedPaymentMethod == "INVOICE";
 
        public decimal Calculate(decimal subtotal, ref string notes)
        {
            notes += "invoice payment; ";
            return 0m;
        }
    }
}