namespace Service.Implement
{
    public class WarehouseOutputService : BaseService<WarehouseOutput, IWarehouseOutputRepository>
    , IWarehouseOutputService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseOutputDetailService _WarehouseOutputDetailService;
        private readonly IWarehouseOutputDetailBarcodeService _WarehouseOutputDetailBarcodeService;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;

        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IWarehouseRequestDetailRepository _WarehouseRequestDetailRepository;
        private readonly IWarehouseInputService _WarehouseInputService;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputDetailBarcodeService _WarehouseInputDetailBarcodeService;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;

        private readonly IMaterialConvertRepository _MaterialConvertRepository;

        private readonly INotificationService _NotificationService;
        private readonly IZaloTokenService _ZaloTokenService;

        public WarehouseOutputService(IWarehouseOutputRepository WarehouseOutputRepository
             , IWebHostEnvironment WebHostEnvironment
             , ICategoryDepartmentRepository categoryDepartmentRepository
             , IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
             , IWarehouseOutputDetailService WarehouseOutputDetailService
             , IWarehouseOutputDetailBarcodeService WarehouseOutputDetailBarcodeService
             , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
             , IWarehouseRequestRepository warehouseRequestRepository
             , IWarehouseRequestDetailRepository warehouseRequestDetailRepository
             , IWarehouseInputService WarehouseInputService
             , IWarehouseInputRepository WarehouseInputRepository
             , IWarehouseInputDetailRepository WarehouseInputDetailRepository
             , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
             , IWarehouseInputDetailBarcodeService WarehouseInputDetailBarcodeService
             , IMembershipRepository MembershipRepository
             , IMembershipDepartmentRepository MembershipDepartmentRepository
             , IMaterialConvertRepository MaterialConvertRepository
             , INotificationService NotificationService
             , IZaloTokenService ZaloTokenService

            ) : base(WarehouseOutputRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WarehouseOutputDetailService = WarehouseOutputDetailService;
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseOutputDetailBarcodeService = WarehouseOutputDetailBarcodeService;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _WarehouseRequestRepository = warehouseRequestRepository;
            _WarehouseRequestDetailRepository = warehouseRequestDetailRepository;
            _WarehouseInputService = WarehouseInputService;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseInputDetailBarcodeService = WarehouseInputDetailBarcodeService;
            _MembershipRepository = MembershipRepository;
            _MembershipDepartmentRepository = MembershipDepartmentRepository;
            _MaterialConvertRepository = MaterialConvertRepository;
            _NotificationService = NotificationService;
            _ZaloTokenService = ZaloTokenService;

        }
        public override void InitializationSave(WarehouseOutput model)
        {
            BaseInitialization(model);
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;
            if (model.WarehouseRequestID > 0)
            {
                var WarehouseRequest = _WarehouseRequestRepository.GetByID(model.WarehouseRequestID.Value);
                model.WarehouseRequestName = WarehouseRequest.Code;
                model.CompanyID = model.CompanyID ?? WarehouseRequest.CompanyID;
                model.SupplierID = model.SupplierID ?? WarehouseRequest.SupplierID;
                model.CustomerID = model.CustomerID ?? WarehouseRequest.CustomerID;
                model.Code = model.Code ?? WarehouseRequest.Code;
                model.Name = model.Name ?? WarehouseRequest.Name;
                model.Description = model.Description ?? WarehouseRequest.Description;
                model.Note = model.Note ?? WarehouseRequest.Note;
                model.ParentID = WarehouseRequest.ParentID;
                model.ParentName = WarehouseRequest.ParentName;
                model.CreateUserID = model.CreateUserID ?? WarehouseRequest.CreateUserID;
                model.CreateUserCode = model.CreateUserCode ?? WarehouseRequest.CreateUserCode;
                model.CreateUserName = model.CreateUserName ?? WarehouseRequest.CreateUserName;


            }

            //model.SupplierID = model.SupplierID ?? GlobalHelper.DepartmentID;
            //model.CustomerID = model.CustomerID ?? GlobalHelper.DepartmentID;
            if (model.SupplierID == null || model.SupplierID == 0)
            {
                if (model.CompanyID > 0)
                {
                    var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.IsSync == true).FirstOrDefault();
                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                    {
                        model.SupplierID = CategoryDepartment.ID;
                    }
                }
            }
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
            if (model.MembershipID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.MembershipID.Value);
                model.MembershipName = Membership.Name;
                model.MembershipCode = Membership.UserName;
                model.Display = Membership.Display;
            }
            model.Code = model.Code ?? model.Date.Value.ToString("yyyyMMddhhmmss") + model.Date.Value.Ticks.ToString();
            model.Name = model.Name ?? model.Code;
            model.Total = model.Total ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public virtual void InitializationSaveHookRack(WarehouseOutput model)
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
            model.Code = model.Code ?? model.Date.Value.ToString("yyyyMMddhhmmss") + model.Date.Value.Ticks.ToString();
            model.Name = model.Name ?? model.Code;
        }
        public override WarehouseOutput SetModelByModelCheck(WarehouseOutput Model, WarehouseOutput ModelCheck)
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
                    Model.WarehouseRequestID = Model.WarehouseRequestID ?? ModelCheck.WarehouseRequestID;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public override async Task<BaseResult<WarehouseOutput>> SaveAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            InitializationSave(BaseParameter.BaseModel);
            //if (BaseParameter.BaseModel.ID > 0)
            //{

            //}
            //else
            //{
            //    var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
            //    SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            //}
            var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<WarehouseOutput>> SaveHookRackAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            InitializationSaveHookRack(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID).OrderBy(o => o.ID).FirstOrDefaultAsync();
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
        public override async Task<BaseResult<WarehouseOutput>> RemoveAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.Count = await _WarehouseOutputRepository.RemoveAsync(BaseParameter.ID);
            result = await SyncRemoveAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    try
                    {
                        await SyncByWarehouseRequestDetailAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                    try
                    {
                        await SyncToWarehouseInputAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                    try
                    {
                        await SendMailAsync(BaseParameter);
                        //await SendNotificationAsync(BaseParameter);
                        //await SendZaloAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncRemoveAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            var List = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
                    BaseParameter<WarehouseOutputDetail> BaseParameterWarehouseOutputDetail = new BaseParameter<WarehouseOutputDetail>();
                    BaseParameterWarehouseOutputDetail.ID = item.ID;
                    await _WarehouseOutputDetailService.RemoveAsync(BaseParameterWarehouseOutputDetail);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncByWarehouseRequestAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    var WarehouseRequest = await _WarehouseRequestRepository.GetByIDAsync(BaseParameter.ID);
                    if (WarehouseRequest.ID > 0)
                    {
                        if (WarehouseRequest.Active == true)
                        {
                            if (WarehouseRequest.IsManagerSupplier == true)
                            {
                                if (WarehouseRequest.IsManagerCustomer == true)
                                {
                                    var WarehouseOutput = new WarehouseOutput();
                                    WarehouseOutput.ParentID = WarehouseRequest.ParentID;
                                    WarehouseOutput.WarehouseRequestID = WarehouseRequest.ID;
                                    WarehouseOutput.Active = WarehouseRequest.Active;
                                    WarehouseOutput.CompanyID = WarehouseRequest.CompanyID;
                                    WarehouseOutput.SupplierID = WarehouseRequest.SupplierID;
                                    WarehouseOutput.CustomerID = WarehouseRequest.CustomerID;
                                    BaseParameter.BaseModel = WarehouseOutput;
                                    await SaveAsync(BaseParameter);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncToWarehouseInputAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter != null)
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.Active == true)
                        {
                            if (BaseParameter.BaseModel.IsComplete == true)
                            {
                                var WarehouseInput = new WarehouseInput();
                                WarehouseInput.Active = BaseParameter.BaseModel.Active;
                                WarehouseInput.IsComplete = BaseParameter.BaseModel.IsComplete;
                                WarehouseInput.WarehouseOutputID = BaseParameter.BaseModel.ID;
                                var BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                                BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                                await _WarehouseInputService.SaveAsync(BaseParameterWarehouseInput);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_ActiveToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.List = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsCompleteToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.List = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.List = new List<WarehouseOutput>();
            if (BaseParameter.Action > 0)
            {
                switch (BaseParameter.Action)
                {
                    case 1:
                        result.List = await GetByCondition(o => o.CustomerID != GlobalHelper.DepartmentIDOffice && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
                        break;
                    case 2:
                        result.List = await GetByCondition(o => o.CustomerID == GlobalHelper.DepartmentIDOffice && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).ToListAsync();
                        break;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipIDToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    result.List = await GetByCondition(o => o.SupplierID != null && ListMembershipDepartmentID.Contains(o.SupplierID.Value)).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.MembershipID > 0)
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                    if (ListMembershipDepartment.Count > 0)
                    {
                        var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                        result.List = await GetByCondition(o => o.SupplierID != null && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.MembershipID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    result.List = await GetByCondition(o => o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == BaseParameter.Active && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
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
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
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
                                        result.List = await GetByCondition(o => o.CustomerID > 0 && !ListDepartmentOfficeID.Contains(o.CustomerID.Value) && o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.CustomerID > 0 && !ListDepartmentOfficeID.Contains(o.CustomerID.Value) && o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    break;
                                case 2:
                                    if (BaseParameter.IsComplete == true)
                                    {
                                        result.List = await GetByCondition(o => o.CustomerID > 0 && ListDepartmentOfficeID.Contains(o.CustomerID.Value) && o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.CustomerID > 0 && ListDepartmentOfficeID.Contains(o.CustomerID.Value) && o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).OrderByDescending(o => o.Date).ToListAsync();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomNoFIFOAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ID);
                if (WarehouseOutput.ID > 0)
                {
                    result.BaseModel = WarehouseOutput;
                    var WarehouseOutputDetailBarcodeCheck = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Active == true && o.Barcode == BaseParameter.SearchString).OrderByDescending(o => o.DateScan).FirstOrDefaultAsync();
                    if (WarehouseOutputDetailBarcodeCheck == null)
                    {
                        var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && o.CategoryDepartmentID == WarehouseOutput.SupplierID && o.Active == true && o.Barcode == BaseParameter.SearchString).OrderByDescending(o => o.DateScan).FirstOrDefaultAsync();
                        if (WarehouseInputDetailBarcode == null)
                        {
                            await MySQLHelper.ERPSyncAsync02(GlobalHelper.MariaDBConectionString);
                            await MySQLHelper.ERPSyncAsync02(GlobalHelper.MariaDBConectionStringDJM);
                            WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && o.CategoryDepartmentID == WarehouseOutput.SupplierID && o.Active == true && o.Barcode == BaseParameter.SearchString).OrderByDescending(o => o.DateScan).FirstOrDefaultAsync();
                        }
                        if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                        {
                            var Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                            var WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialID == WarehouseInputDetailBarcode.MaterialID && o.Active == true).FirstOrDefaultAsync();
                            if (WarehouseOutputDetail == null)
                            {
                                WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialName == WarehouseInputDetailBarcode.MaterialName && o.Active == true).FirstOrDefaultAsync();
                            }
                            if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
                            {
                                if (WarehouseOutputDetail.QuantityGAP > 0)
                                {
                                    Quantity = WarehouseOutputDetail.QuantityGAP;
                                    if (WarehouseInputDetailBarcode.QuantityInventory < Quantity)
                                    {
                                        Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                    }
                                }

                                var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Barcode == BaseParameter.SearchString).OrderByDescending(o => o.DateScan).FirstOrDefaultAsync();
                                if (WarehouseOutputDetailBarcode == null)
                                {
                                    WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                                    WarehouseOutputDetailBarcode.IsFIFO = true;
                                }
                                WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                                WarehouseOutputDetailBarcode.ParentName = WarehouseOutput.Code;
                                WarehouseOutputDetailBarcode.CompanyID = WarehouseOutput.CompanyID;
                                WarehouseOutputDetailBarcode.CompanyName = WarehouseOutput.CompanyName;
                                WarehouseOutputDetailBarcode.WarehouseOutputDetailID = WarehouseOutputDetail.ID;
                                WarehouseOutputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                WarehouseOutputDetailBarcode.Active = true;
                                WarehouseOutputDetailBarcode.IsScan = true;
                                WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                WarehouseOutputDetailBarcode.MaterialID = WarehouseInputDetailBarcode.MaterialID;
                                WarehouseOutputDetailBarcode.MaterialName = WarehouseInputDetailBarcode.MaterialName;
                                WarehouseOutputDetailBarcode.QuantityRequest = WarehouseOutputDetail.Quantity;
                                WarehouseOutputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.QuantityInventory;
                                WarehouseOutputDetailBarcode.Quantity = Quantity;
                                WarehouseOutputDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
                                WarehouseOutputDetailBarcode.Year = WarehouseInputDetailBarcode.Year;
                                WarehouseOutputDetailBarcode.Month = WarehouseInputDetailBarcode.Month;
                                WarehouseOutputDetailBarcode.Day = WarehouseInputDetailBarcode.Day;
                                WarehouseOutputDetailBarcode.Week = WarehouseInputDetailBarcode.Week;
                                WarehouseOutputDetailBarcode.DateInput = WarehouseInputDetailBarcode.DateScan;
                                WarehouseOutputDetailBarcode.CategoryLocationID = WarehouseInputDetailBarcode.CategoryLocationID;
                                WarehouseOutputDetailBarcode.CategoryLocationName = WarehouseInputDetailBarcode.CategoryLocationName;
                                var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.IsCheck = true;
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ID);
                if (WarehouseOutput.ID > 0)
                {
                    result.BaseModel = WarehouseOutput;
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                        if (WarehouseOutputDetailBarcode != null)
                        {
                            if (WarehouseOutputDetailBarcode.ID > 0 && WarehouseOutputDetailBarcode.Quantity > 0)
                            {
                                result.IsCheck = false;
                                WarehouseOutputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                WarehouseOutputDetailBarcode.Active = true;
                                WarehouseOutputDetailBarcode.IsScan = true;
                                var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                            }
                        }
                        if (result.IsCheck == true)
                        {
                            var ListWarehouseInput = await _WarehouseInputService.GetByCondition(o => o.Active == true && o.CustomerID == WarehouseOutput.SupplierID).OrderByDescending(o => o.Date).ToListAsync();
                            if (ListWarehouseInput.Count > 0)
                            {
                                var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                {
                                    var Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                    var WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialID == WarehouseInputDetailBarcode.MaterialID && o.Active == true).FirstOrDefaultAsync();
                                    if (WarehouseOutputDetail == null)
                                    {
                                        WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialName == WarehouseInputDetailBarcode.MaterialName && o.Active == true).FirstOrDefaultAsync();
                                    }
                                    if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
                                    {
                                        if (WarehouseOutputDetail.QuantityGAP > 0)
                                        {
                                            Quantity = WarehouseOutputDetail.QuantityGAP;
                                            if (WarehouseInputDetailBarcode.QuantityInventory < Quantity)
                                            {
                                                Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                            }
                                        }
                                        WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                                        WarehouseOutputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                        WarehouseOutputDetailBarcode.IsFIFO = true;
                                        WarehouseOutputDetailBarcode.Active = true;
                                        WarehouseOutputDetailBarcode.IsScan = true;
                                        WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                        WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                                        WarehouseOutputDetailBarcode.MaterialID = WarehouseInputDetailBarcode.MaterialID;
                                        WarehouseOutputDetailBarcode.MaterialName = WarehouseInputDetailBarcode.MaterialName;
                                        WarehouseOutputDetailBarcode.QuantityRequest = GlobalHelper.InitializationNumber;
                                        WarehouseOutputDetailBarcode.Quantity = Quantity;
                                        WarehouseOutputDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
                                        var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                        BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                        await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.IsCheck = true;
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ID);
                if (WarehouseOutput.ID > 0)
                {
                    result.BaseModel = WarehouseOutput;
                    if (WarehouseOutput.IsComplete != true)
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                            if (WarehouseOutputDetailBarcode != null)
                            {
                                if (WarehouseOutputDetailBarcode.ID > 0 && WarehouseOutputDetailBarcode.Quantity > 0)
                                {
                                    result.IsCheck = false;
                                    WarehouseOutputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                    WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                    WarehouseOutputDetailBarcode.Active = true;
                                    WarehouseOutputDetailBarcode.IsScan = true;
                                    var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                    BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                    await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                                }
                            }
                            if (result.IsCheck == true)
                            {
                                var ListWarehouseInput = await _WarehouseInputService.GetByCondition(o => o.Active == true && o.CustomerID == WarehouseOutput.SupplierID).ToListAsync();
                                if (ListWarehouseInput.Count > 0)
                                {
                                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                    var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                                    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                    {
                                        var WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialID == WarehouseInputDetailBarcode.MaterialID && o.Active == true).FirstOrDefaultAsync();
                                        if (WarehouseOutputDetail == null)
                                        {
                                            WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.MaterialName == WarehouseInputDetailBarcode.MaterialName && o.Active == true).FirstOrDefaultAsync();
                                        }
                                        if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
                                        {
                                            if (WarehouseOutputDetail.QuantityGAP > 0)
                                            {
                                                var Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                                if (WarehouseOutputDetail.QuantityGAP > 0)
                                                {
                                                    Quantity = WarehouseOutputDetail.QuantityGAP;
                                                    if (WarehouseInputDetailBarcode.QuantityInventory < Quantity)
                                                    {
                                                        Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                                    }
                                                }
                                                WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                                                WarehouseOutputDetailBarcode.UpdateUserID = BaseParameter.UpdateUserID;
                                                WarehouseOutputDetailBarcode.IsFIFO = true;
                                                WarehouseOutputDetailBarcode.Active = true;
                                                WarehouseOutputDetailBarcode.IsScan = true;
                                                WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                                WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                                                WarehouseOutputDetailBarcode.WarehouseOutputDetailID = WarehouseOutputDetail.ID;
                                                WarehouseOutputDetailBarcode.MaterialID = WarehouseOutputDetail.MaterialID;
                                                WarehouseOutputDetailBarcode.MaterialName = WarehouseOutputDetail.MaterialName;
                                                WarehouseOutputDetailBarcode.Display = WarehouseOutputDetail.Display;
                                                WarehouseOutputDetailBarcode.QuantityRequest = WarehouseOutputDetail.QuantityRequest;
                                                WarehouseOutputDetailBarcode.Quantity = Quantity;
                                                WarehouseOutputDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
                                                var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                                BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                                await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
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
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeNoFIFOAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.IsCheck = true;
            if (BaseParameter.ID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ID);
                if (WarehouseOutput.ID > 0)
                {
                    result.BaseModel = WarehouseOutput;
                    if (WarehouseOutput.IsComplete != true)
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == WarehouseOutput.ID && o.Barcode == BaseParameter.SearchString).FirstOrDefaultAsync();
                            if (WarehouseOutputDetailBarcode != null)
                            {
                                if (WarehouseOutputDetailBarcode.ID > 0)
                                {
                                    result.IsCheck = false;
                                    WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                                    WarehouseOutputDetailBarcode.Active = true;
                                    WarehouseOutputDetailBarcode.IsScan = true;
                                    var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                    BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                    await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                                }
                            }
                            if (result.IsCheck == true)
                            {
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == result.BaseModel.ID).OrderBy(o => o.CategoryLocationName).ThenBy(o => o.MaterialName).ToListAsync();
                            string HTMLContent = GlobalHelper.InitializationString;
                            if (BaseParameter.Active == true)
                            {
                                StringBuilder HTMLContentSub = new StringBuilder();
                                foreach (var item in ListWarehouseOutputDetailBarcode)
                                {
                                    var BARCODE_AA = GlobalHelper.InitializationString;
                                    var BARCODE_GG = GlobalHelper.InitializationString;
                                    var BARCODE_BB = GlobalHelper.InitializationString;
                                    var BARCODE_CC = GlobalHelper.InitializationString;
                                    var BARCODE_FF = GlobalHelper.InitializationString;

                                    var BARCODE_ZZ = item.DateInput.Value.ToString("yyyy-MM-dd");
                                    var BARCODE_EE = item.Display;
                                    var BARCODE_DD = item.MaterialName;
                                    var BARCODE_HH = item.CategoryLocationName;
                                    var BARCODE_QR = item.Barcode;
                                    var BARCODE_ZZ1 = item.DateInput.Value.ToString("yyyy-MM-dd");
                                    var BARCODE_ZZ2 = item.Week;
                                    var BARCODE_DD1 = item.MaterialName;
                                    var BARCODE_CC1 = item.Quantity.Value.ToString("N0");

                                    var WarehouseInputDetail = await _WarehouseInputDetailRepository.GetByCondition(o => o.Barcode == item.Barcode).FirstOrDefaultAsync();
                                    if (WarehouseInputDetail != null)
                                    {
                                        if (WarehouseInputDetail.ID > 0)
                                        {
                                            BARCODE_FF = WarehouseInputDetail.Description;
                                            BARCODE_BB = WarehouseInputDetail.Quantity.Value.ToString("N0");
                                            BARCODE_CC = WarehouseInputDetail.QuantityInventory.Value.ToString("N0");
                                        }
                                    }
                                    HTMLContentSub.AppendLine(GlobalHelper.CreateHTMLB04(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ, BARCODE_CC1));
                                }
                                HTMLContent = HTMLContentSub.ToString();
                            }
                            else
                            {
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutput.html");
                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                {
                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        HTMLContent = r.ReadToEnd();
                                    }
                                }
                                result.BaseModel.Code = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + result.BaseModel.ID + "' title='" + result.BaseModel.Code + "'><b>" + result.BaseModel.Code + "</b></a>";

                                result.BaseModel.MembershipName = result.BaseModel.MembershipCode + " | " + result.BaseModel.MembershipName;
                                HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                                HTMLContent = HTMLContent.Replace(@"[Date]", result.BaseModel.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                HTMLContent = HTMLContent.Replace(@"[MembershipName]", result.BaseModel.MembershipName);
                                HTMLContent = HTMLContent.Replace(@"[SupplierName]", result.BaseModel.SupplierName);
                                HTMLContent = HTMLContent.Replace(@"[CustomerName]", result.BaseModel.CustomerName);
                                HTMLContent = HTMLContent.Replace(@"[WarehouseRequestName]", result.BaseModel.WarehouseRequestName);

                                StringBuilder Detail = new StringBuilder();
                                int NO = 0;
                                foreach (var item in ListWarehouseOutputDetailBarcode)
                                {
                                    try
                                    {
                                        NO = NO + 1;
                                        var QuantitySNP = item.QuantitySNP ?? GlobalHelper.InitializationNumber;
                                        var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                        var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                        var DateInput = GlobalHelper.InitializationString;
                                        if (item.DateInput != null)
                                        {
                                            DateInput = item.DateInput.Value.ToString("yyyy-MM-dd");
                                        }
                                        int Box = (int)(Quantity / QuantitySNP);
                                        int Remaining = (int)(Quantity % QuantitySNP);
                                        if (Remaining > 0)
                                        {
                                            Box = Box + 1;
                                        }
                                        Detail.AppendLine(@"<tr>");
                                        Detail.AppendLine(@"<td>" + NO + "</td>");
                                        Detail.AppendLine(@"<td>");
                                        Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                        Detail.AppendLine("<hr/>");
                                        Detail.AppendLine(@"<b>" + item.Barcode + "</b>");
                                        Detail.AppendLine(@"</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + Quantity.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + Box.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + DateInput + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + item.Week + "</td>");
                                        Detail.AppendLine(@"<td>" + item.CategoryLocationName + "</td>");
                                        Detail.AppendLine(@"</tr>");
                                    }
                                    catch (Exception ex)
                                    {
                                        string me = ex.Message;
                                    }
                                }

                                HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());
                            }

                            string fileName = SheetName + "_" + result.BaseModel.ID + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLContent);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroupAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            string HTMLContent = GlobalHelper.InitializationString;
                            if (BaseParameter.Active == true)
                            {
                                //StringBuilder HTMLContentSub = new StringBuilder();
                                //foreach (var item in ListWarehouseOutputDetailBarcode)
                                //{
                                //    var BARCODE_AA = GlobalHelper.InitializationString;
                                //    var BARCODE_GG = GlobalHelper.InitializationString;
                                //    var BARCODE_BB = GlobalHelper.InitializationString;
                                //    var BARCODE_CC = GlobalHelper.InitializationString;
                                //    var BARCODE_FF = GlobalHelper.InitializationString;

                                //    var BARCODE_ZZ = item.DateInput.Value.ToString("yyyy-MM-dd");
                                //    var BARCODE_EE = item.Display;
                                //    var BARCODE_DD = item.MaterialName;
                                //    var BARCODE_HH = item.CategoryLocationName;
                                //    var BARCODE_QR = item.Barcode;
                                //    var BARCODE_ZZ1 = item.DateInput.Value.ToString("yyyy-MM-dd");
                                //    var BARCODE_ZZ2 = item.Week;
                                //    var BARCODE_DD1 = item.MaterialName;
                                //    var BARCODE_CC1 = item.Quantity.Value.ToString("N0");

                                //    var WarehouseInputDetail = await _WarehouseInputDetailRepository.GetByCondition(o => o.Barcode == item.Barcode).FirstOrDefaultAsync();
                                //    if (WarehouseInputDetail != null)
                                //    {
                                //        if (WarehouseInputDetail.ID > 0)
                                //        {
                                //            BARCODE_FF = WarehouseInputDetail.Description;
                                //            BARCODE_BB = WarehouseInputDetail.Quantity.Value.ToString("N0");
                                //            BARCODE_CC = WarehouseInputDetail.QuantityInventory.Value.ToString("N0");
                                //        }
                                //    }
                                //    HTMLContentSub.AppendLine(GlobalHelper.CreateHTMLB04(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ, BARCODE_CC1));
                                //}
                                //HTMLContent = HTMLContentSub.ToString();
                            }
                            else
                            {
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutput.html");
                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                {
                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        HTMLContent = r.ReadToEnd();
                                    }
                                }
                                result.BaseModel.Code = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + result.BaseModel.ID + "' title='" + result.BaseModel.Code + "'><b>" + result.BaseModel.Code + "</b></a>";

                                result.BaseModel.MembershipName = result.BaseModel.MembershipCode + " | " + result.BaseModel.MembershipName;
                                HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                                HTMLContent = HTMLContent.Replace(@"[Date]", result.BaseModel.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                HTMLContent = HTMLContent.Replace(@"[MembershipName]", result.BaseModel.MembershipName);
                                HTMLContent = HTMLContent.Replace(@"[SupplierName]", result.BaseModel.SupplierName);
                                HTMLContent = HTMLContent.Replace(@"[CustomerName]", result.BaseModel.CustomerName);
                                HTMLContent = HTMLContent.Replace(@"[WarehouseRequestName]", result.BaseModel.WarehouseRequestName);

                                StringBuilder Detail = new StringBuilder();
                                int NO = 0;
                                var ListWarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == result.BaseModel.ID).OrderBy(o => o.MaterialID).ToListAsync();
                                for (int i = 0; i < ListWarehouseOutputDetail.Count; i++)
                                {
                                    if (ListWarehouseOutputDetail[i].MaterialName == "AVSS03GB_MV")
                                    {
                                    }
                                    var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.CategoryDepartmentID == result.BaseModel.SupplierID && o.Active == true && o.MaterialID == ListWarehouseOutputDetail[i].MaterialID && o.QuantityInventory > 0).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                                    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                    {
                                        ListWarehouseOutputDetail[i].Week = WarehouseInputDetailBarcode.Week;
                                        ListWarehouseOutputDetail[i].CategoryLocationName = WarehouseInputDetailBarcode.CategoryLocationName;
                                        ListWarehouseOutputDetail[i].DateScan = WarehouseInputDetailBarcode.DateScan;
                                    }
                                }
                                ListWarehouseOutputDetail = ListWarehouseOutputDetail.OrderBy(o => o.CategoryLocationName).ToList();
                                foreach (var item in ListWarehouseOutputDetail)
                                {
                                    try
                                    {
                                        NO = NO + 1;
                                        var QuantitySNP = item.QuantitySNP ?? 1;
                                        if (QuantitySNP == 0)
                                        {
                                            QuantitySNP = 1;
                                        }
                                        var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                        var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                        int Box = (int)(Quantity / QuantitySNP);
                                        int Remaining = (int)(Quantity % QuantitySNP);
                                        if (Remaining > 0)
                                        {
                                            Box = Box + 1;
                                        }
                                        var DateInput = GlobalHelper.InitializationString;
                                        if (item.DateScan != null)
                                        {
                                            DateInput = item.DateScan.Value.ToString("yyyy-MM-dd");
                                        }
                                        Detail.AppendLine(@"<tr>");
                                        Detail.AppendLine(@"<td>" + NO + "</td>");
                                        Detail.AppendLine(@"<td>");
                                        Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                        Detail.AppendLine(@"</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + Quantity.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + Box.ToString("N0") + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + DateInput + "</td>");
                                        Detail.AppendLine(@"<td style='text-align: right;'>" + item.Week + "</td>");
                                        Detail.AppendLine(@"<td>" + item.CategoryLocationName + "</td>");
                                        Detail.AppendLine(@"</tr>");
                                    }
                                    catch (Exception ex)
                                    {
                                        string me = ex.Message;
                                    }
                                }

                                HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());
                            }

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLContent);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroup2025Async(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            var ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
                            if (!string.IsNullOrEmpty(result.BaseModel.Description) && result.BaseModel.Description.Contains("MES"))
                            {
                                var ListWarehouseOutput = await GetByCondition(o => o.Active == true && o.IsComplete != true && o.CreateUserID == result.BaseModel.CreateUserID).ToListAsync();
                                if (ListWarehouseOutput.Count > 0)
                                {
                                    var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                                    ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.CategoryLocationName).ThenBy(o => o.MaterialName).ToListAsync();
                                }
                            }
                            else
                            {
                                ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID == result.BaseModel.ID).OrderBy(o => o.CategoryLocationName).ThenBy(o => o.MaterialName).ToListAsync();
                            }
                            ListWarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcode.OrderBy(o => o.CategoryLocationName).ToList();
                            var ListWarehouseOutputDetailBarcodeMaterialID = ListWarehouseOutputDetailBarcode.Select(x => x.MaterialID).Distinct().ToList();
                            var ListWarehouseOutputDetailBarcodePrint = new List<WarehouseOutputDetailBarcode>();
                            foreach (var MaterialID in ListWarehouseOutputDetailBarcodeMaterialID)
                            {
                                var WarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID == MaterialID).OrderBy(o => o.CategoryLocationName).FirstOrDefault();
                                if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                {
                                    WarehouseOutputDetailBarcode.Quantity = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID == MaterialID).Sum(o => o.Quantity);
                                    ListWarehouseOutputDetailBarcodePrint.Add(WarehouseOutputDetailBarcode);
                                }
                            }
                            ListWarehouseOutputDetailBarcodePrint = ListWarehouseOutputDetailBarcodePrint.OrderBy(o => o.CategoryLocationName).ThenBy(o => o.MaterialName).ToList();
                            StringBuilder HTMLContentGroup = new StringBuilder();
                            StringBuilder Detail = new StringBuilder();
                            string HTMLContent = GlobalHelper.InitializationString;
                            int Count = ListWarehouseOutputDetailBarcodePrint.Count;
                            for (int i = 0; i < Count; i++)
                            {
                                var item = ListWarehouseOutputDetailBarcodePrint[i];

                                if (i % 10 == 0)
                                {
                                    HTMLContent = GlobalHelper.InitializationString;
                                    Detail = new StringBuilder();
                                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputGroup.html");
                                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                    {
                                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                        {
                                            HTMLContent = r.ReadToEnd();
                                        }
                                    }
                                    result.BaseModel.Code = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + result.BaseModel.ID + "' title='" + result.BaseModel.Code + "'><b>" + result.BaseModel.Code + "</b></a>";

                                    var MembershipName = result.BaseModel.MembershipCode + " | " + result.BaseModel.MembershipName;
                                    HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                                    HTMLContent = HTMLContent.Replace(@"[Date]", result.BaseModel.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                    HTMLContent = HTMLContent.Replace(@"[MembershipName]", MembershipName);
                                    HTMLContent = HTMLContent.Replace(@"[SupplierName]", result.BaseModel.SupplierName);
                                    HTMLContent = HTMLContent.Replace(@"[CustomerName]", result.BaseModel.CustomerName);
                                    HTMLContent = HTMLContent.Replace(@"[WarehouseRequestName]", result.BaseModel.WarehouseRequestName);

                                }

                                try
                                {
                                    int NO = i + 1;
                                    var QuantitySNP = item.QuantitySNP ?? 1;
                                    if (QuantitySNP == 0)
                                    {
                                        QuantitySNP = 1;
                                    }
                                    var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                    var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                    var DateInput = GlobalHelper.InitializationString;
                                    if (item.DateInput != null)
                                    {
                                        DateInput = item.DateInput.Value.ToString("yyyy-MM-dd");
                                    }
                                    var Week = GlobalHelper.InitializationString;
                                    if (item.Week != null)
                                    {
                                        Week = item.Week.ToString();
                                    }
                                    int Box = (int)(Quantity / QuantitySNP);
                                    int Remaining = (int)(Quantity % QuantitySNP);
                                    if (Remaining > 0)
                                    {
                                        Box = Box + 1;
                                    }
                                    Detail.AppendLine(@"<tr>");
                                    Detail.AppendLine(@"<td>" + NO + "</td>");
                                    Detail.AppendLine(@"<td>");
                                    Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                    //Detail.AppendLine("<hr/>");
                                    //Detail.AppendLine(@"<b>" + item.Barcode + "</b>");
                                    Detail.AppendLine(@"</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Quantity.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Box.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + DateInput + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Week + "</td>");
                                    Detail.AppendLine(@"<td>" + item.CategoryLocationName + "</td>");
                                    Detail.AppendLine(@"</tr>");
                                }
                                catch (Exception ex)
                                {
                                    string me = ex.Message;
                                }
                                if (i % 10 == 9 || i == Count - 1)
                                {
                                    HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());
                                    HTMLContentGroup.AppendLine(HTMLContent);
                                }
                            }


                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLContentGroup.ToString());
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroup2026Async(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            var ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
                            var ListWarehouseOutputDetail = new List<WarehouseOutputDetail>();
                            if (!string.IsNullOrEmpty(result.BaseModel.Description) && result.BaseModel.Description.Contains("MES"))
                            {
                                var ListWarehouseOutput = await GetByCondition(o => o.Active == true && o.IsComplete != true && o.CreateUserID == result.BaseModel.CreateUserID).ToListAsync();
                                if (ListWarehouseOutput.Count > 0)
                                {
                                    var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                                    ListWarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.MaterialName).ToListAsync();
                                }
                            }
                            else
                            {
                                ListWarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == result.BaseModel.ID).OrderBy(o => o.MaterialName).ToListAsync();
                            }
                            var ListWarehouseOutputDetailMaterialID = ListWarehouseOutputDetail.Select(x => x.MaterialID).Distinct().ToList();
                            var ListWarehouseOutputDetailPrint = new List<WarehouseOutputDetail>();
                            foreach (var MaterialID in ListWarehouseOutputDetailMaterialID)
                            {
                                var WarehouseOutputDetail = ListWarehouseOutputDetail.Where(o => o.MaterialID == MaterialID).FirstOrDefault();
                                if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
                                {
                                    WarehouseOutputDetail.Quantity = ListWarehouseOutputDetail.Where(o => o.MaterialID == MaterialID).Sum(o => o.Quantity);
                                    var WarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID == MaterialID).FirstOrDefault();
                                    if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                    {
                                        WarehouseOutputDetail.CategoryLocationName = WarehouseOutputDetailBarcode.CategoryLocationName;
                                        WarehouseOutputDetail.Week = WarehouseOutputDetailBarcode.Week;
                                        WarehouseOutputDetail.DateInput = WarehouseOutputDetailBarcode.DateInput;
                                    }
                                    ListWarehouseOutputDetailPrint.Add(WarehouseOutputDetail);
                                }
                            }
                            StringBuilder HTMLContentGroup = new StringBuilder();
                            StringBuilder Detail = new StringBuilder();
                            string HTMLContent = GlobalHelper.InitializationString;
                            int Count = ListWarehouseOutputDetailPrint.Count;
                            for (int i = 0; i < Count; i++)
                            {
                                var item = ListWarehouseOutputDetailPrint[i];

                                if (i % 10 == 0)
                                {
                                    HTMLContent = GlobalHelper.InitializationString;
                                    Detail = new StringBuilder();
                                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputGroup.html");
                                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                    {
                                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                        {
                                            HTMLContent = r.ReadToEnd();
                                        }
                                    }
                                    result.BaseModel.Code = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + result.BaseModel.ID + "' title='" + result.BaseModel.Code + "'><b>" + result.BaseModel.Code + "</b></a>";

                                    var MembershipName = result.BaseModel.MembershipCode + " | " + result.BaseModel.MembershipName;
                                    HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                                    HTMLContent = HTMLContent.Replace(@"[Date]", result.BaseModel.Date.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                                    HTMLContent = HTMLContent.Replace(@"[MembershipName]", MembershipName);
                                    HTMLContent = HTMLContent.Replace(@"[SupplierName]", result.BaseModel.SupplierName);
                                    HTMLContent = HTMLContent.Replace(@"[CustomerName]", result.BaseModel.CustomerName);
                                    HTMLContent = HTMLContent.Replace(@"[WarehouseRequestName]", result.BaseModel.WarehouseRequestName);

                                }

                                try
                                {
                                    int NO = i + 1;
                                    var QuantitySNP = item.QuantitySNP ?? 1;
                                    if (QuantitySNP == 0)
                                    {
                                        QuantitySNP = 1;
                                    }
                                    var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                    var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                    var DateInput = GlobalHelper.InitializationString;
                                    if (item.DateInput != null)
                                    {
                                        DateInput = item.DateInput.Value.ToString("yyyy-MM-dd");
                                    }
                                    var Week = GlobalHelper.InitializationString;
                                    if (item.Week != null)
                                    {
                                        Week = item.Week.ToString();
                                    }
                                    int Box = (int)(Quantity / QuantitySNP);
                                    int Remaining = (int)(Quantity % QuantitySNP);
                                    if (Remaining > 0)
                                    {
                                        Box = Box + 1;
                                    }
                                    Detail.AppendLine(@"<tr>");
                                    Detail.AppendLine(@"<td>" + NO + "</td>");
                                    Detail.AppendLine(@"<td>");
                                    Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                    //Detail.AppendLine("<hr/>");
                                    //Detail.AppendLine(@"<b>" + item.Barcode + "</b>");
                                    Detail.AppendLine(@"</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Quantity.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Box.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'></td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + DateInput + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Week + "</td>");
                                    Detail.AppendLine(@"<td>" + item.CategoryLocationName + "</td>");
                                    Detail.AppendLine(@"</tr>");
                                }
                                catch (Exception ex)
                                {
                                    string me = ex.Message;
                                }
                                if (i % 10 == 9 || i == Count - 1)
                                {
                                    HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());
                                    HTMLContentGroup.AppendLine(HTMLContent);
                                }
                            }


                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLContentGroup.ToString());
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroupMobileAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            string HTMLContent = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputMobile.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLContent = r.ReadToEnd();
                                }
                            }
                            result.BaseModel.Code = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + result.BaseModel.ID + "' title='" + result.BaseModel.Code + "'><b>" + result.BaseModel.Code + "</b></a>";
                            HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                            HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            StringBuilder Detail = new StringBuilder();
                            int NO = 0;
                            var ListWarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == result.BaseModel.ID).OrderBy(o => o.MaterialID).ToListAsync();
                            for (int i = 0; i < ListWarehouseOutputDetail.Count; i++)
                            {
                                var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.CategoryDepartmentID == result.BaseModel.SupplierID && o.Active == true && o.MaterialID == ListWarehouseOutputDetail[i].MaterialID && o.QuantityInventory > 0).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                {
                                    ListWarehouseOutputDetail[i].Week = WarehouseInputDetailBarcode.Week;
                                    ListWarehouseOutputDetail[i].CategoryLocationName = WarehouseInputDetailBarcode.CategoryLocationName;
                                    ListWarehouseOutputDetail[i].DateScan = WarehouseInputDetailBarcode.DateScan;
                                }
                            }
                            ListWarehouseOutputDetail = ListWarehouseOutputDetail.OrderBy(o => o.Note).ToList();
                            foreach (var item in ListWarehouseOutputDetail)
                            {
                                try
                                {
                                    NO = NO + 1;
                                    var QuantitySNP = item.QuantitySNP ?? 1;
                                    if (QuantitySNP == 0)
                                    {
                                        QuantitySNP = 1;
                                    }
                                    var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                    var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                    int Box = (int)(Quantity / QuantitySNP);
                                    int Remaining = (int)(Quantity % QuantitySNP);
                                    if (Remaining > 0)
                                    {
                                        Box = Box + 1;
                                    }
                                    var DateInput = GlobalHelper.InitializationString;
                                    if (item.DateScan != null)
                                    {
                                        DateInput = item.DateScan.Value.ToString("yyyy-MM-dd");
                                    }
                                    Detail.AppendLine(@"<tr>");
                                    Detail.AppendLine(@"<td>" + NO + "</td>");
                                    Detail.AppendLine(@"<td>");
                                    Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                    Detail.AppendLine(@"</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Quantity.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + Box.ToString("N0") + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + DateInput + "</td>");
                                    Detail.AppendLine(@"<td style='text-align: right;'>" + item.Week + "</td>");
                                    Detail.AppendLine(@"<td>" + item.CategoryLocationName + "</td>");
                                    Detail.AppendLine(@"</tr>");
                                }
                                catch (Exception ex)
                                {
                                    string me = ex.Message;
                                }
                            }

                            HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLContent);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> PrintSumAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            string SheetName = this.GetType().Name;
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
            {
                var ListWarehouseOutput = new List<WarehouseOutput>();

                switch (BaseParameter.CategoryDepartmentID)
                {
                    case 23:
                    case 188:
                        ListWarehouseOutput = await GetByCondition(o => o.Active == true && o.IsComplete == true && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                        break;
                    default:
                        ListWarehouseOutput = await GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                        break;
                }
                if (ListWarehouseOutput.Count > 0)
                {
                    var CategoryDepartmentName = GlobalHelper.InitializationString;
                    switch (BaseParameter.CategoryDepartmentID)
                    {
                        case 23:
                        case 188:
                            CategoryDepartmentName = ListWarehouseOutput[0].SupplierName;
                            break;
                        default:
                            CategoryDepartmentName = ListWarehouseOutput[0].CustomerName;
                            break;
                    }
                    var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).OrderBy(o => o).ToList();
                    var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.Active == true && ListWarehouseOutputID.Contains(o.ParentID ?? 0)).ToListAsync();
                    if (ListWarehouseOutputDetail.Count > 0)
                    {
                        var ListWarehouseOutputDetailSum = ListWarehouseOutputDetail;
                        var ListWarehouseOutputDetailTotal = new List<WarehouseOutputDetail>();
                        var ListWarehouseOutputDetailMaterialID = ListWarehouseOutputDetail.Select(o => o.MaterialID).Distinct().ToList();
                        for (int i = 0; i < ListWarehouseOutputDetailMaterialID.Count; i++)
                        {
                            var ListWarehouseOutputDetailSub = ListWarehouseOutputDetail.Where(o => o.MaterialID == ListWarehouseOutputDetailMaterialID[i]).ToList();
                            if (ListWarehouseOutputDetailSub.Count > 0)
                            {
                                var WarehouseOutputDetail = new WarehouseOutputDetail(); ;
                                WarehouseOutputDetail.ID = 0;
                                WarehouseOutputDetail.ParentID = 0;
                                WarehouseOutputDetail.ParentName = GlobalHelper.InitializationString;
                                WarehouseOutputDetail.MaterialName = ListWarehouseOutputDetailSub[0].MaterialName;
                                WarehouseOutputDetail.QuantitySNP = ListWarehouseOutputDetailSub[0].QuantitySNP;
                                WarehouseOutputDetail.QuantityRequest = ListWarehouseOutputDetailSub.Sum(o => o.QuantityRequest);
                                WarehouseOutputDetail.QuantityActual = ListWarehouseOutputDetailSub.Sum(o => o.QuantityActual);
                                WarehouseOutputDetail.QuantityGAP = ListWarehouseOutputDetailSub.Sum(o => o.QuantityGAP);
                                WarehouseOutputDetail.Quantity = ListWarehouseOutputDetailSub.Sum(o => o.Quantity);
                                ListWarehouseOutputDetailSum.Add(WarehouseOutputDetail);
                                ListWarehouseOutputDetailTotal.Add(WarehouseOutputDetail);
                            }
                        }
                        string HTMLContent = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputSum.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLContent = r.ReadToEnd();
                            }
                        }
                        HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        HTMLContent = HTMLContent.Replace(@"[DateBegin]", BaseParameter.DateBegin.Value.ToString("yyyy-MM-dd"));
                        HTMLContent = HTMLContent.Replace(@"[DateEnd]", BaseParameter.DateEnd.Value.ToString("yyyy-MM-dd"));
                        HTMLContent = HTMLContent.Replace(@"[CategoryDepartmentName]", CategoryDepartmentName);
                        StringBuilder Detail = new StringBuilder();
                        int NO = 0;
                        ListWarehouseOutputDetailSum = ListWarehouseOutputDetailSum.OrderBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
                        foreach (var item in ListWarehouseOutputDetailSum)
                        {
                            try
                            {
                                var QuantitySNP = item.QuantitySNP ?? GlobalHelper.InitializationNumber;
                                var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                var QuantityActual = item.QuantityActual ?? GlobalHelper.InitializationNumber;
                                var QuantityGAP = item.QuantityGAP ?? GlobalHelper.InitializationNumber;

                                var URL = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + item.ParentID + "' title='" + item.ParentName + "'><b>" + item.ParentName + "</b></a>";
                                NO = NO + 1;
                                Detail.AppendLine(@"<tr>");
                                Detail.AppendLine(@"<td>" + NO + "</td>");
                                Detail.AppendLine(@"<td>" + item.ParentID + "</td>");
                                Detail.AppendLine(@"<td>" + URL + "</td>");
                                Detail.AppendLine(@"<td>");
                                Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                Detail.AppendLine(@"</td>");
                                Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold;'>" + Quantity.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold; color: green;'>" + QuantityActual.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold; color: red;'>" + QuantityGAP.ToString("N0") + "</td>");
                                Detail.AppendLine(@"</tr>");
                            }
                            catch (Exception ex)
                            {
                                string me = ex.Message;
                            }
                        }

                        HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());

                        string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                        string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        string filePath = Path.Combine(physicalPathCreate, fileName);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                await w.WriteLineAsync(HTMLContent);
                            }
                        }
                        result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                        HTMLContent = GlobalHelper.InitializationString;
                        physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputTotal.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLContent = r.ReadToEnd();
                            }
                        }
                        HTMLContent = HTMLContent.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        HTMLContent = HTMLContent.Replace(@"[DateBegin]", BaseParameter.DateBegin.Value.ToString("yyyy-MM-dd"));
                        HTMLContent = HTMLContent.Replace(@"[DateEnd]", BaseParameter.DateEnd.Value.ToString("yyyy-MM-dd"));
                        HTMLContent = HTMLContent.Replace(@"[CategoryDepartmentName]", CategoryDepartmentName);
                        Detail = new StringBuilder();
                        NO = 0;
                        ListWarehouseOutputDetailTotal = ListWarehouseOutputDetailTotal.OrderBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
                        foreach (var item in ListWarehouseOutputDetailTotal)
                        {
                            try
                            {
                                var QuantitySNP = item.QuantitySNP ?? GlobalHelper.InitializationNumber;
                                var QuantityRequest = item.QuantityRequest ?? GlobalHelper.InitializationNumber;
                                var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                                var QuantityActual = item.QuantityActual ?? GlobalHelper.InitializationNumber;
                                var QuantityGAP = item.QuantityGAP ?? GlobalHelper.InitializationNumber;

                                var URL = @"<a target='_blank' href='" + GlobalHelper.ERPSite + "/#/WarehouseOutputInfo/" + item.ParentID + "' title='" + item.ParentName + "'><b>" + item.ParentName + "</b></a>";
                                NO = NO + 1;
                                Detail.AppendLine(@"<tr>");
                                Detail.AppendLine(@"<td>" + NO + "</td>");
                                Detail.AppendLine(@"<td>");
                                Detail.AppendLine(@"<b>" + item.MaterialName + "</b>");
                                Detail.AppendLine(@"</td>");
                                Detail.AppendLine(@"<td style='text-align: right;'>" + QuantitySNP.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right;'>" + QuantityRequest.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold;'>" + Quantity.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold; color: green;'>" + QuantityActual.ToString("N0") + "</td>");
                                Detail.AppendLine(@"<td style='text-align: right; font-weight: bold; color: red;'>" + QuantityGAP.ToString("N0") + "</td>");
                                Detail.AppendLine(@"</tr>");
                            }
                            catch (Exception ex)
                            {
                                string me = ex.Message;
                            }
                        }

                        HTMLContent = HTMLContent.Replace(@"[Detail]", Detail.ToString());

                        fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                        physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        filePath = Path.Combine(physicalPathCreate, fileName);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                await w.WriteLineAsync(HTMLContent);
                            }
                        }
                        result.Note = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncByWarehouseRequestDetailAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.WarehouseRequestID > 0)
                    {
                        if (BaseParameter.BaseModel.IsSync == true)
                        {
                        }
                        else
                        {
                            BaseParameter.BaseModel.IsSync = true;
                            var WarehouseRequest = await _WarehouseRequestRepository.GetByIDAsync(BaseParameter.BaseModel.WarehouseRequestID.Value);
                            if (WarehouseRequest.ID > 0)
                            {
                                if (WarehouseRequest.Active == true)
                                {
                                    if (WarehouseRequest.IsManagerSupplier == true)
                                    {
                                        if (WarehouseRequest.IsManagerCustomer == true)
                                        {
                                            var ListWarehouseRequestDetail = await _WarehouseRequestDetailRepository.GetByParentIDToListAsync(WarehouseRequest.ID);
                                            if (ListWarehouseRequestDetail != null && ListWarehouseRequestDetail.Count > 0)
                                            {
                                                var ListWarehouseRequestDetailMaterialID = ListWarehouseRequestDetail.Select(x => x.MaterialID).Distinct().ToList();
                                                var ListWarehouseRequestDetailDistinct = new List<WarehouseRequestDetail>();
                                                for (int i = 0; i < ListWarehouseRequestDetailMaterialID.Count; i++)
                                                {
                                                    var WarehouseRequestDetail = new WarehouseRequestDetail();
                                                    WarehouseRequestDetail.MaterialID = ListWarehouseRequestDetailMaterialID[i];
                                                    WarehouseRequestDetail.MaterialName = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().MaterialName ?? GlobalHelper.InitializationString;
                                                    WarehouseRequestDetail.Quantity = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).Sum(o => o.Quantity);
                                                    WarehouseRequestDetail.CategoryFamilyID = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().CategoryFamilyID ?? GlobalHelper.InitializationNumber;
                                                    WarehouseRequestDetail.CategoryFamilyName = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().CategoryFamilyName ?? GlobalHelper.InitializationString;
                                                    WarehouseRequestDetail.CategoryUnitID = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().CategoryUnitID ?? GlobalHelper.InitializationNumber;
                                                    WarehouseRequestDetail.Display = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().Display ?? GlobalHelper.InitializationString;
                                                    WarehouseRequestDetail.IsSNP = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseRequestDetail.MaterialID).FirstOrDefault().IsSNP ?? false;
                                                    ListWarehouseRequestDetailDistinct.Add(WarehouseRequestDetail);
                                                }
                                                foreach (var item in ListWarehouseRequestDetailDistinct)
                                                {
                                                    if (item.Quantity > 0)
                                                    {
                                                        WarehouseOutputDetail WarehouseOutputDetail = new WarehouseOutputDetail();
                                                        WarehouseOutputDetail.Active = true;
                                                        WarehouseOutputDetail.ParentID = BaseParameter.BaseModel.ID;
                                                        WarehouseOutputDetail.QuantityRequest = item.Quantity;
                                                        WarehouseOutputDetail.CategoryUnitID = item.CategoryUnitID;
                                                        WarehouseOutputDetail.CategoryFamilyID = item.CategoryFamilyID;
                                                        WarehouseOutputDetail.CategoryFamilyName = item.CategoryFamilyName;
                                                        WarehouseOutputDetail.Display = item.Display;
                                                        WarehouseOutputDetail.IsSNP = item.IsSNP;
                                                        WarehouseOutputDetail.Quantity = WarehouseOutputDetail.QuantityRequest;
                                                        WarehouseOutputDetail.MaterialID = item.MaterialID;
                                                        WarehouseOutputDetail.MaterialName = item.MaterialName;
                                                        var WarehouseRequestDetailProductID = ListWarehouseRequestDetail.Where(o => o.MaterialID == WarehouseOutputDetail.MaterialID && o.ProductID > 0).FirstOrDefault();
                                                        if (WarehouseRequestDetailProductID != null && WarehouseRequestDetailProductID.ID > 0)
                                                        {
                                                            //WarehouseOutputDetail.MaterialID = WarehouseRequestDetailProductID.ProductID;
                                                        }
                                                        var BaseParameterWarehouseOutputDetail = new BaseParameter<WarehouseOutputDetail>();
                                                        BaseParameterWarehouseOutputDetail.BaseModel = WarehouseOutputDetail;
                                                        await _WarehouseOutputDetailService.SaveAsync(BaseParameterWarehouseOutputDetail);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            await _WarehouseOutputRepository.UpdateAsync(BaseParameter.BaseModel);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SendMailAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.BaseModel = BaseParameter.BaseModel;
            if (result.BaseModel != null)
            {
                if (result.BaseModel.ID > 0)
                {
                    if (result.BaseModel.MembershipID > 0)
                    {
                        var Membership = await _MembershipRepository.GetByIDAsync(result.BaseModel.MembershipID.Value);
                        if (!string.IsNullOrEmpty(Membership.Email))
                        {
                            string HTMLContent = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseOutputEmail.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLContent = r.ReadToEnd();
                                }
                            }
                            HTMLContent = HTMLContent.Replace(@"[ID]", result.BaseModel.ID.ToString());
                            HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                            if (result.BaseModel.Date == null)
                            {
                                result.BaseModel.Date = GlobalHelper.InitializationDateTime;
                            }
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
                                mail.Subject = result.BaseModel.Code + " - " + result.BaseModel.Date.Value.ToString("dd/MM/yyyy") + " | Output";
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
        public virtual async Task<BaseResult<WarehouseOutput>> SendNotificationAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.BaseModel = BaseParameter.BaseModel;
            if (result.BaseModel != null)
            {
                if (result.BaseModel.ID > 0)
                {
                    if (result.BaseModel.MembershipID > 0)
                    {
                        var Membership = await _MembershipRepository.GetByIDAsync(result.BaseModel.MembershipID.Value);
                        var Notification = new Notification();
                        Notification.ID = result.BaseModel.ID;
                        Notification.Code = result.BaseModel.Code;
                        Notification.Name = result.BaseModel.GetType().Name;
                        Notification.ParentID = Membership.ID;
                        var BaseParameterNotification = new BaseParameter<Notification>();
                        BaseParameterNotification.BaseModel = Notification;
                        await _NotificationService.CreateWarehouseOutputDetailBarcodeFindAsync(BaseParameterNotification);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SendZaloAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            result.BaseModel = BaseParameter.BaseModel;
            if (result.BaseModel != null)
            {
                if (result.BaseModel.ID > 0)
                {
                    if (result.BaseModel.MembershipID > 0)
                    {
                        var Membership = await _MembershipRepository.GetByIDAsync(result.BaseModel.MembershipID.Value);
                        if (Membership.ID > 0 && !string.IsNullOrEmpty(Membership.Phone))
                        {
                            var BaseParameterZaloToken = new BaseParameter<ZaloToken>();
                            var BaseResultZaloToken = await _ZaloTokenService.GetLatestAsync(BaseParameterZaloToken);
                            if (BaseResultZaloToken.BaseModel != null && BaseResultZaloToken.BaseModel.ID > 0)
                            {
                                ZaloToken ZaloToken = BaseResultZaloToken.BaseModel;
                                if (!string.IsNullOrEmpty(ZaloToken.OAAccessToken))
                                {
                                    ZaloZNSDataRequest ZaloZNSDataRequest = new ZaloZNSDataRequest();
                                    if (Membership.Phone.Length == 10)
                                    {
                                        ZaloZNSDataRequest.phone = "84" + Membership.Phone.Substring(1);
                                    }
                                    if (Membership.Phone.Length == 9)
                                    {
                                        ZaloZNSDataRequest.phone = "84" + Membership.Phone;
                                    }
                                    if (!string.IsNullOrEmpty(ZaloZNSDataRequest.phone))
                                    {
                                        ZaloZNSDataRequest.template_id = GlobalHelper.ZaloTemplateID;
                                        ZaloZNSDataRequest.tracking_id = GlobalHelper.InitializationGUICode;
                                        template_data template_data = new template_data();
                                        template_data.membership_code = Membership.Code;
                                        template_data.membership_name = Membership.Name;
                                        template_data.output_id = result.BaseModel.ID.ToString();
                                        template_data.output_code = result.BaseModel.Code;
                                        template_data.warehouse_id = template_data.output_id;
                                        if (result.BaseModel.Date == null)
                                        {
                                            template_data.output_date = GlobalHelper.InitializationString;
                                        }
                                        else
                                        {
                                            template_data.output_date = result.BaseModel.Date.Value.ToString("HH:mm:ss dd/MM/yyyy");
                                        }
                                        if (!string.IsNullOrEmpty(template_data.membership_code) && template_data.membership_code.Length > 30)
                                        {
                                            template_data.membership_code = template_data.membership_code.Substring(0, 30);
                                        }
                                        if (!string.IsNullOrEmpty(template_data.membership_name) && template_data.membership_name.Length > 30)
                                        {
                                            template_data.membership_name = template_data.membership_name.Substring(0, 30);
                                        }
                                        if (!string.IsNullOrEmpty(template_data.output_id) && template_data.output_id.Length > 30)
                                        {
                                            template_data.output_id = template_data.output_id.Substring(0, 30);
                                        }
                                        if (!string.IsNullOrEmpty(template_data.output_code) && template_data.output_code.Length > 30)
                                        {
                                            template_data.output_code = template_data.output_code.Substring(0, 30);
                                        }
                                        if (!string.IsNullOrEmpty(template_data.warehouse_id) && template_data.warehouse_id.Length > 30)
                                        {
                                            template_data.warehouse_id = template_data.warehouse_id.Substring(0, 30);
                                        }
                                        if (!string.IsNullOrEmpty(template_data.output_date) && template_data.output_date.Length > 30)
                                        {
                                            template_data.output_date = template_data.output_date.Substring(0, 30);
                                        }
                                        ZaloZNSDataRequest.template_data = template_data;
                                        await ZaloHelper.ZNSSendAsync(ZaloToken.OAAccessToken, ZaloZNSDataRequest);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> CreateAutoAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();

            //var ListWarehouseInputDetailBarcodeSave = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8961).ToListAsync();
            //if (ListWarehouseInputDetailBarcodeSave.Count > 0)
            //{
            //    var ListWarehouseInputDetailBarcodeSaveBarcode = ListWarehouseInputDetailBarcodeSave.Select(o => o.Barcode).Distinct().ToList();
            //    var ListWarehouseInputDetailBarcodeDJM = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == 188 && !string.IsNullOrEmpty(o.Barcode) && ListWarehouseInputDetailBarcodeSaveBarcode.Contains(o.Barcode)).ToListAsync();

            //    var ListWarehouseInputDetailBarcodeUpdate = new List<WarehouseInputDetailBarcode>();
            //    foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcodeSave)
            //    {
            //        var WarehouseInputDetailBarcodeDJM = ListWarehouseInputDetailBarcodeDJM.Where(o => o.Barcode == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
            //        if (WarehouseInputDetailBarcodeDJM != null && WarehouseInputDetailBarcodeDJM.ID > 0)
            //        {
            //            WarehouseInputDetailBarcode.DateScan = WarehouseInputDetailBarcodeDJM.DateScan ?? GlobalHelper.InitializationDateTime;
            //            ListWarehouseInputDetailBarcodeUpdate.Add(WarehouseInputDetailBarcode);
            //        }
            //    }
            //    await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeUpdate);
            //}

            var ListWarehouseInputDetailBarcodeSave = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8961).ToListAsync();
            foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcodeSave)
            {
                BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
            }

            //var ListWarehouseInputDetailTotal = await _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == 8961).ToListAsync();
            //await _WarehouseInputDetailRepository.RemoveRangeAsync(ListWarehouseInputDetailTotal);
            //var ListWarehouseInputDetailBarcodeTotal = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8961).ToListAsync();
            //await _WarehouseInputDetailBarcodeRepository.RemoveRangeAsync(ListWarehouseInputDetailBarcodeTotal);

            //var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //var ListWarehouseInputDetail = new List<WarehouseInputDetail>();
            //foreach (var WarehouseOutputDetail in ListWarehouseOutputDetail)
            //{
            //    var WarehouseInputDetail = new WarehouseInputDetail();
            //    WarehouseInputDetail.ParentID = 8961;
            //    WarehouseInputDetail.CategoryDepartmentID = 23;
            //    WarehouseInputDetail.Active = true;
            //    WarehouseInputDetail.MaterialID = WarehouseOutputDetail.MaterialID;
            //    WarehouseInputDetail.MaterialName = WarehouseOutputDetail.MaterialName;
            //    WarehouseInputDetail.Quantity = WarehouseOutputDetail.Quantity;
            //    WarehouseInputDetail.QuantityActual = WarehouseOutputDetail.QuantityActual;
            //    WarehouseInputDetail.QuantityGAP = WarehouseOutputDetail.QuantityGAP;
            //    ListWarehouseInputDetail.Add(WarehouseInputDetail);
            //}
            //await _WarehouseInputDetailRepository.AddRangeAsync(ListWarehouseInputDetail);

            //ListWarehouseInputDetailTotal = await _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == 8961).ToListAsync();

            //var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
            //foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
            //{
            //    var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
            //    WarehouseInputDetailBarcode.ParentID = 8961;
            //    WarehouseInputDetailBarcode.CategoryDepartmentID = 23;
            //    WarehouseInputDetailBarcode.Active = true;
            //    WarehouseInputDetailBarcode.MaterialID = WarehouseOutputDetailBarcode.MaterialID;
            //    WarehouseInputDetailBarcode.MaterialName = WarehouseOutputDetailBarcode.MaterialName;
            //    WarehouseInputDetailBarcode.Quantity = WarehouseOutputDetailBarcode.Quantity;
            //    WarehouseInputDetailBarcode.QuantityOutput = 0;
            //    WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity;
            //    WarehouseInputDetailBarcode.Barcode = WarehouseOutputDetailBarcode.Barcode;
            //    WarehouseInputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
            //    var WarehouseInputDetail = ListWarehouseInputDetailTotal.Where(o => o.MaterialID == WarehouseInputDetailBarcode.MaterialID).FirstOrDefault();
            //    if (WarehouseInputDetail != null && WarehouseInputDetail.ID > 0)
            //    {
            //        WarehouseInputDetailBarcode.WarehouseInputDetailID = WarehouseInputDetail.ID;
            //    }
            //    ListWarehouseInputDetailBarcode.Add(WarehouseInputDetailBarcode);
            //}
            //await _WarehouseInputDetailBarcodeRepository.AddRangeAsync(ListWarehouseInputDetailBarcode);


            //var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();

            //var ListWarehouseOutputDetailBarcodeTotal = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == 188 && o.Active == true).ToListAsync();
            //var ListWarehouseInputDetailBarcodeTotal = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == 188 && o.Active == true).ToListAsync();

            //var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //var ListWarehouseInputDetailBarcodeAdd = new List<WarehouseInputDetailBarcode>();
            //foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
            //{
            //    var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcodeTotal.Where(o => o.Barcode == WarehouseOutputDetailBarcode.Barcode).FirstOrDefault();
            //    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
            //    {
            //        var ListWarehouseOutputDetailBarcodeSub = ListWarehouseOutputDetailBarcodeTotal.Where(o => o.Barcode == WarehouseInputDetailBarcode.Barcode).ToList();
            //        WarehouseInputDetailBarcode.QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity);
            //        WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutput;
            //        ListWarehouseInputDetailBarcodeAdd.Add(WarehouseInputDetailBarcode);
            //    }
            //}
            //await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeAdd);

            //await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
            //var ListWarehouseRequestDetail = await _WarehouseRequestDetailRepository.GetByCondition(o => o.ParentID == 8658).ToListAsync();
            //if (ListWarehouseRequestDetail.Count > 0)
            //{
            //    var ListWarehouseRequestDetailMaterialID = ListWarehouseRequestDetail.Select(o => o.MaterialID).Distinct().ToList();

            //    var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //    await _WarehouseOutputDetailRepository.RemoveRangeAsync(ListWarehouseOutputDetail);
            //    var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //    await _WarehouseOutputDetailBarcodeRepository.RemoveRangeAsync(ListWarehouseOutputDetailBarcode);

            //    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeService.GetByCondition(o => o.CategoryDepartmentID == 188 && o.ParentID > 0 && o.Active == true && o.QuantityInventory > 0 && ListWarehouseRequestDetailMaterialID.Contains(o.MaterialID ?? 0)).ToListAsync();
            //    ListWarehouseOutputDetail = new List<WarehouseOutputDetail>();
            //    foreach (var MaterialID in ListWarehouseRequestDetailMaterialID)
            //    {
            //        var WarehouseOutputDetail = new WarehouseOutputDetail();
            //        WarehouseOutputDetail.ParentID = 8311;
            //        WarehouseOutputDetail.Active = true;
            //        WarehouseOutputDetail.MaterialID = MaterialID;
            //        WarehouseOutputDetail.Quantity = ListWarehouseInputDetailBarcode.Where(o => o.MaterialID == MaterialID).Sum(o => o.QuantityInventory);
            //        WarehouseOutputDetail.QuantityActual = WarehouseOutputDetail.Quantity;
            //        WarehouseOutputDetail.QuantityGAP = 0;
            //        var ListWarehouseRequestDetailSub = ListWarehouseRequestDetail.Where(o => o.MaterialID == MaterialID).ToList();
            //        if (ListWarehouseRequestDetailSub.Count > 0)
            //        {
            //            WarehouseOutputDetail.MaterialName = ListWarehouseRequestDetailSub[0].MaterialName;
            //        }
            //        ListWarehouseOutputDetail.Add(WarehouseOutputDetail);

            //    }
            //    await _WarehouseOutputDetailRepository.AddRangeAsync(ListWarehouseOutputDetail);

            //    ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
            //    foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
            //    {
            //        var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
            //        WarehouseOutputDetailBarcode.ParentID = 8311;
            //        WarehouseOutputDetailBarcode.CategoryDepartmentID = 188;
            //        WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
            //        WarehouseOutputDetailBarcode.Date = WarehouseOutputDetailBarcode.DateScan;
            //        WarehouseOutputDetailBarcode.DateInput = WarehouseInputDetailBarcode.DateScan;
            //        WarehouseOutputDetailBarcode.Active = WarehouseInputDetailBarcode.Active;
            //        WarehouseOutputDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
            //        WarehouseOutputDetailBarcode.MaterialID = WarehouseInputDetailBarcode.MaterialID;
            //        WarehouseOutputDetailBarcode.MaterialName = WarehouseInputDetailBarcode.MaterialName;
            //        WarehouseOutputDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantityInventory;
            //        var WarehouseOutputDetail = ListWarehouseOutputDetail.Where(o => o.ParentID == WarehouseOutputDetailBarcode.ParentID && o.MaterialID == WarehouseOutputDetailBarcode.MaterialID).FirstOrDefault();
            //        if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
            //        {
            //            WarehouseOutputDetailBarcode.WarehouseOutputDetailID = WarehouseOutputDetail.ID;
            //        }
            //        ListWarehouseOutputDetailBarcode.Add(WarehouseOutputDetailBarcode);
            //    }
            //    await _WarehouseOutputDetailBarcodeRepository.AddRangeAsync(ListWarehouseOutputDetailBarcode);


            //    ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 8311).ToListAsync();
            //    var ListWarehouseInputDetailBarcodeAdd = new List<WarehouseInputDetailBarcode>();
            //    foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
            //    {
            //        var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == WarehouseOutputDetailBarcode.CategoryDepartmentID && o.Barcode == WarehouseOutputDetailBarcode.Barcode && o.Active == true).FirstOrDefaultAsync();
            //        if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
            //        {
            //            var ListWarehouseOutputDetailBarcodeSub = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == WarehouseInputDetailBarcode.CategoryDepartmentID && o.Barcode == WarehouseInputDetailBarcode.Barcode && o.Active == true).ToListAsync();
            //            WarehouseInputDetailBarcode.QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity);
            //            WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutput;
            //            ListWarehouseInputDetailBarcodeAdd.Add(WarehouseInputDetailBarcode);
            //        }
            //    }
            //    await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeAdd);
            //}
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            else
            {
                if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();

                    switch (BaseParameter.Action)
                    {
                        case 1:
                            result.List = result.List.Where(o => o.Active == true && o.IsComplete != true).ToList();
                            break;
                        case 2:
                            result.List = result.List.Where(o => o.IsComplete == true).ToList();
                            break;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();

            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            else
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    result.List = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();

                    switch (BaseParameter.Action)
                    {
                        case 1:
                            result.List = result.List.Where(o => o.Active == true && o.IsComplete != true).ToList();
                            break;
                        case 2:
                            result.List = result.List.Where(o => o.Active == true && o.IsComplete == true).ToList();
                            break;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_MembershipToListAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();

            if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
            {
                result.List = await GetByCondition(o => o.Active == true && o.IsComplete != true && o.MembershipID > 0 && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
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
                        var ListWarehouseOutput = await GetByCondition(o => o.CompanyID == CompanyID && o.SupplierID == HOOKRACKID && o.CustomerID == FAID).OrderBy(o => o.Date).ToListAsync();
                        if (ListWarehouseOutput.Count > 0)
                        {
                            var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                            var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeService.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.Barcode == Barcode).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                            if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                            {
                                WarehouseOutputDetailBarcode.UpdateUserID = Membership.ID;
                                WarehouseOutputDetailBarcode.UpdateUserName = UserName;
                                WarehouseOutputDetailBarcode.Quantity = 0;
                                BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncOutputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
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
                        string Code = "HOOKRACK-FA-" + DateTime.Now.ToString("yyyyMMdd");
                        var WarehouseOutput = await GetByCondition(o => o.CompanyID == CompanyID && o.Code == Code).FirstOrDefaultAsync();
                        if (WarehouseOutput == null)
                        {
                            WarehouseOutput = new WarehouseOutput();
                            WarehouseOutput.CreateUserID = Membership.ID;
                            WarehouseOutput.CreateUserCode = UserName;
                            WarehouseOutput.Active = true;
                            WarehouseOutput.CompanyID = CompanyID;
                            WarehouseOutput.Code = Code;
                            WarehouseOutput.SupplierID = HOOKRACKID;
                            WarehouseOutput.CustomerID = FAID;
                            WarehouseOutput.Date = DateTime.Now;
                            //BaseParameter<WarehouseOutput> BaseParameterWarehouseOutput = new BaseParameter<WarehouseOutput>();
                            //BaseParameterWarehouseOutput.BaseModel = WarehouseOutput;
                            //await SaveAsync(BaseParameterWarehouseOutput);

                            await _WarehouseOutputRepository.AddAsync(WarehouseOutput);
                        }
                        if (WarehouseOutput.ID > 0)
                        {
                            WarehouseOutputDetailBarcode WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                            WarehouseOutputDetailBarcode.CreateUserID = Membership.ID;
                            WarehouseOutputDetailBarcode.CreateUserCode = UserName;
                            WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                            WarehouseOutputDetailBarcode.Code = Code;
                            WarehouseOutputDetailBarcode.Active = true;
                            WarehouseOutputDetailBarcode.Barcode = Barcode;
                            WarehouseOutputDetailBarcode.MaterialName = trackmtim.LEAD_NM;
                            WarehouseOutputDetailBarcode.DateScan = DateTime.Now;
                            WarehouseOutputDetailBarcode.Quantity = (decimal?)trackmtim.QTY ?? 0;
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
                                WarehouseOutputDetailBarcode.CategoryLocationName = Listtrackmaster[0].HOOK_RACK;
                            }
                            BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                            BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                            await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);

                            if (BaseParameterWarehouseOutputDetailBarcode.BaseModel.ID > 0)
                            {
                                var WarehouseInput = await _WarehouseInputService.GetByCondition(o => o.CompanyID == CompanyID && o.Code == Code).FirstOrDefaultAsync();
                                if (WarehouseInput == null)
                                {
                                    WarehouseInput = new WarehouseInput();
                                    WarehouseInput.CreateUserID = Membership.ID;
                                    WarehouseInput.CreateUserCode = UserName;
                                    WarehouseInput.WarehouseOutputID = WarehouseOutput.ID;
                                    WarehouseInput.Active = true;
                                    WarehouseInput.CompanyID = CompanyID;
                                    WarehouseInput.Code = Code;
                                    WarehouseInput.SupplierID = HOOKRACKID;
                                    WarehouseInput.CustomerID = FAID;
                                    WarehouseInput.Date = DateTime.Now;
                                    //BaseParameter<WarehouseInput> BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                                    //BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                                    //await _WarehouseInputService.SaveAsync(BaseParameterWarehouseInput);

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
                                    WarehouseInputDetailBarcode.Barcode = BaseParameterWarehouseOutputDetailBarcode.BaseModel.Barcode;
                                    WarehouseInputDetailBarcode.MaterialName = BaseParameterWarehouseOutputDetailBarcode.BaseModel.MaterialName;
                                    WarehouseInputDetailBarcode.DateScan = BaseParameterWarehouseOutputDetailBarcode.BaseModel.DateScan;
                                    WarehouseInputDetailBarcode.Quantity = BaseParameterWarehouseOutputDetailBarcode.BaseModel.Quantity;
                                    WarehouseInputDetailBarcode.CategoryLocationName = BaseParameterWarehouseOutputDetailBarcode.BaseModel.CategoryLocationName;

                                    BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
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
        public virtual async Task<BaseResult<WarehouseOutput>> SyncOutputInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
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
                        string BarcodeOld = BaseParameter.SearchString;
                        string Code = "FA-HOOKRACK-" + DateTime.Now.ToString("yyyyMMdd");
                        var WarehouseOutput = await GetByCondition(o => o.CompanyID == CompanyID && o.Code == Code).FirstOrDefaultAsync();
                        if (WarehouseOutput == null)
                        {
                            WarehouseOutput = new WarehouseOutput();
                            WarehouseOutput.CreateUserID = Membership.ID;
                            WarehouseOutput.CreateUserCode = UserName;
                            WarehouseOutput.Active = true;
                            WarehouseOutput.CompanyID = CompanyID;
                            WarehouseOutput.Code = Code;
                            WarehouseOutput.SupplierID = FAID;
                            WarehouseOutput.CustomerID = HOOKRACKID;
                            WarehouseOutput.Date = DateTime.Now;
                            //BaseParameter<WarehouseOutput> BaseParameterWarehouseOutput = new BaseParameter<WarehouseOutput>();
                            //BaseParameterWarehouseOutput.BaseModel = WarehouseOutput;
                            //await SaveAsync(BaseParameterWarehouseOutput);

                            await _WarehouseOutputRepository.AddAsync(WarehouseOutput);
                        }
                        if (WarehouseOutput.ID > 0)
                        {
                            WarehouseOutputDetailBarcode WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                            WarehouseOutputDetailBarcode.CreateUserID = Membership.ID;
                            WarehouseOutputDetailBarcode.CreateUserCode = UserName;
                            WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                            WarehouseOutputDetailBarcode.Code = Code;
                            WarehouseOutputDetailBarcode.Active = true;
                            WarehouseOutputDetailBarcode.Description = BarcodeOld;
                            WarehouseOutputDetailBarcode.Barcode = Barcode;
                            WarehouseOutputDetailBarcode.MaterialName = trackmtim.LEAD_NM;
                            WarehouseOutputDetailBarcode.DateScan = trackmtim.RACKDTM ?? DateTime.Now;
                            WarehouseOutputDetailBarcode.Quantity = (decimal?)trackmtim.QTY ?? 0;
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
                                WarehouseOutputDetailBarcode.CategoryLocationName = Listtrackmaster[0].HOOK_RACK;
                            }
                            BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                            BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                            await _WarehouseOutputDetailBarcodeService.SaveAsync(BaseParameterWarehouseOutputDetailBarcode);

                            if (BaseParameterWarehouseOutputDetailBarcode.BaseModel.ID > 0)
                            {
                                var WarehouseInput = await _WarehouseInputService.GetByCondition(o => o.CompanyID == CompanyID && o.Code == Code).FirstOrDefaultAsync();
                                if (WarehouseInput == null)
                                {
                                    WarehouseInput = new WarehouseInput();
                                    WarehouseInput.CreateUserID = Membership.ID;
                                    WarehouseInput.CreateUserCode = UserName;
                                    WarehouseInput.WarehouseOutputID = WarehouseOutput.ID;
                                    WarehouseInput.Active = true;
                                    WarehouseInput.CompanyID = CompanyID;
                                    WarehouseInput.Code = Code;
                                    WarehouseInput.SupplierID = FAID;
                                    WarehouseInput.CustomerID = HOOKRACKID;
                                    WarehouseInput.Date = DateTime.Now;
                                    //BaseParameter<WarehouseInput> BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                                    //BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                                    //await _WarehouseInputService.SaveAsync(BaseParameterWarehouseInput);

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
                                    WarehouseInputDetailBarcode.Description = BaseParameterWarehouseOutputDetailBarcode.BaseModel.Description;
                                    WarehouseInputDetailBarcode.Barcode = BaseParameterWarehouseOutputDetailBarcode.BaseModel.Barcode;
                                    WarehouseInputDetailBarcode.MaterialName = BaseParameterWarehouseOutputDetailBarcode.BaseModel.MaterialName;
                                    WarehouseInputDetailBarcode.DateScan = BaseParameterWarehouseOutputDetailBarcode.BaseModel.DateScan;
                                    WarehouseInputDetailBarcode.Quantity = BaseParameterWarehouseOutputDetailBarcode.BaseModel.Quantity;
                                    WarehouseInputDetailBarcode.CategoryLocationName = BaseParameterWarehouseOutputDetailBarcode.BaseModel.CategoryLocationName;

                                    BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
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
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 0 && o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.MESID > 0).ToListAsync();
                if (ListWarehouseOutputDetailBarcode.Count > 0)
                {
                    var ListMembership = await _MembershipRepository.GetByCompanyIDToListAsync(BaseParameter.CompanyID.Value);
                    var ListWarehouseOutputDetailBarcodeCode = ListWarehouseOutputDetailBarcode.Select(o => o.Code).Distinct().ToList();
                    foreach (var Code in ListWarehouseOutputDetailBarcodeCode)
                    {
                        var ListWarehouseOutputDetailBarcodeUpdate = ListWarehouseOutputDetailBarcode.Where(o => o.Code == Code).OrderBy(o => o.DateScan).ToList();
                        if (ListWarehouseOutputDetailBarcodeUpdate.Count > 0)
                        {
                            var WarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcodeUpdate[0];
                            if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                            {
                                var WarehouseOutput = new WarehouseOutput();
                                WarehouseOutput.Active = true;
                                WarehouseOutput.Code = Code;
                                WarehouseOutput.CompanyID = WarehouseOutputDetailBarcode.CompanyID;
                                WarehouseOutput.SupplierID = WarehouseOutputDetailBarcode.SupplierID;
                                WarehouseOutput.CustomerID = WarehouseOutputDetailBarcode.CustomerID;
                                WarehouseOutput.Date = WarehouseOutputDetailBarcode.DateScan;
                                WarehouseOutput.CreateUserCode = WarehouseOutputDetailBarcode.UpdateUserCode;
                                var Membership = ListMembership.Where(o => o.UserName == WarehouseOutput.CreateUserCode).FirstOrDefault();
                                if (Membership != null && Membership.ID > 0)
                                {
                                    WarehouseOutput.CreateUserID = Membership.ID;
                                }
                                BaseParameter<WarehouseOutput> BaseParameterWarehouseOutput = new BaseParameter<WarehouseOutput>();
                                BaseParameterWarehouseOutput.BaseModel = WarehouseOutput;
                                await SaveAsync(BaseParameterWarehouseOutput);
                                if (WarehouseOutput.ID > 0)
                                {
                                    for (int i = 0; i < ListWarehouseOutputDetailBarcodeUpdate.Count; i++)
                                    {
                                        ListWarehouseOutputDetailBarcodeUpdate[i].ParentID = WarehouseOutput.ID;
                                        if (Membership != null && Membership.ID > 0)
                                        {
                                            ListWarehouseOutputDetailBarcodeUpdate[i].CreateUserID = Membership.ID;
                                            ListWarehouseOutputDetailBarcodeUpdate[i].UpdateUserID = Membership.ID;
                                            ListWarehouseOutputDetailBarcodeUpdate[i].CreateUserCode = Membership.UserName;
                                            ListWarehouseOutputDetailBarcodeUpdate[i].UpdateUserCode = Membership.UserName;
                                            ListWarehouseOutputDetailBarcodeUpdate[i].CreateUserName = Membership.Name;
                                            ListWarehouseOutputDetailBarcodeUpdate[i].UpdateUserName = Membership.Name;
                                        }
                                    }
                                    await _WarehouseOutputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseOutputDetailBarcodeUpdate);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Action > 0 && BaseParameter.ID > 0)
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
                        var Description = "MES";
                        var Name = "HookRack";
                        var CategoryDepartment = await _CategoryDepartmentRepository.GetByIDAsync(BaseParameter.CategoryDepartmentID.Value);
                        if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                        {
                            switch (BaseParameter.Action)
                            {
                                case 1:
                                    WarehouseInput WarehouseInput = new WarehouseInput();
                                    WarehouseInput.CompanyID = BaseParameter.CompanyID;
                                    WarehouseInput.Description = Description;
                                    WarehouseInput.Name = Name;
                                    WarehouseInput.Date = GlobalHelper.InitializationDateTime;
                                    WarehouseInput.Code = CategoryDepartment.Code + "ToHookRack-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                                    WarehouseInput.SupplierID = BaseParameter.CategoryDepartmentID;
                                    WarehouseInput.CreateUserCode = trackmtim.UpdateUserCode;
                                    WarehouseInput.UpdateUserCode = trackmtim.UpdateUserCode;
                                    switch (BaseParameter.CompanyID)
                                    {
                                        case 16:
                                            WarehouseInput.CustomerID = 205;
                                            break;
                                        case 17:
                                            WarehouseInput.CustomerID = 206;
                                            break;
                                    }
                                    BaseParameter<WarehouseInput> BaseParameterWarehouseInputSave = new BaseParameter<WarehouseInput>();
                                    BaseParameterWarehouseInputSave.BaseModel = WarehouseInput;
                                    await _WarehouseInputService.SaveHookRackAsync(BaseParameterWarehouseInputSave);
                                    if (WarehouseInput.ID > 0)
                                    {
                                        var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                        WarehouseInputDetailBarcode.Description = "HookRack";
                                        WarehouseInputDetailBarcode.ParentID = WarehouseInput.ID;
                                        WarehouseInputDetailBarcode.ParentName = WarehouseInput.Code;
                                        WarehouseInputDetailBarcode.SupplierID = WarehouseInput.SupplierID;
                                        WarehouseInputDetailBarcode.CustomerID = WarehouseInput.CustomerID;
                                        WarehouseInputDetailBarcode.CategoryDepartmentID = WarehouseInput.CustomerID;
                                        WarehouseInputDetailBarcode.Active = true;
                                        WarehouseInputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                                        WarehouseInputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                                        WarehouseInputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                        WarehouseInputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                                        WarehouseInputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseInputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseInputDetailBarcode.Quantity = (decimal)trackmtim.QTY;

                                        BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                        BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                        await _WarehouseInputDetailBarcodeService.SaveHookRackAsync(BaseParameterWarehouseInputDetailBarcode);
                                    }
                                    break;
                                case 2:
                                    WarehouseOutput WarehouseOutput = new WarehouseOutput();
                                    WarehouseOutput.CompanyID = BaseParameter.CompanyID;
                                    WarehouseOutput.Description = Description;
                                    WarehouseOutput.Name = Name;
                                    WarehouseOutput.Date = GlobalHelper.InitializationDateTime;
                                    WarehouseOutput.Code = "HookRackTo" + CategoryDepartment.Code + "-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                                    WarehouseOutput.CustomerID = BaseParameter.CategoryDepartmentID;
                                    WarehouseOutput.CreateUserCode = trackmtim.UpdateUserCode;
                                    WarehouseOutput.UpdateUserCode = trackmtim.UpdateUserCode;
                                    switch (BaseParameter.CompanyID)
                                    {
                                        case 16:
                                            WarehouseOutput.SupplierID = 205;
                                            break;
                                        case 17:
                                            WarehouseOutput.SupplierID = 206;
                                            break;
                                    }
                                    BaseParameter<WarehouseOutput> BaseParameterWarehouseOutputSave = new BaseParameter<WarehouseOutput>();
                                    BaseParameterWarehouseOutputSave.BaseModel = WarehouseOutput;
                                    await SaveHookRackAsync(BaseParameterWarehouseOutputSave);
                                    if (WarehouseOutput.ID > 0)
                                    {
                                        var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();

                                        WarehouseOutputDetailBarcode.Description = "HookRack";
                                        WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                                        WarehouseOutputDetailBarcode.ParentName = WarehouseOutput.Code;
                                        WarehouseOutputDetailBarcode.SupplierID = WarehouseOutput.SupplierID;
                                        WarehouseOutputDetailBarcode.CustomerID = WarehouseOutput.CustomerID;
                                        WarehouseOutputDetailBarcode.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                        WarehouseOutputDetailBarcode.Active = true;
                                        WarehouseOutputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                                        WarehouseOutputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                        WarehouseOutputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                                        WarehouseOutputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                                        WarehouseOutputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseOutputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseOutputDetailBarcode.Quantity = (decimal)trackmtim.QTY;

                                        BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                        BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                        await _WarehouseOutputDetailBarcodeService.SaveHookRackAsync(BaseParameterWarehouseOutputDetailBarcode);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync(BaseParameter<WarehouseOutput> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Action > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                List<trackmtim> Listtrackmtim = new List<trackmtim>();
                string sql = @"select * from trackmtim WHERE TRACK_IDX in (" + BaseParameter.SearchString + ")";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var trackmtim0 = Listtrackmtim[0];
                    var Description = "MES";
                    var Name = "HookRack";
                    var CategoryDepartment = await _CategoryDepartmentRepository.GetByIDAsync(BaseParameter.CategoryDepartmentID.Value);
                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                    {
                        switch (BaseParameter.Action)
                        {
                            case 1:
                                WarehouseInput WarehouseInput = new WarehouseInput();
                                WarehouseInput.CompanyID = BaseParameter.CompanyID;
                                WarehouseInput.Description = Description;
                                WarehouseInput.Name = Name;
                                WarehouseInput.Date = GlobalHelper.InitializationDateTime;
                                WarehouseInput.Code = CategoryDepartment.Code + "ToHookRack-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                                WarehouseInput.SupplierID = BaseParameter.CategoryDepartmentID;
                                WarehouseInput.CreateUserCode = trackmtim0.UpdateUserCode;
                                WarehouseInput.UpdateUserCode = trackmtim0.UpdateUserCode;
                                switch (BaseParameter.CompanyID)
                                {
                                    case 16:
                                        WarehouseInput.CustomerID = 205;
                                        break;
                                    case 17:
                                        WarehouseInput.CustomerID = 206;
                                        break;
                                }
                                BaseParameter<WarehouseInput> BaseParameterWarehouseInputSave = new BaseParameter<WarehouseInput>();
                                BaseParameterWarehouseInputSave.BaseModel = WarehouseInput;
                                await _WarehouseInputService.SaveHookRackAsync(BaseParameterWarehouseInputSave);
                                if (WarehouseInput.ID > 0)
                                {
                                    foreach (var trackmtim in Listtrackmtim)
                                    {
                                        var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                        WarehouseInputDetailBarcode.Description = "HookRack";
                                        WarehouseInputDetailBarcode.ParentID = WarehouseInput.ID;
                                        WarehouseInputDetailBarcode.ParentName = WarehouseInput.Code;
                                        WarehouseInputDetailBarcode.SupplierID = WarehouseInput.SupplierID;
                                        WarehouseInputDetailBarcode.CustomerID = WarehouseInput.CustomerID;
                                        WarehouseInputDetailBarcode.CategoryDepartmentID = WarehouseInput.CustomerID;
                                        WarehouseInputDetailBarcode.Active = true;
                                        WarehouseInputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                                        WarehouseInputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                                        WarehouseInputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                        WarehouseInputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                                        WarehouseInputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseInputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseInputDetailBarcode.Quantity = (decimal)trackmtim.QTY;
                                        BaseParameter<WarehouseInputDetailBarcode> BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                        BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                        await _WarehouseInputDetailBarcodeService.SaveHookRackAsync(BaseParameterWarehouseInputDetailBarcode);
                                    }
                                }
                                break;
                            case 2:
                                WarehouseOutput WarehouseOutput = new WarehouseOutput();
                                WarehouseOutput.CompanyID = BaseParameter.CompanyID;
                                WarehouseOutput.Description = Description;
                                WarehouseOutput.Name = Name;
                                WarehouseOutput.Date = GlobalHelper.InitializationDateTime;
                                WarehouseOutput.Code = "HookRackTo" + CategoryDepartment.Code + "-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                                WarehouseOutput.CustomerID = BaseParameter.CategoryDepartmentID;
                                WarehouseOutput.CreateUserCode = trackmtim0.UpdateUserCode;
                                WarehouseOutput.UpdateUserCode = trackmtim0.UpdateUserCode;
                                switch (BaseParameter.CompanyID)
                                {
                                    case 16:
                                        WarehouseOutput.SupplierID = 205;
                                        break;
                                    case 17:
                                        WarehouseOutput.SupplierID = 206;
                                        break;
                                }
                                BaseParameter<WarehouseOutput> BaseParameterWarehouseOutputSave = new BaseParameter<WarehouseOutput>();
                                BaseParameterWarehouseOutputSave.BaseModel = WarehouseOutput;
                                await SaveHookRackAsync(BaseParameterWarehouseOutputSave);
                                if (WarehouseOutput.ID > 0)
                                {
                                    foreach (var trackmtim in Listtrackmtim)
                                    {
                                        var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                                        WarehouseOutputDetailBarcode.Description = "HookRack";
                                        WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                                        WarehouseOutputDetailBarcode.ParentName = WarehouseOutput.Code;
                                        WarehouseOutputDetailBarcode.SupplierID = WarehouseOutput.SupplierID;
                                        WarehouseOutputDetailBarcode.CustomerID = WarehouseOutput.CustomerID;
                                        WarehouseOutputDetailBarcode.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                        WarehouseOutputDetailBarcode.Active = true;
                                        WarehouseOutputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                                        WarehouseOutputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                        WarehouseOutputDetailBarcode.DateScan = trackmtim.UpdateDate ?? GlobalHelper.InitializationDateTime;
                                        WarehouseOutputDetailBarcode.Barcode = trackmtim.BARCODE_NM;
                                        WarehouseOutputDetailBarcode.CreateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseOutputDetailBarcode.UpdateUserCode = trackmtim.UpdateUserCode;
                                        WarehouseOutputDetailBarcode.Quantity = (decimal)trackmtim.QTY;
                                        BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                        BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                                        await _WarehouseOutputDetailBarcodeService.SaveHookRackAsync(BaseParameterWarehouseOutputDetailBarcode);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}

