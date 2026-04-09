namespace Service.Implement
{
    public class InvoiceOutputFileService : BaseService<InvoiceOutputFile, IInvoiceOutputFileRepository>
    , IInvoiceOutputFileService
    {
    private readonly IInvoiceOutputFileRepository _InvoiceOutputFileRepository;
    public InvoiceOutputFileService(IInvoiceOutputFileRepository InvoiceOutputFileRepository) : base(InvoiceOutputFileRepository)
    {
    _InvoiceOutputFileRepository = InvoiceOutputFileRepository;
    }
  
    }
    }

