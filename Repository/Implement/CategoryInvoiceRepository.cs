namespace Repository.Implement
{
    public class CategoryInvoiceRepository : BaseRepository<CategoryInvoice>
    , ICategoryInvoiceRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryInvoiceRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

