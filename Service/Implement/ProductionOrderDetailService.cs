namespace Service.Implement
{
    public class ProductionOrderDetailService : BaseService<ProductionOrderDetail, IProductionOrderDetailRepository>
    , IProductionOrderDetailService
    {
        private readonly IProductionOrderDetailRepository _ProductionOrderDetailRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryVehicleRepository _CategoryVehicleRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IProductionOrderBOMService _ProductionOrderBOMService;
        private readonly IProductionOrderProductionPlanService _ProductionOrderProductionPlanService;
        private readonly IProductionOrderBOMDetailRepository _ProductionOrderBOMDetailRepository;
        private readonly IProductionOrderMaterialService _ProductionOrderMaterialService;

        private readonly IMembershipRepository _MembershipRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderDetailService(IProductionOrderDetailRepository ProductionOrderDetailRepository

            , IProductionOrderRepository ProductionOrderRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryVehicleRepository categoryVehicleRepository
            , ICategoryFamilyRepository categoryFamilyRepository
            , IBOMRepository BOMRepository
            , IProductionOrderBOMService ProductionOrderBOMService
            , IProductionOrderProductionPlanService ProductionOrderProductionPlanService
            , IProductionOrderBOMDetailRepository ProductionOrderBOMDetailRepository
            , IProductionOrderMaterialService ProductionOrderMaterialService
            , IMembershipRepository MembershipRepository
            , IWebHostEnvironment WebHostEnvironment

            ) : base(ProductionOrderDetailRepository)
        {
            _ProductionOrderDetailRepository = ProductionOrderDetailRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryVehicleRepository = categoryVehicleRepository;
            _CategoryFamilyRepository = categoryFamilyRepository;
            _BOMRepository = BOMRepository;
            _ProductionOrderBOMService = ProductionOrderBOMService;
            _ProductionOrderProductionPlanService = ProductionOrderProductionPlanService;
            _ProductionOrderBOMDetailRepository = ProductionOrderBOMDetailRepository;
            _ProductionOrderMaterialService = ProductionOrderMaterialService;
            _MembershipRepository = MembershipRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void Initialization(ProductionOrderDetail model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Code = Parent.Code;
                model.Name = Parent.Name;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                model.Active = model.Active ?? Parent.Active;
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
            var Material = _MaterialRepository.GetByDescription(model.MaterialCode, model.CompanyID);
            model.MaterialID = Material.ID;
            model.MaterialCode = Material.Code;
            model.MaterialName = Material.Name;


            model.BOMID = model.BOMID ?? 0;
            if (!string.IsNullOrEmpty(model.BOMECN))
            {
                model.BOMECN = model.BOMECN.Trim();
                var BOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialID == model.MaterialID && o.Code == model.BOMECN).OrderByDescending(o => o.Date).FirstOrDefault();
                if (BOM == null)
                {
                    BOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialCode == model.MaterialCode && o.Code == model.BOMECN).OrderByDescending(o => o.Date).FirstOrDefault();
                }
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMID = BOM.ID;
                }
            }
            else
            {
                var BOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialID == model.MaterialID).OrderByDescending(o => o.Date).FirstOrDefault();
                if (BOM == null)
                {
                    BOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialCode == model.MaterialCode).OrderByDescending(o => o.Date).FirstOrDefault();
                }
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMID = BOM.ID;
                    model.BOMECN = BOM.Code;
                    model.BOMECNVersion = BOM.Version;
                }
            }
            if (model.Priority == null || model.Priority == 0)
            {
                var List = GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID > 0).ToList();
                model.Priority = List.Count + 1;
            }
            model.Quantity00 = GlobalHelper.InitializationNumber;
            model.QuantityActual00 = GlobalHelper.InitializationNumber;

            SQLHelper.InitializationNumber<ProductionOrderDetail>(model);
            SQLHelper.InitializationDateTimeName<ProductionOrderDetail>(model);
            SQLHelper.InitializationQuantityGAP<ProductionOrderDetail>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderDetail>(model);
            SQLHelper.InitializationQuantityActual00<ProductionOrderDetail>(model);

            model.QuantityGAP00 = model.Quantity00 - model.QuantityActual00;

            if (model.BOMID == 0)
            {
                model.Active = false;
            }
            else
            {
                var ListBOM = _BOMRepository.GetByCondition(o => o.ParentID == model.BOMID).ToList();
                if (ListBOM.Count == 0)
                {
                    model.Active = false;
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderDetail>> SaveAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (string.IsNullOrEmpty(BaseParameter.BaseModel.BOMECN))
                {
                    var BOM = _BOMRepository.GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Active == true && o.MaterialID == BaseParameter.BaseModel.MaterialID).OrderByDescending(o => o.Date).FirstOrDefault();
                    if (BOM == null)
                    {
                        BOM = _BOMRepository.GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Active == true && o.MaterialCode == BaseParameter.BaseModel.MaterialCode).OrderByDescending(o => o.Date).FirstOrDefault();
                    }
                    if (BOM != null && BOM.ID > 0)
                    {
                        BaseParameter.BaseModel.BOMID = BOM.ID;
                        BaseParameter.BaseModel.BOMECN = BOM.Code;
                        BaseParameter.BaseModel.BOMECNVersion = BOM.Version;
                    }
                }
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.BOMECN == BaseParameter.BaseModel.BOMECN && o.BOMECNVersion == BaseParameter.BaseModel.BOMECNVersion).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                bool IsUpdate = false;
                if (BaseParameter.BaseModel.ID > 0)
                {
                    IsUpdate = true;
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    try
                    {
                        if (IsUpdate == false)
                        {
                            BaseParameter.BaseModel = result.BaseModel;
                            await SyncAsync(BaseParameter);
                        }
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }

            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> SyncAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            //await SyncProductionOrderBOMAsync(BaseParameter);
            //await SyncProductionOrderMaterialAsync(BaseParameter);
            await SyncProductionOrderProductionPlanAsync(BaseParameter);
            //await SyncProductionOrderDetailAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> SyncProductionOrderDetailAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.SortOrder > 1)
                            {
                                var ProductionOrderDetail = new ProductionOrderDetail();
                                ProductionOrderDetail.CompanyID = BaseParameter.BaseModel.CompanyID;
                                ProductionOrderDetail.ParentID = BaseParameter.BaseModel.ParentID;
                                ProductionOrderDetail.MaterialID = GlobalHelper.InitializationNumber;
                                ProductionOrderDetail.SortOrder = -1;
                                ProductionOrderDetail.Priority = -1;
                                var ModelCheck = await GetByCondition(o => o.ParentID == ProductionOrderDetail.ParentID && o.SortOrder == ProductionOrderDetail.SortOrder).FirstOrDefaultAsync();
                                if (ModelCheck == null)
                                {
                                    foreach (PropertyInfo pro in BaseParameter.BaseModel.GetType().GetProperties())
                                    {
                                        if (pro.Name.Contains("Date") && !pro.Name.Contains("DatePO"))
                                        {
                                            foreach (PropertyInfo proDate in ProductionOrderDetail.GetType().GetProperties())
                                            {
                                                if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO"))
                                                {
                                                    if (proDate.Name == pro.Name)
                                                    {
                                                        try
                                                        {
                                                            proDate.SetValue(ProductionOrderDetail, pro.GetValue(BaseParameter.BaseModel), null);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string mes = ex.Message;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    await _ProductionOrderDetailRepository.AddAsync(ProductionOrderDetail);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> SyncProductionOrderBOMAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                var ProductionOrder = await _ProductionOrderRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                if (ProductionOrder.ID > 0)
                                {
                                    var BOM = new BOM();
                                    if (BaseParameter.BaseModel.BOMID > 0)
                                    {
                                        BOM = await _BOMRepository.GetByIDAsync(BaseParameter.BaseModel.BOMID.Value);
                                    }
                                    if (BOM.ID == 0)
                                    {
                                        //BOM = await _BOMRepository.GetByCondition(o => o.CompanyID == ProductionOrder.CompanyID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Active == true).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                                    }
                                    if (BOM != null && BOM.ID > 0)
                                    {
                                        var ProductionOrderBOM = new ProductionOrderBOM();
                                        ProductionOrderBOM.ParentID = BaseParameter.BaseModel.ParentID;
                                        ProductionOrderBOM.ProductionOrderDetailID = BaseParameter.BaseModel.ID;
                                        ProductionOrderBOM.MaterialID = BaseParameter.BaseModel.MaterialID;
                                        ProductionOrderBOM.CategoryUnitID = BaseParameter.BaseModel.CategoryUnitID;
                                        ProductionOrderBOM.CategoryFamilyID = BaseParameter.BaseModel.CategoryFamilyID;
                                        ProductionOrderBOM.QuantityPO = BaseParameter.BaseModel.Quantity00;
                                        ProductionOrderBOM.BOMID = BOM.ID;
                                        var BaseParameterProductionOrderBOM = new BaseParameter<ProductionOrderBOM>();
                                        BaseParameterProductionOrderBOM.BaseModel = ProductionOrderBOM;
                                        await _ProductionOrderBOMService.SaveAsync(BaseParameterProductionOrderBOM);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> SyncProductionOrderProductionPlanAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                var ProductionOrder = _ProductionOrderRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                                if (ProductionOrder.ID > 0)
                                {
                                    var ProductionOrderProductionPlan = new ProductionOrderProductionPlan();
                                    ProductionOrderProductionPlan.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                    ProductionOrderProductionPlan.ParentID = BaseParameter.BaseModel.ParentID;
                                    ProductionOrderProductionPlan.ProductionOrderDetailID = BaseParameter.BaseModel.ID;
                                    ProductionOrderProductionPlan.MaterialID = BaseParameter.BaseModel.MaterialID;
                                    ProductionOrderProductionPlan.CategoryUnitID = BaseParameter.BaseModel.CategoryUnitID;
                                    ProductionOrderProductionPlan.CategoryFamilyID = BaseParameter.BaseModel.CategoryFamilyID;
                                    ProductionOrderProductionPlan.QuantitySNP = BaseParameter.BaseModel.QuantitySNP;
                                    ProductionOrderProductionPlan.QuantityBox = BaseParameter.BaseModel.QuantityBox;
                                    ProductionOrderProductionPlan.Priority = BaseParameter.BaseModel.Priority;

                                    ProductionOrder.DaySpan = ProductionOrder.DaySpan ?? GlobalHelper.DaySpan;
                                    var DaySpan = 0;
                                    var Date00 = BaseParameter.BaseModel.Date01.Value.AddDays((double)ProductionOrder.DaySpan * -1);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date01, Date00, 0, BaseParameter.BaseModel.Quantity01);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date02, BaseParameter.BaseModel.Date01, DaySpan, BaseParameter.BaseModel.Quantity02);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date03, BaseParameter.BaseModel.Date02, DaySpan, BaseParameter.BaseModel.Quantity03);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date04, BaseParameter.BaseModel.Date03, DaySpan, BaseParameter.BaseModel.Quantity04);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date05, BaseParameter.BaseModel.Date04, DaySpan, BaseParameter.BaseModel.Quantity05);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date06, BaseParameter.BaseModel.Date05, DaySpan, BaseParameter.BaseModel.Quantity06);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date07, BaseParameter.BaseModel.Date06, DaySpan, BaseParameter.BaseModel.Quantity07);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date08, BaseParameter.BaseModel.Date07, DaySpan, BaseParameter.BaseModel.Quantity08);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date09, BaseParameter.BaseModel.Date08, DaySpan, BaseParameter.BaseModel.Quantity09);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date10, BaseParameter.BaseModel.Date09, DaySpan, BaseParameter.BaseModel.Quantity10);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date11, BaseParameter.BaseModel.Date10, DaySpan, BaseParameter.BaseModel.Quantity11);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date12, BaseParameter.BaseModel.Date11, DaySpan, BaseParameter.BaseModel.Quantity12);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date13, BaseParameter.BaseModel.Date12, DaySpan, BaseParameter.BaseModel.Quantity13);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date14, BaseParameter.BaseModel.Date13, DaySpan, BaseParameter.BaseModel.Quantity14);
                                    DaySpan = CovertDate(ProductionOrderProductionPlan, BaseParameter.BaseModel.Date15, BaseParameter.BaseModel.Date14, DaySpan, BaseParameter.BaseModel.Quantity15);

                                    var BaseParameterProductionOrderProductionPlan = new BaseParameter<ProductionOrderProductionPlan>();
                                    BaseParameterProductionOrderProductionPlan.BaseModel = ProductionOrderProductionPlan;
                                    await _ProductionOrderProductionPlanService.SaveAsync(BaseParameterProductionOrderProductionPlan);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> SyncProductionOrderMaterialAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                var ListProductionOrderBOMDetail = await _ProductionOrderBOMDetailRepository.GetByCondition(o => o.ProductionOrderDetailID == BaseParameter.BaseModel.ID).ToListAsync();
                                if (ListProductionOrderBOMDetail.Count > 0)
                                {
                                    foreach (var item in ListProductionOrderBOMDetail)
                                    {
                                        var ProductionOrderMaterial = new ProductionOrderMaterial();
                                        ProductionOrderMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                        ProductionOrderMaterial.ProductionOrderDetailID = BaseParameter.BaseModel.ID;
                                        ProductionOrderMaterial.MaterialID01 = BaseParameter.BaseModel.MaterialID;
                                        ProductionOrderMaterial.Priority = BaseParameter.BaseModel.Priority;
                                        ProductionOrderMaterial.MaterialID = item.MaterialID;
                                        ProductionOrderMaterial.CategoryUnitID = item.CategoryUnitID;
                                        ProductionOrderMaterial.CategoryFamilyID = item.CategoryFamilyID;
                                        ProductionOrderMaterial.QuantityBOM = item.QuantityBOM;
                                        ProductionOrderMaterial.Display = item.Display;
                                        ProductionOrderMaterial.BOMID = item.BOMID;

                                        var BaseParameterProductionOrderMaterial = new BaseParameter<ProductionOrderMaterial>();
                                        BaseParameterProductionOrderMaterial.BaseModel = ProductionOrderMaterial;
                                        await _ProductionOrderMaterialService.SaveAsync(BaseParameterProductionOrderMaterial);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        private int CovertDate(ProductionOrderProductionPlan ProductionOrderProductionPlan, DateTime? Date02, DateTime? Date01, int DaySpan, int? Quantity)
        {

            try
            {
                if (Date02 != null && Date01 != null)
                {
                    var DayBegin = DaySpan + 1;
                    TimeSpan? TimeSpan = Date02 - Date01;
                    if (TimeSpan.HasValue)
                    {
                        DaySpan = DaySpan + (int)TimeSpan.Value.TotalDays;
                    }
                    var DaySpanSub = (int)TimeSpan.Value.TotalDays;
                    for (int i = DayBegin; i <= DaySpan; i++)
                    {
                        var IndexString = i.ToString();
                        if (i < 10)
                        {
                            IndexString = "0" + IndexString;
                        }
                        var DateName = "Date" + IndexString;
                        var DatePOName = "DatePO" + IndexString;
                        var QuantityPOName = "QuantityPO" + IndexString;

                        foreach (PropertyInfo proProductionOrderProductionPlan in ProductionOrderProductionPlan.GetType().GetProperties())
                        {
                            if (proProductionOrderProductionPlan.Name == DateName)
                            {
                                var AddDays = DaySpanSub * -1;
                                DaySpanSub = DaySpanSub - 1;
                                var Date = Date02.Value.AddDays((double)AddDays);
                                proProductionOrderProductionPlan.SetValue(ProductionOrderProductionPlan, Date);
                            }
                            if (proProductionOrderProductionPlan.Name == DatePOName)
                            {
                                Date02 = new DateTime(Date02.Value.Year, Date02.Value.Month, Date02.Value.Day, (int)TimeSpan.Value.TotalDays, 0, 0);
                                proProductionOrderProductionPlan.SetValue(ProductionOrderProductionPlan, Date02);
                            }
                            if (proProductionOrderProductionPlan.Name == QuantityPOName)
                            {
                                proProductionOrderProductionPlan.SetValue(ProductionOrderProductionPlan, Quantity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return DaySpan;
        }
        public virtual async Task<BaseResult<ProductionOrderDetail>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderDetail> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderDetail>();
            result.List = new List<ProductionOrderDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var ListAll = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                var List = ListAll;
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    List = ListAll.Where(o => !string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode.Contains(BaseParameter.SearchString)).ToList();
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.CategoryFamilyName) && o.CategoryFamilyName.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.BOMECN) && o.BOMECN.Contains(BaseParameter.SearchString)).ToList();
                    }
                }
                result.List = List;
                ProductionOrderDetail ProductionOrderProductionPlanSemiSum = new ProductionOrderDetail();
                ProductionOrderProductionPlanSemiSum.MaterialCode = "Sum";
                ProductionOrderProductionPlanSemiSum.Priority = 0;
                ProductionOrderProductionPlanSemiSum.Quantity00 = result.List.Sum(o => o.Quantity00);
                ProductionOrderProductionPlanSemiSum.QuantityActual00 = result.List.Sum(o => o.QuantityActual00);
                ProductionOrderProductionPlanSemiSum.QuantityGAP00 = result.List.Sum(o => o.QuantityGAP00);

                ProductionOrderProductionPlanSemiSum.Quantity01 = result.List.Sum(o => o.Quantity01);
                ProductionOrderProductionPlanSemiSum.QuantityActual01 = result.List.Sum(o => o.QuantityActual01);
                ProductionOrderProductionPlanSemiSum.QuantityGAP01 = result.List.Sum(o => o.QuantityGAP01);

                ProductionOrderProductionPlanSemiSum.Quantity02 = result.List.Sum(o => o.Quantity02);
                ProductionOrderProductionPlanSemiSum.QuantityActual02 = result.List.Sum(o => o.QuantityActual02);
                ProductionOrderProductionPlanSemiSum.QuantityGAP02 = result.List.Sum(o => o.QuantityGAP02);

                ProductionOrderProductionPlanSemiSum.Quantity03 = result.List.Sum(o => o.Quantity03);
                ProductionOrderProductionPlanSemiSum.QuantityActual03 = result.List.Sum(o => o.QuantityActual03);
                ProductionOrderProductionPlanSemiSum.QuantityGAP03 = result.List.Sum(o => o.QuantityGAP03);

                ProductionOrderProductionPlanSemiSum.Quantity04 = result.List.Sum(o => o.Quantity04);
                ProductionOrderProductionPlanSemiSum.QuantityActual04 = result.List.Sum(o => o.QuantityActual04);
                ProductionOrderProductionPlanSemiSum.QuantityGAP04 = result.List.Sum(o => o.QuantityGAP04);

                ProductionOrderProductionPlanSemiSum.Quantity05 = result.List.Sum(o => o.Quantity05);
                ProductionOrderProductionPlanSemiSum.QuantityActual05 = result.List.Sum(o => o.QuantityActual05);
                ProductionOrderProductionPlanSemiSum.QuantityGAP05 = result.List.Sum(o => o.QuantityGAP05);

                ProductionOrderProductionPlanSemiSum.Quantity06 = result.List.Sum(o => o.Quantity06);
                ProductionOrderProductionPlanSemiSum.QuantityActual06 = result.List.Sum(o => o.QuantityActual06);
                ProductionOrderProductionPlanSemiSum.QuantityGAP06 = result.List.Sum(o => o.QuantityGAP06);

                ProductionOrderProductionPlanSemiSum.Quantity07 = result.List.Sum(o => o.Quantity07);
                ProductionOrderProductionPlanSemiSum.QuantityActual07 = result.List.Sum(o => o.QuantityActual07);
                ProductionOrderProductionPlanSemiSum.QuantityGAP07 = result.List.Sum(o => o.QuantityGAP07);

                ProductionOrderProductionPlanSemiSum.Quantity08 = result.List.Sum(o => o.Quantity08);
                ProductionOrderProductionPlanSemiSum.QuantityActual08 = result.List.Sum(o => o.QuantityActual08);
                ProductionOrderProductionPlanSemiSum.QuantityGAP08 = result.List.Sum(o => o.QuantityGAP08);

                ProductionOrderProductionPlanSemiSum.Quantity09 = result.List.Sum(o => o.Quantity09);
                ProductionOrderProductionPlanSemiSum.QuantityActual09 = result.List.Sum(o => o.QuantityActual09);
                ProductionOrderProductionPlanSemiSum.QuantityGAP09 = result.List.Sum(o => o.QuantityGAP09);

                ProductionOrderProductionPlanSemiSum.Quantity10 = result.List.Sum(o => o.Quantity10);
                ProductionOrderProductionPlanSemiSum.QuantityActual10 = result.List.Sum(o => o.QuantityActual10);
                ProductionOrderProductionPlanSemiSum.QuantityGAP10 = result.List.Sum(o => o.QuantityGAP10);

                ProductionOrderProductionPlanSemiSum.Quantity11 = result.List.Sum(o => o.Quantity11);
                ProductionOrderProductionPlanSemiSum.QuantityActual11 = result.List.Sum(o => o.QuantityActual11);
                ProductionOrderProductionPlanSemiSum.QuantityGAP11 = result.List.Sum(o => o.QuantityGAP11);

                ProductionOrderProductionPlanSemiSum.Quantity12 = result.List.Sum(o => o.Quantity12);
                ProductionOrderProductionPlanSemiSum.QuantityActual12 = result.List.Sum(o => o.QuantityActual12);
                ProductionOrderProductionPlanSemiSum.QuantityGAP12 = result.List.Sum(o => o.QuantityGAP12);

                ProductionOrderProductionPlanSemiSum.Quantity13 = result.List.Sum(o => o.Quantity13);
                ProductionOrderProductionPlanSemiSum.QuantityActual13 = result.List.Sum(o => o.QuantityActual13);
                ProductionOrderProductionPlanSemiSum.QuantityGAP13 = result.List.Sum(o => o.QuantityGAP13);

                ProductionOrderProductionPlanSemiSum.Quantity14 = result.List.Sum(o => o.Quantity14);
                ProductionOrderProductionPlanSemiSum.QuantityActual14 = result.List.Sum(o => o.QuantityActual14);
                ProductionOrderProductionPlanSemiSum.QuantityGAP14 = result.List.Sum(o => o.QuantityGAP14);

                ProductionOrderProductionPlanSemiSum.Quantity15 = result.List.Sum(o => o.Quantity15);
                ProductionOrderProductionPlanSemiSum.QuantityActual15 = result.List.Sum(o => o.QuantityActual15);
                ProductionOrderProductionPlanSemiSum.QuantityGAP15 = result.List.Sum(o => o.QuantityGAP15);               

                result.List.Add(ProductionOrderProductionPlanSemiSum);
            }
            result.List = result.List.OrderBy(o => o.Priority).ThenBy(o => o.MaterialCode).ToList(); ;
            return result;
        }
    }
}

