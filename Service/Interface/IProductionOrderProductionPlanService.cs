namespace Service.Interface
{
    public interface IProductionOrderProductionPlanService : IBaseService<ProductionOrderProductionPlan>
    {
        Task<BaseResult<ProductionOrderProductionPlan>> SyncQuantityToQuantityCutAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter);
        Task<BaseResult<ProductionOrderProductionPlan>> ExportByParentIDToExcelAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter);
        Task<BaseResult<ProductionOrderProductionPlan>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderProductionPlan> BaseParameter);
    }
}

