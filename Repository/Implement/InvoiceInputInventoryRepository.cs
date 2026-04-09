namespace Repository.Implement
{
    public class InvoiceInputInventoryRepository : BaseRepository<InvoiceInputInventory>
    , IInvoiceInputInventoryRepository
    {
    private readonly Context.Context.Context _context;
    public InvoiceInputInventoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

