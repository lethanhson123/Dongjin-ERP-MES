namespace Service.Implement
{
    public class InvoiceInputHistoryService : BaseService<InvoiceInputHistory, IInvoiceInputHistoryRepository>
    , IInvoiceInputHistoryService
    {
        private readonly IInvoiceInputHistoryRepository _InvoiceInputHistoryRepository;
       
        public InvoiceInputHistoryService(IInvoiceInputHistoryRepository InvoiceInputHistoryRepository
          
            ) : base(InvoiceInputHistoryRepository)
        {
            _InvoiceInputHistoryRepository = InvoiceInputHistoryRepository;
          
        }
      
    }
}

