namespace Service.Implement
{
    public class ProductionOrderBOMDetailService : BaseService<ProductionOrderBOMDetail, IProductionOrderBOMDetailRepository>
    , IProductionOrderBOMDetailService
    {
        private readonly IProductionOrderBOMDetailRepository _ProductionOrderBOMDetailRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        public ProductionOrderBOMDetailService(IProductionOrderBOMDetailRepository ProductionOrderBOMDetailRepository
            , IProductionOrderRepository ProductionOrderRepository
            , IMaterialRepository materialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryFamilyRepository categoryFamilyRepository
            , IBOMRepository bOMRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository

            ) : base(ProductionOrderBOMDetailRepository)
        {
            _ProductionOrderBOMDetailRepository = ProductionOrderBOMDetailRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _MaterialRepository = materialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryFamilyRepository = categoryFamilyRepository;
            _BOMRepository = bOMRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
        }
        public override void Initialization(ProductionOrderBOMDetail model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.Name = Parent.Name;
                model.Active = Parent.Active;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (!string.IsNullOrEmpty(model.Display))
            {
                var Display = model.Display.ToLower();
                var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.Active == true && !string.IsNullOrEmpty(o.Note) && o.Note.ToLower().Contains(Display)).OrderBy(o => o.ID).FirstOrDefault();
                if (CategoryDepartment == null)
                {
                    if (GlobalHelper.DepartmentIDCutting > 0)
                    {
                        CategoryDepartment = _CategoryDepartmentRepository.GetByID(GlobalHelper.DepartmentIDCutting.Value);
                    }
                    if (CategoryDepartment == null)
                    {
                        CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.Active == true && o.Code == "CUT").OrderBy(o => o.ID).FirstOrDefault();
                    }
                    if (CategoryDepartment == null)
                    {
                        CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.Active == true).OrderBy(o => o.ID).FirstOrDefault();
                    }
                    if (CategoryDepartment == null)
                    {
                        CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => 1 == 1).OrderBy(o => o.ID).FirstOrDefault();
                    }
                }
                if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                {
                    model.Display = CategoryDepartment.Code;
                    if (!string.IsNullOrEmpty(CategoryDepartment.Note) && !string.IsNullOrEmpty(model.Display) && CategoryDepartment.Note.Contains(model.Display) == false)
                    {
                        CategoryDepartment.Note = CategoryDepartment.Note + ";" + model.Display;
                        _CategoryDepartmentRepository.Update(CategoryDepartment);
                    }
                }
            }
            if (model.CategoryFamilyID > 0)
            {
                var CategoryFamily = _CategoryFamilyRepository.GetByID(model.CategoryFamilyID.Value);
                model.CategoryFamilyName = CategoryFamily.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryFamilyName))
                {
                    var CategoryFamily = _CategoryFamilyRepository.GetByName(model.CategoryFamilyName);
                    if (CategoryFamily.ID == 0)
                    {
                        CategoryFamily.Active = true;
                        CategoryFamily.Name = model.CategoryUnitName;
                        _CategoryFamilyRepository.Add(CategoryFamily);
                    }
                    model.CategoryFamilyID = CategoryFamily.ID;
                }
            }
          
            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Active = true;
                        CategoryUnit.Name = model.CategoryUnitName;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID = CategoryUnit.ID;
                }
            }
           
            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.QuantitySNP = Material.QuantitySNP;
                model.IsSNP = Material.IsSNP;
            }

            if (model.BOMID > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.ECN = BOM.Code;
            }
            model.Quantity = model.QuantityBOM * model.QuantityPO;
        }

        public override async Task<BaseResult<ProductionOrderBOMDetail>> SaveAsync(BaseParameter<ProductionOrderBOMDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderBOMDetail>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ProductionOrderDetailID == BaseParameter.BaseModel.ProductionOrderDetailID && o.ProductionOrderBOMID == BaseParameter.BaseModel.ProductionOrderBOMID && o.BOMDetailID == BaseParameter.BaseModel.BOMDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            if (result.BaseModel.ID > 0)
            {
                try
                {
                    BaseParameter.BaseModel = result.BaseModel;
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            return result;
        }

        public override async Task<BaseResult<ProductionOrderBOMDetail>> GetByParentIDToListAsync(BaseParameter<ProductionOrderBOMDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderBOMDetail>();
            result.List = await _ProductionOrderBOMDetailRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            result.List = result.List.OrderBy(o => o.ProductionOrderDetailID).ThenByDescending(o => o.Quantity).ToList();
            return result;
        }
    }
}

