namespace Repository.Implement
{
    public class InvoiceOutputFileRepository : BaseRepository<InvoiceOutputFile>
    , IInvoiceOutputFileRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceOutputFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

