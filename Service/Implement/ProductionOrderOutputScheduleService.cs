namespace Service.Implement
{
    public class ProductionOrderOutputScheduleService : BaseService<ProductionOrderOutputSchedule, IProductionOrderOutputScheduleRepository>
    , IProductionOrderOutputScheduleService
    {
        private readonly IProductionOrderOutputScheduleRepository _ProductionOrderOutputScheduleRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderProductionPlanRepository _ProductionOrderProductionPlanRepository;
        private readonly IProductionOrderBOMRepository _ProductionOrderBOMRepository;
        private readonly IProductionOrderBOMDetailRepository _ProductionOrderBOMDetailRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseRequestService _WarehouseRequestService;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IWarehouseRequestDetailService _WarehouseRequestDetailService;
        private readonly IWarehouseRequestDetailRepository _WarehouseRequestDetailRepository;
        private readonly IProductionOrderProductionPlanMaterialRepository _ProductionOrderProductionPlanMaterialRepository;
        public ProductionOrderOutputScheduleService(IProductionOrderOutputScheduleRepository ProductionOrderOutputScheduleRepository

            , IProductionOrderRepository productionOrderRepository
            , IProductionOrderProductionPlanRepository ProductionOrderProductionPlanRepository
            , IProductionOrderBOMRepository ProductionOrderBOMRepository
            , IProductionOrderBOMDetailRepository ProductionOrderBOMDetailRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IWarehouseRequestService warehouseRequestService
            , IWarehouseRequestRepository WarehouseRequestRepository
            , IWarehouseRequestDetailService warehouseRequestDetailService
            , IWarehouseRequestDetailRepository WarehouseRequestDetailRepository
            , IProductionOrderProductionPlanMaterialRepository ProductionOrderProductionPlanMaterialRepository

            ) : base(ProductionOrderOutputScheduleRepository)
        {
            _ProductionOrderOutputScheduleRepository = ProductionOrderOutputScheduleRepository;
            _ProductionOrderRepository = productionOrderRepository;
            _ProductionOrderProductionPlanRepository = ProductionOrderProductionPlanRepository;
            _ProductionOrderBOMRepository = ProductionOrderBOMRepository;
            _ProductionOrderBOMDetailRepository = ProductionOrderBOMDetailRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WarehouseRequestService = warehouseRequestService;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _WarehouseRequestDetailService = warehouseRequestDetailService;
            _WarehouseRequestDetailRepository = WarehouseRequestDetailRepository;
            _ProductionOrderProductionPlanMaterialRepository = ProductionOrderProductionPlanMaterialRepository;
        }

        public override void InitializationSave(ProductionOrderOutputSchedule model)
        {
            BaseInitialization(model);
            var ModelCheck = GetByCondition(o => o.ParentID == model.ParentID && o.Code == model.Code).FirstOrDefault();
            if (ModelCheck != null && ModelCheck.ID > 0)
            {
                model.Begin = ModelCheck.Begin;
                model.End = ModelCheck.End;
            }
            model.Begin = model.Begin ?? GlobalHelper.InitializationDateTime;
            model.End = model.End ?? GlobalHelper.InitializationDateTime;
            if (model.Begin != null)
            {
                model.Begin = new DateTime(model.Begin.Value.Year, model.Begin.Value.Month, model.Begin.Value.Day, 0, 0, 0);
            }
            if (model.End != null)
            {
                model.End = new DateTime(model.End.Value.Year, model.End.Value.Month, model.End.Value.Day, 0, 0, 0);
            }
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Active = Parent.Active;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                if (string.IsNullOrEmpty(model.Code))
                {
                    model.Code = Parent.Code;
                    model.Code = model.Code + "-" + model.Begin.Value.ToString("yyyyMMdd") + "-" + model.End.Value.ToString("yyyyMMdd");
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderOutputSchedule>> SaveAsync(BaseParameter<ProductionOrderOutputSchedule> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderOutputSchedule>();
            if (BaseParameter.BaseModel != null)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code && o.CreateDate != null && BaseParameter.BaseModel.CreateDate != null && o.CreateDate.Value.Date == BaseParameter.BaseModel.CreateDate.Value.Date).OrderBy(o => o.CreateDate).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<ProductionOrderOutputSchedule>> SyncAsync(BaseParameter<ProductionOrderOutputSchedule> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderOutputSchedule>();
            await SyncWarehouseRequestAsync(BaseParameter);
            return result;
        }

        public virtual async Task<BaseResult<ProductionOrderOutputSchedule>> SyncWarehouseRequestAsync(BaseParameter<ProductionOrderOutputSchedule> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderOutputSchedule>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.Active == true)
                            {
                                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
                                {
                                    if (BaseParameter.BaseModel.Begin != null)
                                    {
                                        if (BaseParameter.BaseModel.End != null)
                                        {
                                            if (BaseParameter.BaseModel.End >= BaseParameter.BaseModel.Begin)
                                            {
                                                var ListProductionOrderOutputSchedule = await _ProductionOrderOutputScheduleRepository.GetByCondition(o => o.Active == true && o.Code == BaseParameter.BaseModel.Code).OrderBy(o => o.CreateDate).ToListAsync();
                                                if (ListProductionOrderOutputSchedule.Count > 1)
                                                {
                                                    var WarehouseRequestCheck = await _WarehouseRequestService.GetByCondition(o => o.ProductionOrderOutputScheduleID == BaseParameter.BaseModel.ID).FirstOrDefaultAsync();
                                                    if (WarehouseRequestCheck == null)
                                                    {
                                                        var ID = ListProductionOrderOutputSchedule[0].ID;
                                                        var ListWarehouseRequest = await _WarehouseRequestService.GetByCondition(o => o.Active == true && o.ProductionOrderOutputScheduleID == ID).OrderBy(o => o.CreateDate).ToListAsync();
                                                        if (ListWarehouseRequest.Count > 0)
                                                        {
                                                            foreach (var WarehouseRequest in ListWarehouseRequest)
                                                            {
                                                                if (WarehouseRequest != null && WarehouseRequest.ID > 0)
                                                                {
                                                                    ID = WarehouseRequest.ID;
                                                                    WarehouseRequest.ID = 0;
                                                                    var Index = ListProductionOrderOutputSchedule.Count.ToString();
                                                                    if (ListProductionOrderOutputSchedule.Count < 10)
                                                                    {
                                                                        Index = "0" + Index;
                                                                    }
                                                                    WarehouseRequest.Name = WarehouseRequest.Name + "-" + Index;
                                                                    WarehouseRequest.Code = WarehouseRequest.Code + "-" + Index;
                                                                    WarehouseRequest.ProductionOrderOutputScheduleID = BaseParameter.BaseModel.ID;
                                                                    var BaseParameterWarehouseRequest = new BaseParameter<WarehouseRequest>();
                                                                    BaseParameterWarehouseRequest.BaseModel = WarehouseRequest;
                                                                    await _WarehouseRequestService.SaveAsync(BaseParameterWarehouseRequest);
                                                                    if (WarehouseRequest.ID > 0)
                                                                    {
                                                                        var ListProductionOrderOutputScheduleID = ListProductionOrderOutputSchedule.Select(o => o.ID).ToList();
                                                                        var ListWarehouseRequestSum = await _WarehouseRequestService.GetByCondition(o => o.Active == true && o.ProductionOrderOutputScheduleID > 0 && ListProductionOrderOutputScheduleID.Contains(o.ProductionOrderOutputScheduleID.Value)).OrderBy(o => o.CreateDate).ToListAsync();
                                                                        if (ListWarehouseRequestSum.Count > 0)
                                                                        {
                                                                            var ListWarehouseRequestID = ListWarehouseRequestSum.Select(o => o.ID).ToList();
                                                                            var ListWarehouseRequestDetailSum = await _WarehouseRequestDetailRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseRequestID.Contains(o.ParentID.Value)).ToListAsync();
                                                                            var ListWarehouseRequestDetail = await _WarehouseRequestDetailRepository.GetByParentIDToListAsync(ID);
                                                                            foreach (var WarehouseRequestDetail in ListWarehouseRequestDetail)
                                                                            {
                                                                                WarehouseRequestDetail.ID = 0;
                                                                                WarehouseRequestDetail.ParentID = WarehouseRequest.ID;
                                                                                var List = ListWarehouseRequestDetailSum.Where(o => o.Display == WarehouseRequestDetail.Display && o.MaterialID == WarehouseRequestDetail.MaterialID).ToList();
                                                                                var SUM = List.Sum(o => o.Quantity);
                                                                                WarehouseRequestDetail.Quantity = WarehouseRequestDetail.QuantityInvoice - SUM;
                                                                                if (WarehouseRequestDetail.Quantity < 0)
                                                                                {
                                                                                    WarehouseRequestDetail.Quantity = 0;
                                                                                }
                                                                                var BaseParameterWarehouseRequestDetail = new BaseParameter<WarehouseRequestDetail>();
                                                                                BaseParameterWarehouseRequestDetail.BaseModel = WarehouseRequestDetail;
                                                                                await _WarehouseRequestDetailService.SaveAsync(BaseParameterWarehouseRequestDetail);
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var ListWarehouseRequest = await _WarehouseRequestService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID).ToListAsync();
                                                    if (ListWarehouseRequest.Count > 0)
                                                    {
                                                        var ListWarehouseRequestID = ListWarehouseRequest.Select(o => o.ID).ToList();
                                                        var ListWarehouseRequestDetail = await _WarehouseRequestDetailService.GetByCondition(o => o.ParentID != null && ListWarehouseRequestID.Contains(o.ParentID.Value)).ToListAsync();
                                                        foreach (var WarehouseRequestDetail in ListWarehouseRequestDetail)
                                                        {
                                                            WarehouseRequestDetail.QuantityCheck = 0;
                                                            await _WarehouseRequestDetailRepository.UpdateAsync(WarehouseRequestDetail);
                                                        }
                                                    }

                                                    var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanRepository.GetByParentIDToListAsync(BaseParameter.BaseModel.ParentID.Value);
                                                    if (ListProductionOrderProductionPlan.Count > 0)
                                                    {
                                                        foreach (var ProductionOrderProductionPlan in ListProductionOrderProductionPlan)
                                                        {
                                                            if (ProductionOrderProductionPlan.MaterialID > 0)
                                                            {
                                                                var Quantity = GlobalHelper.InitializationNumber;
                                                                for (int i = 1; i <= 105; i++)
                                                                {
                                                                    var Index = i.ToString();
                                                                    if (i < 10)
                                                                    {
                                                                        Index = "0" + Index;
                                                                    }
                                                                    var proDateName = "Date" + Index;
                                                                    foreach (PropertyInfo proDate in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                    {
                                                                        if (proDate.Name == proDateName && (proDate.GetValue(ProductionOrderProductionPlan) != null))
                                                                        {
                                                                            var ProductionOrderProductionPlanDate = (DateTime)proDate.GetValue(ProductionOrderProductionPlan);
                                                                            for (DateTime date = BaseParameter.BaseModel.Begin.Value.Date; date.Date <= BaseParameter.BaseModel.End.Value.Date; date = date.AddDays(1))
                                                                            {
                                                                                if (ProductionOrderProductionPlanDate.Date == date)
                                                                                {
                                                                                    var proQuantityName = "Quantity" + Index;
                                                                                    foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                    {
                                                                                        if (proQuantity.Name == proQuantityName && (proQuantity.GetValue(ProductionOrderProductionPlan) != null))
                                                                                        {
                                                                                            Quantity = Quantity + (int)proQuantity.GetValue(ProductionOrderProductionPlan);
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                    break;
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                    }
                                                                }


                                                                var ProductionOrderBOM = await _ProductionOrderBOMRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == ProductionOrderProductionPlan.MaterialID).FirstOrDefaultAsync();
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
                                                                                    var CategoryDepartment = ListCategoryDepartment.Where(o => !string.IsNullOrEmpty(o.Note) && o.Note.ToLower().Contains(Display.ToLower())).FirstOrDefault();
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
                                                                                var WarehouseRequest = new WarehouseRequest();
                                                                                WarehouseRequest.ParentID = BaseParameter.BaseModel.ParentID;
                                                                                WarehouseRequest.ParentName = BaseParameter.BaseModel.ParentName;
                                                                                WarehouseRequest.ProductionOrderOutputScheduleID = BaseParameter.BaseModel.ID;
                                                                                WarehouseRequest.CustomerID = CategoryDepartment.ID;
                                                                                WarehouseRequest.Note = BaseParameter.BaseModel.Note;
                                                                                WarehouseRequest.Date = GlobalHelper.InitializationDateTime;
                                                                                WarehouseRequest.Description = BaseParameter.BaseModel.Begin.Value.ToString("yyyyMMdd") + "-" + BaseParameter.BaseModel.End.Value.ToString("yyyyMMdd");
                                                                                WarehouseRequest.Name = WarehouseRequest.ParentName + "-" + CategoryDepartment.Code;
                                                                                WarehouseRequest.Code = WarehouseRequest.ParentName + "-" + CategoryDepartment.Code + "-" + WarehouseRequest.Date.Value.ToString("yyyyMMdd") + "-" + BaseParameter.BaseModel.End.Value.ToString("yyyyMMdd");

                                                                                //WarehouseRequest.Name = WarehouseRequest.Name + "-01";
                                                                                //WarehouseRequest.Code = WarehouseRequest.Code + "-01";
                                                                                var BaseParameterWarehouseRequest = new BaseParameter<WarehouseRequest>();
                                                                                BaseParameterWarehouseRequest.BaseModel = WarehouseRequest;
                                                                                await _WarehouseRequestService.SaveAsync(BaseParameterWarehouseRequest);
                                                                                if (WarehouseRequest.ID > 0)
                                                                                {
                                                                                    var ListProductionOrderBOMDetailOK = ListProductionOrderBOMDetail.Where(o => o.Display == CategoryDepartment.Display).ToList();
                                                                                    foreach (var item in ListProductionOrderBOMDetailOK)
                                                                                    {
                                                                                        var ListProductionOrderBOMDetailMaterialID = ListProductionOrderBOMDetail.Where(o => o.MaterialID == item.MaterialID).ToList();
                                                                                        var QuantityBOM = ListProductionOrderBOMDetailMaterialID.Sum(o => o.QuantityBOM);
                                                                                        var WarehouseRequestDetail = new WarehouseRequestDetail();
                                                                                        WarehouseRequestDetail.ParentID = WarehouseRequest.ID;
                                                                                        WarehouseRequestDetail.MaterialID = item.MaterialID;
                                                                                        WarehouseRequestDetail.CategoryFamilyID = item.CategoryFamilyID;
                                                                                        WarehouseRequestDetail.CategoryUnitID = item.CategoryUnitID;
                                                                                        WarehouseRequestDetail.Display = item.Display;

                                                                                        try
                                                                                        {
                                                                                            var WarehouseRequestDetailSub = await _WarehouseRequestDetailService.GetByCondition(o => o.ParentID == WarehouseRequestDetail.ParentID && o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefaultAsync();
                                                                                            if (WarehouseRequestDetailSub != null && WarehouseRequestDetailSub.ID > 0)
                                                                                            {
                                                                                                if (WarehouseRequestDetailSub.QuantityCheck != null)
                                                                                                {
                                                                                                    Quantity = Quantity + (int)WarehouseRequestDetailSub.QuantityCheck.Value;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {
                                                                                            string mes = ex.Message;
                                                                                        }


                                                                                        WarehouseRequestDetail.QuantityCheck = Quantity;
                                                                                        WarehouseRequestDetail.QuantityInvoice = QuantityBOM * WarehouseRequestDetail.QuantityCheck;
                                                                                        WarehouseRequestDetail.Quantity = WarehouseRequestDetail.QuantityInvoice;

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
                    }
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

