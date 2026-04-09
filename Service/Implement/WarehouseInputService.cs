namespace Service.Implement
{
    public class WarehouseInputService : BaseService<WarehouseInput, IWarehouseInputRepository>
    , IWarehouseInputService
    {
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseInputDetailService _WarehouseInputDetailService;
        private readonly IWarehouseInputDetailBarcodeService _WarehouseInputDetailBarcodeService;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeMaterialService _WarehouseInputDetailBarcodeMaterialService;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;

        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;

        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;

        private readonly IMaterialRepository _MaterialRepository;
        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IInventoryRepository _InventoryRepository;
        private readonly IInventoryDetailBarcodeRepository _InventoryDetailBarcodeRepository;

        private readonly IProductionOrderRepository _ProductionOrderRepository;
        public WarehouseInputService(IWarehouseInputRepository WarehouseInputRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IWarehouseInputDetailService WarehouseInputDetailService
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IWarehouseInputDetailBarcodeService WarehouseInputDetailBarcodeService
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseInputDetailBarcodeMaterialService WarehouseInputDetailBarcodeMaterialService
            , IInvoiceInputRepository invoiceInputRepository
            , IInvoiceInputDetailRepository invoiceInputDetailRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IWarehouseRequestRepository WarehouseRequestRepository

            , IMaterialRepository MaterialRepository
            , IMaterialConvertRepository MaterialConvertRepository
            , IMembershipDepartmentRepository MembershipDepartmentRepository
            , IMembershipRepository MembershipRepository
            , IInventoryRepository InventoryRepository
            , IInventoryDetailBarcodeRepository InventoryDetailBarcodeRepository

            , IProductionOrderRepository ProductionOrderRepository
            ) : base(WarehouseInputRepository)
        {
            _WarehouseInputRepository = WarehouseInputRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WarehouseInputDetailService = WarehouseInputDetailService;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseInputDetailBarcodeService = WarehouseInputDetailBarcodeService;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseInputDetailBarcodeMaterialService = WarehouseInputDetailBarcodeMaterialService;
            _InvoiceInputRepository = invoiceInputRepository;
            _InvoiceInputDetailRepository = invoiceInputDetailRepository;

            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _WarehouseRequestRepository = WarehouseRequestRepository;

            _ProductionOrderRepository = ProductionOrderRepository;

            _MaterialRepository = MaterialRepository;
            _MaterialConvertRepository = MaterialConvertRepository;
            _MembershipDepartmentRepository = MembershipDepartmentRepository;
            _MembershipRepository = MembershipRepository;

            _InventoryRepository = InventoryRepository;
            _InventoryDetailBarcodeRepository = InventoryDetailBarcodeRepository;

        }
        public override void InitializationSave(WarehouseInput model)
        {
            BaseInitialization(model);

            if (model.InvoiceInputID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.InvoiceInputName))
                {
                    var InvoiceInput = _InvoiceInputRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.InvoiceInputName).FirstOrDefault();
                    if (InvoiceInput != null && InvoiceInput.ID > 0)
                    {
                        model.InvoiceInputID = InvoiceInput.ID;
                    }
                }
            }
            if (model.InvoiceInputID > 0)
            {
                var InvoiceInput = _InvoiceInputRepository.GetByID(model.InvoiceInputID.Value);
                model.InvoiceInputName = InvoiceInput.Code;
                model.CompanyID = model.CompanyID ?? InvoiceInput.CustomerID;
                model.Date = model.Date ?? InvoiceInput.DateETA;
                model.Code = model.Code ?? model.InvoiceInputName;
                model.TotalFinal = model.TotalFinal ?? InvoiceInput.TotalFinal;
            }
            if (model.WarehouseOutputID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.WarehouseOutputName))
                {
                    var WarehouseOutput = _WarehouseOutputRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.WarehouseOutputName).FirstOrDefault();
                    if (WarehouseOutput != null && WarehouseOutput.ID > 0)
                    {
                        model.WarehouseOutputID = WarehouseOutput.ID;
                    }
                }
            }
            if (model.WarehouseOutputID > 0)
            {
                var WarehouseOutput = _WarehouseOutputRepository.GetByID(model.WarehouseOutputID.Value);
                model.WarehouseOutputName = WarehouseOutput.Code;
                model.CompanyID = model.CompanyID ?? WarehouseOutput.CompanyID;
                model.SupplierID = model.SupplierID ?? WarehouseOutput.SupplierID;
                model.CustomerID = model.CustomerID ?? WarehouseOutput.CustomerID;
                model.Date = model.Date ?? WarehouseOutput.Date;
                model.Code = model.Code ?? WarehouseOutput.Code;
                model.Name = model.Name ?? WarehouseOutput.Name;

                model.CreateUserID = model.CreateUserID ?? WarehouseOutput.CreateUserID;
                model.CreateUserCode = model.CreateUserCode ?? WarehouseOutput.CreateUserCode;
                model.CreateUserName = model.CreateUserName ?? WarehouseOutput.CreateUserName;

                if (WarehouseOutput.WarehouseRequestID > 0)
                {
                    var WarehouseRequest = _WarehouseRequestRepository.GetByID(WarehouseOutput.WarehouseRequestID.Value);
                    model.CreateUserID = WarehouseRequest.CreateUserID;
                    model.CreateUserCode = WarehouseRequest.CreateUserCode;
                    model.CreateUserName = WarehouseRequest.CreateUserName;
                }
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;

            if (model.ParentID > 0)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(model.ParentName))
                {
                    var ProductionOrder = _ProductionOrderRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.ParentName).FirstOrDefault();
                    if (ProductionOrder != null && ProductionOrder.ID > 0)
                    {
                        model.ParentID = ProductionOrder.ID;
                    }
                }
            }
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Code = model.Code ?? model.ParentName + "_" + model.Date.Value.ToString("yyyyMMddhhmmss");
                model.Active = model.Active ?? Parent.Active;
            }

            if (model.SupplierID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = CategoryDepartment.Name;
            }
            else
            {
                if (model.CompanyID > 0)
                {
                    var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(GlobalHelper.Office)).FirstOrDefault();
                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                    {
                        model.SupplierID = CategoryDepartment.ID;
                        model.SupplierName = CategoryDepartment.Name;
                    }
                }
            }
            if (model.CustomerID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = CategoryDepartment.Name;
            }
            else
            {
                if (model.CompanyID > 0)
                {
                    var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.IsSync == true).FirstOrDefault();
                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                    {
                        model.CustomerID = CategoryDepartment.ID;
                        model.CustomerName = CategoryDepartment.Name;
                    }
                }
            }

            model.Code = model.Code ?? model.InvoiceInputName;
            model.Code = model.Code ?? model.Date.Value.ToString("yyyyMMddhhmmss") + model.Date.Value.Ticks.ToString();
            model.Name = model.Name ?? model.Code;
            model.Total = model.Total ?? model.TotalFinal;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * (model.Tax / 100);
            model.TotalDiscount = model.Total * (model.Discount / 100);
        }
        public virtual void InitializationSaveHookRack(WarehouseInput model)
        {
            BaseInitialization(model);

            if (model.SupplierID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = CategoryDepartment.Name;
            }
            if (model.CustomerID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = CategoryDepartment.Name;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;
            model.Code = model.Code ?? model.InvoiceInputName;
            model.Code = model.Code ?? model.Date.Value.ToString("yyyyMMddhhmmss") + model.Date.Value.Ticks.ToString();
            model.Name = model.Name ?? model.Code;
        }
        public override WarehouseInput SetModelByModelCheck(WarehouseInput Model, WarehouseInput ModelCheck)
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
                    Model.Date = Model.Date ?? ModelCheck.Date;
                    Model.SupplierID = Model.SupplierID ?? ModelCheck.SupplierID;
                    Model.CustomerID = Model.CustomerID ?? ModelCheck.CustomerID;
                    Model.ParentID = Model.ParentID ?? ModelCheck.ParentID;
                    Model.WarehouseOutputID = Model.WarehouseOutputID ?? ModelCheck.WarehouseOutputID;
                    Model.InvoiceInputID = Model.InvoiceInputID ?? ModelCheck.InvoiceInputID;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.IsSync = Model.IsSync ?? ModelCheck.IsSync;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public override async Task<BaseResult<WarehouseInput>> SaveAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            InitializationSave(BaseParameter.BaseModel);
            //if (BaseParameter.BaseModel.ID > 0)
            //{

            //}
            //else
            //{
            //    var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
            //    SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            //}
            var ModelCheck = new WarehouseInput();
            if (BaseParameter.BaseModel.InvoiceInputID > 0)
            {
                ModelCheck = await GetByCondition(o => o.InvoiceInputID == BaseParameter.BaseModel.InvoiceInputID && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
            }
            else
            {
                ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
            }
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.SupplierID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CustomerID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CompanyID == null)
            {
                IsSave = false;
            }
            if (IsSave == true)
            {
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
                        result = await SyncAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SaveHookRackAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            InitializationSaveHookRack(BaseParameter.BaseModel);
            var ModelCheck = new WarehouseInput();
            ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.SupplierID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CustomerID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CompanyID == null)
            {
                IsSave = false;
            }
            if (IsSave == true)
            {
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
        public override async Task<BaseResult<WarehouseInput>> RemoveAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            result.Count = await _WarehouseInputRepository.RemoveAsync(BaseParameter.ID);
            result = await SyncRemoveAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            //await SyncWarehouseInputAsync(BaseParameter);
            result = await SyncInvoiceInputAsync(BaseParameter);
            await SyncWarehouseInputDetailAsync(BaseParameter);
            //await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncWarehouseInputAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Root == true)
                    {
                        var List = await _WarehouseInputRepository.GetByCondition(o => o.Root == true && o.CustomerID == BaseParameter.BaseModel.CustomerID && o.ID != BaseParameter.BaseModel.ID).ToListAsync();
                        if (List != null && List.Count > 0)
                        {
                            foreach (var item in List)
                            {
                                item.Active = false;
                                await _WarehouseInputRepository.UpdateAsync(item);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncInvoiceInputAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.InvoiceInputID > 0)
                    {
                        if (BaseParameter.BaseModel.IsSync == true)
                        {
                        }
                        else
                        {
                            BaseParameter.BaseModel.IsSync = true;
                            var InvoiceInput = await _InvoiceInputRepository.GetByIDAsync(BaseParameter.BaseModel.InvoiceInputID.Value);
                            if (InvoiceInput.ID > 0)
                            {
                                BaseParameter.BaseModel.Code = InvoiceInput.Code;
                                if (InvoiceInput.Active == true)
                                {
                                    var List = await _InvoiceInputDetailRepository.GetByParentIDAndActiveToListAsync(InvoiceInput.ID, true);
                                    if (List != null && List.Count > 0)
                                    {
                                        foreach (var item in List)
                                        {
                                            WarehouseInputDetail WarehouseInputDetail = new WarehouseInputDetail();
                                            //WarehouseInputDetail.Active = true;
                                            WarehouseInputDetail.ParentID = BaseParameter.BaseModel.ID;
                                            WarehouseInputDetail.Barcode = item.Barcode;
                                            WarehouseInputDetail.QuantityInvoice = item.Quantity;
                                            WarehouseInputDetail.Quantity = WarehouseInputDetail.QuantityInvoice;
                                            WarehouseInputDetail.Price = item.Price;
                                            WarehouseInputDetail.Total = item.Total;
                                            WarehouseInputDetail.TotalInvoice = WarehouseInputDetail.Total;
                                            WarehouseInputDetail.CategoryUnitID = item.CategoryUnitID;
                                            WarehouseInputDetail.MaterialID = item.MaterialID;
                                            WarehouseInputDetail.MaterialName = item.MaterialName;
                                            WarehouseInputDetail.QuantitySNP = item.QuantitySNP;
                                            WarehouseInputDetail.Display = item.Display;
                                            WarehouseInputDetail.Description = item.Description;
                                            var BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                                            BaseParameterWarehouseInputDetail.BaseModel = WarehouseInputDetail;
                                            await _WarehouseInputDetailService.SaveAsync(BaseParameterWarehouseInputDetail);
                                            BaseParameter.BaseModel.Total = BaseParameter.BaseModel.Total + WarehouseInputDetail.Total;
                                        }
                                    }
                                }
                            }
                            await _WarehouseInputRepository.UpdateAsync(BaseParameter.BaseModel);
                        }
                    }
                }
            }
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncWarehouseInputDetailAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.WarehouseOutputID > 0)
                        {
                            if (BaseParameter.BaseModel.IsSync == true)
                            {
                            }
                            else
                            {
                                BaseParameter.BaseModel.IsSync = true;
                                await _WarehouseInputRepository.UpdateAsync(BaseParameter.BaseModel);
                                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.BaseModel.WarehouseOutputID.Value);
                                if (WarehouseOutput.ID > 0 && WarehouseOutput.Active == true && WarehouseOutput.IsComplete == true)
                                {
                                    var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Active == true).ToListAsync();
                                    foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
                                    {
                                        var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                        WarehouseInputDetailBarcode.ParentID = BaseParameter.BaseModel.ID;
                                        WarehouseInputDetailBarcode.MaterialID = WarehouseOutputDetailBarcode.MaterialID;
                                        WarehouseInputDetailBarcode.Barcode = WarehouseOutputDetailBarcode.Barcode;
                                        WarehouseInputDetailBarcode.Quantity = WarehouseOutputDetailBarcode.Quantity;
                                        WarehouseInputDetailBarcode.Active = WarehouseOutputDetailBarcode.Active;
                                        var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                        BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                        await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                    }

                                    //var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByParentIDToListAsync(WarehouseOutput.ID);
                                    //if (ListWarehouseOutputDetail != null && ListWarehouseOutputDetail.Count > 0)
                                    //{
                                    //    foreach (var WarehouseOutputDetail in ListWarehouseOutputDetail)
                                    //    {
                                    //        WarehouseInputDetail WarehouseInputDetail = new WarehouseInputDetail();
                                    //        WarehouseInputDetail.ParentID = BaseParameter.BaseModel.ID;
                                    //        WarehouseInputDetail.Active = WarehouseOutputDetail.Active;
                                    //        WarehouseInputDetail.Barcode = WarehouseOutputDetail.Barcode;
                                    //        WarehouseInputDetail.QuantityInvoice = WarehouseOutputDetail.QuantityActual;
                                    //        WarehouseInputDetail.Quantity = WarehouseInputDetail.QuantityInvoice;
                                    //        WarehouseInputDetail.Price = WarehouseOutputDetail.Price;
                                    //        WarehouseInputDetail.Total = WarehouseOutputDetail.Total;
                                    //        WarehouseInputDetail.TotalInvoice = WarehouseInputDetail.Total;
                                    //        WarehouseInputDetail.CategoryUnitID = WarehouseOutputDetail.CategoryUnitID;
                                    //        WarehouseInputDetail.MaterialID = WarehouseOutputDetail.MaterialID;
                                    //        var BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                                    //        BaseParameterWarehouseInputDetail.BaseModel = WarehouseInputDetail;
                                    //        var BaseResultWarehouseInputDetail = await _WarehouseInputDetailService.SaveAsync(BaseParameterWarehouseInputDetail);
                                    //        if (BaseResultWarehouseInputDetail != null)
                                    //        {
                                    //            if (BaseResultWarehouseInputDetail.BaseModel != null)
                                    //            {
                                    //                if (BaseResultWarehouseInputDetail.BaseModel.ID > 0)
                                    //                {
                                    //                    var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == WarehouseOutputDetail.ParentID && o.MaterialID == WarehouseOutputDetail.MaterialID).ToListAsync();
                                    //                    foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
                                    //                    {
                                    //                        var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                    //                        WarehouseInputDetailBarcode.ParentID = BaseResultWarehouseInputDetail.BaseModel.ParentID;
                                    //                        WarehouseInputDetailBarcode.WarehouseInputDetailID = BaseResultWarehouseInputDetail.BaseModel.ID;
                                    //                        WarehouseInputDetailBarcode.MaterialID = BaseResultWarehouseInputDetail.BaseModel.MaterialID;
                                    //                        WarehouseInputDetailBarcode.Barcode = WarehouseOutputDetailBarcode.Barcode;
                                    //                        WarehouseInputDetailBarcode.Quantity = WarehouseOutputDetailBarcode.Quantity;
                                    //                        var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                    //                        BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                    //                        await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                await _WarehouseInputRepository.UpdateAsync(BaseParameter.BaseModel);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.IsComplete == true)
                        {
                            if (BaseParameter.BaseModel.Root != true)
                            {
                                var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ID && o.Active != true).ToListAsync();
                                foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                                {
                                    WarehouseInputDetailBarcode.Active = true;
                                    WarehouseInputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                    var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                    BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                    await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncRemoveAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            result.Count = 0;
            var List = await _WarehouseInputDetailRepository.GetByParentIDToListAsync(BaseParameter.ID);
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
                    BaseParameter<WarehouseInputDetail> BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                    BaseParameterWarehouseInputDetail.ID = item.ID;
                    var resultWarehouseInputDetail = await _WarehouseInputDetailService.RemoveAsync(BaseParameterWarehouseInputDetail);
                    result.Count = result.Count + resultWarehouseInputDetail.Count;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsCompleteToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            result.List = await GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            result.List = new List<WarehouseInput>();
            if (BaseParameter.Action > 0)
            {
                switch (BaseParameter.Action)
                {
                    case 1:
                        result.List = await GetByCondition(o => o.SupplierID != GlobalHelper.DepartmentIDOffice && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
                        break;
                    case 2:
                        result.List = await GetByCondition(o => o.SupplierID == GlobalHelper.DepartmentIDOffice && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
                        break;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipIDToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();

            string sql = @"UPDATE WarehouseInput JOIN WarehouseInputDetailBarcode ON WarehouseInput.ID=WarehouseInputDetailBarcode.ParentID SET WarehouseInputDetailBarcode.ParentName=WarehouseInput.Code WHERE WarehouseInputDetailBarcode.ParentID>0 AND WarehouseInputDetailBarcode.ParentName IS NULL";
            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

            sql = @"UPDATE WarehouseOutput JOIN WarehouseOutputDetailBarcode ON WarehouseOutput.ID=WarehouseOutputDetailBarcode.ParentID SET WarehouseOutputDetailBarcode.ParentName=WarehouseOutput.Code WHERE WarehouseOutputDetailBarcode.ParentID>0 AND WarehouseOutputDetailBarcode.ParentName IS NULL";
            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value)).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    if (BaseParameter.IsComplete == true)
                    {
                        result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    var ListDepartmentOffice = await _CategoryDepartmentRepository.GetByCondition(o => ListMembershipDepartmentID.Contains(o.ID) && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(GlobalHelper.Office)).ToListAsync();
                    if (ListDepartmentOffice.Count > 0)
                    {
                        var ListDepartmentOfficeID = ListDepartmentOffice.Select(o => o.ID).ToList();
                        if (BaseParameter.Action > 0)
                        {
                            switch (BaseParameter.Action)
                            {
                                case 1:
                                    if (BaseParameter.IsComplete == true)
                                    {
                                        result.List = await GetByCondition(o => o.SupplierID != null && !ListDepartmentOfficeID.Contains(o.SupplierID.Value) && o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.SupplierID != null && !ListDepartmentOfficeID.Contains(o.SupplierID.Value) && o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    break;
                                case 2:
                                    if (BaseParameter.IsComplete == true)
                                    {
                                        result.List = await GetByCondition(o => o.SupplierID != null && ListDepartmentOfficeID.Contains(o.SupplierID.Value) && o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.SupplierID != null && ListDepartmentOfficeID.Contains(o.SupplierID.Value) && o.CustomerID != null && ListMembershipDepartmentID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            else
            {
                if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
                {
                    //result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                    switch (BaseParameter.Action)
                    {
                        case 0:
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                            break;
                        case 1:
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsComplete != true).ToListAsync();
                            break;
                        case 2:
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsComplete == true).ToListAsync();
                            break;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.CompanyID > 0)
            {
                if (BaseParameter.CategoryDepartmentID > 0)
                {
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        var List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                        if (List.Count > 0)
                        {
                            result.List = List.Where(o => (!string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString))).ToList();
                            if (result.List.Count == 0)
                            {
                                result.List = List.Where(o => (!string.IsNullOrEmpty(o.Name) && o.Name.Contains(BaseParameter.SearchString))).ToList();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = List.Where(o => (!string.IsNullOrEmpty(o.InvoiceInputName) && o.InvoiceInputName.Contains(BaseParameter.SearchString))).ToList();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = List.Where(o => (!string.IsNullOrEmpty(o.WarehouseOutputName) && o.WarehouseOutputName.Contains(BaseParameter.SearchString))).ToList();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = List.Where(o => (!string.IsNullOrEmpty(o.ParentName) && o.ParentName.Contains(BaseParameter.SearchString))).ToList();
                            }
                        }

                    }
                    else
                    {
                        if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                        {
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();

                            switch (BaseParameter.Action)
                            {
                                case 0:
                                    break;
                                case 1:
                                    result.List = result.List.Where(o => o.Active == true && o.IsComplete != true).ToList();
                                    break;
                                case 2:
                                    result.List = result.List.Where(o => o.Active == true && o.IsComplete == true).ToList();
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> GetByBarcodeAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            result.IsCheck = true;
            if (BaseParameter.ID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.ID);
                    if (WarehouseInput.ID > 0)
                    {
                        result.BaseModel = WarehouseInput;
                        if (WarehouseInput.IsComplete != true)
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseInput.ID && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                            if (WarehouseInputDetailBarcode != null)
                            {
                                if (WarehouseInputDetailBarcode.ID > 0)
                                {
                                    result.IsCheck = false;
                                    WarehouseInputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                    //WarehouseInputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                    WarehouseInputDetailBarcode.Active = true;
                                    WarehouseInputDetailBarcode.IsScan = true;
                                    var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                    BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                    await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> CreateAutoAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();

            int pageSize = 10;
            DateTime Now = new DateTime(2026, 3, 22);
            var ListWarehouseInput = await GetByCondition(o => o.Active == true && o.WarehouseOutputID > 0 && o.Date != null && o.Date.Value.Date == Now.Date).OrderByDescending(o => o.Date).Skip(BaseParameter.Year.Value * pageSize).Take(pageSize).ToListAsync();
            //var ListWarehouseInput = await GetByCondition(o => o.Active == true && o.WarehouseOutputID > 0 && o.Date != null && o.Date.Value.Date >= Now.Date).OrderBy(o => o.Date).ToListAsync();
            //var ListWarehouseInput = await GetByCondition(o => o.ID == 8260).ToListAsync();
            foreach (var item in ListWarehouseInput)
            {
                item.Active = true;
                item.IsComplete = true;
                item.IsSync = false;
                BaseParameter.BaseModel = item;
                await SaveAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.UserName))
            {
                long? CompanyID = 16;
                long? CUTID = 86;
                long? HOOKRACKID = 205;
                long? FAID = 26;
                var UserName = BaseParameter.UserName.Trim();
                var Membership = await _MembershipRepository.GetByCondition(o => o.UserName == UserName).FirstOrDefaultAsync();
                if (Membership == null)
                {
                    Membership = new Membership();
                }
                if (Membership != null && Membership.ID > 0)
                {
                    CompanyID = Membership.CompanyID;
                }
                if (CompanyID == 17)
                {
                    CUTID = 195;
                    HOOKRACKID = 206;
                    FAID = 196;
                }
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(CompanyID.Value);
                string sql = @"select * from trackmtim where TRACK_IDX=" + BaseParameter.ID;
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtrackmtim = new List<trackmtim>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var trackmtim = Listtrackmtim[0];
                    if (trackmtim != null && trackmtim.TRACK_IDX > 0 && !string.IsNullOrEmpty(trackmtim.BARCODE_NM))
                    {
                        string Barcode = trackmtim.BARCODE_NM.Trim();
                        string Code = "CUT-HOOKRACK-" + DateTime.Now.ToString("yyyyMMdd");
                        var WarehouseInput = await GetByCondition(o => o.CompanyID == CompanyID && o.Code == Code).FirstOrDefaultAsync();
                        if (WarehouseInput == null)
                        {
                            WarehouseInput = new WarehouseInput();
                            WarehouseInput.CreateUserID = Membership.ID;
                            WarehouseInput.CreateUserCode = UserName;
                            WarehouseInput.Active = true;
                            WarehouseInput.CompanyID = CompanyID;
                            WarehouseInput.Code = Code;
                            WarehouseInput.SupplierID = CUTID;
                            WarehouseInput.CustomerID = HOOKRACKID;
                            WarehouseInput.Date = DateTime.Now;

                            //BaseParameter<WarehouseInput> BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                            //BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                            //await SaveAsync(BaseParameterWarehouseInput);

                            await _WarehouseInputRepository.AddAsync(WarehouseInput);
                        }

                        if (WarehouseInput.ID > 0)
                        {
                            WarehouseInputDetailBarcode WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                            WarehouseInputDetailBarcode.CreateUserID = Membership.ID;
                            WarehouseInputDetailBarcode.CreateUserCode = UserName;
                            WarehouseInputDetailBarcode.ParentID = WarehouseInput.ID;
                            WarehouseInputDetailBarcode.Code = Code;
                            WarehouseInputDetailBarcode.Active = true;
                            WarehouseInputDetailBarcode.Barcode = Barcode;
                            WarehouseInputDetailBarcode.MaterialName = trackmtim.LEAD_NM;
                            WarehouseInputDetailBarcode.DateScan = trackmtim.RACKDTM ?? DateTime.Now;
                            WarehouseInputDetailBarcode.Quantity = (decimal?)trackmtim.QTY ?? 0;
                            sql = @"select * from trackmaster where RACK_IDX=" + trackmtim.RACK_IDX;
                            ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                            var Listtrackmaster = new List<trackmaster>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Listtrackmaster.AddRange(SQLHelper.ToList<trackmaster>(dt));
                            }
                            if (Listtrackmaster.Count > 0)
                            {
                                WarehouseInputDetailBarcode.CategoryLocationName = Listtrackmaster[0].HOOK_RACK;
                            }
                            BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                            BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                            await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.UserName))
            {
                long? CompanyID = 16;
                long? CUTID = 86;
                long? HOOKRACKID = 205;
                long? FAID = 26;
                var UserName = BaseParameter.UserName.Trim();
                var Membership = await _MembershipRepository.GetByCondition(o => o.UserName == UserName).FirstOrDefaultAsync();
                if (Membership == null)
                {
                    Membership = new Membership();
                }
                if (Membership != null && Membership.ID > 0)
                {
                    CompanyID = Membership.CompanyID;
                }
                if (CompanyID == 17)
                {
                    CUTID = 195;
                    HOOKRACKID = 206;
                    FAID = 196;
                }
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(CompanyID.Value);
                string sql = @"select * from trackmtim where TRACK_IDX=" + BaseParameter.ID;
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtrackmtim = new List<trackmtim>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var trackmtim = Listtrackmtim[0];
                    if (trackmtim != null && trackmtim.TRACK_IDX > 0 && !string.IsNullOrEmpty(trackmtim.BARCODE_NM))
                    {
                        string Barcode = trackmtim.BARCODE_NM.Trim();

                        var ListWarehouseInput = await GetByCondition(o => o.CompanyID == CompanyID && o.SupplierID == HOOKRACKID && o.CustomerID == FAID).OrderBy(o => o.Date).ToListAsync();
                        if (ListWarehouseInput.Count > 0)
                        {
                            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                            var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeService.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Barcode == Barcode).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                            if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                            {
                                WarehouseInputDetailBarcode.UpdateUserID = Membership.ID;
                                WarehouseInputDetailBarcode.UpdateUserName = UserName;
                                WarehouseInputDetailBarcode.Quantity = 0;
                                BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                            }
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncFromMES_C03ByCompanyID_Action_IDAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Action > 0 && BaseParameter.ID > 0)
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                List<trackmtim> Listtrackmtim = new List<trackmtim>();
                string sql = @"select * from trackmtim WHERE TRACK_IDX=" + BaseParameter.ID;
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var trackmtim = Listtrackmtim[0];
                    if (trackmtim != null && trackmtim.TRACK_IDX > 0 && !string.IsNullOrEmpty(trackmtim.BARCODE_NM))
                    {
                        trackmtim.BARCODE_NM = trackmtim.BARCODE_NM.Trim();
                        var Code = "HookRack-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                        if (BaseParameter.Action > 100)
                        {
                            var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                            WarehouseOutputDetailBarcode.Description = "HookRack";
                            WarehouseOutputDetailBarcode.ParentID = 0;
                            WarehouseOutputDetailBarcode.Active = true;
                            WarehouseOutputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                            WarehouseOutputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                            WarehouseOutputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                            WarehouseOutputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                            WarehouseOutputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                            WarehouseOutputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                            WarehouseOutputDetailBarcode.Quantity = 0;
                            var SupplierID = 0;
                            var CustomerID = 0;
                            switch (WarehouseOutputDetailBarcode.CompanyID)
                            {
                                case 16:
                                    SupplierID = 205;
                                    switch (BaseParameter.Action)
                                    {
                                        case 101:
                                            CustomerID = 217;
                                            break;
                                        case 102:
                                            CustomerID = 218;
                                            break;
                                        case 103:
                                            CustomerID = 221;
                                            break;
                                        case 104:
                                            CustomerID = 26;
                                            break;
                                    }
                                    break;
                                case 17:
                                    SupplierID = 206;
                                    switch (BaseParameter.Action)
                                    {
                                        case 101:
                                            CustomerID = 224;
                                            break;
                                        case 102:
                                            CustomerID = 225;
                                            break;
                                        case 103:
                                            CustomerID = 226;
                                            break;
                                        case 104:
                                            CustomerID = 196;
                                            break;
                                    }
                                    break;
                            }
                            WarehouseOutputDetailBarcode.SupplierID = SupplierID;
                            WarehouseOutputDetailBarcode.CustomerID = CustomerID;
                            WarehouseOutputDetailBarcode.CategoryDepartmentID = SupplierID;
                            WarehouseOutputDetailBarcode.ParentName = Code + "-" + WarehouseOutputDetailBarcode.SupplierID + "-" + WarehouseOutputDetailBarcode.CustomerID;
                            WarehouseOutputDetailBarcode.Code = WarehouseOutputDetailBarcode.ParentName;
                            if (!string.IsNullOrEmpty(WarehouseOutputDetailBarcode.Barcode))
                            {
                                var WarehouseOutputDetailBarcodeCheck = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.MESID == WarehouseOutputDetailBarcode.MESID).FirstOrDefaultAsync();
                                if (WarehouseOutputDetailBarcodeCheck == null)
                                {
                                    WarehouseOutputDetailBarcode.MaterialName = WarehouseOutputDetailBarcode.Barcode.Split('$')[0];
                                    var Material = await _MaterialRepository.GetByDescriptionAsync(WarehouseOutputDetailBarcode.MaterialName, WarehouseOutputDetailBarcode.CompanyID);
                                    WarehouseOutputDetailBarcode.MaterialID = Material.ID;
                                    WarehouseOutputDetailBarcode.MaterialName = Material.Code;
                                    WarehouseOutputDetailBarcode.QuantitySNP = Material.QuantitySNP;
                                    WarehouseOutputDetailBarcode.Display = Material.Name;
                                    WarehouseOutputDetailBarcode.IsSNP = WarehouseOutputDetailBarcode.IsSNP ?? Material.IsSNP;
                                    WarehouseOutputDetailBarcode.FileName = Material.CategoryLineName;
                                    WarehouseOutputDetailBarcode.CategoryFamilyID = WarehouseOutputDetailBarcode.CategoryFamilyID ?? Material.CategoryFamilyID;
                                    WarehouseOutputDetailBarcode.CategoryFamilyName = WarehouseOutputDetailBarcode.CategoryFamilyName ?? Material.CategoryFamilyName;
                                    WarehouseOutputDetailBarcode.CategoryCompanyName = Material.OriginalEquipmentManufacturer;

                                    if (WarehouseOutputDetailBarcode.Barcode.Split('$').Length > 2)
                                    {
                                        try
                                        {
                                            WarehouseOutputDetailBarcode.Quantity = decimal.Parse(WarehouseOutputDetailBarcode.Barcode.Split('$')[2]);
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                        }
                                    }
                                    await _WarehouseOutputDetailBarcodeRepository.AddAsync(WarehouseOutputDetailBarcode);
                                }
                            }
                        }
                        else
                        {
                            var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                            WarehouseInputDetailBarcode.Description = "HookRack";
                            WarehouseInputDetailBarcode.ParentID = 0;
                            WarehouseInputDetailBarcode.Active = true;
                            WarehouseInputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                            WarehouseInputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                            WarehouseInputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                            WarehouseInputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                            WarehouseInputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                            WarehouseInputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                            WarehouseInputDetailBarcode.Quantity = 0;
                            var SupplierID = 0;
                            var CustomerID = 0;
                            switch (WarehouseInputDetailBarcode.CompanyID)
                            {
                                case 16:
                                    CustomerID = 205;
                                    switch (BaseParameter.Action)
                                    {
                                        case 11:
                                            SupplierID = 217;
                                            break;
                                        case 12:
                                            SupplierID = 218;
                                            break;
                                        case 13:
                                            SupplierID = 221;
                                            break;
                                        case 14:
                                            SupplierID = 26;
                                            break;
                                    }
                                    break;
                                case 17:
                                    CustomerID = 206;
                                    switch (BaseParameter.Action)
                                    {
                                        case 11:
                                            SupplierID = 224;
                                            break;
                                        case 12:
                                            SupplierID = 225;
                                            break;
                                        case 13:
                                            SupplierID = 226;
                                            break;
                                        case 14:
                                            SupplierID = 196;
                                            break;
                                    }
                                    break;
                            }
                            WarehouseInputDetailBarcode.SupplierID = SupplierID;
                            WarehouseInputDetailBarcode.CustomerID = CustomerID;
                            WarehouseInputDetailBarcode.CategoryDepartmentID = CustomerID;
                            WarehouseInputDetailBarcode.ParentName = Code + "-" + WarehouseInputDetailBarcode.SupplierID + "-" + WarehouseInputDetailBarcode.CustomerID;
                            WarehouseInputDetailBarcode.Code = WarehouseInputDetailBarcode.ParentName;
                            if (!string.IsNullOrEmpty(WarehouseInputDetailBarcode.Barcode))
                            {
                                var WarehouseInputDetailBarcodeCheck = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.MESID == WarehouseInputDetailBarcode.MESID).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcodeCheck == null)
                                {
                                    WarehouseInputDetailBarcode.MaterialName = WarehouseInputDetailBarcode.Barcode.Split('$')[0];
                                    var Material = await _MaterialRepository.GetByDescriptionAsync(WarehouseInputDetailBarcode.MaterialName, WarehouseInputDetailBarcode.CompanyID);
                                    WarehouseInputDetailBarcode.MaterialID = Material.ID;
                                    WarehouseInputDetailBarcode.MaterialName = Material.Code;
                                    WarehouseInputDetailBarcode.QuantitySNP = Material.QuantitySNP;
                                    WarehouseInputDetailBarcode.Display = Material.Name;
                                    WarehouseInputDetailBarcode.IsSNP = WarehouseInputDetailBarcode.IsSNP ?? Material.IsSNP;
                                    WarehouseInputDetailBarcode.FileName = Material.CategoryLineName;
                                    WarehouseInputDetailBarcode.CategoryFamilyID = WarehouseInputDetailBarcode.CategoryFamilyID ?? Material.CategoryFamilyID;
                                    WarehouseInputDetailBarcode.CategoryFamilyName = WarehouseInputDetailBarcode.CategoryFamilyName ?? Material.CategoryFamilyName;
                                    WarehouseInputDetailBarcode.CategoryCompanyName = Material.OriginalEquipmentManufacturer;

                                    if (WarehouseInputDetailBarcode.Barcode.Split('$').Length > 2)
                                    {
                                        try
                                        {
                                            WarehouseInputDetailBarcode.Quantity = decimal.Parse(WarehouseInputDetailBarcode.Barcode.Split('$')[2]);
                                            WarehouseInputDetailBarcode.QuantitySNP = WarehouseInputDetailBarcode.Quantity;
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                        }
                                    }
                                    await _WarehouseInputDetailBarcodeRepository.AddAsync(WarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter<WarehouseInput> BaseParameter)
        {
            var result = new BaseResult<WarehouseInput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 0 && o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.MESID > 0).ToListAsync();
                if (ListWarehouseInputDetailBarcode.Count > 0)
                {
                    var ListMembership = await _MembershipRepository.GetByCompanyIDToListAsync(BaseParameter.CompanyID.Value);
                    var ListWarehouseInputDetailBarcodeCode = ListWarehouseInputDetailBarcode.Select(o => o.Code).Distinct().ToList();
                    foreach (var Code in ListWarehouseInputDetailBarcodeCode)
                    {
                        var ListWarehouseInputDetailBarcodeUpdate = ListWarehouseInputDetailBarcode.Where(o => o.Code == Code).OrderBy(o => o.DateScan).ToList();
                        if (ListWarehouseInputDetailBarcodeUpdate.Count > 0)
                        {
                            var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcodeUpdate[0];
                            if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                            {
                                var WarehouseInput = new WarehouseInput();
                                WarehouseInput.Active = true;
                                WarehouseInput.Code = Code;
                                WarehouseInput.CompanyID = WarehouseInputDetailBarcode.CompanyID;
                                WarehouseInput.SupplierID = WarehouseInputDetailBarcode.SupplierID;
                                WarehouseInput.CustomerID = WarehouseInputDetailBarcode.CustomerID;
                                WarehouseInput.Date = WarehouseInputDetailBarcode.DateScan;
                                WarehouseInput.CreateUserCode = WarehouseInputDetailBarcode.UpdateUserCode;
                                var Membership = ListMembership.Where(o => o.UserName == WarehouseInput.CreateUserCode).FirstOrDefault();
                                if (Membership != null && Membership.ID > 0)
                                {
                                    WarehouseInput.CreateUserID = Membership.ID;
                                }
                                BaseParameter<WarehouseInput> BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                                BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                                await SaveAsync(BaseParameterWarehouseInput);
                                if (WarehouseInput.ID > 0)
                                {
                                    for (int i = 0; i < ListWarehouseInputDetailBarcodeUpdate.Count; i++)
                                    {
                                        ListWarehouseInputDetailBarcodeUpdate[i].ParentID = WarehouseInput.ID;
                                        if (Membership != null && Membership.ID > 0)
                                        {
                                            ListWarehouseInputDetailBarcodeUpdate[i].CreateUserID = Membership.ID;
                                            ListWarehouseInputDetailBarcodeUpdate[i].UpdateUserID = Membership.ID;
                                            ListWarehouseInputDetailBarcodeUpdate[i].CreateUserCode = Membership.UserName;
                                            ListWarehouseInputDetailBarcodeUpdate[i].UpdateUserCode = Membership.UserName;
                                            ListWarehouseInputDetailBarcodeUpdate[i].CreateUserName = Membership.Name;
                                            ListWarehouseInputDetailBarcodeUpdate[i].UpdateUserName = Membership.Name;
                                        }
                                    }
                                    await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeUpdate);
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

