namespace Repository.Implement
{
    public class WarehouseRequestConfirmRepository : BaseRepository<WarehouseRequestConfirm>
    , IWarehouseRequestConfirmRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseRequestConfirmRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

