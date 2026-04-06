using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Discounts;
using LegacyRenewalApp.Fees;
using LegacyRenewalApp.Infrastructure;
using LegacyRenewalApp.Invoicing;
using LegacyRenewalApp.Tax;
 
namespace LegacyRenewalApp
{
    /// <summary>
    /// Pure composition root: constructs the full object graph for <see cref="SubscriptionRenewalService"/>.
    /// Consumer code (LegacyRenewalAppConsumer) may call this instead of newing the service directly,
    /// or a real DI container can replace this class entirely.
    /// </summary>
    public static class SubscriptionRenewalServiceFactory
    {
        public static SubscriptionRenewalService Create(bool useLoyaltyPoints = false)
        {
            // Infrastructure adapters
            ICustomerRepository          customerRepo   = new CustomerRepositoryAdapter();
            ISubscriptionPlanRepository  planRepo       = new SubscriptionPlanRepositoryAdapter();
            IBillingGateway              billingGateway = new LegacyBillingGatewayAdapter();
 
            // Validation
            var validator = new RenewalRequestValidator();
 
            // Discounts – order matters: segment → tenure → volume → points
            IEnumerable<IDiscountRule> discountRules = new IDiscountRule[]
            {
                new SegmentDiscountRule(),
                new LoyaltyYearsDiscountRule(),
                new TeamSizeDiscountRule(),
                new LoyaltyPointsDiscountRule(useLoyaltyPoints),
            };
            var discountCalculator = new DiscountCalculator(discountRules);
 
            // Fees
            ISupportFeeProvider supportFeeProvider = new PlanBasedSupportFeeProvider();
 
            IEnumerable<IPaymentFeeCalculator> paymentCalculators = new IPaymentFeeCalculator[]
            {
                new CardPaymentFeeCalculator(),
                new BankTransferFeeCalculator(),
                new PayPalFeeCalculator(),
                new InvoicePaymentFeeCalculator(),
            };
            var paymentFeeResolver = new PaymentFeeResolver(paymentCalculators);
 
            // Tax
            ITaxRateProvider taxRateProvider = new CountryTaxRateProvider();
 
            // Invoice assembly
            var invoiceFactory = new RenewalInvoiceFactory();
 
            // Notifications
            var notificationService = new RenewalNotificationService(billingGateway);
 
            return new SubscriptionRenewalService(
                customerRepo,
                planRepo,
                validator,
                discountCalculator,
                supportFeeProvider,
                paymentFeeResolver,
                taxRateProvider,
                invoiceFactory,
                notificationService,
                billingGateway);
        }
    }
}