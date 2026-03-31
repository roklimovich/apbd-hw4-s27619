namespace LegacyRenewalApp.Interfaces
{
    public interface IDiscountRule
    {   
        // <summary>
        // Returns the discount amount to substract from baseAmount, and appends readonable notes describing
        // the applied discount. Returns 0 and leaves notes unchanged when the rule does not apply.
        // </summary>>
        decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount, decimal baseAmount, ref string notes);
    }
}