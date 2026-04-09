namespace Service.Interface
{
    public interface IProductionOrderCuttingOrderService : IBaseService<ProductionOrderCuttingOrder>
    {
        Task<BaseResult<ProductionOrderCuttingOrder>> GetByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter);
        Task<BaseResult<ProductionOrderCuttingOrder>> ExportToExcelAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter);
        Task<BaseResult<ProductionOrderCuttingOrder>> SyncByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter);
        Task<BaseResult<ProductionOrderCuttingOrder>> SyncMESByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter);
    }
}

