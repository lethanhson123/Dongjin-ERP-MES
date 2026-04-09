namespace Service.Interface
{
    public interface IProductionOrderSPSTOrderService : IBaseService<ProductionOrderSPSTOrder>
    {
        Task<BaseResult<ProductionOrderSPSTOrder>> GetByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter);
        Task<BaseResult<ProductionOrderSPSTOrder>> ExportToExcelAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter);
        Task<BaseResult<ProductionOrderSPSTOrder>> SyncByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter);
        Task<BaseResult<ProductionOrderSPSTOrder>> SyncMESByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter);
    }
}

