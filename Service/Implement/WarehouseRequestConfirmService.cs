
namespace Service.Implement
{
    public class WarehouseRequestConfirmService : BaseService<WarehouseRequestConfirm, IWarehouseRequestConfirmRepository>
    , IWarehouseRequestConfirmService
    {
        private readonly IWarehouseRequestConfirmRepository _WarehouseRequestConfirmRepository;
        private readonly IWarehouseRequestService _WarehouseRequestService;
        private readonly IMembershipRepository _MembershipRepository;

        public WarehouseRequestConfirmService(IWarehouseRequestConfirmRepository WarehouseRequestConfirmRepository
            , IWarehouseRequestService WarehouseRequestService
            , IMembershipRepository MembershipRepository

            ) : base(WarehouseRequestConfirmRepository)
        {
            _WarehouseRequestConfirmRepository = WarehouseRequestConfirmRepository;
            _WarehouseRequestService = WarehouseRequestService;
            _MembershipRepository = MembershipRepository;

        }
        public override void Initialization(WarehouseRequestConfirm model)
        {
            BaseInitialization(model);
            model.Date = GlobalHelper.InitializationDateTime;
            if (model.MembershipID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.MembershipID.Value);
                if (Membership != null)
                {
                    model.Code = Membership.Code;
                    model.MembershipName = Membership.Name;
                    model.Description = Membership.CategoryDepartmentName;
                }
            }
        }

        public override async Task<BaseResult<WarehouseRequestConfirm>> SaveAsync(BaseParameter<WarehouseRequestConfirm> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequestConfirm>();

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
                BaseParameter.BaseModel = result.BaseModel;
                await SyncAsync(BaseParameter);
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequestConfirm>> SyncAsync(BaseParameter<WarehouseRequestConfirm> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequestConfirm>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        var Membership = await _MembershipRepository.GetByIDAsync(BaseParameter.BaseModel.MembershipID.Value);
                        if (Membership.ID > 0)
                        {
                            if (Membership.CategoryPositionID == GlobalHelper.PositionID)
                            {
                                var BaseParameterWarehouseRequest = new BaseParameter<WarehouseRequest>();
                                BaseParameterWarehouseRequest.ID = BaseParameter.BaseModel.ParentID.Value;
                                var BaseResultWarehouseRequest = await _WarehouseRequestService.GetByIDAsync(BaseParameterWarehouseRequest);
                                if (BaseResultWarehouseRequest.BaseModel.ID > 0)
                                {
                                    int IsCheck = 0;
                                    if (BaseResultWarehouseRequest.BaseModel.SupplierID == Membership.CategoryDepartmentID)
                                    {
                                        BaseResultWarehouseRequest.BaseModel.IsManagerSupplier = true;
                                        IsCheck = IsCheck + 1;
                                    }
                                    if (BaseResultWarehouseRequest.BaseModel.CustomerID == Membership.CategoryDepartmentID)
                                    {
                                        BaseResultWarehouseRequest.BaseModel.IsManagerCustomer = true;
                                        IsCheck = IsCheck + 1;
                                    }
                                    if (IsCheck > 0)
                                    {
                                        BaseParameterWarehouseRequest.BaseModel = BaseResultWarehouseRequest.BaseModel;
                                        await _WarehouseRequestService.SaveAsync(BaseParameterWarehouseRequest);
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

