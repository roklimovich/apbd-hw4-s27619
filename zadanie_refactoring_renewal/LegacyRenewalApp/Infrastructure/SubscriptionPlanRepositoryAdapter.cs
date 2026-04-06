using LegacyRenewalApp.Interfaces;
 
namespace LegacyRenewalApp.Infrastructure
{
    public class SubscriptionPlanRepositoryAdapter : ISubscriptionPlanRepository
    {
        private readonly SubscriptionPlanRepository _inner = new SubscriptionPlanRepository();
 
        public SubscriptionPlan GetByCode(string planCode) => _inner.GetByCode(planCode);
    }
}