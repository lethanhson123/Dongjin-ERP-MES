namespace Repository.Implement
{
    public class InvoiceOutputRepository : BaseRepository<InvoiceOutput>
    , IInvoiceOutputRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceOutputRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

