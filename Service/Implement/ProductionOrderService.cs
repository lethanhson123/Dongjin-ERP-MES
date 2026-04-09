namespace Service.Implement
{
    public class ProductionOrderService : BaseService<ProductionOrder, IProductionOrderRepository>
    , IProductionOrderService
    {
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;

        private readonly IMembershipCompanyRepository _MembershipCompanyRepository;
        public ProductionOrderService(IProductionOrderRepository ProductionOrderRepository

            , ICompanyRepository companyRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IMembershipCompanyRepository MembershipCompanyRepository

            ) : base(ProductionOrderRepository)
        {
            _ProductionOrderRepository = ProductionOrderRepository;
            _CompanyRepository = companyRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _MembershipCompanyRepository = MembershipCompanyRepository;
        }
        public override void InitializationSave(ProductionOrder model)
        {
            BaseInitialization(model);
            model.Name = model.Name ?? "1.0";
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Year ?? model.Date.Value.Year;
            model.Month = model.Month ?? model.Date.Value.Month;
            model.Day = model.Day ?? model.Date.Value.Day;
            //model.SupplierID = model.SupplierID ?? GlobalHelper.CompanyID;
            //model.CustomerID = model.CustomerID ?? GlobalHelper.CompanyID;
            model.CompanyID = model.CustomerID ?? GlobalHelper.CompanyID;
            if (model.CompanyID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CompanyID.Value);
                model.CompanyName = Company.Name;
            }
            if (model.SupplierID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = Company.Name;
            }
            if (model.CustomerID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = Company.Name;
            }
            if (string.IsNullOrEmpty(model.Code))
            {
                model.Code = model.SupplierName + "-" + model.Date.Value.ToString("yyyyMMdd") + "-" + model.Date.Value.Ticks.ToString();
            }
        }
        public override ProductionOrder SetModelByModelCheck(ProductionOrder Model, ProductionOrder ModelCheck)
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
                    Model.DateEnd = Model.DateEnd ?? ModelCheck.DateEnd;
                    Model.SupplierID = Model.SupplierID ?? ModelCheck.SupplierID;
                    Model.CustomerID = Model.CustomerID ?? ModelCheck.CustomerID;
                    Model.ParentID = Model.ParentID ?? ModelCheck.ParentID;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public override async Task<BaseResult<ProductionOrder>> SaveAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<ProductionOrder>> SyncAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                }
            }
            return result;
        }
        public override async Task<BaseResult<ProductionOrder>> RemoveAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            if (BaseParameter.ID > 0)
            {
                var ListWarehouseInput = await _WarehouseInputRepository.GetByParentIDToListAsync(BaseParameter.ID);
                var ListWarehouseOutput = await _WarehouseOutputRepository.GetByParentIDToListAsync(BaseParameter.ID);
                if (ListWarehouseInput.Count == 0 && ListWarehouseOutput.Count == 0)
                {
                    result.Count = await _ProductionOrderRepository.RemoveAsync(BaseParameter.ID);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrder>> GetByActive_IsCompleteToListAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            result.List = await GetByCondition(o => o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrder>> GetByMembershipIDToListAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipCompanyID.Contains(o.CustomerID.Value)).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrder>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    if (BaseParameter.IsComplete == true)
                    {
                        result.List = await GetByCondition(o => o.CustomerID > 0 && ListMembershipCompanyID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete == BaseParameter.IsComplete).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CustomerID > 0 && ListMembershipCompanyID.Contains(o.CustomerID.Value) && o.Active == BaseParameter.Active && o.IsComplete != true).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrder>> GetByCompanyID_Year_Month_ActionToListAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            result.List = new List<ProductionOrder>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.Year == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    }
                    else
                    {
                        if (BaseParameter.Month == 0)
                        {
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Date != null && o.Date.Value.Year == BaseParameter.Year).ToListAsync();
                        }
                        else
                        {
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Date != null && o.Date.Value.Year == BaseParameter.Year && o.Date.Value.Month == BaseParameter.Month).ToListAsync();
                        }
                    }
                    switch (BaseParameter.Action)
                    {
                        case 1:
                            result.List = result.List.Where(o => o.Active == true).ToList();
                            break;
                        case 2:
                            result.List = result.List.Where(o => o.IsComplete == true).ToList();
                            break;
                    }
                }
            }
            result.List = result.List.OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToList();
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrder>> GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync(BaseParameter<ProductionOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrder>();
            result.List = new List<ProductionOrder>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                    switch (BaseParameter.Action)
                    {
                        case 1:
                            result.List = result.List.Where(o => o.Active == true).ToList();
                            break;
                        case 2:
                            result.List = result.List.Where(o => o.IsComplete == true).ToList();
                            break;
                    }
                }
            }
            result.List = result.List.OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToList();
            return result;
        }
    }
}

