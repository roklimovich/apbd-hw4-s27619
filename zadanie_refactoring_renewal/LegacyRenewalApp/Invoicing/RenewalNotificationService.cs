using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Notifications
{
    public class RenewalNotificationService
    {
        private readonly IBillingGateway _billingGateway;
 
        public RenewalNotificationService(IBillingGateway billingGateway)
        {
            _billingGateway = billingGateway;
        }
 
        public void SendRenewalEmail(Customer customer, string planCode, decimal finalAmount)
        {
            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                return;
            }
 
            string subject = "Subscription renewal invoice";
            string body =
                $"Hello {customer.FullName}, your renewal for plan {planCode} " +
                $"has been prepared. Final amount: {finalAmount:F2}.";
 
            _billingGateway.SendEmail(customer.Email, subject, body);
        }
    }
}