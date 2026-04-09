namespace Repository.Implement
{
    public class MaterialRepository : BaseRepository<Material>
    , IMaterialRepository
    {
        private readonly Context.Context.Context _context;
        public MaterialRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
        public override int Add(Material model)
        {
            int result = 0;
            try
            {
                Initialization(model);
                _context.ChangeTracker.Clear();
                var existModel = GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.Code).FirstOrDefault();
                if (existModel == null)
                {
                    _context.Set<Material>().Add(model);
                }
                else
                {
                    if (existModel != null && existModel.ID > 0)
                    {
                        model.ID = existModel.ID;
                        _context.Set<Material>().Update(model);
                    }
                }
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public override async Task<int> AddAsync(Material model)
        {
            int result = 0;
            try
            {
                Initialization(model);
                _context.ChangeTracker.Clear();
                var existModel = await GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.Code).FirstOrDefaultAsync();
                if (existModel == null)
                {
                    _context.Set<Material>().Add(model);
                }
                else
                {
                    if (existModel != null && existModel.ID > 0)
                    {
                        model.ID = existModel.ID;
                        _context.Set<Material>().Update(model);
                    }
                }
                result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual Material GetByDescription(string Description, long? CompanyID)
        {
            var result = new Material();
            if (!string.IsNullOrEmpty(Description) && CompanyID > 0)
            {
                Description = Description.Trim();
                var MaterialConvert = _context.Set<MaterialConvert>().AsNoTracking().Where(o => o.CompanyID == CompanyID && o.Code == Description).FirstOrDefault();
                if (MaterialConvert != null && MaterialConvert.ParentID > 0 && MaterialConvert.MaterialID > 0)
                {
                    result = GetByID(MaterialConvert.ParentID.Value);
                }
                else
                {
                    result = GetByCondition(o => o.CompanyID == CompanyID && o.Active == true && o.Code == Description).FirstOrDefault();
                }                
            }
            if (result == null)
            {
                result = new Material();
                result.Active = true;
                result.Code = Description;
                result.CompanyID = CompanyID;
                Add(result);
            }
            if (result == null)
            {
                result = new Material();
            }
            return result;
        }
        public virtual async Task<Material> GetByDescriptionAsync(string Description, long? CompanyID)
        {
            var result = new Material();
            if (!string.IsNullOrEmpty(Description) && CompanyID > 0)
            {
                Description = Description.Trim();
                var MaterialConvert = await _context.Set<MaterialConvert>().AsNoTracking().Where(o => o.CompanyID == CompanyID && o.Code == Description).FirstOrDefaultAsync();
                if (MaterialConvert != null && MaterialConvert.ParentID > 0 && MaterialConvert.MaterialID > 0)
                {
                    result = await GetByIDAsync(MaterialConvert.ParentID.Value);
                }
                else
                {
                    result = await GetByCondition(o => o.CompanyID == CompanyID && o.Active == true && o.Code == Description).FirstOrDefaultAsync();
                }               
                if (result == null)
                {
                    result = new Material();
                    result.Active = true;
                    result.Code = Description;
                    result.CompanyID = CompanyID;
                    await AddAsync(result);
                }
            }
            if (result == null)
            {
                result = new Material();
            }
            return result;
        }
        public virtual Material GetByDescription_CompanyID_QuantitySNP(string Description, long? CompanyID, int? QuantitySNP)
        {
            var result = new Material();
            if (!string.IsNullOrEmpty(Description) && CompanyID > 0)
            {
                Description = Description.Trim();
                var MaterialConvert = _context.Set<MaterialConvert>().AsNoTracking().Where(o => o.CompanyID == CompanyID && o.Code == Description).FirstOrDefault();
                if (MaterialConvert != null && MaterialConvert.ParentID > 0 && MaterialConvert.MaterialID > 0)
                {
                    result = GetByID(MaterialConvert.ParentID.Value);
                }
                else
                {
                    result = GetByCondition(o => o.CompanyID == CompanyID && o.Active == true && o.Code == Description).FirstOrDefault();
                }                
                if (result == null)
                {
                    result = new Material();
                    result.Active = true;
                    result.Code = Description;
                    result.CompanyID = CompanyID;
                    Add(result);
                }
                if (QuantitySNP > 0)
                {
                    result.QuantitySNP = QuantitySNP;
                    Update(result);
                }
            }
            if (result == null)
            {
                result = new Material();
            }
            return result;
        }
        public virtual Material GetByDescription_CompanyID_QuantitySNP_Name(string Description, long? CompanyID, int? QuantitySNP, string Name)
        {
            var result = new Material();
            if (!string.IsNullOrEmpty(Description) && CompanyID > 0)
            {
                Description = Description.Trim();
                var MaterialConvert = _context.Set<MaterialConvert>().AsNoTracking().Where(o => o.CompanyID == CompanyID && o.Code == Description).FirstOrDefault();
                if (MaterialConvert != null && MaterialConvert.ParentID > 0 && MaterialConvert.MaterialID > 0)
                {
                    result = GetByID(MaterialConvert.ParentID.Value);
                }
                else
                {
                    result = GetByCondition(o => o.CompanyID == CompanyID && o.Active == true && o.Code == Description).FirstOrDefault();
                }              
                if (result == null)
                {
                    result = new Material();
                    result.Active = true;
                    result.Code = Description;
                    result.Name = Name;
                    result.CompanyID = CompanyID;
                    Add(result);
                }
                else
                {
                    result.Name = Name;
                    Update(result);
                }
                if (QuantitySNP > 0)
                {
                    result.QuantitySNP = QuantitySNP;
                    Update(result);
                }
            }
            if (result == null)
            {
                result = new Material();
            }
            return result;
        }
    }
}

