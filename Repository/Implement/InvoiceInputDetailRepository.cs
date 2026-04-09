namespace Repository.Implement
{
    public class InvoiceInputDetailRepository : BaseRepository<InvoiceInputDetail>
    , IInvoiceInputDetailRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceInputDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

