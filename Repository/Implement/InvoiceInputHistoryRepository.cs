namespace Repository.Implement
{
    public class InvoiceInputRepository : BaseRepository<InvoiceInput>
    , IInvoiceInputRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceInputRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

