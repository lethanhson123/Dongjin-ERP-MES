namespace Service.Implement
{
    public class MaterialReplaceService : BaseService<MaterialReplace, IMaterialReplaceRepository>
    , IMaterialReplaceService
    {
        private readonly IMaterialReplaceRepository _MaterialReplaceRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IBOMRepository _BOMRepository;
        public MaterialReplaceService(IMaterialReplaceRepository MaterialReplaceRepository
            , IMaterialRepository MaterialRepository
            , IBOMRepository BOMRepository

            ) : base(MaterialReplaceRepository)
        {
            _MaterialReplaceRepository = MaterialReplaceRepository;
            _MaterialRepository = MaterialRepository;
            _BOMRepository = BOMRepository;
        }
        public override void InitializationSave(MaterialReplace model)
        {
            BaseInitialization(model);
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            if (model.ParentID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ParentName))
                {
                    var Material = _MaterialRepository.GetByDescription(model.ParentName, model.CompanyID);
                    model.ParentID = Material.ID;
                }
            }
            if (model.ParentID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Material.Code;
            }
            if (model.MaterialID01 > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.MaterialName01))
                {
                    var Material = _MaterialRepository.GetByDescription(model.MaterialName01, model.CompanyID);
                    model.MaterialID01 = Material.ID;
                }
            }
            if (model.MaterialID01 > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID01.Value);
                model.MaterialName01 = Material.Code;
            }
            if (model.MaterialID02 > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.MaterialName02))
                {
                    var Material = _MaterialRepository.GetByDescription(model.MaterialName02, model.CompanyID);
                    model.MaterialID02 = Material.ID;
                }
            }
            if (model.MaterialID02 > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID02.Value);
                model.MaterialName02 = Material.Code;
            }
            if (model.MaterialID02 > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ECN))
                {
                    var BOM = _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == model.CompanyID && o.MaterialID == model.ParentID && o.Code == model.ECN).OrderByDescending(o => o.Date).FirstOrDefault();
                    if (BOM != null && BOM.ID > 0)
                    {
                        model.BOMID = BOM.ID;
                    }
                }
            }
            if (model.BOMID > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.ECN = BOM.Code;
            }
        }

        public override async Task<BaseResult<MaterialReplace>> SaveAsync(BaseParameter<MaterialReplace> BaseParameter)
        {
            var result = new BaseResult<MaterialReplace>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID02 == BaseParameter.BaseModel.MaterialID02).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            if (result.BaseModel != null && result.BaseModel.ID > 0)
            {
            }
            return result;
        }
    }
}

