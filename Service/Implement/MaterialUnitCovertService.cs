namespace Service.Implement
{
    public class MaterialUnitCovertService : BaseService<MaterialUnitCovert, IMaterialUnitCovertRepository>
    , IMaterialUnitCovertService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IMaterialUnitCovertRepository _MaterialUnitCovertRepository;

        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        public MaterialUnitCovertService(IMaterialUnitCovertRepository MaterialUnitCovertRepository
            , IWebHostEnvironment WebHostEnvironment
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            ) : base(MaterialUnitCovertRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _MaterialUnitCovertRepository = MaterialUnitCovertRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;

        }
        public override void InitializationSave(MaterialUnitCovert model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.ParentName, model.CompanyID);
                model.ParentID = Material.ID;
            }
            if (model.ParentID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Material.Code;
            }
            //if (model.MaterialID > 0)
            //{
            //}
            //else
            //{
            //    var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
            //    model.MaterialID = Material.ID;
            //}
            //if (model.MaterialID > 0)
            //{
            //    var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
            //    model.MaterialName = Material.Code;
            //}
            if (model.CategoryUnitID01 > 0)
            {
            }
            else
            {
                var CategoryUnitID = _CategoryUnitRepository.GetByName(model.CategoryUnitName01);
                model.CategoryUnitID01 = CategoryUnitID.ID;
            }
            if (model.CategoryUnitID01 > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID01.Value);
                model.CategoryUnitName01 = CategoryUnit.Name;
            }
            if (model.CategoryUnitID02 > 0)
            {
            }
            else
            {
                var CategoryUnitID = _CategoryUnitRepository.GetByName(model.CategoryUnitName02);
                model.CategoryUnitID02 = CategoryUnitID.ID;
            }
            if (model.CategoryUnitID02 > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID02.Value);
                model.CategoryUnitName02 = CategoryUnit.Name;
            }
        }
        public override async Task<BaseResult<MaterialUnitCovert>> SaveAsync(BaseParameter<MaterialUnitCovert> BaseParameter)
        {
            var result = new BaseResult<MaterialUnitCovert>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryUnitID01 == BaseParameter.BaseModel.CategoryUnitID01 && o.CategoryUnitID02 == BaseParameter.BaseModel.CategoryUnitID02).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);

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

