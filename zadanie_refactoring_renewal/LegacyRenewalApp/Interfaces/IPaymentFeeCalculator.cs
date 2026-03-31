namespace LegacyRenewalApp.Interfaces
{
    public interface IPaymentFeeCalculator
    {
        bool Supports(string normilizedPaymentMethod);
        
        // <summary>
        // Returns the fee amount and apends the note.
        // </summary>
        decimal CalculateFee(decimal subtotal, ref string notes);
    }
}