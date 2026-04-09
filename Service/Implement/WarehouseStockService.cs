namespace Service.Implement
{
    public class WarehouseStockService : BaseService<WarehouseStock, IWarehouseStockRepository>
    , IWarehouseStockService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseStockDetailService _WarehouseStockDetailService;
        private readonly IWarehouseStockRepository _WarehouseStockRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IBOMRepository _BOMRepository;

        public WarehouseStockService(IWarehouseStockRepository WarehouseStockRepository
            , IWebHostEnvironment WebHostEnvironment
            , IWarehouseStockDetailService WarehouseStockDetailService
            , IWarehouseInputRepository warehouseInputRepository
            , IWarehouseOutputRepository warehouseOutputRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IMaterialRepository materialRepository
            , IInvoiceInputRepository InvoiceInputRepository
            , IInvoiceInputDetailRepository InvoiceInputDetailRepository
            , IWarehouseRequestRepository WarehouseRequestRepository
            , IBOMRepository BOMRepository


            ) : base(WarehouseStockRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseStockDetailService = WarehouseStockDetailService;
            _WarehouseStockRepository = WarehouseStockRepository;
            _WarehouseInputRepository = warehouseInputRepository;
            _WarehouseOutputRepository = warehouseOutputRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _MaterialRepository = materialRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
            _InvoiceInputDetailRepository = InvoiceInputDetailRepository;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _BOMRepository = BOMRepository;

        }
        public override void InitializationSave(WarehouseStock model)
        {
            BaseInitialization(model);
            model.Action = model.Action ?? 1;
            model.Active = model.Active ?? true;
            model.CategoryDepartmentID = model.CategoryDepartmentID ?? 0;
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.Name;
                model.CompanyID = CategoryDepartment.CompanyID;
            }
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            if (!string.IsNullOrEmpty(model.Code))
            {
                var Material = _MaterialRepository.GetByDescription(model.Code, model.CompanyID);
                model.ParentID = Material.ID;
            }
            if (model.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.ParentName = Parent.Name;
                model.Name = Parent.Name;
                model.FileName = Parent.CategoryLineName;
                model.Description = model.Description ?? Parent.CategoryLocationName;
            }


            model.QuantityInput00 = model.QuantityInput00 ?? 0;
            model.QuantityOutput00 = model.QuantityOutput00 ?? 0;
            model.Quantity00 = model.Quantity00 ?? model.QuantityInput00 - model.QuantityOutput00;
            model.QuantityBegin = model.QuantityBegin ?? 0;
            model.QuantityEnd = model.QuantityBegin + model.Quantity00;

        }
        public virtual WarehouseStock SetModelByModelCheck(WarehouseStock Model, WarehouseStock ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                    switch (Model.ProductionOrderID)
                    {
                        case 0:
                            Model.QuantityInput00 = Model.QuantityInput00 + ModelCheck.QuantityInput00;
                            Model.QuantityOutput00 = ModelCheck.QuantityOutput00;
                            break;
                        case 1:
                            Model.QuantityInput00 = ModelCheck.QuantityInput00;
                            Model.QuantityOutput00 = Model.QuantityOutput00 + ModelCheck.QuantityOutput00;
                            break;
                    }
                    //switch (Model.Action)
                    //{
                    //    case 3:
                    //        Model.Quantity01 = ModelCheck.Quantity01;
                    //        break;
                    //}
                }
            }
            return Model;
        }
        public override async Task<BaseResult<WarehouseStock>> SaveAsync(BaseParameter<WarehouseStock> BaseParameter)
        {
            var result = new BaseResult<WarehouseStock>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Action == BaseParameter.BaseModel.Action && o.Year == BaseParameter.BaseModel.Year && o.Month == BaseParameter.BaseModel.Month && o.Day == BaseParameter.BaseModel.Day).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<WarehouseStock>> GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync(BaseParameter<WarehouseStock> BaseParameter)
        {
            var result = new BaseResult<WarehouseStock>();
            result.List = new List<WarehouseStock>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Action > 0)
            {
                if (BaseParameter.Action == 1 && BaseParameter.CategoryDepartmentID > 0)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Action == BaseParameter.Action && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Day == BaseParameter.Day).OrderByDescending(o => o.QuantityOutput00).ToListAsync();
                }
                if (BaseParameter.Action == 2)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Action == BaseParameter.Action && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Day == BaseParameter.Day).OrderBy(o => o.Display).ThenBy(o => o.Code).ToListAsync();
                }
                if (BaseParameter.Action == 3)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Action == BaseParameter.Action).OrderByDescending(o => o.Quantity02).ThenByDescending(o => o.Quantity01).ToListAsync();
                }
                if (BaseParameter.Action == 4)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Action == BaseParameter.Action).OrderBy(o => o.Code).ToListAsync();
                    if (BaseParameter.Active == true)
                    {
                        if (result.List.Count > 0)
                        {
                            var ListWarehouseStockID = result.List.Select(o => o.ID).ToList();
                            var ListWarehouseStockDetail = await _WarehouseStockDetailService.GetByCondition(o => ListWarehouseStockID.Contains(o.ParentID ?? 0) && o.Quantity > 0).ToListAsync();
                            if (ListWarehouseStockDetail.Count > 0)
                            {
                                var ListWarehouseStockDetailID = ListWarehouseStockDetail.Select(o => o.ParentID ?? 0).Distinct().ToList();
                                result.List = result.List.Where(o => ListWarehouseStockDetailID.Contains(o.ID)).ToList();
                            }
                            else
                            {
                                result.List = new List<WarehouseStock>();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseStock>> SyncAsync(BaseParameter<WarehouseStock> BaseParameter)
        {
            var result = new BaseResult<WarehouseStock>();
            result.List = new List<WarehouseStock>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Action > 0)
            {
                await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);
                int IsCheck = 0;
                if (BaseParameter.Year > 0)
                {
                    if (BaseParameter.Month > 0)
                    {
                        if (BaseParameter.Day > 0)
                        {
                            IsCheck = 10;
                        }
                        else
                        {
                            IsCheck = 20;
                        }
                    }
                    else
                    {
                        IsCheck = 30;
                    }
                }
                if (BaseParameter.Action == 1 && BaseParameter.CategoryDepartmentID > 0)
                {
                    string sql = @"DELETE FROM WarehouseStock WHERE CategoryDepartmentID=" + BaseParameter.CategoryDepartmentID + " AND Action=" + BaseParameter.Action;
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                    await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);

                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                        var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                        switch (IsCheck)
                        {
                            case 0:
                                ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.DateScan).ToListAsync();
                                break;
                            case 10:
                                ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day).OrderBy(o => o.DateScan).ToListAsync();
                                break;
                            case 20:
                                ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).OrderBy(o => o.DateScan).ToListAsync();
                                break;
                            case 30:
                                ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).OrderBy(o => o.DateScan).ToListAsync();
                                break;
                        }
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            var ListWarehouseStock = new List<WarehouseStock>();
                            var ListWarehouseInputDetailBarcodeMaterialName = ListWarehouseInputDetailBarcode.Select(o => o.MaterialName).Distinct().ToList();
                            foreach (var MaterialName in ListWarehouseInputDetailBarcodeMaterialName)
                            {
                                var WarehouseStock = new WarehouseStock();
                                WarehouseStock.CompanyID = BaseParameter.CompanyID;
                                WarehouseStock.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                                WarehouseStock.Action = BaseParameter.Action;
                                WarehouseStock.Year = BaseParameter.Year;
                                WarehouseStock.Month = BaseParameter.Month;
                                WarehouseStock.Day = BaseParameter.Day;
                                WarehouseStock.UpdateDate = GlobalHelper.InitializationDateTime;
                                WarehouseStock.ProductionOrderID = 0;
                                WarehouseStock.Code = MaterialName;
                                WarehouseStock.QuantityInput00 = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == WarehouseStock.Code).Sum(o => o.Quantity ?? 0);
                                WarehouseStock.QuantityOutput00 = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == WarehouseStock.Code).Sum(o => o.QuantityOutput ?? 0);
                                WarehouseStock.Quantity00 = WarehouseStock.QuantityInput00 - WarehouseStock.QuantityOutput00;
                                WarehouseStock.FileName = string.Join(",", ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == WarehouseStock.Code).Select(o => o.FileName).Distinct().OrderBy(o => o).ToList());
                                WarehouseStock.Description = string.Join(",", ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == WarehouseStock.Code).Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList());
                                ListWarehouseStock.Add(WarehouseStock);
                            }
                            await _WarehouseStockRepository.AddRangeAsync(ListWarehouseStock);
                        }
                    }
                }
                if (BaseParameter.Action == 2)
                {
                    string sql = @"DELETE FROM WarehouseStock WHERE CompanyID=" + BaseParameter.CompanyID + " AND Action=" + BaseParameter.Action;
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                    sql = @"UPDATE InvoiceInputDetail JOIN InvoiceInput ON InvoiceInputDetail.ParentID=InvoiceInput.ID SET InvoiceInputDetail.Code=InvoiceInput.Code";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

                    var ListInvoiceInput = new List<InvoiceInput>();
                    switch (IsCheck)
                    {
                        case 0:
                            ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CompanyID).OrderBy(o => o.DateETA).ToListAsync();
                            break;
                        case 10:
                            ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CompanyID && o.DateETA != null && o.DateETA.Value.Year == BaseParameter.Year && o.DateETA.Value.Month == BaseParameter.Month && o.DateETA.Value.Day == BaseParameter.Day).OrderBy(o => o.DateETA).ToListAsync();
                            break;
                        case 20:
                            ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CompanyID && o.DateETA != null && o.DateETA.Value.Year == BaseParameter.Year && o.DateETA.Value.Month == BaseParameter.Month).OrderBy(o => o.DateETA).ToListAsync();
                            break;
                        case 30:
                            ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CompanyID && o.DateETA != null && o.DateETA.Value.Year == BaseParameter.Year).OrderBy(o => o.DateETA).ToListAsync();
                            break;
                    }

                    if (ListInvoiceInput.Count > 0)
                    {
                        var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).Distinct().ToList();
                        var ListInvoiceInputDetail = await _InvoiceInputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListInvoiceInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.ParentID).ToListAsync();
                        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.InvoiceInputID > 0 && ListInvoiceInputID.Contains(o.InvoiceInputID ?? 0)).OrderBy(o => o.Date).ToListAsync();
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                        var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.DateScan).ToListAsync();
                        var ListWarehouseStock = new List<WarehouseStock>();
                        foreach (var InvoiceInput in ListInvoiceInput)
                        {
                            var ListInvoiceInputDetailSub = ListInvoiceInputDetail.Where(o => o.ParentID == InvoiceInput.ID).ToList();
                            var ListWarehouseInputSub = ListWarehouseInput.Where(o => o.InvoiceInputID == InvoiceInput.ID).ToList();
                            var ListWarehouseInputSubID = ListWarehouseInputSub.Select(o => o.ID).ToList();
                            var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcode.Where(o => ListWarehouseInputSubID.Contains(o.ParentID ?? 0)).ToList();
                            var ListWarehouseInputDetailBarcodeSubMeterialName = ListWarehouseInputDetailBarcodeSub.Select(o => o.MaterialName).Distinct().ToList();
                            foreach (var MaterialName in ListWarehouseInputDetailBarcodeSubMeterialName)
                            {
                                var WarehouseStock = new WarehouseStock();
                                WarehouseStock.Date = InvoiceInput.DateETA;
                                WarehouseStock.Name = InvoiceInput.ID.ToString();
                                WarehouseStock.Display = InvoiceInput.Code;
                                WarehouseStock.CompanyID = BaseParameter.CompanyID;
                                WarehouseStock.Action = BaseParameter.Action;
                                WarehouseStock.Year = BaseParameter.Year;
                                WarehouseStock.Month = BaseParameter.Month;
                                WarehouseStock.Day = BaseParameter.Day;
                                WarehouseStock.UpdateDate = GlobalHelper.InitializationDateTime;
                                WarehouseStock.ProductionOrderID = 0;
                                WarehouseStock.Code = MaterialName;
                                WarehouseStock.QuantityStock = ListInvoiceInputDetailSub.Where(o => o.MaterialName == WarehouseStock.Code).Sum(o => o.Quantity ?? 0);
                                if (WarehouseStock.QuantityStock == 0)
                                {
                                    var Material = ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).FirstOrDefault();
                                    if (Material != null && Material.ID > 0)
                                    {
                                        WarehouseStock.QuantityStock = ListInvoiceInputDetailSub.Where(o => o.MaterialID == Material.ID).Sum(o => o.Quantity ?? 0);
                                    }
                                }
                                if (WarehouseStock.QuantityStock == 0)
                                {
                                    WarehouseStock.QuantityStock = ListInvoiceInputDetailSub.Where(o => o.MaterialName.Contains(WarehouseStock.Code)).Sum(o => o.Quantity ?? 0);
                                }
                                if (WarehouseStock.QuantityStock == 0)
                                {
                                    WarehouseStock.QuantityStock = ListInvoiceInputDetailSub.Where(o => WarehouseStock.Code.Contains(o.MaterialName)).Sum(o => o.Quantity ?? 0);
                                }                               
                                WarehouseStock.QuantityInput00 = ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).Sum(o => o.Quantity ?? 0);
                                WarehouseStock.QuantityOutput00 = ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).Sum(o => o.QuantityOutput ?? 0);
                                WarehouseStock.Quantity00 = WarehouseStock.QuantityInput00 - WarehouseStock.QuantityOutput00;
                                WarehouseStock.FileName = string.Join(",", ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).Select(o => o.FileName).Distinct().OrderBy(o => o).ToList());
                                WarehouseStock.Description = string.Join(",", ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList());
                                ListWarehouseStock.Add(WarehouseStock);
                            }
                        }
                        await _WarehouseStockRepository.AddRangeAsync(ListWarehouseStock);
                    }
                }
                if (BaseParameter.Action == 3 && BaseParameter.CategoryDepartmentID > 0)
                {
                    string sql = @"DELETE FROM WarehouseStock WHERE CategoryDepartmentID=" + BaseParameter.CategoryDepartmentID + " AND Action=" + BaseParameter.Action;
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

                    DateTime DateNow = GlobalHelper.InitializationDateTime;
                    DateTime Date06Month = DateNow.AddMonths(-6);
                    var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.DateScan != null).OrderBy(o => o.DateScan).ToListAsync();
                    }
                    var ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
                    var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID).ToListAsync();
                    if (ListWarehouseOutput.Count > 0)
                    {
                        var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                        ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.Active == true && o.DateScan != null).OrderBy(o => o.DateScan).ToListAsync();
                    }

                    var ListWarehouseInputDetailBarcodeMaterialName = ListWarehouseInputDetailBarcode.Select(o => o.MaterialName).Distinct().ToList();
                    foreach (var MaterialName in ListWarehouseInputDetailBarcodeMaterialName)
                    {
                        var WarehouseStock = new WarehouseStock();
                        WarehouseStock.Date = DateNow;
                        WarehouseStock.ProductionOrderID = 0;
                        WarehouseStock.Code = MaterialName;
                        WarehouseStock.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                        WarehouseStock.Action = BaseParameter.Action;
                        WarehouseStock.Year = DateNow.Year;
                        WarehouseStock.Month = DateNow.Month;
                        WarehouseStock.Day = DateNow.Day;
                        var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == MaterialName && o.DateScan != null && o.DateScan >= Date06Month).ToList();
                        if (ListWarehouseInputDetailBarcodeSub.Count > 0)
                        {
                            var ListWarehouseInputDetailBarcodeBarcode = ListWarehouseInputDetailBarcodeSub.Select(o => o.Barcode).Distinct().ToList();
                            var ListWarehouseOutputDetailBarcodeSub = ListWarehouseOutputDetailBarcode.Where(o => !string.IsNullOrEmpty(o.Barcode) && ListWarehouseInputDetailBarcodeBarcode.Contains(o.Barcode)).ToList();
                            var QuantityInput = ListWarehouseInputDetailBarcodeSub.Sum(o => o.Quantity ?? 0);
                            var QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity ?? 0);
                            var QuantityStock = QuantityInput - QuantityOutput;
                            WarehouseStock.Quantity01 = QuantityStock;
                        }
                        WarehouseStock.Description = WarehouseStock.Description + string.Join(";", ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).ToList().Select(o => o.CategoryLocationName).Distinct());
                        ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == MaterialName && o.DateScan != null && o.DateScan < Date06Month).ToList();
                        if (ListWarehouseInputDetailBarcodeSub.Count > 0)
                        {
                            var ListWarehouseInputDetailBarcodeBarcode = ListWarehouseInputDetailBarcodeSub.Select(o => o.Barcode).Distinct().ToList();
                            var ListWarehouseOutputDetailBarcodeSub = ListWarehouseOutputDetailBarcode.Where(o => !string.IsNullOrEmpty(o.Barcode) && ListWarehouseInputDetailBarcodeBarcode.Contains(o.Barcode)).ToList();
                            var QuantityInput = ListWarehouseInputDetailBarcodeSub.Sum(o => o.Quantity ?? 0);
                            var QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity ?? 0);
                            var QuantityStock = QuantityInput - QuantityOutput;
                            WarehouseStock.Quantity02 = QuantityStock;
                        }

                        WarehouseStock.Quantity00 = WarehouseStock.Quantity01 + WarehouseStock.Quantity02;
                        WarehouseStock.Description = WarehouseStock.Description + string.Join(";", ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialName == WarehouseStock.Code).ToList().Select(o => o.CategoryLocationName).Distinct());
                        var BaseParameterWarehouseStock = new BaseParameter<WarehouseStock>();
                        BaseParameterWarehouseStock.BaseModel = WarehouseStock;
                        await SaveAsync(BaseParameterWarehouseStock);
                    }
                }
                if (BaseParameter.Action == 4 && BaseParameter.CategoryDepartmentID > 0)
                {
                    var CategoryDepartment = await _CategoryDepartmentRepository.GetByIDAsync(BaseParameter.CategoryDepartmentID.Value);
                    if (CategoryDepartment.ID > 0 && CategoryDepartment.Code == "HOOKRACK")
                    {
                        var ListBOM = new List<BOM>();
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.ParentID > 0 && !string.IsNullOrEmpty(o.MaterialCode) && o.ParentName == BaseParameter.SearchString).OrderByDescending(o => o.Date).ToListAsync();
                            if (ListBOM.Count == 0)
                            {
                                ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.ParentID > 0 && !string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode == BaseParameter.SearchString).OrderByDescending(o => o.Date).ToListAsync();
                            }
                            if (ListBOM.Count == 0)
                            {
                                ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.ParentID > 0 && !string.IsNullOrEmpty(o.MaterialCode) && o.Code == BaseParameter.SearchString).OrderByDescending(o => o.Date).ToListAsync();
                            }
                        }
                        else
                        {
                            ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.ParentID > 0 && !string.IsNullOrEmpty(o.MaterialCode)).OrderByDescending(o => o.Date).ToListAsync();
                        }
                        if (ListBOM.Count > 0)
                        {
                            var ListBOMMaterialCode = ListBOM.Select(o => o.MaterialCode).Distinct().OrderBy(o => o).ToList();
                            var ListBOMParentName = ListBOM.Select(o => o.ParentName).Distinct().OrderBy(o => o).ToList();

                            List<WarehouseStockDetail> ListWarehouseStockDetail = new List<WarehouseStockDetail>();
                            var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                            var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                            if (ListWarehouseInput.Count > 0)
                            {
                                var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.QuantityInventory > 0 && !string.IsNullOrEmpty(o.MaterialName) && ListBOMMaterialCode.Contains(o.MaterialName)).OrderBy(o => o.MaterialName).ToListAsync();
                            }


                            foreach (var MaterialCode in ListBOMMaterialCode)
                            {
                                var WarehouseStock = new WarehouseStock();
                                WarehouseStock.ProductionOrderID = 0;
                                WarehouseStock.Code = MaterialCode;
                                WarehouseStock.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                                WarehouseStock.Action = BaseParameter.Action;
                                WarehouseStock.QuantityStock = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == MaterialCode).Sum(o => o.QuantityInventory) ?? 0;
                                StringBuilder HTMLContent = new StringBuilder();
                                HTMLContent.AppendLine(@"<div class='row'>");
                                var ListBOMParentNameSub = ListBOM.Where(o => o.MaterialCode == MaterialCode).Select(o => o.ParentName).Distinct().OrderBy(o => o).ToList();
                                foreach (var ParentName in ListBOMParentNameSub)
                                {
                                    var BOM = ListBOM.Where(o => o.Active == true && o.MaterialCode == MaterialCode && o.ParentName == ParentName).OrderByDescending(o => o.Date).FirstOrDefault();
                                    if (BOM == null)
                                    {
                                        BOM = new BOM();
                                    }
                                    WarehouseStock.Name = WarehouseStock.Name + ";" + BOM.Code;
                                    BOM.Quantity = BOM.Quantity ?? 0;

                                    string url = GlobalHelper.ERPSite + "/#/BOMInfo/" + BOM.ID;
                                    HTMLContent.AppendLine(@"<div style='width: 300px !important; border-right: 1px solid #000000; padding: 2px; display: inline-block;'>");
                                    HTMLContent.AppendLine(@"<div><b>" + ParentName + "</b> ECN: <a target='_blank' href='" + url + "' title='" + url + "' style='color: blue;'>" + BOM.Code + "</a></div>");
                                    HTMLContent.AppendLine(@"<div style='width: 100px !important; display: inline-block;'>Stock: <b>" + WarehouseStock.QuantityStock.Value.ToString("N0") + "</b></div>");
                                    HTMLContent.AppendLine(@"<div style='width: 80px !important; display: inline-block;'>BOM: <b style='color: blue;'>" + BOM.Quantity.Value.ToString("N0") + "</b></div>");
                                    string Ready = "[" + ParentName + "Ready]";
                                    HTMLContent.AppendLine(@"<div style='width: 80px !important; display: inline-block;'>Ready: <b style='color: green;'>" + Ready + "</b> |</div>");
                                    HTMLContent.AppendLine(@"</div>");

                                    WarehouseStockDetail WarehouseStockDetail = new WarehouseStockDetail();
                                    WarehouseStockDetail.Code = WarehouseStock.Code;
                                    WarehouseStockDetail.QuantityStock = WarehouseStock.QuantityStock;
                                    WarehouseStockDetail.Name = ParentName;
                                    WarehouseStockDetail.BOMID = BOM.ID;
                                    WarehouseStockDetail.BOMCode = BOM.Code;
                                    WarehouseStockDetail.BOMQuantity = BOM.Quantity;
                                    ListWarehouseStockDetail.Add(WarehouseStockDetail);
                                }
                                HTMLContent.AppendLine(@"</div>");
                                WarehouseStock.HTMLContent = HTMLContent.ToString();
                                result.List.Add(WarehouseStock);
                            }
                            var ListWarehouseStock = result.List;
                            if (ListWarehouseStock != null && ListWarehouseStock.Count > 0)
                            {
                                for (int i = 0; i < ListWarehouseStock.Count; i++)
                                {
                                    if (ListWarehouseStock[i] != null && !string.IsNullOrEmpty(ListWarehouseStock[i].HTMLContent))
                                    {
                                        var ListBOMParentNameSub = ListBOM.Where(o => o.MaterialCode == ListWarehouseStock[i].Code).Select(o => o.ParentName).Distinct().OrderBy(o => o).ToList();
                                        foreach (var ParentName in ListBOMParentNameSub)
                                        {
                                            decimal ReadyCount = 0;
                                            var ListBOMSub = ListBOM.Where(o => o.ParentID > 0 && o.Active == true && o.ParentName == ParentName && o.IsLeadNo == true).OrderBy(o => o.MaterialCode).ToList();
                                            if (ListBOMSub.Count > 0)
                                            {
                                                var ListBOMSubMaterialCode = ListBOMSub.Select(o => o.MaterialCode).Distinct().OrderBy(o => o).ToList();
                                                var ListWarehouseStockSub = ListWarehouseStock.Where(o => !string.IsNullOrEmpty(o.Code) && ListBOMSubMaterialCode.Contains(o.Code)).ToList();
                                                if (ListWarehouseStockSub.Count == ListBOMSubMaterialCode.Count)
                                                {
                                                    ReadyCount = ListWarehouseStockSub.Min(o => o.QuantityStock) ?? 0;
                                                }
                                            }
                                            string Ready = ParentName + "Ready";
                                            ListWarehouseStock[i].HTMLContent = ListWarehouseStock[i].HTMLContent.Replace("[" + Ready + "]", ReadyCount.ToString("N0"));

                                            for (int j = 0; j < ListWarehouseStockDetail.Count; j++)
                                            {
                                                if (ListWarehouseStockDetail[j].Code == ListWarehouseStock[i].Code && ListWarehouseStockDetail[j].Name == ParentName)
                                                {
                                                    ListWarehouseStockDetail[j].Quantity = ReadyCount;
                                                }
                                            }
                                        }
                                        var BaseParameterWarehouseStock = new BaseParameter<WarehouseStock>();
                                        BaseParameterWarehouseStock.BaseModel = ListWarehouseStock[i];
                                        await SaveAsync(BaseParameterWarehouseStock);
                                        if (BaseParameterWarehouseStock.BaseModel.ID > 0)
                                        {
                                            var ListWarehouseStockDetailSub = ListWarehouseStockDetail.Where(o => o.Code == BaseParameterWarehouseStock.BaseModel.Code).ToList();
                                            foreach (var WarehouseStockDetail in ListWarehouseStockDetailSub)
                                            {
                                                WarehouseStockDetail.ParentID = BaseParameterWarehouseStock.BaseModel.ID;
                                                var BaseParameterWarehouseStockDetail = new BaseParameter<WarehouseStockDetail>();
                                                BaseParameterWarehouseStockDetail.BaseModel = WarehouseStockDetail;
                                                await _WarehouseStockDetailService.SaveAsync(BaseParameterWarehouseStockDetail);
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
    }
}

