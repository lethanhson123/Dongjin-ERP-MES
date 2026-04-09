namespace Service.Implement
{
    public class InvoiceOutputService : BaseService<InvoiceOutput, IInvoiceOutputRepository>
    , IInvoiceOutputService
    {
        private readonly IInvoiceOutputRepository _InvoiceOutputRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IInvoiceOutputDetailRepository _InvoiceOutputDetailRepository;
        private readonly IInvoiceOutputFileRepository _InvoiceOutputFileRepository;
        private readonly IMembershipCompanyRepository _MembershipCompanyRepository;
        public InvoiceOutputService(IInvoiceOutputRepository InvoiceOutputRepository
            , ICompanyRepository CompanyRepository
            , IInvoiceOutputDetailRepository InvoiceOutputDetailRepository
            , IInvoiceOutputFileRepository InvoiceOutputFileRepository
            , IMembershipCompanyRepository MembershipCompanyRepository
            ) : base(InvoiceOutputRepository)
        {
            _InvoiceOutputRepository = InvoiceOutputRepository;
            _CompanyRepository = CompanyRepository;
            _InvoiceOutputDetailRepository = InvoiceOutputDetailRepository;
            _InvoiceOutputFileRepository = InvoiceOutputFileRepository;
            _MembershipCompanyRepository = MembershipCompanyRepository;
        }
        public override void InitializationSave(InvoiceOutput model)
        {
            BaseInitialization(model);
            model.Code = model.Code ?? GlobalHelper.InitializationDateTimeCode;
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;
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
            model.Total = model.Total ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override async Task<BaseResult<InvoiceOutput>> SaveAsync(BaseParameter<InvoiceOutput> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutput>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Active == true && o.Code == BaseParameter.BaseModel.Code && o.SupplierID == BaseParameter.BaseModel.SupplierID && o.CustomerID == BaseParameter.BaseModel.CustomerID).FirstOrDefaultAsync();
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
                    await SyncAsync(result);
                }
            }
            return result;
        }
        public override async Task<BaseResult<InvoiceOutput>> RemoveAsync(BaseParameter<InvoiceOutput> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutput>();
            result.Count = await _InvoiceOutputRepository.RemoveAsync(BaseParameter.ID);
            if (result.Count > 0)
            {
                await SyncRemoveAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceOutput>> SyncAsync(BaseResult<InvoiceOutput> BaseResult)
        {
            var result = new BaseResult<InvoiceOutput>();
            var List = await _InvoiceOutputFileRepository.GetByCondition(item => item.Code == BaseResult.BaseModel.Code).ToListAsync();
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
                    item.ParentID = BaseResult.BaseModel.ID;
                    await _InvoiceOutputFileRepository.UpdateAsync(item);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceOutput>> SyncRemoveAsync(BaseParameter<InvoiceOutput> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutput>();
            var List = await _InvoiceOutputDetailRepository.GetByParentIDToListAsync(BaseParameter.ID);
            if (List != null && List.Count > 0)
            {
                await _InvoiceOutputDetailRepository.RemoveRangeAsync(List);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceOutput>> GetByMembershipIDToListAsync(BaseParameter<InvoiceOutput> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipCompanyID.Contains(o.CustomerID)).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceOutput>> GetByMembershipID_ActiveToListAsync(BaseParameter<InvoiceOutput> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipCompanyID.Contains(o.CustomerID) && o.Active == BaseParameter.Active).OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToListAsync();
                }
            }
            return result;
        }
    }
}

