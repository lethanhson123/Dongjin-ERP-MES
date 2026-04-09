namespace Repository.Implement
{
    public class WarehouseRequestDetailRepository : BaseRepository<WarehouseRequestDetail>
    , IWarehouseRequestDetailRepository
    {
        private readonly Context.Context.Context _context;
        public WarehouseRequestDetailRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
        public virtual async Task<int> AddFromMESAsync(WarehouseRequestDetail model)
        {
            int result = 0;
            try
            {
                var Membership = await _context.Set<Membership>().AsNoTracking().Where(o => o.USER_IDX == model.UpdateUserID).FirstOrDefaultAsync();
                if (Membership != null)
                {
                    model.CreateUserID = Membership.ID;
                    model.UpdateUserID = Membership.ID;
                }
                WarehouseRequest WarehouseRequest = new WarehouseRequest();
                WarehouseRequest.Active = true;
                WarehouseRequest.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
                WarehouseRequest.CreateUserID = model.CreateUserID;
                WarehouseRequest.UpdateUserID = model.UpdateUserID;
                WarehouseRequest.CreateDate = GlobalHelper.InitializationDateTime;
                WarehouseRequest.UpdateDate = GlobalHelper.InitializationDateTime;
                WarehouseRequest.ParentName = "CUT: Production Plan";
                switch (model.CompanyID)
                {
                    case 16:
                        WarehouseRequest.ParentID = 327;
                        WarehouseRequest.SupplierID = 23;
                        WarehouseRequest.CustomerID = 86;
                        break;
                    case 17:
                        WarehouseRequest.ParentID = 328;
                        WarehouseRequest.SupplierID = 188;
                        WarehouseRequest.CustomerID = 195;
                        break;
                }

                WarehouseRequest.Date = model.Date ?? GlobalHelper.InitializationDateTime;
                WarehouseRequest.Year = WarehouseRequest.Date.Value.Year;
                WarehouseRequest.Month = WarehouseRequest.Date.Value.Month;
                WarehouseRequest.Day = WarehouseRequest.Date.Value.Day;
                if (WarehouseRequest.SupplierID > 0)
                {
                    var Customer = await _context.Set<CategoryDepartment>().AsNoTracking().Where(o => o.ID == WarehouseRequest.SupplierID).FirstOrDefaultAsync();
                    if (Customer != null && Customer.ID > 0)
                    {
                        WarehouseRequest.SupplierName = Customer.Code;
                    }
                }
                if (WarehouseRequest.CustomerID > 0)
                {
                    var Customer = await _context.Set<CategoryDepartment>().AsNoTracking().Where(o => o.ID == WarehouseRequest.CustomerID).FirstOrDefaultAsync();
                    if (Customer != null && Customer.ID > 0)
                    {
                        WarehouseRequest.CustomerName = Customer.Code;
                    }
                }
                WarehouseRequest.Code = WarehouseRequest.ParentName + "-" + WarehouseRequest.Date.Value.ToString("yyyyMMdd") + "-" + WarehouseRequest.CustomerName;
                WarehouseRequest.Name = WarehouseRequest.Code;
                var WarehouseRequestCheck = await _context.Set<WarehouseRequest>().AsNoTracking().Where(o => o.Active == true && o.ParentID == WarehouseRequest.ParentID && o.Code == WarehouseRequest.Code).FirstOrDefaultAsync();
                if (WarehouseRequestCheck == null)
                {
                    _context.Set<WarehouseRequest>().Add(WarehouseRequest);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    WarehouseRequest.ID = WarehouseRequestCheck.ID;
                }
                if (WarehouseRequest.ID > 0)
                {
                    model.ParentID = WarehouseRequest.ID;
                    model.Code = WarehouseRequest.Code;
                    model.CompanyID = WarehouseRequest.CompanyID;
                    model.Active = false;
                    Initialization(model);
                    _context.ChangeTracker.Clear();
                    _context.Set<WarehouseRequestDetail>().Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

