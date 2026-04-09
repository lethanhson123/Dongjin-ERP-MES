namespace Service.Interface
{
    public interface IWarehouseStockService : IBaseService<WarehouseStock>
    {
        Task<BaseResult<WarehouseStock>> GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync(BaseParameter<WarehouseStock> BaseParameter);
        Task<BaseResult<WarehouseStock>> SyncAsync(BaseParameter<WarehouseStock> BaseParameter);
    }
}

