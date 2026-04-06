using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Discounts;  
using LegacyRenewalApp.Fees;
using LegacyRenewalApp.Infrastructure;
using LegacyRenewalApp.Invoicing;
using LegacyRenewalApp.Notifications;
using LegacyRenewalApp.Tax;
using LegacyRenewalApp.Validation;
 
namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly RenewalRequestValidator _validator;
        private readonly DiscountCalculator _discountCalculator;
        private readonly ISupportFeeProvider _supportFeeProvider;
        private readonly PaymentFeeResolver _paymentFeeResolver;
        private readonly ITaxRateProvider _taxRateProvider;
        private readonly RenewalInvoiceFactory _invoiceFactory;
        private readonly RenewalNotificationService _notificationService;
        private readonly IBillingGateway _billingGateway;
 
        public SubscriptionRenewalService()
            : this(
                new CustomerRepositoryAdapter(),
                new SubscriptionPlanRepositoryAdapter(),
                new RenewalRequestValidator(),
                new DiscountCalculator(new IDiscountRule[]
                {
                    new SegmentDiscountRule(),
                    new LoyaltyYearsDiscountRule(),
                    new TeamSizeDiscountRule(),
                    new LoyaltyPointsDiscountRule(useLoyaltyPoints: false),
                }),
                new PlanBasedSupportFeeProvider(),
                new PaymentFeeResolver(new IPaymentFeeCalculator[]
                {
                    new CardPaymentFeeCalculator(),
                    new BankTransferFeeCalculator(),
                    new PayPalFeeCalculator(),
                    new InvoicePaymentFeeCalculator(),
                }),
                new CountryTaxRateProvider(),
                new RenewalInvoiceFactory(),
                new RenewalNotificationService(new LegacyBillingGatewayAdapter()),
                new LegacyBillingGatewayAdapter())
        {
        }
 
        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            RenewalRequestValidator validator,
            DiscountCalculator discountCalculator,
            ISupportFeeProvider supportFeeProvider,
            PaymentFeeResolver paymentFeeResolver,
            ITaxRateProvider taxRateProvider,
            RenewalInvoiceFactory invoiceFactory,
            RenewalNotificationService notificationService,
            IBillingGateway billingGateway)
        {
            _customerRepository  = customerRepository;
            _planRepository      = planRepository;
            _validator           = validator;
            _discountCalculator  = discountCalculator;
            _supportFeeProvider  = supportFeeProvider;
            _paymentFeeResolver  = paymentFeeResolver;
            _taxRateProvider     = taxRateProvider;
            _invoiceFactory      = invoiceFactory;
            _notificationService = notificationService;
            _billingGateway      = billingGateway;
        }
 
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            // 1. Validate raw inputs
            _validator.Validate(customerId, planCode, seatCount, paymentMethod);
 
            string normalizedPlanCode      = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();
 
            // 2. Load domain data
            var customer = _customerRepository.GetById(customerId);
            var plan     = _planRepository.GetByCode(normalizedPlanCode);
 
            // 3. Validate business rules
            _validator.ValidateCustomerIsActive(customer);
 
            // 4. Base amount
            decimal baseAmount     = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            decimal discountAmount = 0m;
            string  notes          = string.Empty;
 
            // 5. Apply discounts
            decimal subtotalAfterDiscount = _discountCalculator.ApplyDiscounts(
                customer, plan, seatCount, baseAmount, ref discountAmount, ref notes);
 
            // 6. Premium support fee
            decimal supportFee = 0m;
            if (includePremiumSupport)
            {
                supportFee = _supportFeeProvider.GetSupportFee(normalizedPlanCode);
                notes     += "premium support included; ";
            }
 
            // 7. Payment processing fee
            decimal feeBase    = subtotalAfterDiscount + supportFee;
            decimal paymentFee = _paymentFeeResolver.Resolve(normalizedPaymentMethod, feeBase, ref notes);
 
            // 8. Tax
            decimal taxBase   = feeBase + paymentFee;
            decimal taxRate   = _taxRateProvider.GetTaxRate(customer.Country);
            decimal taxAmount = taxBase * taxRate;
 
            decimal finalAmount = taxBase + taxAmount;
 
            // 9. Build invoice (factory enforces minimum invoice floor)
            var invoice = _invoiceFactory.Create(
                customer,
                normalizedPlanCode,
                normalizedPaymentMethod,
                seatCount,
                baseAmount,
                discountAmount,
                supportFee,
                paymentFee,
                taxAmount,
                finalAmount,
                notes);
 
            // 10. Persist
            _billingGateway.SaveInvoice(invoice);
 
            // 11. Notify customer
            _notificationService.SendRenewalEmail(customer, normalizedPlanCode, invoice.FinalAmount);
 
            return invoice;
        }
    }
}