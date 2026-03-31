namespace LegacyRenewalApp.Interfaces
{
    public interface ISupportFeeProvider
    {
        decimal GetSupportFee(string normalizedPaymentCode);
    }
}