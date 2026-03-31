namespace LegacyRenewalApp.Interfaces
{
    public interface IBillingGateway
    {
        void SaveInvoce(RenewalInvoice invoice);
        void SendEmail(string to, string subject, string body);
    }
}