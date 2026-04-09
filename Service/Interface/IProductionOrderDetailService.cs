namespace Service.Interface
{
    public interface IProductionOrderDetailService : IBaseService<ProductionOrderDetail>
    {
        Task<BaseResult<ProductionOrderDetail>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderDetail> BaseParameter);
    }
}

