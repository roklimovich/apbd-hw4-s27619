using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Infrastructure
{
    /// <summary>
    /// Thin adapter that wraps the legacy static gateway behind the IBillingGateway abstraction.
    /// The static class itself is never modified.
    /// </summary>
    public class LegacyBillingGatewayAdapter : IBillingGateway
    {
        public void SaveInvoice(RenewalInvoice invoice)
        {
            LegacyBillingGateway.SaveInvoice(invoice);
        }
 
        public void SendEmail(string to, string subject, string body)
        {
            LegacyBillingGateway.SendEmail(to, subject, body);
        }
    }
}