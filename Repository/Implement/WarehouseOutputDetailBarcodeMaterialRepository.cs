namespace Repository.Implement
{
    public class WarehouseOutputDetailBarcodeMaterialRepository : BaseRepository<WarehouseOutputDetailBarcodeMaterial>
    , IWarehouseOutputDetailBarcodeMaterialRepository
    {
        private readonly Context.Context.Context _context;
        public WarehouseOutputDetailBarcodeMaterialRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
    }
}

