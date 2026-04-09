namespace Repository.Implement
{
    public class InvoiceInputFileRepository : BaseRepository<InvoiceInputFile>
    , IInvoiceInputFileRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceInputFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

