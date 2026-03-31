namespace LegacyRenewalApp.Interfaces
{
    public interface ITaxFeeProvider
    {
        decimal GetTaxRate(string country);
    }
}