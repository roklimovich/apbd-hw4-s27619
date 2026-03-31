namespace LegacyRenewalApp.Interfaces
{
    public interface ISubsriptionPlanRepository
    {
        SubsriptionPlan GetByCode(string planCode);
    }
}