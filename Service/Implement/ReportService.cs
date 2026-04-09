namespace Service.Implement
{
    public class ReportService : BaseService<Report, IReportRepository>
    , IReportService
    {
        private readonly IReportRepository _ReportRepository;
        private readonly IReportDetailService _ReportDetailService;
        private readonly IReportDetailRepository _ReportDetailRepository;

        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderProductionPlanSemiRepository _ProductionOrderProductionPlanSemiRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IWarehouseStockRepository _WarehouseStockRepository;
        public ReportService(IReportRepository ReportRepository
            , IReportDetailService ReportDetailService
            , IReportDetailRepository ReportDetailRepository
            , IWarehouseInventoryRepository WarehouseInventoryRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IInvoiceInputRepository InvoiceInputRepository
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderProductionPlanSemiRepository ProductionOrderProductionPlanSemiRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IBOMRepository bOMRepository
            , IWarehouseStockRepository WarehouseStockRepository

            ) : base(ReportRepository)
        {
            _ReportRepository = ReportRepository;
            _ReportDetailService = ReportDetailService;
            _ReportDetailRepository = ReportDetailRepository;
            _WarehouseInventoryRepository = WarehouseInventoryRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderProductionPlanSemiRepository = ProductionOrderProductionPlanSemiRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _BOMRepository = bOMRepository;
            _WarehouseStockRepository = WarehouseStockRepository;
        }
        public override void InitializationSave(Report model)
        {
            BaseInitialization(model);
            model.Code = model.Code ?? GlobalHelper.InitializationDateTimeCode0001;
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;
        }
        public override async Task<BaseResult<Report>> SaveAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            if (BaseParameter.BaseModel != null)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_001ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "WarehouseInputDetailBarcode" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var DateLast = Report.Date.Value.AddDays(-1);
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == result.BaseModel.ParentID).ToListAsync();
                    var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                    var ListWarehouseInputDetailBarcodeLast = new List<WarehouseInputDetailBarcode>();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Date == Report.Date.Value.Date).ToListAsync();
                        ListWarehouseInputDetailBarcodeLast = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Date == DateLast.Date).ToListAsync();
                    }
                    ReportDetail.Quantity01 = ListWarehouseInputDetailBarcode.Count;
                    ReportDetail.QuantityActual01 = ListWarehouseInputDetailBarcodeLast.Count;
                    if (ReportDetail.QuantityActual01 == 0)
                    {
                        ReportDetail.QuantityActual01 = 1;
                    }
                    if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                    {
                        ReportDetail.Active = true;
                    }
                    else
                    {
                        ReportDetail.Active = false;
                    }
                    ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_002ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "WarehouseOutputDetailBarcode" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var DateLast = Report.Date.Value.AddDays(-1);
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.CustomerID == result.BaseModel.ParentID).ToListAsync();
                    var ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
                    var ListWarehouseOutputDetailBarcodeLast = new List<WarehouseOutputDetailBarcode>();
                    ReportDetail.Quantity01 = ListWarehouseOutputDetailBarcode.Count;
                    ReportDetail.QuantityActual01 = ListWarehouseOutputDetailBarcodeLast.Count;
                    if (ReportDetail.QuantityActual01 == 0)
                    {
                        ReportDetail.QuantityActual01 = 1;
                    }
                    if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                    {
                        ReportDetail.Active = true;
                    }
                    else
                    {
                        ReportDetail.Active = false;
                    }
                    ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_003ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "WarehouseInput" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var Month = Report.Month - 1;
                    var Year = Report.Year;
                    if (Month == 0)
                    {
                        Month = 12;
                        Year = Year - 1;
                    }
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == result.BaseModel.ParentID && o.Date != null && (o.Year == Report.Year || o.Year == Year) && (o.Month == Report.Month || o.Month == Month)).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListNow = ListWarehouseInput.Where(o => o.Year == Report.Year && o.Month == Report.Month).ToList();
                        var ListLast = ListWarehouseInput.Where(o => o.Year == Year && o.Month == Month).ToList();
                        ReportDetail.Quantity01 = ListNow.Count;
                        ReportDetail.QuantityActual01 = ListLast.Count;
                        if (ReportDetail.QuantityActual01 == 0)
                        {
                            ReportDetail.QuantityActual01 = 1;
                        }
                        if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                        {
                            ReportDetail.Active = true;
                        }
                        else
                        {
                            ReportDetail.Active = false;
                        }
                        ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    }
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_004ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();

            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "WarehouseOutput" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var Month = Report.Month - 1;
                    var Year = Report.Year;
                    if (Month == 0)
                    {
                        Month = 12;
                        Year = Year - 1;
                    }
                    var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.CustomerID == result.BaseModel.ParentID && o.Date != null && (o.Year == Report.Year || o.Year == Year) && (o.Month == Report.Month || o.Month == Month)).ToListAsync();
                    if (ListWarehouseOutput.Count > 0)
                    {
                        var ListNow = ListWarehouseOutput.Where(o => o.Year == Report.Year && o.Month == Report.Month).ToList();
                        var ListLast = ListWarehouseOutput.Where(o => o.Year == Year && o.Month == Month).ToList();
                        ReportDetail.Quantity01 = ListNow.Count;
                        ReportDetail.QuantityActual01 = ListLast.Count;
                        if (ReportDetail.QuantityActual01 == 0)
                        {
                            ReportDetail.QuantityActual01 = 1;
                        }
                        if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                        {
                            ReportDetail.Active = true;
                        }
                        else
                        {
                            ReportDetail.Active = false;
                        }
                        ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    }
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_005ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "InvoiceInput" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var Month = Report.Month - 1;
                    var Year = Report.Year;
                    if (Month == 0)
                    {
                        Month = 12;
                        Year = Year - 1;
                    }
                    var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == CategoryDepartment.CompanyID && o.DateETA != null && ((o.Year == Report.Year && o.Month == Report.Month) || (o.Year == Year && o.Month == Month))).ToListAsync();
                    if (ListInvoiceInput.Count > 0)
                    {
                        var ListNow = ListInvoiceInput.Where(o => o.Year == Report.Year && o.Month == Report.Month).ToList();
                        var ListLast = ListInvoiceInput.Where(o => o.Year == Year && o.Month == Month).ToList();
                        ReportDetail.Quantity01 = ListNow.Count;
                        ReportDetail.QuantityActual01 = ListLast.Count;
                        if (ReportDetail.QuantityActual01 == 0)
                        {
                            ReportDetail.QuantityActual01 = 1;
                        }
                        if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                        {
                            ReportDetail.Active = true;
                        }
                        else
                        {
                            ReportDetail.Active = false;
                        }
                        ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    }
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_006ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "ProductionOrder" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ReportDetail = new ReportDetail();
                    ReportDetail.ParentID = result.BaseModel.ID;
                    var Month = Report.Month - 1;
                    var Year = Report.Year;
                    if (Month == 0)
                    {
                        Month = 12;
                        Year = Year - 1;
                    }
                    var ListProductionOrder = await _ProductionOrderRepository.GetByCondition(o => o.Active == true && o.CustomerID == CategoryDepartment.CompanyID && o.Date != null && (o.Year == Report.Year || o.Year == Year) && (o.Month == Report.Month || o.Month == Month)).ToListAsync();
                    if (ListProductionOrder.Count > 0)
                    {
                        var ListNow = ListProductionOrder.Where(o => o.Year == Report.Year && o.Month == Report.Month).ToList();
                        var ListLast = ListProductionOrder.Where(o => o.Year == Year && o.Month == Month).ToList();
                        ReportDetail.Quantity01 = ListNow.Count;
                        ReportDetail.QuantityActual01 = ListLast.Count;
                        if (ReportDetail.QuantityActual01 == 0)
                        {
                            ReportDetail.QuantityActual01 = 1;
                        }
                        if (ReportDetail.Quantity01 > ReportDetail.QuantityActual01)
                        {
                            ReportDetail.Active = true;
                        }
                        else
                        {
                            ReportDetail.Active = false;
                        }
                        ReportDetail.QuantityGAP01 = ReportDetail.Quantity01 / ReportDetail.QuantityActual01 * 100;
                    }
                    var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                    BaseParameterReportDetail.BaseModel = ReportDetail;
                    await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetWarehouse001_007ToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            var Report = new Report();
            Report.CompanyID = BaseParameter.CompanyID;
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == Report.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Report.ParentID = CategoryDepartment.ID;
                Report.Date = GlobalHelper.InitializationDateTime;
                Report.Code = "WarehouseMaterial" + Report.Date.Value.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == result.BaseModel.ParentID).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == Report.Year && o.DateScan.Value.Month == Report.Month).ToListAsync();
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            var ListWarehouseInputDetailBarcodeMaterialName = ListWarehouseInputDetailBarcode.Select(o => o.MaterialName).Distinct().ToList();
                            foreach (var MaterialName in ListWarehouseInputDetailBarcodeMaterialName)
                            {
                                var ReportDetail = new ReportDetail();
                                ReportDetail.ParentID = result.BaseModel.ID;
                                ReportDetail.Code = MaterialName;
                                var ListWarehouseInputDetailBarcodeMaterial = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == ReportDetail.Code).ToList();
                                ReportDetail.Quantity01 = ListWarehouseInputDetailBarcodeMaterial.Count;
                                ReportDetail.Quantity02 = ListWarehouseInputDetailBarcodeMaterial.Sum(o => o.Quantity);
                                var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                                BaseParameterReportDetail.BaseModel = ReportDetail;
                                await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                            }
                        }
                    }
                    var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == result.BaseModel.ParentID).ToListAsync();
                    if (ListWarehouseOutput.Count > 0)
                    {
                        var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                        var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == Report.Year && o.DateScan.Value.Month == Report.Month).ToListAsync();
                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                        {
                            var ListWarehouseOutputDetailBarcodeMaterialName = ListWarehouseOutputDetailBarcode.Select(o => o.MaterialName).Distinct().ToList();
                            foreach (var MaterialName in ListWarehouseOutputDetailBarcodeMaterialName)
                            {
                                var ReportDetail = await _ReportDetailService.GetByCondition(o => o.ParentID == result.BaseModel.ID && o.Code == MaterialName).FirstOrDefaultAsync();
                                if (ReportDetail == null)
                                {
                                    ReportDetail = new ReportDetail();
                                }
                                ReportDetail.ParentID = result.BaseModel.ID;
                                ReportDetail.Code = MaterialName;
                                var ListWarehouseInputDetailBarcodeMaterial = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialName == ReportDetail.Code).ToList();
                                ReportDetail.Quantity03 = ListWarehouseInputDetailBarcodeMaterial.Count;
                                ReportDetail.Quantity04 = ListWarehouseInputDetailBarcodeMaterial.Sum(o => o.Quantity);
                                var BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                                BaseParameterReportDetail.BaseModel = ReportDetail;
                                await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                            }
                        }
                    }
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Report();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetByProductionTrackingToListAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    Report Report = new Report();
                    Report.CompanyID = BaseParameter.CompanyID;
                    Report.ParentID = BaseParameter.CompanyID;
                    Report.Code = GlobalHelper.InitializationDateTime.ToString("yyyyMMdd") + BaseParameter.SearchStringFilter01 + " | " + BaseParameter.SearchStringFilter + " | " + BaseParameter.SearchString;
                    BaseParameter.BaseModel = Report;
                    result = await SaveAsync(BaseParameter);
                    if (result.BaseModel != null && result.BaseModel.ID > 0)
                    {
                        BOM BOM = new BOM();
                        if (!string.IsNullOrEmpty(BaseParameter.SearchStringFilter))
                        {
                            BaseParameter.SearchStringFilter = BaseParameter.SearchStringFilter.Trim();
                            BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.Code == BaseParameter.SearchStringFilter && o.MaterialCode == BaseParameter.SearchString).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                        }
                        if (BOM == null || BOM.ID == 0)
                        {
                            BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.MaterialCode == BaseParameter.SearchString).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                        }
                        if (BOM != null && BOM.ID > 0)
                        {
                            var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == BOM.ID).OrderByDescending(o => o.Level).ToListAsync();
                            if (ListBOM.Count > 0)
                            {
                                var ListBOMMaterialCode = ListBOM.Select(o => o.MaterialCode).ToList();
                                ProductionOrder ProductionOrder = new ProductionOrder();
                                var ListProductionOrderProductionPlanSemi = new List<ProductionOrderProductionPlanSemi>();
                                if (!string.IsNullOrEmpty(BaseParameter.SearchStringFilter01))
                                {
                                    ProductionOrder = await _ProductionOrderRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.Code == BaseParameter.SearchStringFilter01).FirstOrDefaultAsync();
                                    if (ProductionOrder != null && ProductionOrder.ID > 0)
                                    {
                                        ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.SortOrder > 2 && o.ParentID == ProductionOrder.ID && o.MaterialCode01 == BaseParameter.SearchString).ToListAsync();
                                    }
                                }
                                var ListWarehouseStock = await _WarehouseStockRepository.GetByCondition(o => o.Year == 0 && o.Month == 0 && o.Day == 0 && o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.CategoryDepartmentName) && o.CategoryDepartmentName.Contains("HOOK_RACK") && !string.IsNullOrEmpty(o.Code) && ListBOMMaterialCode.Contains(o.Code)).ToListAsync();
                                string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                                string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                                List<torderlist> Listtorderlist = new List<torderlist>();
                                string sql = @"select LEAD_NO, IFNULL(SUM(TOT_QTY),0) AS 'TOT_QTY', IFNULL(SUM(PERFORMN),0) AS 'PERFORMN' from torderlist WHERE UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') AND (MC NOT IN ('SHIELD WIRE') OR MC NOT IN ('SHIELD WIRE')) GROUP BY LEAD_NO";
                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                                }
                                List<torderlist> ListtorderlistSHIELDWIRE = new List<torderlist>();
                                sql = @"select LEAD_NO, IFNULL(SUM(TOT_QTY),0) AS 'TOT_QTY', IFNULL(SUM(PERFORMN),0) AS 'PERFORMN' from torderlist WHERE UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') AND DSCN_YN='Y' AND (MC IN ('SHIELD WIRE') OR MC IN ('SHIELD WIRE')) GROUP BY LEAD_NO";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    ListtorderlistSHIELDWIRE.AddRange(SQLHelper.ToList<torderlist>(dt));
                                }
                                List<torderlist_lp> Listtorderlist_lp = new List<torderlist_lp>();
                                sql = @"SELECT TORDERLIST.LEAD_NO, IFNULL(SUM(TORDERLIST_LP.TOT_QTY),0) AS 'TOT_QTY', IFNULL(SUM(TORDERLIST_LP.PERFORMN_L),0) AS 'PERFORMN_L' , IFNULL(SUM(TORDERLIST_LP.PERFORMN_R),0) AS 'PERFORMN_R' from TORDERLIST_LP JOIN TORDERLIST ON TORDERLIST_LP.ORDER_IDX=TORDERLIST.ORDER_IDX WHERE TORDERLIST_LP.UPDATE_DTM >= '" + DateBegin + "' AND TORDERLIST_LP.UPDATE_DTM <= '" + DateEnd + "' AND TORDERLIST_LP.`CONDITION` IN ('Complete', 'Working') GROUP BY TORDERLIST.LEAD_NO";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    Listtorderlist_lp.AddRange(SQLHelper.ToList<torderlist_lp>(dt));
                                }
                                List<torderlist_spst> Listtorderlist_spst = new List<torderlist_spst>();
                                sql = @"SELECT LEAD_NO, IFNULL(SUM(PO_QTY),0) AS 'PO_QTY', IFNULL(SUM(PERFORMN),0) AS 'PERFORMN' from torderlist_spst WHERE UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') GROUP BY LEAD_NO";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    Listtorderlist_spst.AddRange(SQLHelper.ToList<torderlist_spst>(dt));
                                }
                                foreach (var MaterialCode in ListBOMMaterialCode)
                                {
                                    if (!string.IsNullOrEmpty(MaterialCode))
                                    {
                                        var MaterialCodeTrim = MaterialCode.Trim();
                                        ReportDetail ReportDetail = new ReportDetail();
                                        ReportDetail.ParentID = result.BaseModel.ID;
                                        ReportDetail.Code = MaterialCodeTrim;
                                        ReportDetail.Name = BaseParameter.SearchString;
                                        ReportDetail.Display = BaseParameter.SearchStringFilter;
                                        ReportDetail.Description = BaseParameter.SearchStringFilter01;
                                        ReportDetail.QuantityActual03 = ProductionOrder.ID;
                                        var BOMSub = ListBOM.Where(o => o.MaterialCode == MaterialCodeTrim).FirstOrDefault();
                                        if (BOMSub != null && BOMSub.ID > 0)
                                        {
                                            ReportDetail.SortOrder = BOMSub.Level;
                                        }
                                        var ProductionOrderProductionPlanSemi = ListProductionOrderProductionPlanSemi.Where(o => o.MaterialCode == MaterialCodeTrim).FirstOrDefault();
                                        if (ProductionOrderProductionPlanSemi != null && ProductionOrderProductionPlanSemi.ID > 0)
                                        {
                                            ReportDetail.Quantity01 = ProductionOrderProductionPlanSemi.Quantity00;
                                            ReportDetail.QuantityActual01 = ProductionOrderProductionPlanSemi.BOMID;
                                            ReportDetail.QuantityActual02 = ProductionOrderProductionPlanSemi.BOMID01;
                                        }
                                        var WarehouseStock = ListWarehouseStock.Where(o => o.Code == MaterialCodeTrim).FirstOrDefault();
                                        if (WarehouseStock != null && WarehouseStock.ID > 0)
                                        {
                                            ReportDetail.Quantity02 = WarehouseStock.Quantity00;
                                        }
                                        var torderlist = Listtorderlist.Where(o => !string.IsNullOrEmpty(o.LEAD_NO) && o.LEAD_NO.Trim() == MaterialCodeTrim).FirstOrDefault();
                                        if (torderlist != null && !string.IsNullOrEmpty(torderlist.LEAD_NO))
                                        {
                                            ReportDetail.Quantity03 = (decimal?)torderlist.TOT_QTY;
                                            ReportDetail.Quantity04 = (decimal?)torderlist.PERFORMN;
                                            ReportDetail.Quantity05 = ReportDetail.Quantity04 * 100 / ReportDetail.Quantity03;
                                        }
                                        var torderlistSHIELDWIRE = ListtorderlistSHIELDWIRE.Where(o => !string.IsNullOrEmpty(o.LEAD_NO) && o.LEAD_NO.Trim() == MaterialCodeTrim).FirstOrDefault();
                                        if (torderlistSHIELDWIRE != null && !string.IsNullOrEmpty(torderlistSHIELDWIRE.LEAD_NO))
                                        {
                                            ReportDetail.Quantity06 = (decimal?)torderlistSHIELDWIRE.TOT_QTY;
                                            ReportDetail.Quantity07 = (decimal?)torderlistSHIELDWIRE.PERFORMN;
                                            ReportDetail.Quantity08 = ReportDetail.Quantity07 * 100 / ReportDetail.Quantity06;
                                        }
                                        var torderlist_lp = Listtorderlist_lp.Where(o => !string.IsNullOrEmpty(o.LEAD_NO) && o.LEAD_NO.Trim() == MaterialCodeTrim).FirstOrDefault();
                                        if (torderlist_lp != null && !string.IsNullOrEmpty(torderlist_lp.LEAD_NO))
                                        {
                                            ReportDetail.Quantity09 = (decimal?)torderlist_lp.TOT_QTY;
                                            ReportDetail.Quantity10 = (decimal?)torderlist_lp.PERFORMN_L;
                                            ReportDetail.Quantity11 = (decimal?)torderlist_lp.PERFORMN_R;
                                            ReportDetail.Quantity12 = (ReportDetail.Quantity10 + ReportDetail.Quantity11) * 100 / (ReportDetail.Quantity09 * 2);
                                            ReportDetail.Quantity16 = 0;
                                            if (torderlist_lp.CONDITION == "Complete")
                                            {
                                                ReportDetail.Quantity16 = ReportDetail.Quantity09;
                                            }
                                        }
                                        var torderlist_spst = Listtorderlist_spst.Where(o => !string.IsNullOrEmpty(o.LEAD_NO) && o.LEAD_NO.Trim() == MaterialCodeTrim).FirstOrDefault();
                                        if (torderlist_spst != null && !string.IsNullOrEmpty(torderlist_spst.LEAD_NO))
                                        {
                                            ReportDetail.Quantity13 = (decimal?)torderlist_spst.PO_QTY;
                                            ReportDetail.Quantity14 = (decimal?)torderlist_spst.PERFORMN;
                                            ReportDetail.Quantity15 = ReportDetail.Quantity14 * 100 / ReportDetail.Quantity13;
                                        }
                                        ReportDetail.Quantity02 = ReportDetail.Quantity02 ?? 0;
                                        ReportDetail.Quantity04 = ReportDetail.Quantity04 ?? 0;
                                        ReportDetail.Quantity07 = ReportDetail.Quantity07 ?? 0;
                                        ReportDetail.Quantity16 = ReportDetail.Quantity16 ?? 0;
                                        ReportDetail.Quantity14 = ReportDetail.Quantity14 ?? 0;

                                        ReportDetail.Quantity17 = ReportDetail.Quantity02 + ReportDetail.Quantity04 + ReportDetail.Quantity07 + ReportDetail.Quantity16 + ReportDetail.Quantity14;
                                        BaseParameter<ReportDetail> BaseParameterReportDetail = new BaseParameter<ReportDetail>();
                                        BaseParameterReportDetail.BaseModel = ReportDetail;
                                        await _ReportDetailService.SaveAsync(BaseParameterReportDetail);
                                    }
                                }
                            }
                        }

                        var ListReportDetail = await _ReportDetailService.GetByCondition(o => o.ParentID == result.BaseModel.ID).ToListAsync();
                        decimal? Quantity18 = ListReportDetail.Min(o => o.Quantity17);
                        foreach (var ReportDetail in ListReportDetail)
                        {
                            ReportDetail.Quantity18 = Quantity18;
                            await _ReportDetailRepository.UpdateAsync(ReportDetail);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetByProductionTracking2026Async(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0)
            {
                var Code = BaseParameter.CompanyID + "-ProductionTracking2026-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                result.BaseModel = await GetByCondition(o => o.Code == Code).FirstOrDefaultAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> SyncByProductionTracking2026Async(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Date != null)
            {
                Report Report = new Report();
                Report.CompanyID = BaseParameter.CompanyID;
                Report.ParentID = BaseParameter.CompanyID;
                Report.Code = BaseParameter.CompanyID + "-ProductionTracking2026-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ListReportDetailExist = await _ReportDetailRepository.GetByCondition(o => o.ParentID == result.BaseModel.ID).ToListAsync();
                    await _ReportDetailRepository.RemoveRangeAsync(ListReportDetailExist);
                    var ListReportDetail = new List<ReportDetail>();
                    string DateBegin = BaseParameter.Date.Value.ToString("yyyy-MM-dd 00:00:00");
                    string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);

                    List<trackmtim> Listtrackmtim = new List<trackmtim>();
                    string sql = @"select LEAD_NM, IFNULL(SUM(QTY),0) AS 'QTY' from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM != '' GROUP BY LEAD_NM";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                    }
                    foreach (var trackmtim in Listtrackmtim)
                    {
                        var ReportDetail = ListReportDetail.Where(o => o.Code == trackmtim.LEAD_NM).FirstOrDefault();
                        if (ReportDetail == null)
                        {
                            ReportDetail = new ReportDetail();
                            ReportDetail.ParentID = result.BaseModel.ID;
                            ReportDetail.Code = trackmtim.LEAD_NM;
                            ReportDetail.Quantity01 = (decimal?)trackmtim.QTY;
                            if (ReportDetail.SortOrder == null)
                            {
                                if (!string.IsNullOrEmpty(ReportDetail.Code) && ReportDetail.Code.Length > 2)
                                {
                                    string SPSTCode = ReportDetail.Code.Substring(0, 2);
                                    if (SPSTCode == "ST" || SPSTCode == "SP")
                                    {
                                        ReportDetail.SortOrder = 9;
                                    }
                                }
                            }
                            ListReportDetail.Add(ReportDetail);
                        }
                        else
                        {
                            for (int i = 0; i < ListReportDetail.Count; i++)
                            {
                                if (ListReportDetail[i].Code == trackmtim.LEAD_NM)
                                {
                                    ListReportDetail[i].ParentID = result.BaseModel.ID;
                                    ListReportDetail[i].Code = trackmtim.LEAD_NM;
                                    ListReportDetail[i].Quantity01 = (decimal?)trackmtim.QTY;
                                    if (ListReportDetail[i].SortOrder == null)
                                    {
                                        if (!string.IsNullOrEmpty(ListReportDetail[i].Code) && ListReportDetail[i].Code.Length > 2)
                                        {
                                            string SPSTCode = ListReportDetail[i].Code.Substring(0, 2);
                                            if (SPSTCode == "ST" || SPSTCode == "SP")
                                            {
                                                ListReportDetail[i].SortOrder = 9;
                                            }
                                        }
                                    }
                                    i = ListReportDetail.Count;
                                }
                            }
                        }
                    }

                    List<torderlist> Listtorderlist = new List<torderlist>();
                    StringBuilder sqlLine = new StringBuilder();
                    sqlLine.AppendLine(@"SELECT a.LEAD_NO, SUM(a.TOT_QTY) AS 'TOT_QTY', SUM(case when a.PERFORMN<a.TOT_QTY then a.PERFORMN ELSE a.TOT_QTY END ) AS 'PERFORMN' FROM (");
                    sqlLine.AppendLine(@"SELECT");
                    sqlLine.AppendLine(@"TORDERLIST.LEAD_NO");
                    sqlLine.AppendLine(@", TORDERLIST.BUNDLE_SIZE");
                    sqlLine.AppendLine(@", TORDERLIST.ORDER_IDX");
                    sqlLine.AppendLine(@", TORDERLIST.TOT_QTY");
                    sqlLine.AppendLine(@", (TORDERLIST.BUNDLE_SIZE*(SELECT COUNT(*) FROM torder_barcode WHERE torder_barcode.ORDER_IDX=TORDERLIST.ORDER_IDX AND torder_barcode.WORK_END IS NOT NULL AND torder_barcode.WORK_END >= '" + DateBegin + "' AND torder_barcode.WORK_END <= '" + DateEnd + "' AND torder_barcode.TORDER_BARCODENM NOT IN (select BARCODE_NM from trackmtim where RACKCODE NOT IN ('OUTPUT') AND BARCODE_NM != '' AND LEAD_NM=TORDERLIST.LEAD_NO))) AS 'PERFORMN'");
                    sqlLine.AppendLine(@"FROM TORDERLIST");
                    sqlLine.AppendLine(@"WHERE TORDERLIST.UPDATE_DTM >= '" + DateBegin + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST.UPDATE_DTM <= '" + DateEnd + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST.`CONDITION` IN ('Complete', 'Working')");
                    sqlLine.AppendLine(@"AND TORDERLIST.MC NOT IN ('SHIELD WIRE')");
                    sqlLine.AppendLine(@") a GROUP BY a.LEAD_NO ORDER BY a.LEAD_NO ASC");
                    sql = sqlLine.ToString();
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    foreach (var torderlist in Listtorderlist)
                    {
                        var ReportDetail = ListReportDetail.Where(o => o.Code == torderlist.LEAD_NO).FirstOrDefault();
                        if (ReportDetail == null)
                        {
                            ReportDetail = new ReportDetail();
                            ReportDetail.ParentID = result.BaseModel.ID;
                            ReportDetail.Code = torderlist.LEAD_NO;
                            ReportDetail.Quantity05 = (decimal?)torderlist.TOT_QTY;
                            ReportDetail.Quantity06 = (decimal?)torderlist.PERFORMN;
                            ListReportDetail.Add(ReportDetail);
                        }
                        else
                        {
                            for (int i = 0; i < ListReportDetail.Count; i++)
                            {
                                if (ListReportDetail[i].Code == torderlist.LEAD_NO)
                                {
                                    ListReportDetail[i].ParentID = result.BaseModel.ID;
                                    ListReportDetail[i].Code = torderlist.LEAD_NO;
                                    ListReportDetail[i].Quantity05 = (decimal?)torderlist.TOT_QTY;
                                    ListReportDetail[i].Quantity06 = (decimal?)torderlist.PERFORMN;
                                    i = ListReportDetail.Count;
                                }
                            }
                        }
                    }

                    Listtorderlist = new List<torderlist>();
                    sqlLine = new StringBuilder();
                    sqlLine.AppendLine(@"SELECT a.LEAD_NO, SUM(a.TOT_QTY) AS 'TOT_QTY', SUM(a.PERFORMN_L) AS 'BUNDLE_SIZE', SUM(a.PERFORMN_R) AS 'ADJ_AF_QTY', SUM(case when a.PERFORMN<a.TOT_QTY then a.PERFORMN ELSE a.TOT_QTY END ) AS 'PERFORMN' FROM (");
                    sqlLine.AppendLine(@"SELECT");
                    sqlLine.AppendLine(@"TORDERLIST.LEAD_NO");
                    sqlLine.AppendLine(@", TORDERLIST.BUNDLE_SIZE");
                    sqlLine.AppendLine(@", TORDERLIST.ORDER_IDX");
                    sqlLine.AppendLine(@", TORDERLIST_LP.TOT_QTY");
                    sqlLine.AppendLine(@", TORDERLIST_LP.PERFORMN_L");
                    sqlLine.AppendLine(@", TORDERLIST_LP.PERFORMN_R");
                    sqlLine.AppendLine(@", (TORDERLIST_LP.BUNDLE_SIZE*(SELECT COUNT(*) FROM torder_barcode_lp WHERE torder_barcode_lp.ORDER_IDX=TORDERLIST.ORDER_IDX AND torder_barcode_lp.WORK_END IS NOT NULL AND torder_barcode_lp.WORK_END >= '" + DateBegin + "' AND torder_barcode_lp.WORK_END <= '" + DateEnd + "' AND torder_barcode_lp.TORDER_BARCODENM NOT IN (select BARCODE_NM from trackmtim where RACKCODE NOT IN ('OUTPUT') AND BARCODE_NM != '' AND LEAD_NM=TORDERLIST.LEAD_NO))) AS 'PERFORMN'");
                    sqlLine.AppendLine(@"FROM TORDERLIST JOIN TORDERLIST_LP ON TORDERLIST_LP.ORDER_IDX=TORDERLIST.ORDER_IDX");
                    sqlLine.AppendLine(@"WHERE TORDERLIST.UPDATE_DTM >= '" + DateBegin + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST_LP.UPDATE_DTM <= '" + DateEnd + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST_LP.`CONDITION` IN ('Complete', 'Working')");
                    sqlLine.AppendLine(@") a GROUP BY a.LEAD_NO ORDER BY a.LEAD_NO ASC");
                    sql = sqlLine.ToString();
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    foreach (var torderlist in Listtorderlist)
                    {
                        var ReportDetail = ListReportDetail.Where(o => o.Code == torderlist.LEAD_NO).FirstOrDefault();
                        if (ReportDetail == null)
                        {
                            ReportDetail = new ReportDetail();
                            ReportDetail.ParentID = result.BaseModel.ID;
                            ReportDetail.Code = torderlist.LEAD_NO;
                            ReportDetail.Quantity10 = (decimal?)torderlist.TOT_QTY;
                            ReportDetail.Quantity13 = (decimal?)torderlist.PERFORMN;
                            ReportDetail.Quantity11 = (decimal?)torderlist.BUNDLE_SIZE;
                            ReportDetail.Quantity12 = (decimal?)torderlist.ADJ_AF_QTY;
                            ListReportDetail.Add(ReportDetail);
                        }
                        else
                        {
                            for (int i = 0; i < ListReportDetail.Count; i++)
                            {
                                if (ListReportDetail[i].Code == torderlist.LEAD_NO)
                                {
                                    ListReportDetail[i].ParentID = result.BaseModel.ID;
                                    ListReportDetail[i].Code = torderlist.LEAD_NO;
                                    ListReportDetail[i].Quantity10 = (decimal?)torderlist.TOT_QTY;
                                    ListReportDetail[i].Quantity13 = (decimal?)torderlist.PERFORMN;
                                    ListReportDetail[i].Quantity11 = (decimal?)torderlist.BUNDLE_SIZE;
                                    ListReportDetail[i].Quantity12 = (decimal?)torderlist.ADJ_AF_QTY;
                                    i = ListReportDetail.Count;
                                }
                            }
                        }
                    }



                    Listtorderlist = new List<torderlist>();
                    sqlLine = new StringBuilder();
                    sqlLine.AppendLine(@"SELECT a.LEAD_NO, SUM(a.PO_QTY) AS 'TOT_QTY', SUM(case when a.PERFORMN<a.PO_QTY then a.PERFORMN ELSE a.PO_QTY END ) AS 'PERFORMN' FROM (");
                    sqlLine.AppendLine(@"SELECT");
                    sqlLine.AppendLine(@"TORDERLIST_SPST.LEAD_NO");
                    sqlLine.AppendLine(@", TORDERLIST_SPST.BUNDLE_SIZE");
                    sqlLine.AppendLine(@", TORDERLIST_SPST.ORDER_IDX");
                    sqlLine.AppendLine(@", TORDERLIST_SPST.PO_QTY");
                    sqlLine.AppendLine(@", (TORDERLIST_SPST.BUNDLE_SIZE*(SELECT COUNT(*) FROM torder_barcode_sp WHERE torder_barcode_sp.ORDER_IDX=TORDERLIST_SPST.ORDER_IDX AND torder_barcode_sp.WORK_END IS NOT NULL AND torder_barcode_sp.WORK_END >= '" + DateBegin + "' AND torder_barcode_sp.WORK_END <= '" + DateEnd + "' AND torder_barcode_sp.TORDER_BARCODENM NOT IN (select BARCODE_NM from trackmtim where RACKCODE NOT IN ('OUTPUT') AND BARCODE_NM != '' AND LEAD_NM=TORDERLIST_SPST.LEAD_NO))) AS 'PERFORMN'");
                    sqlLine.AppendLine(@"FROM TORDERLIST_SPST");
                    sqlLine.AppendLine(@"WHERE TORDERLIST_SPST.UPDATE_DTM >= '" + DateBegin + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST_SPST.UPDATE_DTM <= '" + DateEnd + "'");
                    sqlLine.AppendLine(@"AND TORDERLIST_SPST.`CONDITION` IN ('Complete', 'Working')");
                    sqlLine.AppendLine(@") a GROUP BY a.LEAD_NO ORDER BY a.LEAD_NO ASC");
                    sql = sqlLine.ToString();
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    foreach (var torderlist in Listtorderlist)
                    {
                        var ReportDetail = ListReportDetail.Where(o => o.Code == torderlist.LEAD_NO).FirstOrDefault();
                        if (ReportDetail == null)
                        {
                            ReportDetail = new ReportDetail();
                            ReportDetail.SortOrder = 9;
                            ReportDetail.ParentID = result.BaseModel.ID;
                            ReportDetail.Code = torderlist.LEAD_NO;
                            ReportDetail.Quantity20 = (decimal?)torderlist.TOT_QTY;
                            ReportDetail.Quantity21 = (decimal?)torderlist.PERFORMN;
                            ListReportDetail.Add(ReportDetail);
                        }
                        else
                        {
                            for (int i = 0; i < ListReportDetail.Count; i++)
                            {
                                if (ListReportDetail[i].Code == torderlist.LEAD_NO)
                                {
                                    ListReportDetail[i].ParentID = result.BaseModel.ID;
                                    ListReportDetail[i].Code = torderlist.LEAD_NO;
                                    ListReportDetail[i].Quantity20 = (decimal?)torderlist.TOT_QTY;
                                    ListReportDetail[i].Quantity21 = (decimal?)torderlist.PERFORMN;
                                    i = ListReportDetail.Count;
                                }
                            }
                        }
                    }

                    var ListSPSTLEAD_PN = ListReportDetail.Where(o => o.SortOrder == 9).OrderBy(o => o.Code).Select(o => o.Code).Distinct().ToList();
                    List<CategoryUnit> ListSPST_LeadNo = new List<CategoryUnit>();
                    foreach (var LEAD_PN in ListSPSTLEAD_PN)
                    {
                        sql = @"SELECT LEAD_PN AS 'Code', '" + LEAD_PN + "' AS 'Name' FROM torder_lead_bom WHERE LEAD_INDEX IN (SELECT torder_lead_bom_spst.S_PART_IDX FROM torder_lead_bom_spst WHERE torder_lead_bom_spst.M_PART_IDX IN (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN IN ('" + LEAD_PN + "')))";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            ListSPST_LeadNo.AddRange(SQLHelper.ToList<CategoryUnit>(dt));
                        }
                    }

                    var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    ListReportDetail = ListReportDetail.OrderByDescending(o => o.Active).ThenBy(o => o.Code).ToList();
                    for (int i = 0; i < ListReportDetail.Count; i++)
                    {
                        ListReportDetail[i].Active = true;
                        if (ListReportDetail[i].SortOrder == 9)
                        {
                            ListReportDetail[i].Active = false;
                        }
                        ListReportDetail[i].CompanyID = BaseParameter.CompanyID;
                        ListReportDetail[i].Quantity02 = 0;
                        ListReportDetail[i].Quantity30 = 0;
                        ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity01 ?? 0);
                        ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity13 ?? 0);
                        ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity21 ?? 0);
                        if (ListReportDetail[i].Quantity13 == null)
                        {
                            ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity06 ?? 0);
                        }
                        var ListBOMSub = ListBOM.Where(o => o.MaterialCode == ListReportDetail[i].Code).ToList();
                        if (ListBOMSub.Count > 0)
                        {
                            ListReportDetail[i].Description = string.Join(",", ListBOMSub.Select(o => o.ParentName).Distinct().ToList());
                        }
                    }
                    if (ListSPST_LeadNo.Count > 0)
                    {
                        for (int i = 0; i < ListReportDetail.Count; i++)
                        {
                            if (ListReportDetail[i].SortOrder == null)
                            {
                                string Code = ListReportDetail[i].Code;
                                var ListSPST_LeadNoSub = ListSPST_LeadNo.Where(o => o.Code == ListReportDetail[i].Code).ToList();
                                if (ListSPST_LeadNoSub.Count > 0)
                                {
                                    var ListSPST_LeadNoSubName = ListSPST_LeadNoSub.Select(o => o.Name).ToList();
                                    var ListReportDetailSPST = ListReportDetail.Where(o => ListSPST_LeadNoSubName.Contains(o.Code)).ToList();
                                    ListReportDetail[i].Note = string.Join(",", ListSPST_LeadNoSubName);
                                    ListReportDetail[i].Quantity02 = ListReportDetailSPST.Sum(o => o.Quantity30);
                                    ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity02 ?? 0);
                                }
                            }
                        }
                    }

                    await _ReportDetailRepository.AddRangeAsync(ListReportDetail);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> GetByWarehouseStockLongTermAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
            {
                var Code = BaseParameter.CompanyID + "-" + BaseParameter.CategoryDepartmentID + "-WarehouseStockLongTerm-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                result.BaseModel = await GetByCondition(o => o.Code == Code).FirstOrDefaultAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> SyncByWarehouseStockLongTermAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
            {
                Report Report = new Report();
                Report.CompanyID = BaseParameter.CompanyID;
                Report.ParentID = BaseParameter.CompanyID;
                Report.Code = BaseParameter.CompanyID + "-" + BaseParameter.CategoryDepartmentID + "-WarehouseStockLongTerm-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                    MySQLHelper.ERPSyncAsync03(GlobalHelper.ERP_MariaDBConectionString);
                    DateTime Now = GlobalHelper.InitializationDateTime;
                    DateTime NowYear = Now.AddYears(-1);
                    DateTime NowMonth = Now.AddMonths(-6);
                    var ListWarehouseInputDetailBarcodeYear = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => !string.IsNullOrEmpty(o.CategoryFamilyName) && o.CategoryFamilyName.ToLower() != "tape" && o.DateScan != null && o.DateScan <= NowYear && o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.QuantityInventory > 0).ToListAsync();
                    var ListWarehouseInputDetailBarcodeMonth = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => !string.IsNullOrEmpty(o.CategoryFamilyName) && o.CategoryFamilyName.ToLower() == "tape" && o.DateScan != null && o.DateScan <= NowMonth && o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.QuantityInventory > 0).ToListAsync();

                    var ListReportDetailExist = await _ReportDetailRepository.GetByCondition(o => o.ParentID == result.BaseModel.ID).ToListAsync();
                    await _ReportDetailRepository.RemoveRangeAsync(ListReportDetailExist);
                    var ListReportDetail = new List<ReportDetail>();
                    var ListWarehouseInputDetailBarcodeMaterialID = ListWarehouseInputDetailBarcodeYear.Select(o => o.MaterialID).Distinct().ToList();
                    foreach (var MaterialID in ListWarehouseInputDetailBarcodeMaterialID)
                    {
                        ReportDetail ReportDetail = new ReportDetail();
                        ReportDetail.ParentID = result.BaseModel.ID;
                        ReportDetail.SortOrder = 10;
                        ReportDetail.Date01 = NowYear;
                        ReportDetail.CompanyID = MaterialID;
                        var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcodeYear.Where(o => o.MaterialID == MaterialID).ToList();
                        ReportDetail.Date03 = ListWarehouseInputDetailBarcodeSub.Max(o => o.DateScan);
                        ReportDetail.Date02 = ListWarehouseInputDetailBarcodeSub.Min(o => o.DateScan);
                        ReportDetail.Quantity01 = ListWarehouseInputDetailBarcodeSub.Sum(o => o.QuantityInventory);
                        ReportDetail.Code = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.MaterialName).Distinct().ToList());
                        ReportDetail.Name = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.CategoryFamilyName).Distinct().ToList());
                        ReportDetail.Display = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.CategoryCompanyName).Distinct().ToList());
                        ListReportDetail.Add(ReportDetail);
                    }
                    ListWarehouseInputDetailBarcodeMaterialID = ListWarehouseInputDetailBarcodeMonth.Select(o => o.MaterialID).Distinct().ToList();
                    foreach (var MaterialID in ListWarehouseInputDetailBarcodeMaterialID)
                    {
                        ReportDetail ReportDetail = new ReportDetail();
                        ReportDetail.ParentID = result.BaseModel.ID;
                        ReportDetail.SortOrder = 100;
                        ReportDetail.Date01 = NowMonth;
                        ReportDetail.CompanyID = MaterialID;
                        var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcodeMonth.Where(o => o.MaterialID == MaterialID).ToList();
                        ReportDetail.Date03 = ListWarehouseInputDetailBarcodeSub.Max(o => o.DateScan);
                        ReportDetail.Date02 = ListWarehouseInputDetailBarcodeSub.Min(o => o.DateScan);
                        ReportDetail.Quantity01 = ListWarehouseInputDetailBarcodeSub.Sum(o => o.QuantityInventory);
                        ReportDetail.Code = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.MaterialName).Distinct().ToList());
                        ReportDetail.Name = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.CategoryFamilyName).Distinct().ToList());
                        ReportDetail.Display = string.Join(",", ListWarehouseInputDetailBarcodeSub.Select(o => o.CategoryCompanyName).Distinct().ToList());
                        ListReportDetail.Add(ReportDetail);
                    }
                    await _ReportDetailRepository.AddRangeAsync(ListReportDetail);
                    ListReportDetail = new List<ReportDetail>();
                    var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                    ListWarehouseInputDetailBarcode.AddRange(ListWarehouseInputDetailBarcodeYear);
                    ListWarehouseInputDetailBarcode.AddRange(ListWarehouseInputDetailBarcodeMonth);
                    foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                    {
                        ReportDetail ReportDetail = new ReportDetail();
                        ReportDetail.ParentID = result.BaseModel.ID;
                        ReportDetail.SortOrder = 1000;
                        ReportDetail.CompanyID = WarehouseInputDetailBarcode.MaterialID;
                        ReportDetail.Code = WarehouseInputDetailBarcode.MaterialName;
                        ReportDetail.Description = WarehouseInputDetailBarcode.Barcode;
                        ReportDetail.Note = WarehouseInputDetailBarcode.CategoryLocationName;
                        ReportDetail.Name = WarehouseInputDetailBarcode.CategoryFamilyName;
                        ReportDetail.Display = WarehouseInputDetailBarcode.CategoryCompanyName;
                        ReportDetail.Date01 = WarehouseInputDetailBarcode.DateScan;
                        ReportDetail.Quantity01 = WarehouseInputDetailBarcode.QuantityInventory;
                        ListReportDetail.Add(ReportDetail);
                    }
                    await _ReportDetailRepository.AddRangeAsync(ListReportDetail);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Report>> HookRackGetByCompanyID_Begin_End_SearchStringAsync(BaseParameter<Report> BaseParameter)
        {
            var result = new BaseResult<Report>();
            result.BaseModel = new Report();
            if (BaseParameter.CompanyID > 0)
            {
                BaseParameter.DateBegin = BaseParameter.DateBegin ?? GlobalHelper.InitializationDateTime;
                BaseParameter.DateEnd = BaseParameter.DateEnd ?? GlobalHelper.InitializationDateTime;

                BaseParameter.DateBegin = new DateTime(BaseParameter.DateBegin.Value.Year, BaseParameter.DateBegin.Value.Month, BaseParameter.DateBegin.Value.Day, 0, 0, 0);
                BaseParameter.DateEnd = new DateTime(BaseParameter.DateEnd.Value.Year, BaseParameter.DateEnd.Value.Month, BaseParameter.DateEnd.Value.Day, 23, 59, 59);

                var DateBegin = BaseParameter.DateBegin.Value.ToString("yyyy-MM-dd HH:mm:ss");
                var DateEnd = BaseParameter.DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

                Report Report = new Report();
                Report.CompanyID = BaseParameter.CompanyID;
                Report.ParentID = BaseParameter.CompanyID;
                Report.Code = "HookRack-" + Report.CompanyID + "-" + BaseParameter.DateBegin.Value.ToString("yyyyMMdd") + "-" + BaseParameter.DateEnd.Value.ToString("yyyyMMdd") + "-" + GlobalHelper.InitializationDateTimeCode;
                BaseParameter.BaseModel = Report;
                result = await SaveAsync(BaseParameter);
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    string sql = GlobalHelper.InitializationString;
                    if (BaseParameter.Active == true)
                    {
                        sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') ORDER BY RACKDTM DESC";
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') AND LEAD_NM in ('" + BaseParameter.SearchString + "') ORDER BY RACKDTM DESC";
                        }
                    }
                    else
                    {
                        sql = @"select * from trackmtim where (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') ORDER BY RACKDTM DESC";
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            sql = @"select * from trackmtim where (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') AND LEAD_NM in ('" + BaseParameter.SearchString + "') ORDER BY RACKDTM DESC";
                        }
                    }
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);

                    var Listtrackmtim = new List<trackmtim>();
                    if (ds.Tables.Count > 0)
                    {
                        Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                    }
                    var ListReportDetail = new List<ReportDetail>();
                    var ListtrackmtimLeadNM = Listtrackmtim.Select(o => o.LEAD_NM).Distinct().ToList();
                    foreach (var LEAD_NM in ListtrackmtimLeadNM)
                    {
                        ReportDetail ReportDetail = new ReportDetail();
                        ReportDetail.ParentID = result.BaseModel.ID;
                        ReportDetail.ParentName = result.BaseModel.Code;
                        ReportDetail.Code = LEAD_NM;
                        var ListtrackmtimSub = Listtrackmtim.Where(o => o.LEAD_NM == LEAD_NM && o.RACKCODE == "INPUT").ToList();
                        ReportDetail.Quantity00 = (decimal)ListtrackmtimSub.Sum(o => o.QTY);
                        ReportDetail.Date01 = ListtrackmtimSub.Min(o => o.RACKDTM);
                        ReportDetail.Date02 = ListtrackmtimSub.Max(o => o.RACKDTM);
                        ReportDetail.Name = string.Join(",", ListtrackmtimSub.Select(o => o.FinishGoodsCode).Distinct().ToList());
                        ReportDetail.Display = string.Join(",", ListtrackmtimSub.Select(o => o.ECN).Distinct().ToList());
                        ReportDetail.Description = string.Join(",", ListtrackmtimSub.Select(o => o.POCode).Distinct().ToList());
                        var ListBOMSub = ListBOM.Where(o => o.MaterialCode == LEAD_NM).ToList();
                        ReportDetail.Note = string.Join(",", ListBOMSub.Select(o => o.ParentName).Distinct().ToList());
                        ReportDetail.FileName = string.Join(",", ListBOMSub.Select(o => o.ParentID).Distinct().ToList());
                        ListReportDetail.Add(ReportDetail);
                    }
                    await _ReportDetailRepository.AddRangeAsync(ListReportDetail);
                }
            }
            return result;
        }


    }
}

