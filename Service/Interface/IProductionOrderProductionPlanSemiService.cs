namespace Service.Interface
{
    public interface IProductionOrderProductionPlanSemiService : IBaseService<ProductionOrderProductionPlanSemi>
    {
        Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncByQuantityActualAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter);
        Task<BaseResult<ProductionOrderProductionPlanSemi>> ExportByParentIDToExcelAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter);
        Task<BaseResult<ProductionOrderProductionPlanSemi>> SortByListAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter);
        Task<BaseResult<ProductionOrderProductionPlanSemi>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter);
    }
}

