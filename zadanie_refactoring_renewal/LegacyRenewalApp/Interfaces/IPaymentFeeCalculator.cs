namespace LegacyRenewalApp.Interfaces
{
    public interface IPaymentFeeCalculator
    {
        bool Supports(string normilizedPaymentMethod);
        
        // <summary>
        // Returns the fee amount and appends the note.
        // </summary>
        decimal Calculate(decimal subtotal, ref string notes);
    }
}