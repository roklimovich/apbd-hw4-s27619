using System;
 
namespace LegacyRenewalApp.Invoicing
{
    public class RenewalInvoiceFactory
    {
        private const decimal MinimumInvoiceAmount = 500m;
 
        public RenewalInvoice Create(
            Customer customer,
            string normalizedPlanCode,
            string normalizedPaymentMethod,
            int seatCount,
            decimal baseAmount,
            decimal discountAmount,
            decimal supportFee,
            decimal paymentFee,
            decimal taxAmount,
            decimal preFloorFinalAmount,
            string notes)
        {
            decimal finalAmount = preFloorFinalAmount;
            string adjustedNotes = notes;
 
            if (finalAmount < MinimumInvoiceAmount)
            {
                finalAmount = MinimumInvoiceAmount;
                adjustedNotes += "minimum invoice amount applied; ";
            }
 
            return new RenewalInvoice
            {
                InvoiceNumber  = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customer.Id}-{normalizedPlanCode}",
                CustomerName   = customer.FullName,
                PlanCode       = normalizedPlanCode,
                PaymentMethod  = normalizedPaymentMethod,
                SeatCount      = seatCount,
                BaseAmount     = Round(baseAmount),
                DiscountAmount = Round(discountAmount),
                SupportFee     = Round(supportFee),
                PaymentFee     = Round(paymentFee),
                TaxAmount      = Round(taxAmount),
                FinalAmount    = Round(finalAmount),
                Notes          = adjustedNotes.Trim(),
                GeneratedAt    = DateTime.UtcNow,
            };
        }
 
        private static decimal Round(decimal value) =>
            Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}