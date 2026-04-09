namespace Repository.Implement
{
    public class CategoryDepartmentRepository : BaseRepository<CategoryDepartment>
    , ICategoryDepartmentRepository
    {
        private readonly Context.Context.Context _context;
        public CategoryDepartmentRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
        public virtual async Task<List<CategoryDepartment>> GetByMembershipID_CompanyID_ActiveToListAsync(long? MembershipID, long? CompanyID, bool? Active)
        {
            var result = new List<CategoryDepartment>();
            if (MembershipID > 0 && CompanyID > 0)
            {
                var Membership = await _context.Set<Membership>().AsNoTracking().Where(o => o.CompanyID == CompanyID && o.USER_IDX == MembershipID && o.Active == Active).FirstOrDefaultAsync();
                if (Membership != null && Membership.ID > 0)
                {
                    List<long> ListMembershipDepartmentID = await _context.Set<MembershipDepartment>().AsNoTracking().Where(o => o.ParentID == Membership.ID && o.Active == Active).Select(o => o.CategoryDepartmentID.Value).ToListAsync();
                    if (ListMembershipDepartmentID.Count > 0)
                    {
                        result = await GetByCondition(o => o.CompanyID == CompanyID && ListMembershipDepartmentID.Contains(o.ID) && o.Active == true).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<List<CategoryDepartment>> GetByMembershipID_ActiveToListAsync(long? MembershipID, long? CompanyID, bool? Active)
        {
            var result = new List<CategoryDepartment>();
            if (MembershipID > 0)
            {
                var Membership = await _context.Set<Membership>().AsNoTracking().Where(o => o.USER_IDX == MembershipID && o.Active == Active).FirstOrDefaultAsync();
                if (Membership != null && Membership.ID > 0)
                {
                    List<long> ListMembershipDepartmentID = await _context.Set<MembershipDepartment>().AsNoTracking().Where(o => o.ParentID == Membership.ID && o.Active == Active).Select(o => o.CategoryDepartmentID.Value).ToListAsync();
                    if (ListMembershipDepartmentID.Count > 0)
                    {
                        result = await GetByCondition(o => o.CompanyID == CompanyID && ListMembershipDepartmentID.Contains(o.ID) && o.Active == true).ToListAsync();
                    }
                }
            }
            return result;
        }
    }
}

