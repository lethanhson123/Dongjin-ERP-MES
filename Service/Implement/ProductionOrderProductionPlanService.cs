namespace Service.Implement
{
    public class ProductionOrderProductionPlanService : BaseService<ProductionOrderProductionPlan, IProductionOrderProductionPlanRepository>
    , IProductionOrderProductionPlanService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IProductionOrderProductionPlanRepository _ProductionOrderProductionPlanRepository;

        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderDetailRepository _ProductionOrderDetailRepository;
        private readonly IProductionOrderProductionPlanMaterialService _ProductionOrderProductionPlanMaterialService;
        private readonly IProductionOrderProductionPlanSemiService _ProductionOrderProductionPlanSemiService;
        private readonly IProductionOrderProductionPlanSemiRepository _ProductionOrderProductionPlanSemiRepository;
        private readonly IProductionOrderBOMRepository _ProductionOrderBOMRepository;
        private readonly IProductionOrderBOMDetailRepository _ProductionOrderBOMDetailRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryVehicleRepository _CategoryVehicleRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IWarehouseRequestDetailService _WarehouseRequestDetailService;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IProductionOrderBOMService _ProductionOrderBOMService;
        private readonly IProductionOrderProductionPlanBackupRepository _ProductionOrderProductionPlanBackupRepository;
        public ProductionOrderProductionPlanService(IProductionOrderProductionPlanRepository ProductionOrderProductionPlanRepository
            , IWebHostEnvironment WebHostEnvironment
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderDetailRepository ProductionOrderDetailRepository
            , IProductionOrderProductionPlanMaterialService ProductionOrderProductionPlanMaterialService
            , IProductionOrderProductionPlanSemiService ProductionOrderProductionPlanSemiService
            , IProductionOrderProductionPlanSemiRepository ProductionOrderProductionPlanSemiRepository
            , IProductionOrderBOMRepository ProductionOrderBOMRepository
            , IProductionOrderBOMDetailRepository ProductionOrderBOMDetailRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryVehicleRepository categoryVehicleRepository
            , ICategoryFamilyRepository categoryFamilyRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IWarehouseInventoryRepository WarehouseInventoryRepository
            , IWarehouseRequestRepository WarehouseRequestRepository
            , IWarehouseRequestDetailService WarehouseRequestDetailService
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IMembershipRepository MembershipRepository
            , IBOMRepository BOMRepository
            , IProductionOrderBOMService ProductionOrderBOMService
            , IProductionOrderProductionPlanBackupRepository ProductionOrderProductionPlanBackupRepository

            ) : base(ProductionOrderProductionPlanRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _ProductionOrderProductionPlanRepository = ProductionOrderProductionPlanRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderDetailRepository = ProductionOrderDetailRepository;
            _ProductionOrderProductionPlanMaterialService = ProductionOrderProductionPlanMaterialService;
            _ProductionOrderProductionPlanSemiService = ProductionOrderProductionPlanSemiService;
            _ProductionOrderProductionPlanSemiRepository = ProductionOrderProductionPlanSemiRepository;
            _ProductionOrderBOMRepository = ProductionOrderBOMRepository;
            _ProductionOrderBOMDetailRepository = ProductionOrderBOMDetailRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryVehicleRepository = categoryVehicleRepository;
            _CategoryFamilyRepository = categoryFamilyRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WarehouseInventoryRepository = WarehouseInventoryRepository;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _WarehouseRequestDetailService = WarehouseRequestDetailService;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _MembershipRepository = MembershipRepository;
            _BOMRepository = BOMRepository;
            _ProductionOrderBOMService = ProductionOrderBOMService;
            _ProductionOrderProductionPlanBackupRepository = ProductionOrderProductionPlanBackupRepository;
        }
        public override void Initialization(ProductionOrderProductionPlan model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Code = Parent.Code;
                model.Name = Parent.Name;
                model.Active = model.Active ?? Parent.Active;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;

                if (model.ID == 0)
                {
                    var ProductionOrderDetail = _ProductionOrderDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID).FirstOrDefault();
                    if (ProductionOrderDetail != null && ProductionOrderDetail.ID > 0)
                    {
                        model.BOMECN = model.BOMECN ?? ProductionOrderDetail.BOMECN;
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
                model.MaterialID = Material.ID;
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.QuantitySNP = Material.QuantitySNP;
            }


            var Date01 = model.Date01;
            var No = 2;
            var NoString = No.ToString();
            if (No < 10)
            {
                NoString = "0" + NoString;
            }
            var NameDate = "Date" + NoString;
            foreach (PropertyInfo proDate in model.GetType().GetProperties())
            {
                NoString = No.ToString();
                if (No < 10)
                {
                    NoString = "0" + NoString;
                }
                NameDate = "Date" + NoString;
                if (proDate.Name == NameDate)
                {
                    var day = No - 1;
                    proDate.SetValue(model, Date01?.AddDays(day), null);
                    No = No + 1;
                }
            }



            SQLHelper.InitializationNumber<ProductionOrderProductionPlan>(model);
            SQLHelper.InitializationDateTimeName<ProductionOrderProductionPlan>(model);

            model.Priority = model.Priority ?? 1;

            model.Quantity00 = GlobalHelper.InitializationNumber;
            model.QuantityCut00 = GlobalHelper.InitializationNumber;
            model.QuantityActual00 = GlobalHelper.InitializationNumber;

            SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlan>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderProductionPlan>(model);
            SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlan>(model);
            SQLHelper.InitializationQuantityCut00<ProductionOrderProductionPlan>(model);

            model.QuantityGAP00 = model.Quantity00 - model.QuantityActual00;

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
                else
                {
                    model.Active = true;
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderProductionPlan>> SaveAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                //var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.BOMECN == BaseParameter.BaseModel.BOMECN && o.BOMECNVersion == BaseParameter.BaseModel.BOMECNVersion).FirstOrDefaultAsync();
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            //await SyncWarehouseRequestAsync(BaseParameter);
            //await SyncProductionOrderBOMAsync(BaseParameter);
            //await SyncProductionOrderProductionPlanMaterialAsync(BaseParameter);
            await SyncProductionOrderProductionPlanBackupAsync(BaseParameter);
            //await SyncProductionOrderProductionPlanAsync(BaseParameter);
            await SyncProductionOrderProductionPlanSemiAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncProductionOrderProductionPlanBackupAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        ProductionOrderProductionPlanBackup ProductionOrderProductionPlanBackup = new ProductionOrderProductionPlanBackup();
                        foreach (PropertyInfo proProductionOrderProductionPlan in BaseParameter.BaseModel.GetType().GetProperties())
                        {
                            foreach (PropertyInfo proProductionOrderProductionPlanBackup in ProductionOrderProductionPlanBackup.GetType().GetProperties())
                            {
                                if (proProductionOrderProductionPlan.Name == proProductionOrderProductionPlanBackup.Name)
                                {
                                    if (proProductionOrderProductionPlan.Name == "ID")
                                    {
                                    }
                                    else
                                    {
                                        proProductionOrderProductionPlanBackup.SetValue(ProductionOrderProductionPlanBackup, proProductionOrderProductionPlan.GetValue(BaseParameter.BaseModel), null);
                                    }
                                }
                            }
                        }

                        if (BaseParameter.BaseModel.SortOrder == 0)
                        {
                            var ProductionOrderProductionPlanBackupCheck = await _ProductionOrderProductionPlanBackupRepository.GetByCondition(o => o.ParentID == ProductionOrderProductionPlanBackup.ParentID && o.SortOrder == ProductionOrderProductionPlanBackup.SortOrder).FirstOrDefaultAsync();
                            if (ProductionOrderProductionPlanBackupCheck == null)
                            {
                                await _ProductionOrderProductionPlanBackupRepository.AddAsync(ProductionOrderProductionPlanBackup);
                            }
                        }
                        else
                        {
                            await _ProductionOrderProductionPlanBackupRepository.AddAsync(ProductionOrderProductionPlanBackup);
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncProductionOrderBOMAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
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
                                var BOM = await _BOMRepository.GetByCondition(o => o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Active == true).FirstOrDefaultAsync();
                                if ((BOM != null) && (BOM.ID > 0))
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
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public async Task<BaseResult<ProductionOrderProductionPlan>> SyncWarehouseRequestAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ProductionOrderDetailID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                var ProductionOrderBOM = await _ProductionOrderBOMRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                                if ((ProductionOrderBOM != null) && (ProductionOrderBOM.ID > 0))
                                {
                                    var ListProductionOrderBOMDetail = await _ProductionOrderBOMDetailRepository.GetByCondition(o => o.ProductionOrderBOMID == ProductionOrderBOM.ID).ToListAsync();
                                    if (ListProductionOrderBOMDetail.Count > 0)
                                    {
                                        var ListProductionOrderBOMDetailDisplay = ListProductionOrderBOMDetail.Select(o => o.Display).Distinct().ToList();
                                        if (ListProductionOrderBOMDetailDisplay.Count > 0)
                                        {
                                            var ListCategoryDepartment = await _CategoryDepartmentRepository.GetAllToListAsync();
                                            var ListCategoryDepartmentOK = new List<CategoryDepartment>();
                                            foreach (var Display in ListProductionOrderBOMDetailDisplay)
                                            {
                                                var DepartmentID = GlobalHelper.DepartmentIDCutting;
                                                if (!string.IsNullOrEmpty(Display))
                                                {
                                                    var CategoryDepartment = ListCategoryDepartment.Where(o => o.Note != null && o.Note.ToLower().Contains(Display.ToLower())).FirstOrDefault();
                                                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                                                    {
                                                        DepartmentID = CategoryDepartment.ID;
                                                    }
                                                }
                                                var CategoryDepartmentOK = ListCategoryDepartment.Where(o => o.ID == DepartmentID).FirstOrDefault();
                                                if (CategoryDepartmentOK != null && CategoryDepartmentOK.ID > 0)
                                                {
                                                    CategoryDepartmentOK.Display = Display;
                                                    ListCategoryDepartmentOK.Add(CategoryDepartmentOK);
                                                }
                                            }
                                            foreach (var CategoryDepartment in ListCategoryDepartmentOK)
                                            {
                                                foreach (PropertyInfo proProductionOrderProductionPlan in BaseParameter.BaseModel.GetType().GetProperties())
                                                {
                                                    Type typeProductionOrderProductionPlan = Nullable.GetUnderlyingType(proProductionOrderProductionPlan.PropertyType) ?? proProductionOrderProductionPlan.PropertyType;
                                                    if (proProductionOrderProductionPlan.Name.Contains("Active") && proProductionOrderProductionPlan.GetValue(BaseParameter.BaseModel) != null && (bool?)proProductionOrderProductionPlan.GetValue(BaseParameter.BaseModel) == true && proProductionOrderProductionPlan.Name != "Active")
                                                    {
                                                        var Index = proProductionOrderProductionPlan.Name.Substring(6);
                                                        try
                                                        {
                                                            var IndexNumber = int.Parse(Index);
                                                            var IndexNumberString = IndexNumber.ToString();
                                                            if (IndexNumber < 10)
                                                            {
                                                                IndexNumberString = "0" + IndexNumberString;
                                                            }
                                                            if (IndexNumber > 0)
                                                            {
                                                                var WarehouseRequest = new WarehouseRequest();
                                                                WarehouseRequest.ParentID = BaseParameter.BaseModel.ParentID;
                                                                WarehouseRequest.CustomerID = CategoryDepartment.ID;
                                                                WarehouseRequest.Display = BaseParameter.BaseModel.MaterialCode;
                                                                var Quantity = GlobalHelper.InitializationNumber;
                                                                var ProductionOrderProductionPlanDateName = "Date" + IndexNumberString;
                                                                foreach (PropertyInfo proProductionOrderProductionPlanDate in BaseParameter.BaseModel.GetType().GetProperties())
                                                                {
                                                                    Type typeProductionOrderProductionPlanDate = Nullable.GetUnderlyingType(proProductionOrderProductionPlanDate.PropertyType) ?? proProductionOrderProductionPlanDate.PropertyType;
                                                                    if (proProductionOrderProductionPlanDate.Name == ProductionOrderProductionPlanDateName)
                                                                    {
                                                                        WarehouseRequest.Date = (DateTime?)proProductionOrderProductionPlanDate.GetValue(BaseParameter.BaseModel);
                                                                        break;
                                                                    }
                                                                }
                                                                var ProductionOrderProductionPlanQuantity = "Quantity" + IndexNumberString;
                                                                foreach (PropertyInfo proProductionOrderProductionPlanQuantity in BaseParameter.BaseModel.GetType().GetProperties())
                                                                {
                                                                    Type typeProductionOrderProductionPlanQuantity = Nullable.GetUnderlyingType(proProductionOrderProductionPlanQuantity.PropertyType) ?? proProductionOrderProductionPlanQuantity.PropertyType;
                                                                    if (proProductionOrderProductionPlanQuantity.Name == ProductionOrderProductionPlanQuantity)
                                                                    {
                                                                        Quantity = (int)proProductionOrderProductionPlanQuantity.GetValue(BaseParameter.BaseModel);
                                                                        break;
                                                                    }
                                                                }
                                                                WarehouseRequestInitialization(WarehouseRequest);
                                                                var ModelCheck = await _WarehouseRequestRepository.GetByCondition(o => o.ParentID == WarehouseRequest.ParentID && o.Code == WarehouseRequest.Code && o.Year == WarehouseRequest.Year && o.Month == WarehouseRequest.Month && o.Day == WarehouseRequest.Day).FirstOrDefaultAsync();
                                                                if (ModelCheck != null)
                                                                {
                                                                    if (ModelCheck.ID > 0)
                                                                    {
                                                                        WarehouseRequest.ID = ModelCheck.ID;
                                                                    }
                                                                }
                                                                if (WarehouseRequest.ID > 0)
                                                                {
                                                                    await _WarehouseRequestRepository.UpdateAsync(WarehouseRequest);
                                                                }
                                                                else
                                                                {
                                                                    await _WarehouseRequestRepository.AddAsync(WarehouseRequest);
                                                                }
                                                                if (WarehouseRequest.ID > 0)
                                                                {
                                                                    await WarehouseRequestSendMailAsync(WarehouseRequest);
                                                                    var ListProductionOrderBOMDetailOK = ListProductionOrderBOMDetail.Where(o => o.Display == CategoryDepartment.Display).ToList();
                                                                    foreach (var item in ListProductionOrderBOMDetailOK)
                                                                    {
                                                                        var WarehouseRequestDetail = new WarehouseRequestDetail();
                                                                        WarehouseRequestDetail.ParentID = WarehouseRequest.ID;
                                                                        WarehouseRequestDetail.MaterialID = item.MaterialID;
                                                                        WarehouseRequestDetail.CategoryFamilyID = item.CategoryFamilyID;
                                                                        WarehouseRequestDetail.CategoryUnitID = item.CategoryUnitID;
                                                                        WarehouseRequestDetail.Display = item.Display;
                                                                        WarehouseRequestDetail.QuantityInvoice = item.QuantityBOM * Quantity;
                                                                        WarehouseRequestDetail.Quantity = (int)WarehouseRequestDetail.QuantityInvoice;
                                                                        var DecimalPart = WarehouseRequestDetail.QuantityInvoice - WarehouseRequestDetail.Quantity;
                                                                        if (DecimalPart > 0)
                                                                        {
                                                                            WarehouseRequestDetail.Quantity = WarehouseRequestDetail.Quantity + 1;
                                                                        }
                                                                        var BaseParameterWarehouseRequestDetail = new BaseParameter<WarehouseRequestDetail>();
                                                                        BaseParameterWarehouseRequestDetail.BaseModel = WarehouseRequestDetail;
                                                                        await _WarehouseRequestDetailService.SaveAsync(BaseParameterWarehouseRequestDetail);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public void WarehouseRequestInitialization(WarehouseRequest model)
        {
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Year ?? model.Date.Value.Year;
            model.Month = model.Month ?? model.Date.Value.Month;
            model.Day = model.Day ?? model.Date.Value.Day;
            model.SupplierID = model.SupplierID ?? GlobalHelper.DepartmentID;
            model.CustomerID = model.CustomerID ?? GlobalHelper.DepartmentID;
            if (model.SupplierID > 0)
            {
                var Customer = _CategoryDepartmentRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = Customer.Name;
            }
            if (model.CustomerID > 0)
            {
                var Customer = _CategoryDepartmentRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = Customer.Name;
            }
            model.Code = model.Code ?? model.ParentName + "-" + model.Display + "-" + model.Date.Value.ToString("yyyyMMdd") + "-" + model.CustomerName;
            model.Total = model.Total ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * (model.Tax / 100);
            model.TotalDiscount = model.Total * (model.Discount / 100);
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public async Task<int> WarehouseRequestSendMailAsync(WarehouseRequest model)
        {
            var result = GlobalHelper.InitializationNumber;
            if (model != null)
            {
                if (model.ID > 0)
                {
                    var ListMembership = await _MembershipRepository.GetByCondition(o => o.Active == true && o.CategoryPositionID == GlobalHelper.PositionID && (o.CategoryDepartmentID == model.SupplierID || o.CategoryDepartmentID == model.CustomerID)).ToListAsync();
                    foreach (var Membership in ListMembership)
                    {
                        if (!string.IsNullOrEmpty(Membership.Email))
                        {
                            string HTMLContent = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseRequestEmail.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLContent = r.ReadToEnd();
                                }
                            }

                            HTMLContent = HTMLContent.Replace(@"[ERPSite]", GlobalHelper.ERPSite);
                            HTMLContent = HTMLContent.Replace(@"[ID]", model.ID.ToString());
                            HTMLContent = HTMLContent.Replace(@"[Code]", model.Code);
                            HTMLContent = HTMLContent.Replace(@"[ParentName]", model.ParentName);
                            HTMLContent = HTMLContent.Replace(@"[Date]", model.Date.Value.ToString("yyyy-MM-dd"));

                            if (!string.IsNullOrEmpty(HTMLContent))
                            {
                                Helper.Model.Mail mail = new Helper.Model.Mail();
                                mail.MailFrom = GlobalHelper.MasterEmailUser;
                                mail.UserName = GlobalHelper.MasterEmailUser;
                                mail.Password = GlobalHelper.MasterEmailPassword;
                                mail.SMTPPort = GlobalHelper.SMTPPort;
                                mail.SMTPServer = GlobalHelper.SMTPServer;
                                mail.IsMailBodyHtml = GlobalHelper.IsMailBodyHtml;
                                mail.IsMailUsingSSL = GlobalHelper.IsMailUsingSSL;
                                mail.Display = GlobalHelper.MasterEmailDisplay;
                                mail.Subject = model.Code + " - " + GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy HH:mm:ss") + " | Request";
                                mail.Content = HTMLContent;
                                mail.MailTo = Membership.Email;
                                MailHelper.SendMail(mail);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncProductionOrderProductionPlanMaterialAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
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
                                var ListProductionOrderBOMDetail = await _ProductionOrderBOMDetailRepository.GetByCondition(o => o.ProductionOrderDetailID == BaseParameter.BaseModel.ProductionOrderDetailID).ToListAsync();
                                if (ListProductionOrderBOMDetail.Count > 0)
                                {
                                    foreach (var item in ListProductionOrderBOMDetail)
                                    {
                                        var ProductionOrderProductionPlanMaterial = new ProductionOrderProductionPlanMaterial();
                                        ProductionOrderProductionPlanMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                        ProductionOrderProductionPlanMaterial.ProductionOrderDetailID = BaseParameter.BaseModel.ProductionOrderDetailID;
                                        ProductionOrderProductionPlanMaterial.ProductionOrderProductionPlanID = BaseParameter.BaseModel.ID;
                                        ProductionOrderProductionPlanMaterial.MaterialID01 = BaseParameter.BaseModel.MaterialID;
                                        ProductionOrderProductionPlanMaterial.Priority = BaseParameter.BaseModel.Priority;
                                        ProductionOrderProductionPlanMaterial.MaterialID = item.MaterialID;
                                        ProductionOrderProductionPlanMaterial.CategoryUnitID = item.CategoryUnitID;
                                        ProductionOrderProductionPlanMaterial.CategoryFamilyID = item.CategoryFamilyID;
                                        ProductionOrderProductionPlanMaterial.QuantityBOM = item.QuantityBOM;
                                        ProductionOrderProductionPlanMaterial.Display = item.Display;
                                        ProductionOrderProductionPlanMaterial.BOMID = item.BOMID;

                                        var BaseParameterProductionOrderProductionPlanMaterial = new BaseParameter<ProductionOrderProductionPlanMaterial>();
                                        BaseParameterProductionOrderProductionPlanMaterial.BaseModel = ProductionOrderProductionPlanMaterial;
                                        await _ProductionOrderProductionPlanMaterialService.SaveAsync(BaseParameterProductionOrderProductionPlanMaterial);
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncProductionOrderProductionPlanAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
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
                                var ProductionOrderProductionPlan = new ProductionOrderProductionPlan();
                                ProductionOrderProductionPlan.ParentID = BaseParameter.BaseModel.ParentID;
                                ProductionOrderProductionPlan.MaterialID = GlobalHelper.InitializationNumber;
                                ProductionOrderProductionPlan.SortOrder = -1;
                                ProductionOrderProductionPlan.Priority = -1;
                                var ModelCheck = await GetByCondition(o => o.ParentID == ProductionOrderProductionPlan.ParentID && o.SortOrder == ProductionOrderProductionPlan.SortOrder).FirstOrDefaultAsync();
                                if (ModelCheck == null)
                                {
                                    foreach (PropertyInfo pro in BaseParameter.BaseModel.GetType().GetProperties())
                                    {
                                        if (pro.Name.Contains("Date") && !pro.Name.Contains("DatePO"))
                                        {
                                            foreach (PropertyInfo proDate in ProductionOrderProductionPlan.GetType().GetProperties())
                                            {
                                                if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO"))
                                                {
                                                    if (proDate.Name == pro.Name)
                                                    {
                                                        try
                                                        {
                                                            proDate.SetValue(ProductionOrderProductionPlan, pro.GetValue(BaseParameter.BaseModel), null);
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
                                    await _ProductionOrderProductionPlanRepository.AddAsync(ProductionOrderProductionPlan);
                                    BaseParameter<ProductionOrderProductionPlan> BaseParameterProductionOrderProductionPlan = new BaseParameter<ProductionOrderProductionPlan>();
                                    BaseParameterProductionOrderProductionPlan.BaseModel = ProductionOrderProductionPlan;
                                    await SyncProductionOrderProductionPlanBackupAsync(BaseParameterProductionOrderProductionPlan);
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncProductionOrderProductionPlanSemiAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
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
                                if (BaseParameter.BaseModel.BOMID > 0)
                                {
                                    var ListProductionOrderProductionPlanSemiAdd = new List<ProductionOrderProductionPlanSemi>();
                                    var ListProductionOrderProductionPlanSemiUpdate = new List<ProductionOrderProductionPlanSemi>();
                                    var ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.ProductionOrderProductionPlanID == BaseParameter.BaseModel.ID).ToListAsync();
                                    var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.BOMID && o.IsLeadNo == true).OrderBy(o => o.MaterialCode).ToListAsync();
                                    foreach (var BOM in ListBOM)
                                    {
                                        var ProductionOrderProductionPlanSemi = ListProductionOrderProductionPlanSemi.Where(o => o.ProductionOrderProductionPlanID == BaseParameter.BaseModel.ID && o.BOMID == BOM.ID).FirstOrDefault();
                                        if (ProductionOrderProductionPlanSemi == null)
                                        {
                                            ProductionOrderProductionPlanSemi = new ProductionOrderProductionPlanSemi();
                                            ProductionOrderProductionPlanSemi.Priority = BaseParameter.BaseModel.Priority;
                                            ProductionOrderProductionPlanSemi.ParentID = BaseParameter.BaseModel.ParentID;
                                            ProductionOrderProductionPlanSemi.ProductionOrderProductionPlanID = BaseParameter.BaseModel.ID;
                                            ProductionOrderProductionPlanSemi.MaterialID01 = BaseParameter.BaseModel.MaterialID;
                                            ProductionOrderProductionPlanSemi.MaterialCode01 = BaseParameter.BaseModel.MaterialCode;
                                            ProductionOrderProductionPlanSemi.BOMID01 = BaseParameter.BaseModel.BOMID;
                                            ProductionOrderProductionPlanSemi.BOMECN01 = BaseParameter.BaseModel.BOMECN;
                                            ProductionOrderProductionPlanSemi.BOMECNVersion01 = BaseParameter.BaseModel.BOMECNVersion;
                                            ProductionOrderProductionPlanSemi.BOMID = BOM.ID;
                                            ProductionOrderProductionPlanSemi.BOMECN = BOM.Code;
                                            ProductionOrderProductionPlanSemi.BOMECNVersion = BOM.Version;
                                            ProductionOrderProductionPlanSemi.IsLeadNo = BOM.IsLeadNo;
                                            ProductionOrderProductionPlanSemi.IsSPST = BOM.IsSPST;
                                            ProductionOrderProductionPlanSemi.MaterialID = BOM.MaterialID;
                                            ProductionOrderProductionPlanSemi.MaterialCode = BOM.MaterialCode;
                                            ProductionOrderProductionPlanSemi.SortOrder = 10;
                                            ProductionOrderProductionPlanSemi.ParentID01 = BOM.ParentID01;
                                            ProductionOrderProductionPlanSemi.ParentName01 = BOM.ParentName01;
                                            ProductionOrderProductionPlanSemi.ParentID02 = BOM.ParentID02;
                                            ProductionOrderProductionPlanSemi.ParentName02 = BOM.ParentName02;
                                            ProductionOrderProductionPlanSemi.ParentID03 = BOM.ParentID03;
                                            ProductionOrderProductionPlanSemi.ParentName03 = BOM.ParentName03;
                                            ProductionOrderProductionPlanSemi.ParentID04 = BOM.ParentID04;
                                            ProductionOrderProductionPlanSemi.ParentName04 = BOM.ParentName04;
                                            try
                                            {
                                                ProductionOrderProductionPlanSemi.BOMQuantity = 0;
                                                var ListBOMSub = ListBOM.Where(o => o.MaterialID == BOM.MaterialID).OrderBy(o => o.ID).ToList();
                                                if (ListBOMSub.Count > 0)
                                                {
                                                    if (BOM.ID == ListBOMSub[0].ID)
                                                    {
                                                        ProductionOrderProductionPlanSemi.BOMQuantity = ListBOMSub.Sum(o => (int?)o.Quantity);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string mes = ex.Message;
                                            }
                                            ListProductionOrderProductionPlanSemiAdd.Add(ProductionOrderProductionPlanSemi);
                                        }
                                        else
                                        {
                                            ListProductionOrderProductionPlanSemiUpdate.Add(ProductionOrderProductionPlanSemi);
                                        }
                                    }
                                    ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.BOMID && o.IsSPST == true).OrderBy(o => o.MaterialCode).ToListAsync();
                                    foreach (var BOM in ListBOM)
                                    {
                                        var ProductionOrderProductionPlanSemi = ListProductionOrderProductionPlanSemi.Where(o => o.ProductionOrderProductionPlanID == BaseParameter.BaseModel.ID && o.BOMID == BOM.ID).FirstOrDefault();
                                        if (ProductionOrderProductionPlanSemi == null)
                                        {
                                            ProductionOrderProductionPlanSemi = new ProductionOrderProductionPlanSemi();
                                            ProductionOrderProductionPlanSemi.Priority = BaseParameter.BaseModel.Priority;
                                            ProductionOrderProductionPlanSemi.ParentID = BaseParameter.BaseModel.ParentID;
                                            ProductionOrderProductionPlanSemi.ProductionOrderProductionPlanID = BaseParameter.BaseModel.ID;
                                            ProductionOrderProductionPlanSemi.MaterialID01 = BaseParameter.BaseModel.MaterialID;
                                            ProductionOrderProductionPlanSemi.MaterialCode01 = BaseParameter.BaseModel.MaterialCode;
                                            ProductionOrderProductionPlanSemi.BOMID01 = BaseParameter.BaseModel.BOMID;
                                            ProductionOrderProductionPlanSemi.BOMECN01 = BaseParameter.BaseModel.BOMECN;
                                            ProductionOrderProductionPlanSemi.BOMECNVersion01 = BaseParameter.BaseModel.BOMECNVersion;
                                            ProductionOrderProductionPlanSemi.BOMID = BOM.ID;
                                            ProductionOrderProductionPlanSemi.BOMECN = BOM.Code;
                                            ProductionOrderProductionPlanSemi.BOMECNVersion = BOM.Version;
                                            ProductionOrderProductionPlanSemi.IsLeadNo = BOM.IsLeadNo;
                                            ProductionOrderProductionPlanSemi.IsSPST = BOM.IsSPST;
                                            ProductionOrderProductionPlanSemi.MaterialID = BOM.MaterialID;
                                            ProductionOrderProductionPlanSemi.MaterialCode = BOM.MaterialCode;
                                            ProductionOrderProductionPlanSemi.SortOrder = 20;
                                            ProductionOrderProductionPlanSemi.ParentID01 = BOM.ParentID01;
                                            ProductionOrderProductionPlanSemi.ParentName01 = BOM.ParentName01;
                                            ProductionOrderProductionPlanSemi.ParentID02 = BOM.ParentID02;
                                            ProductionOrderProductionPlanSemi.ParentName02 = BOM.ParentName02;
                                            ProductionOrderProductionPlanSemi.ParentID03 = BOM.ParentID03;
                                            ProductionOrderProductionPlanSemi.ParentName03 = BOM.ParentName03;
                                            ProductionOrderProductionPlanSemi.ParentID04 = BOM.ParentID04;
                                            ProductionOrderProductionPlanSemi.ParentName04 = BOM.ParentName04;
                                            try
                                            {
                                                ProductionOrderProductionPlanSemi.BOMQuantity = 0;
                                                var ListBOMSub = ListBOM.Where(o => o.MaterialID == BOM.MaterialID).OrderBy(o => o.ID).ToList();
                                                if (ListBOMSub.Count > 0)
                                                {
                                                    if (BOM.ID == ListBOMSub[0].ID)
                                                    {
                                                        ProductionOrderProductionPlanSemi.BOMQuantity = ListBOMSub.Sum(o => (int?)o.Quantity);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string mes = ex.Message;
                                            }
                                            ListProductionOrderProductionPlanSemiAdd.Add(ProductionOrderProductionPlanSemi);
                                        }
                                        else
                                        {
                                            ListProductionOrderProductionPlanSemiUpdate.Add(ProductionOrderProductionPlanSemi);
                                        }
                                    }
                                    if (ListProductionOrderProductionPlanSemiAdd.Count > 0)
                                    {
                                        for (int i = 0; i < ListProductionOrderProductionPlanSemiAdd.Count; i++)
                                        {
                                            ProductionOrderProductionPlanSemiInitialization(ListProductionOrderProductionPlanSemiAdd[i], BaseParameter.BaseModel);
                                        }
                                        await _ProductionOrderProductionPlanSemiRepository.AddRangeAsync(ListProductionOrderProductionPlanSemiAdd);

                                        //ProductionOrderProductionPlanSemi ProductionOrderProductionPlanSemiEmpty = ListProductionOrderProductionPlanSemiAdd[0];
                                        //ProductionOrderProductionPlanSemiEmpty.ID = 0;
                                        //ProductionOrderProductionPlanSemiEmpty.MaterialID01 = GlobalHelper.InitializationNumber;
                                        //ProductionOrderProductionPlanSemiEmpty.MaterialID = GlobalHelper.InitializationNumber;
                                        //ProductionOrderProductionPlanSemiEmpty.MaterialCode = GlobalHelper.InitializationString;
                                        //ProductionOrderProductionPlanSemiEmpty.MaterialCode01 = GlobalHelper.InitializationString;
                                        //ProductionOrderProductionPlanSemiEmpty.SortOrder = -1;
                                        //ProductionOrderProductionPlanSemiEmpty.Priority = -1;
                                        //ProductionOrderProductionPlanSemiEmpty.IsLeadNo = null;
                                        //ProductionOrderProductionPlanSemiEmpty.IsSPST = null;
                                        //var ModelCheck = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.ParentID == ProductionOrderProductionPlanSemiEmpty.ParentID && o.SortOrder == ProductionOrderProductionPlanSemiEmpty.SortOrder).FirstOrDefaultAsync();
                                        //if (ModelCheck == null)
                                        //{
                                        //    await _ProductionOrderProductionPlanSemiRepository.AddAsync(ProductionOrderProductionPlanSemiEmpty);
                                        //}
                                    }
                                    if (ListProductionOrderProductionPlanSemiUpdate.Count > 0)
                                    {
                                        for (int i = 0; i < ListProductionOrderProductionPlanSemiUpdate.Count; i++)
                                        {
                                            ProductionOrderProductionPlanSemiInitialization(ListProductionOrderProductionPlanSemiUpdate[i], BaseParameter.BaseModel);
                                        }
                                        await _ProductionOrderProductionPlanSemiRepository.UpdateRangeAsync(ListProductionOrderProductionPlanSemiUpdate);
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
        private void ProductionOrderProductionPlanSemiInitialization(ProductionOrderProductionPlanSemi model, ProductionOrderProductionPlan ProductionOrderProductionPlan)
        {
            model.BOMQuantity = model.BOMQuantity ?? 1;
            if (model.ProductionOrderProductionPlanID > 0)
            {
                model.Active = ProductionOrderProductionPlan.Active;
                model.ParentID = ProductionOrderProductionPlan.ParentID;
                model.ParentName = ProductionOrderProductionPlan.ParentName;
                model.Code = ProductionOrderProductionPlan.Code;
                model.Name = ProductionOrderProductionPlan.Name;
                model.CompanyID = ProductionOrderProductionPlan.CompanyID;
                model.CompanyName = ProductionOrderProductionPlan.CompanyName;
                model.ProductionOrderDetailID = ProductionOrderProductionPlan.ProductionOrderDetailID;
                model.MaterialID01 = ProductionOrderProductionPlan.MaterialID;
                model.MaterialCode01 = ProductionOrderProductionPlan.MaterialCode;
                model.MaterialName01 = ProductionOrderProductionPlan.MaterialName;

                int Index = 1;
                foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfo in ProductionOrderProductionPlan.GetType().GetProperties())
                {
                    foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in model.GetType().GetProperties())
                    {
                        string IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + Index.ToString();
                        }
                        string DateString = "Date" + IndexName;
                        string DateNameString = "Date" + IndexName + "Name";
                        string QuantityNameString = "Quantity" + IndexName;
                        string QuantityCutNameString = "QuantityCut" + IndexName;
                        string ActiveNameString = "Active" + IndexName;
                        string QuantityActualNameString = "QuantityActual" + IndexName;
                        if (ProductionOrderProductionPlanPropertyInfo.Name == ProductionOrderProductionPlanSemiPropertyInfo.Name)
                        {
                            if (ProductionOrderProductionPlanPropertyInfo.Name == DateString)
                            {
                                ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan), null);
                            }

                            if (ProductionOrderProductionPlanPropertyInfo.Name == DateNameString)
                            {
                                ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan), null);
                            }

                            if (ProductionOrderProductionPlanPropertyInfo.Name == QuantityNameString)
                            {
                                try
                                {
                                    foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfoCut in ProductionOrderProductionPlan.GetType().GetProperties())
                                    {
                                        if (ProductionOrderProductionPlanPropertyInfoCut.Name == QuantityCutNameString)
                                        {
                                            int? ProductionPlanQuantityCut = (int?)ProductionOrderProductionPlanPropertyInfoCut.GetValue(ProductionOrderProductionPlan);
                                            int? Quantity = model.BOMQuantity * ProductionPlanQuantityCut;
                                            ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, Quantity, null);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                Index = Index + 1;
                            }
                        }
                    }
                }
            }
            model.Priority = model.Priority ?? 1;
            model.Quantity00 = GlobalHelper.InitializationNumber;
            model.QuantityActual00 = GlobalHelper.InitializationNumber;
            SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanSemi>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(model);
            SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(model);
            ProductionOrderProductionPlanSemiSetQuantityGAP(model);
            model.QuantityGAP00 = model.Quantity00 - model.QuantityActual00;
        }
        private void ProductionOrderProductionPlanSemiSetQuantityGAP(ProductionOrderProductionPlanSemi model)
        {
            var Index01 = 1;
            foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in model.GetType().GetProperties())
            {

                string IndexName = Index01.ToString();
                if (Index01 < 10)
                {
                    IndexName = "0" + Index01.ToString();
                }
                string QuantityGAPName = "QuantityGAP" + IndexName;
                if (ProductionOrderProductionPlanSemiPropertyInfo.Name == QuantityGAPName)
                {
                    int QuantityGAP = 0;
                    if (ProductionOrderProductionPlanSemiPropertyInfo.GetValue(model) != null)
                    {
                        QuantityGAP = (int)ProductionOrderProductionPlanSemiPropertyInfo.GetValue(model);
                    }
                    if (Index01 > 1)
                    {
                        var Index02 = Index01 - 1;
                        IndexName = Index02.ToString();
                        if (Index02 < 10)
                        {
                            IndexName = "0" + Index02.ToString();
                        }
                        QuantityGAPName = "QuantityGAP" + IndexName;
                        foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfoSub in model.GetType().GetProperties())
                        {
                            if (ProductionOrderProductionPlanSemiPropertyInfoSub.Name == QuantityGAPName)
                            {
                                if (ProductionOrderProductionPlanSemiPropertyInfoSub.GetValue(model) != null)
                                {
                                    QuantityGAP = QuantityGAP + (int)ProductionOrderProductionPlanSemiPropertyInfoSub.GetValue(model);
                                    break;
                                }
                            }
                        }
                    }
                    ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, QuantityGAP, null);
                    Index01 = Index01 + 1;
                }
            }
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncQuantityToQuantityCutAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.SortOrder > 0).ToListAsync();
                        foreach (var ProductionOrderProductionPlan in ListProductionOrderProductionPlan)
                        {
                            int Index = 1;
                            foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                            {
                                string IndexName = Index.ToString();
                                if (Index < 10)
                                {
                                    IndexName = "0" + IndexName;
                                }
                                string QuantityName = "Quantity" + IndexName;
                                string QuantityCutName = "QuantityCut" + IndexName;
                                if (proQuantity.Name == QuantityName)
                                {
                                    foreach (PropertyInfo proQuantityCut in ProductionOrderProductionPlan.GetType().GetProperties())
                                    {
                                        if (proQuantityCut.Name == QuantityCutName)
                                        {
                                            proQuantityCut.SetValue(ProductionOrderProductionPlan, proQuantity.GetValue(ProductionOrderProductionPlan), null);
                                        }
                                    }
                                    Index = Index + 1;
                                }
                            }
                            SQLHelper.InitializationQuantityCut00<ProductionOrderProductionPlan>(ProductionOrderProductionPlan);
                            await _ProductionOrderProductionPlanRepository.UpdateAsync(ProductionOrderProductionPlan);
                            var ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.ProductionOrderProductionPlanID == ProductionOrderProductionPlan.ID && o.SortOrder > 1).ToListAsync();
                            Index = 1;
                            foreach (PropertyInfo proQuantityCut in ProductionOrderProductionPlan.GetType().GetProperties())
                            {
                                string IndexName = Index.ToString();
                                if (Index < 10)
                                {
                                    IndexName = "0" + IndexName;
                                }
                                string QuantityName = "Quantity" + IndexName;
                                string QuantityCutName = "QuantityCut" + IndexName;
                                string DateString = "Date" + IndexName;
                                string QuantityActualNameString = "QuantityActual" + IndexName;
                                if (proQuantityCut.Name == QuantityCutName)
                                {
                                    try
                                    {
                                        if (proQuantityCut.GetValue(ProductionOrderProductionPlan) != null)
                                        {
                                            int QuantityCut = (int)proQuantityCut.GetValue(ProductionOrderProductionPlan);
                                            if (QuantityCut > 0)
                                            {
                                                for (int i = 0; i < ListProductionOrderProductionPlanSemi.Count; i++)
                                                {
                                                    foreach (PropertyInfo proQuantity in ListProductionOrderProductionPlanSemi[i].GetType().GetProperties())
                                                    {
                                                        if (proQuantity.Name == QuantityName)
                                                        {
                                                            int? Quantity = ListProductionOrderProductionPlanSemi[i].BOMQuantity * QuantityCut;
                                                            proQuantity.SetValue(ListProductionOrderProductionPlanSemi[i], Quantity, null);
                                                        }
                                                    }
                                                    SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                                    SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                                    SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                                    ProductionOrderProductionPlanSemiSetQuantityGAP(ListProductionOrderProductionPlanSemi[i]);
                                                    ListProductionOrderProductionPlanSemi[i].QuantityGAP00 = ListProductionOrderProductionPlanSemi[i].Quantity00 - ListProductionOrderProductionPlanSemi[i].QuantityActual00;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string mes = ex.Message;
                                    }
                                    Index = Index + 1;
                                }
                            }
                            await _ProductionOrderProductionPlanSemiRepository.UpdateRangeAsync(ListProductionOrderProductionPlanSemi);
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> ExportByParentIDToExcelAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            result.List = new List<ProductionOrderProductionPlan>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                if (result.List.Count > 0)
                {
                    string fileName = "ProductionOrderProductionPlan-" + BaseParameter.ParentID + "-" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcel(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        private void InitializationExcel(List<ProductionOrderProductionPlan> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "Line";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";

                list = list.OrderBy(o => o.ID).ToList();
                var ProductionOrderProductionPlan = list[0];
                int Index = 1;
                foreach (PropertyInfo proProductionOrderProductionPlan in ProductionOrderProductionPlan.GetType().GetProperties())
                {
                    string IndexName = Index.ToString();
                    if (Index < 10)
                    {
                        IndexName = "0" + IndexName;
                    }
                    string DateString = "Date" + IndexName;
                    if (proProductionOrderProductionPlan.Name == DateString)
                    {
                        try
                        {
                            DateTime DateTime = (DateTime)proProductionOrderProductionPlan.GetValue(ProductionOrderProductionPlan);
                            column = column + 1;
                            workSheet.Cells[row, column].Value = DateTime.ToString("MM/dd/yyyy");
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                        Index = Index + 1;
                    }
                }
                row = row + 1;
                list = list.OrderBy(o => o.Priority).ToList();
                foreach (ProductionOrderProductionPlan item in list)
                {
                    workSheet.Cells[row, 1].Value = item.CategoryLineName;
                    workSheet.Cells[row, 2].Value = item.MaterialCode;

                    Index = 1;
                    int columnIndex = 3;
                    foreach (PropertyInfo proProductionOrderProductionPlan in item.GetType().GetProperties())
                    {
                        string IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + IndexName;
                        }
                        string QuantityString = "Quantity" + IndexName;
                        if (proProductionOrderProductionPlan.Name == QuantityString)
                        {
                            try
                            {
                                int Quantity = (int)proProductionOrderProductionPlan.GetValue(item);
                                workSheet.Cells[row, columnIndex].Value = Quantity;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            Index = Index + 1;
                            columnIndex = columnIndex + 1;
                        }
                    }
                    row = row + 1;
                }

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 16;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                row = row + 1;

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            result.List = new List<ProductionOrderProductionPlan>();
            if (BaseParameter.ParentID > 0)
            {
                var ListAll = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                var List = ListAll;
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    List = ListAll.Where(o => !string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode.Contains(BaseParameter.SearchString)).ToList();
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.CategoryLineName) && o.CategoryLineName.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.BOMECN) && o.BOMECN.Contains(BaseParameter.SearchString)).ToList();
                    }
                }
                result.List = List;
                ProductionOrderProductionPlan ProductionOrderProductionPlanSemiSum = new ProductionOrderProductionPlan();
                ProductionOrderProductionPlanSemiSum.MaterialCode = "Sum";
                ProductionOrderProductionPlanSemiSum.Priority = 0;
                ProductionOrderProductionPlanSemiSum.Quantity00 = result.List.Sum(o => o.Quantity00);
                ProductionOrderProductionPlanSemiSum.QuantityActual00 = result.List.Sum(o => o.QuantityActual00);
                ProductionOrderProductionPlanSemiSum.QuantityGAP00 = result.List.Sum(o => o.QuantityGAP00);

                ProductionOrderProductionPlanSemiSum.Quantity01 = result.List.Sum(o => o.Quantity01);
                ProductionOrderProductionPlanSemiSum.QuantityActual01 = result.List.Sum(o => o.QuantityActual01);
                ProductionOrderProductionPlanSemiSum.QuantityGAP01 = result.List.Sum(o => o.QuantityGAP01);
                ProductionOrderProductionPlanSemiSum.QuantityCut01 = result.List.Sum(o => o.QuantityCut01);

                ProductionOrderProductionPlanSemiSum.Quantity02 = result.List.Sum(o => o.Quantity02);
                ProductionOrderProductionPlanSemiSum.QuantityActual02 = result.List.Sum(o => o.QuantityActual02);
                ProductionOrderProductionPlanSemiSum.QuantityGAP02 = result.List.Sum(o => o.QuantityGAP02);
                ProductionOrderProductionPlanSemiSum.QuantityCut02 = result.List.Sum(o => o.QuantityCut02);

                ProductionOrderProductionPlanSemiSum.Quantity03 = result.List.Sum(o => o.Quantity03);
                ProductionOrderProductionPlanSemiSum.QuantityActual03 = result.List.Sum(o => o.QuantityActual03);
                ProductionOrderProductionPlanSemiSum.QuantityGAP03 = result.List.Sum(o => o.QuantityGAP03);
                ProductionOrderProductionPlanSemiSum.QuantityCut03 = result.List.Sum(o => o.QuantityCut03);

                ProductionOrderProductionPlanSemiSum.Quantity04 = result.List.Sum(o => o.Quantity04);
                ProductionOrderProductionPlanSemiSum.QuantityActual04 = result.List.Sum(o => o.QuantityActual04);
                ProductionOrderProductionPlanSemiSum.QuantityGAP04 = result.List.Sum(o => o.QuantityGAP04);
                ProductionOrderProductionPlanSemiSum.QuantityCut04 = result.List.Sum(o => o.QuantityCut04);

                ProductionOrderProductionPlanSemiSum.Quantity05 = result.List.Sum(o => o.Quantity05);
                ProductionOrderProductionPlanSemiSum.QuantityActual05 = result.List.Sum(o => o.QuantityActual05);
                ProductionOrderProductionPlanSemiSum.QuantityGAP05 = result.List.Sum(o => o.QuantityGAP05);
                ProductionOrderProductionPlanSemiSum.QuantityCut05 = result.List.Sum(o => o.QuantityCut05);

                ProductionOrderProductionPlanSemiSum.Quantity06 = result.List.Sum(o => o.Quantity06);
                ProductionOrderProductionPlanSemiSum.QuantityActual06 = result.List.Sum(o => o.QuantityActual06);
                ProductionOrderProductionPlanSemiSum.QuantityGAP06 = result.List.Sum(o => o.QuantityGAP06);
                ProductionOrderProductionPlanSemiSum.QuantityCut06 = result.List.Sum(o => o.QuantityCut06);

                ProductionOrderProductionPlanSemiSum.Quantity07 = result.List.Sum(o => o.Quantity07);
                ProductionOrderProductionPlanSemiSum.QuantityActual07 = result.List.Sum(o => o.QuantityActual07);
                ProductionOrderProductionPlanSemiSum.QuantityGAP07 = result.List.Sum(o => o.QuantityGAP07);
                ProductionOrderProductionPlanSemiSum.QuantityCut07 = result.List.Sum(o => o.QuantityCut07);

                ProductionOrderProductionPlanSemiSum.Quantity08 = result.List.Sum(o => o.Quantity08);
                ProductionOrderProductionPlanSemiSum.QuantityActual08 = result.List.Sum(o => o.QuantityActual08);
                ProductionOrderProductionPlanSemiSum.QuantityGAP08 = result.List.Sum(o => o.QuantityGAP08);
                ProductionOrderProductionPlanSemiSum.QuantityCut08 = result.List.Sum(o => o.QuantityCut08);

                ProductionOrderProductionPlanSemiSum.Quantity09 = result.List.Sum(o => o.Quantity09);
                ProductionOrderProductionPlanSemiSum.QuantityActual09 = result.List.Sum(o => o.QuantityActual09);
                ProductionOrderProductionPlanSemiSum.QuantityGAP09 = result.List.Sum(o => o.QuantityGAP09);
                ProductionOrderProductionPlanSemiSum.QuantityCut09 = result.List.Sum(o => o.QuantityCut09);

                ProductionOrderProductionPlanSemiSum.Quantity10 = result.List.Sum(o => o.Quantity10);
                ProductionOrderProductionPlanSemiSum.QuantityActual10 = result.List.Sum(o => o.QuantityActual10);
                ProductionOrderProductionPlanSemiSum.QuantityGAP10 = result.List.Sum(o => o.QuantityGAP10);
                ProductionOrderProductionPlanSemiSum.QuantityCut10 = result.List.Sum(o => o.QuantityCut10);

                ProductionOrderProductionPlanSemiSum.Quantity11 = result.List.Sum(o => o.Quantity11);
                ProductionOrderProductionPlanSemiSum.QuantityActual11 = result.List.Sum(o => o.QuantityActual11);
                ProductionOrderProductionPlanSemiSum.QuantityGAP11 = result.List.Sum(o => o.QuantityGAP11);
                ProductionOrderProductionPlanSemiSum.QuantityCut11 = result.List.Sum(o => o.QuantityCut11);

                ProductionOrderProductionPlanSemiSum.Quantity12 = result.List.Sum(o => o.Quantity12);
                ProductionOrderProductionPlanSemiSum.QuantityActual12 = result.List.Sum(o => o.QuantityActual12);
                ProductionOrderProductionPlanSemiSum.QuantityGAP12 = result.List.Sum(o => o.QuantityGAP12);
                ProductionOrderProductionPlanSemiSum.QuantityCut12 = result.List.Sum(o => o.QuantityCut12);

                ProductionOrderProductionPlanSemiSum.Quantity13 = result.List.Sum(o => o.Quantity13);
                ProductionOrderProductionPlanSemiSum.QuantityActual13 = result.List.Sum(o => o.QuantityActual13);
                ProductionOrderProductionPlanSemiSum.QuantityGAP13 = result.List.Sum(o => o.QuantityGAP13);
                ProductionOrderProductionPlanSemiSum.QuantityCut13 = result.List.Sum(o => o.QuantityCut13);

                ProductionOrderProductionPlanSemiSum.Quantity14 = result.List.Sum(o => o.Quantity14);
                ProductionOrderProductionPlanSemiSum.QuantityActual14 = result.List.Sum(o => o.QuantityActual14);
                ProductionOrderProductionPlanSemiSum.QuantityGAP14 = result.List.Sum(o => o.QuantityGAP14);
                ProductionOrderProductionPlanSemiSum.QuantityCut14 = result.List.Sum(o => o.QuantityCut14);

                ProductionOrderProductionPlanSemiSum.Quantity15 = result.List.Sum(o => o.Quantity15);
                ProductionOrderProductionPlanSemiSum.QuantityActual15 = result.List.Sum(o => o.QuantityActual15);
                ProductionOrderProductionPlanSemiSum.QuantityGAP15 = result.List.Sum(o => o.QuantityGAP15);
                ProductionOrderProductionPlanSemiSum.QuantityCut15 = result.List.Sum(o => o.QuantityCut15);

                ProductionOrderProductionPlanSemiSum.Quantity16 = result.List.Sum(o => o.Quantity16);
                ProductionOrderProductionPlanSemiSum.QuantityActual16 = result.List.Sum(o => o.QuantityActual16);
                ProductionOrderProductionPlanSemiSum.QuantityGAP16 = result.List.Sum(o => o.QuantityGAP16);
                ProductionOrderProductionPlanSemiSum.QuantityCut16 = result.List.Sum(o => o.QuantityCut16);

                ProductionOrderProductionPlanSemiSum.Quantity17 = result.List.Sum(o => o.Quantity17);
                ProductionOrderProductionPlanSemiSum.QuantityActual17 = result.List.Sum(o => o.QuantityActual17);
                ProductionOrderProductionPlanSemiSum.QuantityGAP17 = result.List.Sum(o => o.QuantityGAP17);
                ProductionOrderProductionPlanSemiSum.QuantityCut17 = result.List.Sum(o => o.QuantityCut17);

                ProductionOrderProductionPlanSemiSum.Quantity18 = result.List.Sum(o => o.Quantity18);
                ProductionOrderProductionPlanSemiSum.QuantityActual18 = result.List.Sum(o => o.QuantityActual18);
                ProductionOrderProductionPlanSemiSum.QuantityGAP18 = result.List.Sum(o => o.QuantityGAP18);
                ProductionOrderProductionPlanSemiSum.QuantityCut18 = result.List.Sum(o => o.QuantityCut18);

                ProductionOrderProductionPlanSemiSum.Quantity19 = result.List.Sum(o => o.Quantity19);
                ProductionOrderProductionPlanSemiSum.QuantityActual19 = result.List.Sum(o => o.QuantityActual19);
                ProductionOrderProductionPlanSemiSum.QuantityGAP19 = result.List.Sum(o => o.QuantityGAP19);
                ProductionOrderProductionPlanSemiSum.QuantityCut19 = result.List.Sum(o => o.QuantityCut19);

                ProductionOrderProductionPlanSemiSum.Quantity20 = result.List.Sum(o => o.Quantity20);
                ProductionOrderProductionPlanSemiSum.QuantityActual20 = result.List.Sum(o => o.QuantityActual20);
                ProductionOrderProductionPlanSemiSum.QuantityGAP20 = result.List.Sum(o => o.QuantityGAP20);
                ProductionOrderProductionPlanSemiSum.QuantityCut20 = result.List.Sum(o => o.QuantityCut20);

                ProductionOrderProductionPlanSemiSum.Quantity21 = result.List.Sum(o => o.Quantity21);
                ProductionOrderProductionPlanSemiSum.QuantityActual21 = result.List.Sum(o => o.QuantityActual21);
                ProductionOrderProductionPlanSemiSum.QuantityGAP21 = result.List.Sum(o => o.QuantityGAP21);
                ProductionOrderProductionPlanSemiSum.QuantityCut21 = result.List.Sum(o => o.QuantityCut21);

                ProductionOrderProductionPlanSemiSum.Quantity22 = result.List.Sum(o => o.Quantity22);
                ProductionOrderProductionPlanSemiSum.QuantityActual22 = result.List.Sum(o => o.QuantityActual22);
                ProductionOrderProductionPlanSemiSum.QuantityGAP22 = result.List.Sum(o => o.QuantityGAP22);
                ProductionOrderProductionPlanSemiSum.QuantityCut22 = result.List.Sum(o => o.QuantityCut22);

                ProductionOrderProductionPlanSemiSum.Quantity23 = result.List.Sum(o => o.Quantity23);
                ProductionOrderProductionPlanSemiSum.QuantityActual23 = result.List.Sum(o => o.QuantityActual23);
                ProductionOrderProductionPlanSemiSum.QuantityGAP23 = result.List.Sum(o => o.QuantityGAP23);
                ProductionOrderProductionPlanSemiSum.QuantityCut23 = result.List.Sum(o => o.QuantityCut23);

                ProductionOrderProductionPlanSemiSum.Quantity24 = result.List.Sum(o => o.Quantity24);
                ProductionOrderProductionPlanSemiSum.QuantityActual24 = result.List.Sum(o => o.QuantityActual24);
                ProductionOrderProductionPlanSemiSum.QuantityGAP24 = result.List.Sum(o => o.QuantityGAP24);
                ProductionOrderProductionPlanSemiSum.QuantityCut24 = result.List.Sum(o => o.QuantityCut24);

                ProductionOrderProductionPlanSemiSum.Quantity25 = result.List.Sum(o => o.Quantity25);
                ProductionOrderProductionPlanSemiSum.QuantityActual25 = result.List.Sum(o => o.QuantityActual25);
                ProductionOrderProductionPlanSemiSum.QuantityGAP25 = result.List.Sum(o => o.QuantityGAP25);
                ProductionOrderProductionPlanSemiSum.QuantityCut25 = result.List.Sum(o => o.QuantityCut25);

                ProductionOrderProductionPlanSemiSum.Quantity26 = result.List.Sum(o => o.Quantity26);
                ProductionOrderProductionPlanSemiSum.QuantityActual26 = result.List.Sum(o => o.QuantityActual26);
                ProductionOrderProductionPlanSemiSum.QuantityGAP26 = result.List.Sum(o => o.QuantityGAP26);
                ProductionOrderProductionPlanSemiSum.QuantityCut26 = result.List.Sum(o => o.QuantityCut26);

                ProductionOrderProductionPlanSemiSum.Quantity27 = result.List.Sum(o => o.Quantity27);
                ProductionOrderProductionPlanSemiSum.QuantityActual27 = result.List.Sum(o => o.QuantityActual27);
                ProductionOrderProductionPlanSemiSum.QuantityGAP27 = result.List.Sum(o => o.QuantityGAP27);
                ProductionOrderProductionPlanSemiSum.QuantityCut27 = result.List.Sum(o => o.QuantityCut27);

                ProductionOrderProductionPlanSemiSum.Quantity28 = result.List.Sum(o => o.Quantity28);
                ProductionOrderProductionPlanSemiSum.QuantityActual28 = result.List.Sum(o => o.QuantityActual28);
                ProductionOrderProductionPlanSemiSum.QuantityGAP28 = result.List.Sum(o => o.QuantityGAP28);
                ProductionOrderProductionPlanSemiSum.QuantityCut28 = result.List.Sum(o => o.QuantityCut28);

                ProductionOrderProductionPlanSemiSum.Quantity29 = result.List.Sum(o => o.Quantity29);
                ProductionOrderProductionPlanSemiSum.QuantityActual29 = result.List.Sum(o => o.QuantityActual29);
                ProductionOrderProductionPlanSemiSum.QuantityGAP29 = result.List.Sum(o => o.QuantityGAP29);
                ProductionOrderProductionPlanSemiSum.QuantityCut29 = result.List.Sum(o => o.QuantityCut29);

                ProductionOrderProductionPlanSemiSum.Quantity30 = result.List.Sum(o => o.Quantity30);
                ProductionOrderProductionPlanSemiSum.QuantityActual30 = result.List.Sum(o => o.QuantityActual30);
                ProductionOrderProductionPlanSemiSum.QuantityGAP30 = result.List.Sum(o => o.QuantityGAP30);
                ProductionOrderProductionPlanSemiSum.QuantityCut30 = result.List.Sum(o => o.QuantityCut30);

                ProductionOrderProductionPlanSemiSum.Quantity31 = result.List.Sum(o => o.Quantity31);
                ProductionOrderProductionPlanSemiSum.QuantityActual31 = result.List.Sum(o => o.QuantityActual31);
                ProductionOrderProductionPlanSemiSum.QuantityGAP31 = result.List.Sum(o => o.QuantityGAP31);
                ProductionOrderProductionPlanSemiSum.QuantityCut31 = result.List.Sum(o => o.QuantityCut31);

                ProductionOrderProductionPlanSemiSum.Quantity32 = result.List.Sum(o => o.Quantity32);
                ProductionOrderProductionPlanSemiSum.QuantityActual32 = result.List.Sum(o => o.QuantityActual32);
                ProductionOrderProductionPlanSemiSum.QuantityGAP32 = result.List.Sum(o => o.QuantityGAP32);
                ProductionOrderProductionPlanSemiSum.QuantityCut32 = result.List.Sum(o => o.QuantityCut32);

                ProductionOrderProductionPlanSemiSum.Quantity33 = result.List.Sum(o => o.Quantity33);
                ProductionOrderProductionPlanSemiSum.QuantityActual33 = result.List.Sum(o => o.QuantityActual33);
                ProductionOrderProductionPlanSemiSum.QuantityGAP33 = result.List.Sum(o => o.QuantityGAP33);
                ProductionOrderProductionPlanSemiSum.QuantityCut33 = result.List.Sum(o => o.QuantityCut33);

                ProductionOrderProductionPlanSemiSum.Quantity34 = result.List.Sum(o => o.Quantity34);
                ProductionOrderProductionPlanSemiSum.QuantityActual34 = result.List.Sum(o => o.QuantityActual34);
                ProductionOrderProductionPlanSemiSum.QuantityGAP34 = result.List.Sum(o => o.QuantityGAP34);
                ProductionOrderProductionPlanSemiSum.QuantityCut34 = result.List.Sum(o => o.QuantityCut34);

                ProductionOrderProductionPlanSemiSum.Quantity35 = result.List.Sum(o => o.Quantity35);
                ProductionOrderProductionPlanSemiSum.QuantityActual35 = result.List.Sum(o => o.QuantityActual35);
                ProductionOrderProductionPlanSemiSum.QuantityGAP35 = result.List.Sum(o => o.QuantityGAP35);
                ProductionOrderProductionPlanSemiSum.QuantityCut35 = result.List.Sum(o => o.QuantityCut35);

                ProductionOrderProductionPlanSemiSum.Quantity36 = result.List.Sum(o => o.Quantity36);
                ProductionOrderProductionPlanSemiSum.QuantityActual36 = result.List.Sum(o => o.QuantityActual36);
                ProductionOrderProductionPlanSemiSum.QuantityGAP36 = result.List.Sum(o => o.QuantityGAP36);
                ProductionOrderProductionPlanSemiSum.QuantityCut36 = result.List.Sum(o => o.QuantityCut36);

                ProductionOrderProductionPlanSemiSum.Quantity37 = result.List.Sum(o => o.Quantity37);
                ProductionOrderProductionPlanSemiSum.QuantityActual37 = result.List.Sum(o => o.QuantityActual37);
                ProductionOrderProductionPlanSemiSum.QuantityGAP37 = result.List.Sum(o => o.QuantityGAP37);
                ProductionOrderProductionPlanSemiSum.QuantityCut37 = result.List.Sum(o => o.QuantityCut37);

                ProductionOrderProductionPlanSemiSum.Quantity38 = result.List.Sum(o => o.Quantity38);
                ProductionOrderProductionPlanSemiSum.QuantityActual38 = result.List.Sum(o => o.QuantityActual38);
                ProductionOrderProductionPlanSemiSum.QuantityGAP38 = result.List.Sum(o => o.QuantityGAP38);
                ProductionOrderProductionPlanSemiSum.QuantityCut38 = result.List.Sum(o => o.QuantityCut38);

                ProductionOrderProductionPlanSemiSum.Quantity39 = result.List.Sum(o => o.Quantity39);
                ProductionOrderProductionPlanSemiSum.QuantityActual39 = result.List.Sum(o => o.QuantityActual39);
                ProductionOrderProductionPlanSemiSum.QuantityGAP39 = result.List.Sum(o => o.QuantityGAP39);
                ProductionOrderProductionPlanSemiSum.QuantityCut39 = result.List.Sum(o => o.QuantityCut39);

                ProductionOrderProductionPlanSemiSum.Quantity40 = result.List.Sum(o => o.Quantity40);
                ProductionOrderProductionPlanSemiSum.QuantityActual40 = result.List.Sum(o => o.QuantityActual40);
                ProductionOrderProductionPlanSemiSum.QuantityGAP40 = result.List.Sum(o => o.QuantityGAP40);
                ProductionOrderProductionPlanSemiSum.QuantityCut40 = result.List.Sum(o => o.QuantityCut40);

                ProductionOrderProductionPlanSemiSum.Quantity41 = result.List.Sum(o => o.Quantity41);
                ProductionOrderProductionPlanSemiSum.QuantityActual41 = result.List.Sum(o => o.QuantityActual41);
                ProductionOrderProductionPlanSemiSum.QuantityGAP41 = result.List.Sum(o => o.QuantityGAP41);
                ProductionOrderProductionPlanSemiSum.QuantityCut41 = result.List.Sum(o => o.QuantityCut41);

                ProductionOrderProductionPlanSemiSum.Quantity42 = result.List.Sum(o => o.Quantity42);
                ProductionOrderProductionPlanSemiSum.QuantityActual42 = result.List.Sum(o => o.QuantityActual42);
                ProductionOrderProductionPlanSemiSum.QuantityGAP42 = result.List.Sum(o => o.QuantityGAP42);
                ProductionOrderProductionPlanSemiSum.QuantityCut42 = result.List.Sum(o => o.QuantityCut42);

                ProductionOrderProductionPlanSemiSum.Quantity43 = result.List.Sum(o => o.Quantity43);
                ProductionOrderProductionPlanSemiSum.QuantityActual43 = result.List.Sum(o => o.QuantityActual43);
                ProductionOrderProductionPlanSemiSum.QuantityGAP43 = result.List.Sum(o => o.QuantityGAP43);
                ProductionOrderProductionPlanSemiSum.QuantityCut43 = result.List.Sum(o => o.QuantityCut43);

                ProductionOrderProductionPlanSemiSum.Quantity44 = result.List.Sum(o => o.Quantity44);
                ProductionOrderProductionPlanSemiSum.QuantityActual44 = result.List.Sum(o => o.QuantityActual44);
                ProductionOrderProductionPlanSemiSum.QuantityGAP44 = result.List.Sum(o => o.QuantityGAP44);
                ProductionOrderProductionPlanSemiSum.QuantityCut44 = result.List.Sum(o => o.QuantityCut44);

                ProductionOrderProductionPlanSemiSum.Quantity45 = result.List.Sum(o => o.Quantity45);
                ProductionOrderProductionPlanSemiSum.QuantityActual45 = result.List.Sum(o => o.QuantityActual45);
                ProductionOrderProductionPlanSemiSum.QuantityGAP45 = result.List.Sum(o => o.QuantityGAP45);
                ProductionOrderProductionPlanSemiSum.QuantityCut45 = result.List.Sum(o => o.QuantityCut45);

                ProductionOrderProductionPlanSemiSum.Quantity46 = result.List.Sum(o => o.Quantity46);
                ProductionOrderProductionPlanSemiSum.QuantityActual46 = result.List.Sum(o => o.QuantityActual46);
                ProductionOrderProductionPlanSemiSum.QuantityGAP46 = result.List.Sum(o => o.QuantityGAP46);
                ProductionOrderProductionPlanSemiSum.QuantityCut46 = result.List.Sum(o => o.QuantityCut46);

                ProductionOrderProductionPlanSemiSum.Quantity47 = result.List.Sum(o => o.Quantity47);
                ProductionOrderProductionPlanSemiSum.QuantityActual47 = result.List.Sum(o => o.QuantityActual47);
                ProductionOrderProductionPlanSemiSum.QuantityGAP47 = result.List.Sum(o => o.QuantityGAP47);
                ProductionOrderProductionPlanSemiSum.QuantityCut47 = result.List.Sum(o => o.QuantityCut47);

                ProductionOrderProductionPlanSemiSum.Quantity48 = result.List.Sum(o => o.Quantity48);
                ProductionOrderProductionPlanSemiSum.QuantityActual48 = result.List.Sum(o => o.QuantityActual48);
                ProductionOrderProductionPlanSemiSum.QuantityGAP48 = result.List.Sum(o => o.QuantityGAP48);
                ProductionOrderProductionPlanSemiSum.QuantityCut48 = result.List.Sum(o => o.QuantityCut48);

                ProductionOrderProductionPlanSemiSum.Quantity49 = result.List.Sum(o => o.Quantity49);
                ProductionOrderProductionPlanSemiSum.QuantityActual49 = result.List.Sum(o => o.QuantityActual49);
                ProductionOrderProductionPlanSemiSum.QuantityGAP49 = result.List.Sum(o => o.QuantityGAP49);
                ProductionOrderProductionPlanSemiSum.QuantityCut49 = result.List.Sum(o => o.QuantityCut49);

                ProductionOrderProductionPlanSemiSum.Quantity50 = result.List.Sum(o => o.Quantity50);
                ProductionOrderProductionPlanSemiSum.QuantityActual50 = result.List.Sum(o => o.QuantityActual50);
                ProductionOrderProductionPlanSemiSum.QuantityGAP50 = result.List.Sum(o => o.QuantityGAP50);
                ProductionOrderProductionPlanSemiSum.QuantityCut50 = result.List.Sum(o => o.QuantityCut50);

                ProductionOrderProductionPlanSemiSum.Quantity51 = result.List.Sum(o => o.Quantity51);
                ProductionOrderProductionPlanSemiSum.QuantityActual51 = result.List.Sum(o => o.QuantityActual51);
                ProductionOrderProductionPlanSemiSum.QuantityGAP51 = result.List.Sum(o => o.QuantityGAP51);
                ProductionOrderProductionPlanSemiSum.QuantityCut51 = result.List.Sum(o => o.QuantityCut51);

                ProductionOrderProductionPlanSemiSum.Quantity52 = result.List.Sum(o => o.Quantity52);
                ProductionOrderProductionPlanSemiSum.QuantityActual52 = result.List.Sum(o => o.QuantityActual52);
                ProductionOrderProductionPlanSemiSum.QuantityGAP52 = result.List.Sum(o => o.QuantityGAP52);
                ProductionOrderProductionPlanSemiSum.QuantityCut52 = result.List.Sum(o => o.QuantityCut52);

                ProductionOrderProductionPlanSemiSum.Quantity53 = result.List.Sum(o => o.Quantity53);
                ProductionOrderProductionPlanSemiSum.QuantityActual53 = result.List.Sum(o => o.QuantityActual53);
                ProductionOrderProductionPlanSemiSum.QuantityGAP53 = result.List.Sum(o => o.QuantityGAP53);
                ProductionOrderProductionPlanSemiSum.QuantityCut53 = result.List.Sum(o => o.QuantityCut53);

                ProductionOrderProductionPlanSemiSum.Quantity54 = result.List.Sum(o => o.Quantity54);
                ProductionOrderProductionPlanSemiSum.QuantityActual54 = result.List.Sum(o => o.QuantityActual54);
                ProductionOrderProductionPlanSemiSum.QuantityGAP54 = result.List.Sum(o => o.QuantityGAP54);
                ProductionOrderProductionPlanSemiSum.QuantityCut54 = result.List.Sum(o => o.QuantityCut54);

                ProductionOrderProductionPlanSemiSum.Quantity55 = result.List.Sum(o => o.Quantity55);
                ProductionOrderProductionPlanSemiSum.QuantityActual55 = result.List.Sum(o => o.QuantityActual55);
                ProductionOrderProductionPlanSemiSum.QuantityGAP55 = result.List.Sum(o => o.QuantityGAP55);
                ProductionOrderProductionPlanSemiSum.QuantityCut55 = result.List.Sum(o => o.QuantityCut55);

                ProductionOrderProductionPlanSemiSum.Quantity56 = result.List.Sum(o => o.Quantity56);
                ProductionOrderProductionPlanSemiSum.QuantityActual56 = result.List.Sum(o => o.QuantityActual56);
                ProductionOrderProductionPlanSemiSum.QuantityGAP56 = result.List.Sum(o => o.QuantityGAP56);
                ProductionOrderProductionPlanSemiSum.QuantityCut56 = result.List.Sum(o => o.QuantityCut56);

                ProductionOrderProductionPlanSemiSum.Quantity57 = result.List.Sum(o => o.Quantity57);
                ProductionOrderProductionPlanSemiSum.QuantityActual57 = result.List.Sum(o => o.QuantityActual57);
                ProductionOrderProductionPlanSemiSum.QuantityGAP57 = result.List.Sum(o => o.QuantityGAP57);
                ProductionOrderProductionPlanSemiSum.QuantityCut57 = result.List.Sum(o => o.QuantityCut57);

                ProductionOrderProductionPlanSemiSum.Quantity58 = result.List.Sum(o => o.Quantity58);
                ProductionOrderProductionPlanSemiSum.QuantityActual58 = result.List.Sum(o => o.QuantityActual58);
                ProductionOrderProductionPlanSemiSum.QuantityGAP58 = result.List.Sum(o => o.QuantityGAP58);
                ProductionOrderProductionPlanSemiSum.QuantityCut58 = result.List.Sum(o => o.QuantityCut58);

                ProductionOrderProductionPlanSemiSum.Quantity59 = result.List.Sum(o => o.Quantity59);
                ProductionOrderProductionPlanSemiSum.QuantityActual59 = result.List.Sum(o => o.QuantityActual59);
                ProductionOrderProductionPlanSemiSum.QuantityGAP59 = result.List.Sum(o => o.QuantityGAP59);
                ProductionOrderProductionPlanSemiSum.QuantityCut59 = result.List.Sum(o => o.QuantityCut59);

                ProductionOrderProductionPlanSemiSum.Quantity60 = result.List.Sum(o => o.Quantity60);
                ProductionOrderProductionPlanSemiSum.QuantityActual60 = result.List.Sum(o => o.QuantityActual60);
                ProductionOrderProductionPlanSemiSum.QuantityGAP60 = result.List.Sum(o => o.QuantityGAP60);
                ProductionOrderProductionPlanSemiSum.QuantityCut60 = result.List.Sum(o => o.QuantityCut60);

                ProductionOrderProductionPlanSemiSum.Quantity61 = result.List.Sum(o => o.Quantity61);
                ProductionOrderProductionPlanSemiSum.QuantityActual61 = result.List.Sum(o => o.QuantityActual61);
                ProductionOrderProductionPlanSemiSum.QuantityGAP61 = result.List.Sum(o => o.QuantityGAP61);
                ProductionOrderProductionPlanSemiSum.QuantityCut61 = result.List.Sum(o => o.QuantityCut61);

                ProductionOrderProductionPlanSemiSum.Quantity62 = result.List.Sum(o => o.Quantity62);
                ProductionOrderProductionPlanSemiSum.QuantityActual62 = result.List.Sum(o => o.QuantityActual62);
                ProductionOrderProductionPlanSemiSum.QuantityGAP62 = result.List.Sum(o => o.QuantityGAP62);
                ProductionOrderProductionPlanSemiSum.QuantityCut62 = result.List.Sum(o => o.QuantityCut62);

                ProductionOrderProductionPlanSemiSum.Quantity63 = result.List.Sum(o => o.Quantity63);
                ProductionOrderProductionPlanSemiSum.QuantityActual63 = result.List.Sum(o => o.QuantityActual63);
                ProductionOrderProductionPlanSemiSum.QuantityGAP63 = result.List.Sum(o => o.QuantityGAP63);
                ProductionOrderProductionPlanSemiSum.QuantityCut63 = result.List.Sum(o => o.QuantityCut63);

                ProductionOrderProductionPlanSemiSum.Quantity64 = result.List.Sum(o => o.Quantity64);
                ProductionOrderProductionPlanSemiSum.QuantityActual64 = result.List.Sum(o => o.QuantityActual64);
                ProductionOrderProductionPlanSemiSum.QuantityGAP64 = result.List.Sum(o => o.QuantityGAP64);
                ProductionOrderProductionPlanSemiSum.QuantityCut64 = result.List.Sum(o => o.QuantityCut64);

                ProductionOrderProductionPlanSemiSum.Quantity65 = result.List.Sum(o => o.Quantity65);
                ProductionOrderProductionPlanSemiSum.QuantityActual65 = result.List.Sum(o => o.QuantityActual65);
                ProductionOrderProductionPlanSemiSum.QuantityGAP65 = result.List.Sum(o => o.QuantityGAP65);
                ProductionOrderProductionPlanSemiSum.QuantityCut65 = result.List.Sum(o => o.QuantityCut65);

                ProductionOrderProductionPlanSemiSum.Quantity66 = result.List.Sum(o => o.Quantity66);
                ProductionOrderProductionPlanSemiSum.QuantityActual66 = result.List.Sum(o => o.QuantityActual66);
                ProductionOrderProductionPlanSemiSum.QuantityGAP66 = result.List.Sum(o => o.QuantityGAP66);
                ProductionOrderProductionPlanSemiSum.QuantityCut66 = result.List.Sum(o => o.QuantityCut66);

                ProductionOrderProductionPlanSemiSum.Quantity67 = result.List.Sum(o => o.Quantity67);
                ProductionOrderProductionPlanSemiSum.QuantityActual67 = result.List.Sum(o => o.QuantityActual67);
                ProductionOrderProductionPlanSemiSum.QuantityGAP67 = result.List.Sum(o => o.QuantityGAP67);
                ProductionOrderProductionPlanSemiSum.QuantityCut67 = result.List.Sum(o => o.QuantityCut67);

                ProductionOrderProductionPlanSemiSum.Quantity68 = result.List.Sum(o => o.Quantity68);
                ProductionOrderProductionPlanSemiSum.QuantityActual68 = result.List.Sum(o => o.QuantityActual68);
                ProductionOrderProductionPlanSemiSum.QuantityGAP68 = result.List.Sum(o => o.QuantityGAP68);
                ProductionOrderProductionPlanSemiSum.QuantityCut68 = result.List.Sum(o => o.QuantityCut68);

                ProductionOrderProductionPlanSemiSum.Quantity69 = result.List.Sum(o => o.Quantity69);
                ProductionOrderProductionPlanSemiSum.QuantityActual69 = result.List.Sum(o => o.QuantityActual69);
                ProductionOrderProductionPlanSemiSum.QuantityGAP69 = result.List.Sum(o => o.QuantityGAP69);
                ProductionOrderProductionPlanSemiSum.QuantityCut69 = result.List.Sum(o => o.QuantityCut69);

                ProductionOrderProductionPlanSemiSum.Quantity70 = result.List.Sum(o => o.Quantity70);
                ProductionOrderProductionPlanSemiSum.QuantityActual70 = result.List.Sum(o => o.QuantityActual70);
                ProductionOrderProductionPlanSemiSum.QuantityGAP70 = result.List.Sum(o => o.QuantityGAP70);
                ProductionOrderProductionPlanSemiSum.QuantityCut70 = result.List.Sum(o => o.QuantityCut70);

                ProductionOrderProductionPlanSemiSum.Quantity71 = result.List.Sum(o => o.Quantity71);
                ProductionOrderProductionPlanSemiSum.QuantityActual71 = result.List.Sum(o => o.QuantityActual71);
                ProductionOrderProductionPlanSemiSum.QuantityGAP71 = result.List.Sum(o => o.QuantityGAP71);
                ProductionOrderProductionPlanSemiSum.QuantityCut71 = result.List.Sum(o => o.QuantityCut71);

                ProductionOrderProductionPlanSemiSum.Quantity72 = result.List.Sum(o => o.Quantity72);
                ProductionOrderProductionPlanSemiSum.QuantityActual72 = result.List.Sum(o => o.QuantityActual72);
                ProductionOrderProductionPlanSemiSum.QuantityGAP72 = result.List.Sum(o => o.QuantityGAP72);
                ProductionOrderProductionPlanSemiSum.QuantityCut72 = result.List.Sum(o => o.QuantityCut72);

                ProductionOrderProductionPlanSemiSum.Quantity73 = result.List.Sum(o => o.Quantity73);
                ProductionOrderProductionPlanSemiSum.QuantityActual73 = result.List.Sum(o => o.QuantityActual73);
                ProductionOrderProductionPlanSemiSum.QuantityGAP73 = result.List.Sum(o => o.QuantityGAP73);
                ProductionOrderProductionPlanSemiSum.QuantityCut73 = result.List.Sum(o => o.QuantityCut73);

                ProductionOrderProductionPlanSemiSum.Quantity74 = result.List.Sum(o => o.Quantity74);
                ProductionOrderProductionPlanSemiSum.QuantityActual74 = result.List.Sum(o => o.QuantityActual74);
                ProductionOrderProductionPlanSemiSum.QuantityGAP74 = result.List.Sum(o => o.QuantityGAP74);
                ProductionOrderProductionPlanSemiSum.QuantityCut74 = result.List.Sum(o => o.QuantityCut74);

                ProductionOrderProductionPlanSemiSum.Quantity75 = result.List.Sum(o => o.Quantity75);
                ProductionOrderProductionPlanSemiSum.QuantityActual75 = result.List.Sum(o => o.QuantityActual75);
                ProductionOrderProductionPlanSemiSum.QuantityGAP75 = result.List.Sum(o => o.QuantityGAP75);
                ProductionOrderProductionPlanSemiSum.QuantityCut75 = result.List.Sum(o => o.QuantityCut75);

                ProductionOrderProductionPlanSemiSum.Quantity76 = result.List.Sum(o => o.Quantity76);
                ProductionOrderProductionPlanSemiSum.QuantityActual76 = result.List.Sum(o => o.QuantityActual76);
                ProductionOrderProductionPlanSemiSum.QuantityGAP76 = result.List.Sum(o => o.QuantityGAP76);
                ProductionOrderProductionPlanSemiSum.QuantityCut76 = result.List.Sum(o => o.QuantityCut76);

                ProductionOrderProductionPlanSemiSum.Quantity77 = result.List.Sum(o => o.Quantity77);
                ProductionOrderProductionPlanSemiSum.QuantityActual77 = result.List.Sum(o => o.QuantityActual77);
                ProductionOrderProductionPlanSemiSum.QuantityGAP77 = result.List.Sum(o => o.QuantityGAP77);
                ProductionOrderProductionPlanSemiSum.QuantityCut77 = result.List.Sum(o => o.QuantityCut77);

                ProductionOrderProductionPlanSemiSum.Quantity78 = result.List.Sum(o => o.Quantity78);
                ProductionOrderProductionPlanSemiSum.QuantityActual78 = result.List.Sum(o => o.QuantityActual78);
                ProductionOrderProductionPlanSemiSum.QuantityGAP78 = result.List.Sum(o => o.QuantityGAP78);
                ProductionOrderProductionPlanSemiSum.QuantityCut78 = result.List.Sum(o => o.QuantityCut78);

                ProductionOrderProductionPlanSemiSum.Quantity79 = result.List.Sum(o => o.Quantity79);
                ProductionOrderProductionPlanSemiSum.QuantityActual79 = result.List.Sum(o => o.QuantityActual79);
                ProductionOrderProductionPlanSemiSum.QuantityGAP79 = result.List.Sum(o => o.QuantityGAP79);
                ProductionOrderProductionPlanSemiSum.QuantityCut79 = result.List.Sum(o => o.QuantityCut79);

                ProductionOrderProductionPlanSemiSum.Quantity80 = result.List.Sum(o => o.Quantity80);
                ProductionOrderProductionPlanSemiSum.QuantityActual80 = result.List.Sum(o => o.QuantityActual80);
                ProductionOrderProductionPlanSemiSum.QuantityGAP80 = result.List.Sum(o => o.QuantityGAP80);
                ProductionOrderProductionPlanSemiSum.QuantityCut80 = result.List.Sum(o => o.QuantityCut80);

                ProductionOrderProductionPlanSemiSum.Quantity81 = result.List.Sum(o => o.Quantity81);
                ProductionOrderProductionPlanSemiSum.QuantityActual81 = result.List.Sum(o => o.QuantityActual81);
                ProductionOrderProductionPlanSemiSum.QuantityGAP81 = result.List.Sum(o => o.QuantityGAP81);
                ProductionOrderProductionPlanSemiSum.QuantityCut81 = result.List.Sum(o => o.QuantityCut81);

                ProductionOrderProductionPlanSemiSum.Quantity82 = result.List.Sum(o => o.Quantity82);
                ProductionOrderProductionPlanSemiSum.QuantityActual82 = result.List.Sum(o => o.QuantityActual82);
                ProductionOrderProductionPlanSemiSum.QuantityGAP82 = result.List.Sum(o => o.QuantityGAP82);
                ProductionOrderProductionPlanSemiSum.QuantityCut82 = result.List.Sum(o => o.QuantityCut82);

                ProductionOrderProductionPlanSemiSum.Quantity83 = result.List.Sum(o => o.Quantity83);
                ProductionOrderProductionPlanSemiSum.QuantityActual83 = result.List.Sum(o => o.QuantityActual83);
                ProductionOrderProductionPlanSemiSum.QuantityGAP83 = result.List.Sum(o => o.QuantityGAP83);
                ProductionOrderProductionPlanSemiSum.QuantityCut83 = result.List.Sum(o => o.QuantityCut83);

                ProductionOrderProductionPlanSemiSum.Quantity84 = result.List.Sum(o => o.Quantity84);
                ProductionOrderProductionPlanSemiSum.QuantityActual84 = result.List.Sum(o => o.QuantityActual84);
                ProductionOrderProductionPlanSemiSum.QuantityGAP84 = result.List.Sum(o => o.QuantityGAP84);
                ProductionOrderProductionPlanSemiSum.QuantityCut84 = result.List.Sum(o => o.QuantityCut84);

                ProductionOrderProductionPlanSemiSum.Quantity85 = result.List.Sum(o => o.Quantity85);
                ProductionOrderProductionPlanSemiSum.QuantityActual85 = result.List.Sum(o => o.QuantityActual85);
                ProductionOrderProductionPlanSemiSum.QuantityGAP85 = result.List.Sum(o => o.QuantityGAP85);
                ProductionOrderProductionPlanSemiSum.QuantityCut85 = result.List.Sum(o => o.QuantityCut85);

                ProductionOrderProductionPlanSemiSum.Quantity86 = result.List.Sum(o => o.Quantity86);
                ProductionOrderProductionPlanSemiSum.QuantityActual86 = result.List.Sum(o => o.QuantityActual86);
                ProductionOrderProductionPlanSemiSum.QuantityGAP86 = result.List.Sum(o => o.QuantityGAP86);
                ProductionOrderProductionPlanSemiSum.QuantityCut86 = result.List.Sum(o => o.QuantityCut86);

                ProductionOrderProductionPlanSemiSum.Quantity87 = result.List.Sum(o => o.Quantity87);
                ProductionOrderProductionPlanSemiSum.QuantityActual87 = result.List.Sum(o => o.QuantityActual87);
                ProductionOrderProductionPlanSemiSum.QuantityGAP87 = result.List.Sum(o => o.QuantityGAP87);
                ProductionOrderProductionPlanSemiSum.QuantityCut87 = result.List.Sum(o => o.QuantityCut87);

                ProductionOrderProductionPlanSemiSum.Quantity88 = result.List.Sum(o => o.Quantity88);
                ProductionOrderProductionPlanSemiSum.QuantityActual88 = result.List.Sum(o => o.QuantityActual88);
                ProductionOrderProductionPlanSemiSum.QuantityGAP88 = result.List.Sum(o => o.QuantityGAP88);
                ProductionOrderProductionPlanSemiSum.QuantityCut88 = result.List.Sum(o => o.QuantityCut88);

                ProductionOrderProductionPlanSemiSum.Quantity89 = result.List.Sum(o => o.Quantity89);
                ProductionOrderProductionPlanSemiSum.QuantityActual89 = result.List.Sum(o => o.QuantityActual89);
                ProductionOrderProductionPlanSemiSum.QuantityGAP89 = result.List.Sum(o => o.QuantityGAP89);
                ProductionOrderProductionPlanSemiSum.QuantityCut89 = result.List.Sum(o => o.QuantityCut89);

                ProductionOrderProductionPlanSemiSum.Quantity90 = result.List.Sum(o => o.Quantity90);
                ProductionOrderProductionPlanSemiSum.QuantityActual90 = result.List.Sum(o => o.QuantityActual90);
                ProductionOrderProductionPlanSemiSum.QuantityGAP90 = result.List.Sum(o => o.QuantityGAP90);
                ProductionOrderProductionPlanSemiSum.QuantityCut90 = result.List.Sum(o => o.QuantityCut90);

                ProductionOrderProductionPlanSemiSum.Quantity91 = result.List.Sum(o => o.Quantity91);
                ProductionOrderProductionPlanSemiSum.QuantityActual91 = result.List.Sum(o => o.QuantityActual91);
                ProductionOrderProductionPlanSemiSum.QuantityGAP91 = result.List.Sum(o => o.QuantityGAP91);
                ProductionOrderProductionPlanSemiSum.QuantityCut91 = result.List.Sum(o => o.QuantityCut91);

                ProductionOrderProductionPlanSemiSum.Quantity92 = result.List.Sum(o => o.Quantity92);
                ProductionOrderProductionPlanSemiSum.QuantityActual92 = result.List.Sum(o => o.QuantityActual92);
                ProductionOrderProductionPlanSemiSum.QuantityGAP92 = result.List.Sum(o => o.QuantityGAP92);
                ProductionOrderProductionPlanSemiSum.QuantityCut92 = result.List.Sum(o => o.QuantityCut92);

                ProductionOrderProductionPlanSemiSum.Quantity93 = result.List.Sum(o => o.Quantity93);
                ProductionOrderProductionPlanSemiSum.QuantityActual93 = result.List.Sum(o => o.QuantityActual93);
                ProductionOrderProductionPlanSemiSum.QuantityGAP93 = result.List.Sum(o => o.QuantityGAP93);
                ProductionOrderProductionPlanSemiSum.QuantityCut93 = result.List.Sum(o => o.QuantityCut93);

                ProductionOrderProductionPlanSemiSum.Quantity94 = result.List.Sum(o => o.Quantity94);
                ProductionOrderProductionPlanSemiSum.QuantityActual94 = result.List.Sum(o => o.QuantityActual94);
                ProductionOrderProductionPlanSemiSum.QuantityGAP94 = result.List.Sum(o => o.QuantityGAP94);
                ProductionOrderProductionPlanSemiSum.QuantityCut94 = result.List.Sum(o => o.QuantityCut94);

                ProductionOrderProductionPlanSemiSum.Quantity95 = result.List.Sum(o => o.Quantity95);
                ProductionOrderProductionPlanSemiSum.QuantityActual95 = result.List.Sum(o => o.QuantityActual95);
                ProductionOrderProductionPlanSemiSum.QuantityGAP95 = result.List.Sum(o => o.QuantityGAP95);
                ProductionOrderProductionPlanSemiSum.QuantityCut95 = result.List.Sum(o => o.QuantityCut95);

                ProductionOrderProductionPlanSemiSum.Quantity96 = result.List.Sum(o => o.Quantity96);
                ProductionOrderProductionPlanSemiSum.QuantityActual96 = result.List.Sum(o => o.QuantityActual96);
                ProductionOrderProductionPlanSemiSum.QuantityGAP96 = result.List.Sum(o => o.QuantityGAP96);
                ProductionOrderProductionPlanSemiSum.QuantityCut96 = result.List.Sum(o => o.QuantityCut96);

                ProductionOrderProductionPlanSemiSum.Quantity97 = result.List.Sum(o => o.Quantity97);
                ProductionOrderProductionPlanSemiSum.QuantityActual97 = result.List.Sum(o => o.QuantityActual97);
                ProductionOrderProductionPlanSemiSum.QuantityGAP97 = result.List.Sum(o => o.QuantityGAP97);
                ProductionOrderProductionPlanSemiSum.QuantityCut97 = result.List.Sum(o => o.QuantityCut97);

                ProductionOrderProductionPlanSemiSum.Quantity98 = result.List.Sum(o => o.Quantity98);
                ProductionOrderProductionPlanSemiSum.QuantityActual98 = result.List.Sum(o => o.QuantityActual98);
                ProductionOrderProductionPlanSemiSum.QuantityGAP98 = result.List.Sum(o => o.QuantityGAP98);
                ProductionOrderProductionPlanSemiSum.QuantityCut98 = result.List.Sum(o => o.QuantityCut98);

                ProductionOrderProductionPlanSemiSum.Quantity99 = result.List.Sum(o => o.Quantity99);
                ProductionOrderProductionPlanSemiSum.QuantityActual99 = result.List.Sum(o => o.QuantityActual99);
                ProductionOrderProductionPlanSemiSum.QuantityGAP99 = result.List.Sum(o => o.QuantityGAP99);
                ProductionOrderProductionPlanSemiSum.QuantityCut99 = result.List.Sum(o => o.QuantityCut99);

                ProductionOrderProductionPlanSemiSum.Quantity100 = result.List.Sum(o => o.Quantity100);
                ProductionOrderProductionPlanSemiSum.QuantityActual100 = result.List.Sum(o => o.QuantityActual100);
                ProductionOrderProductionPlanSemiSum.QuantityGAP100 = result.List.Sum(o => o.QuantityGAP100);
                ProductionOrderProductionPlanSemiSum.QuantityCut100 = result.List.Sum(o => o.QuantityCut100);

                ProductionOrderProductionPlanSemiSum.Quantity101 = result.List.Sum(o => o.Quantity101);
                ProductionOrderProductionPlanSemiSum.QuantityActual101 = result.List.Sum(o => o.QuantityActual101);
                ProductionOrderProductionPlanSemiSum.QuantityGAP101 = result.List.Sum(o => o.QuantityGAP101);
                ProductionOrderProductionPlanSemiSum.QuantityCut101 = result.List.Sum(o => o.QuantityCut101);

                ProductionOrderProductionPlanSemiSum.Quantity102 = result.List.Sum(o => o.Quantity102);
                ProductionOrderProductionPlanSemiSum.QuantityActual102 = result.List.Sum(o => o.QuantityActual102);
                ProductionOrderProductionPlanSemiSum.QuantityGAP102 = result.List.Sum(o => o.QuantityGAP102);
                ProductionOrderProductionPlanSemiSum.QuantityCut102 = result.List.Sum(o => o.QuantityCut102);

                ProductionOrderProductionPlanSemiSum.Quantity103 = result.List.Sum(o => o.Quantity103);
                ProductionOrderProductionPlanSemiSum.QuantityActual103 = result.List.Sum(o => o.QuantityActual103);
                ProductionOrderProductionPlanSemiSum.QuantityGAP103 = result.List.Sum(o => o.QuantityGAP103);
                ProductionOrderProductionPlanSemiSum.QuantityCut103 = result.List.Sum(o => o.QuantityCut103);

                ProductionOrderProductionPlanSemiSum.Quantity104 = result.List.Sum(o => o.Quantity104);
                ProductionOrderProductionPlanSemiSum.QuantityActual104 = result.List.Sum(o => o.QuantityActual104);
                ProductionOrderProductionPlanSemiSum.QuantityGAP104 = result.List.Sum(o => o.QuantityGAP104);
                ProductionOrderProductionPlanSemiSum.QuantityCut104 = result.List.Sum(o => o.QuantityCut104);

                ProductionOrderProductionPlanSemiSum.Quantity105 = result.List.Sum(o => o.Quantity105);
                ProductionOrderProductionPlanSemiSum.QuantityActual105 = result.List.Sum(o => o.QuantityActual105);
                ProductionOrderProductionPlanSemiSum.QuantityGAP105 = result.List.Sum(o => o.QuantityGAP105);
                ProductionOrderProductionPlanSemiSum.QuantityCut105 = result.List.Sum(o => o.QuantityCut105);

                result.List.Add(ProductionOrderProductionPlanSemiSum);
            }
            result.List = result.List.OrderBy(o => o.Priority).ThenBy(o => o.MaterialCode).ToList(); ;
            return result;
        }
    }
}

