namespace Service.Interface
{
    public interface IProductionOrderService : IBaseService<ProductionOrder>
    {
        Task<BaseResult<ProductionOrder>> GetByActive_IsCompleteToListAsync(BaseParameter<ProductionOrder> BaseParameter);
        Task<BaseResult<ProductionOrder>> GetByMembershipIDToListAsync(BaseParameter<ProductionOrder> BaseParameter);
        Task<BaseResult<ProductionOrder>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<ProductionOrder> BaseParameter);
        Task<BaseResult<ProductionOrder>> GetByCompanyID_Year_Month_ActionToListAsync(BaseParameter<ProductionOrder> BaseParameter);
        Task<BaseResult<ProductionOrder>> GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync(BaseParameter<ProductionOrder> BaseParameter);
    }
}

