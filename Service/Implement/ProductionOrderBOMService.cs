namespace Service.Implement
{
    public class ProductionOrderBOMService : BaseService<ProductionOrderBOM, IProductionOrderBOMRepository>
    , IProductionOrderBOMService
    {
        private readonly IProductionOrderBOMRepository _ProductionOrderBOMRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderDetailRepository _ProductionOrderDetailRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IProductionOrderBOMDetailService _ProductionOrderBOMDetailService;
        private readonly IBOMRepository _BOMRepository;
        public ProductionOrderBOMService(IProductionOrderBOMRepository ProductionOrderBOMRepository
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderDetailRepository ProductionOrderDetailRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryFamilyRepository CategoryFamilyRepository
            , IBOMDetailRepository BOMDetailRepository
            , IProductionOrderBOMDetailService ProductionOrderBOMDetailService
            , IBOMRepository BOMRepository

            ) : base(ProductionOrderBOMRepository)
        {
            _ProductionOrderBOMRepository = ProductionOrderBOMRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderDetailRepository = ProductionOrderDetailRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryFamilyRepository = CategoryFamilyRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _ProductionOrderBOMDetailService = ProductionOrderBOMDetailService;
            _BOMRepository = BOMRepository;
        }
        public override void Initialization(ProductionOrderBOM model)
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
            if (model.ProductionOrderDetailID > 0)
            {
                var ProductionOrderDetail = _ProductionOrderDetailRepository.GetByID(model.ProductionOrderDetailID.Value);
                model.QuantityPO = ProductionOrderDetail.Quantity00;
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
                var CategoryFamily = _CategoryFamilyRepository.GetByID(model.CategoryFamilyID.Value);
                model.CategoryFamilyName = CategoryFamily.Name;
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
            }
            if (model.BOMID > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.BOMCode = BOM.Code;
                model.BOMName = BOM.Name;
                model.BOMVersion = BOM.Version;
                model.QuantityBOM = BOM.Quantity;
            }
            model.Quantity = model.QuantityPO * model.QuantityBOM;
        }
        public override async Task<BaseResult<ProductionOrderBOM>> SaveAsync(BaseParameter<ProductionOrderBOM> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderBOM>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.BOMID == BaseParameter.BaseModel.BOMID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
                    await SyncAsync(BaseParameter);
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderBOM>> SyncAsync(BaseParameter<ProductionOrderBOM> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderBOM>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.BOMID > 0)
                        {
                            var ListBOMDetail = await _BOMDetailRepository.GetByParentIDAndActiveToListAsync(BaseParameter.BaseModel.BOMID.Value, true);
                            if (ListBOMDetail.Count > 0)
                            {
                                foreach (var item in ListBOMDetail)
                                {
                                    var ProductionOrderBOMDetail = new ProductionOrderBOMDetail();
                                    ProductionOrderBOMDetail.ParentID = BaseParameter.BaseModel.ParentID;
                                    ProductionOrderBOMDetail.ProductionOrderBOMID = BaseParameter.BaseModel.ID;
                                    ProductionOrderBOMDetail.ProductionOrderDetailID = BaseParameter.BaseModel.ProductionOrderDetailID;
                                    ProductionOrderBOMDetail.QuantityPO = BaseParameter.BaseModel.QuantityPO;
                                    ProductionOrderBOMDetail.BOMID = BaseParameter.BaseModel.BOMID;
                                    ProductionOrderBOMDetail.Display = item.Code;
                                    ProductionOrderBOMDetail.BOMDetailID = item.ID;
                                    ProductionOrderBOMDetail.MaterialID = item.MaterialID02;
                                    ProductionOrderBOMDetail.CategoryUnitID = item.CategoryUnitID02;
                                    ProductionOrderBOMDetail.QuantityBOM = item.Quantity02;
                                    var BaseParameterProductionOrderBOMDetail = new BaseParameter<ProductionOrderBOMDetail>();
                                    BaseParameterProductionOrderBOMDetail.BaseModel = ProductionOrderBOMDetail;
                                    await _ProductionOrderBOMDetailService.SaveAsync(BaseParameterProductionOrderBOMDetail);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}

