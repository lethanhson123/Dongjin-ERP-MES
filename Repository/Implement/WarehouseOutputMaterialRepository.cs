namespace Repository.Implement
{
    public class WarehouseOutputMaterialRepository : BaseRepository<WarehouseOutputMaterial>
    , IWarehouseOutputMaterialRepository
    {
        private readonly Context.Context.Context _context;
        public WarehouseOutputMaterialRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
    }
}

