namespace Repository.Implement
{
    public class InvoiceOutputDetailRepository : BaseRepository<InvoiceOutputDetail>
    , IInvoiceOutputDetailRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceOutputDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

