namespace Repository.Implement
{
    public class InvoiceInputHistoryRepository : BaseRepository<InvoiceInputHistory>
    , IInvoiceInputHistoryRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceInputHistoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

