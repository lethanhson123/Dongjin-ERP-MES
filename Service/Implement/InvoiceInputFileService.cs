namespace Service.Implement
{
    public class InvoiceInputFileService : BaseService<InvoiceInputFile, IInvoiceInputFileRepository>
    , IInvoiceInputFileService
    {
        private readonly IInvoiceInputFileRepository _InvoiceInputFileRepository;
        public InvoiceInputFileService(IInvoiceInputFileRepository InvoiceInputFileRepository) : base(InvoiceInputFileRepository)
        {
            _InvoiceInputFileRepository = InvoiceInputFileRepository;
        }
    }
}

